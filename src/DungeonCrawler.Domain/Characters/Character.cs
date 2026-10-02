using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler.Domain.Abilities;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Items;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Characters
{
    /// <summary>
    /// The player's character: identity plus the components that define it
    /// (class, attributes, abilities, loadout, inventory). It coordinates those
    /// components and keeps them consistent; the rules themselves live in the
    /// components and in the formulas.
    /// </summary>
    public sealed class Character : AggregateRoot<Guid>, ICombatant
    {
        private readonly List<LearnedAbility> _abilities = new List<LearnedAbility>();

        private Character(Guid id, string name, CharacterClass characterClass)
            : base(id)
        {
            Name = name;
            Class = characterClass;
            Level = Level.First;
            BaseAttributes = characterClass.BaseAttributes;
            Inventory = new Inventory(Balance.InventoryCapacity);
            Loadout = new Loadout();

            // Every class starts knowing its first ability.
            _abilities.Add(new LearnedAbility(characterClass.Abilities[0]));

            Health = Health.Full(MaxHealth);
            Resource = Resource.Full(MaxResource);
        }

        public string Name { get; }

        public CharacterClass Class { get; }

        public Level Level { get; private set; }

        /// <summary>Accumulated experience since level 1.</summary>
        public int Experience { get; private set; }

        public Attributes BaseAttributes { get; private set; }

        public int UnspentAttributePoints { get; private set; }

        public int UnspentAbilityPoints { get; private set; }

        public int Gold { get; private set; }

        public Health Health { get; private set; }

        public Resource Resource { get; private set; }

        public Inventory Inventory { get; }

        public Loadout Loadout { get; }

        public bool IsGuarding { get; private set; }

        public IReadOnlyList<LearnedAbility> Abilities
        {
            get { return _abilities.AsReadOnly(); }
        }

        /// <summary>Base attributes plus the bonuses of the equipped items.</summary>
        public Attributes EffectiveAttributes
        {
            get { return BaseAttributes.Plus(Loadout.AttributeBonus); }
        }

        public bool IsDefeated
        {
            get { return Health.IsDepleted; }
        }

        public int MaxHealth
        {
            get
            {
                return StatFormulas.MaxHealth(
                    Class.BaseHealth,
                    EffectiveAttributes.Vitality,
                    Level,
                    Loadout.Modifiers.MaxHealth);
            }
        }

        public int MaxResource
        {
            get
            {
                return StatFormulas.MaxResource(
                    Class.BaseResource,
                    EffectiveAttributes.Intelligence,
                    Class.ResourcePerIntelligence);
            }
        }

        public int Defense
        {
            get { return StatFormulas.Defense(EffectiveAttributes.Vitality, Loadout.Modifiers.Defense); }
        }

        public int Speed
        {
            get { return EffectiveAttributes.Agility; }
        }

        /// <summary>Flat damage added to every attack by the equipped items.</summary>
        public int AttackBonus
        {
            get { return Loadout.Modifiers.Attack; }
        }

        public int BasicAttackDamage
        {
            get
            {
                return StatFormulas.BasicAttackDamage(
                    EffectiveAttributes.Get(Class.PrimaryAttribute),
                    AttackBonus);
            }
        }

        public Probability CriticalChance
        {
            get { return StatFormulas.CriticalChance(Class.BaseCriticalChance, EffectiveAttributes.Luck); }
        }

        public Probability Evasion
        {
            get { return StatFormulas.Evasion(EffectiveAttributes.Agility); }
        }

        public static Character Create(Guid id, string name, CharacterClass characterClass)
        {
            Guard.AgainstNullOrWhiteSpace(name, nameof(name));
            Guard.AgainstNull(characterClass, nameof(characterClass));

            return new Character(id, name.Trim(), characterClass);
        }

        // ---------------------------------------------------------------
        // Progression
        // ---------------------------------------------------------------

        /// <summary>
        /// Adds experience and applies every level up it causes. Each level grants
        /// attribute and ability points and fully restores the character.
        /// </summary>
        /// <returns>The number of levels gained.</returns>
        public int GainExperience(int amount)
        {
            Guard.AgainstNegative(amount, nameof(amount));

            Experience = Math.Min(ExperienceCurve.MaxTotal, Experience + amount);

            var newLevel = ExperienceCurve.LevelFor(Experience);
            var levelsGained = newLevel.Value - Level.Value;
            if (levelsGained == 0)
            {
                return 0;
            }

            Level = newLevel;
            UnspentAttributePoints += levelsGained * Balance.AttributePointsPerLevel;
            UnspentAbilityPoints += levelsGained * Balance.AbilityPointsPerLevel;
            RecoverFully();

            return levelsGained;
        }

        /// <exception cref="DomainException">There are not enough unspent points.</exception>
        public void AllocateAttribute(AttributeType attribute, int points)
        {
            if (points < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(points), points, "At least one point must be allocated.");
            }

            if (points > UnspentAttributePoints)
            {
                throw new DomainException(
                    $"Cannot allocate {points} points: only {UnspentAttributePoints} available.");
            }

            var missingHealth = Health.Maximum - Health.Current;

            BaseAttributes = BaseAttributes.Increase(attribute, points);
            UnspentAttributePoints -= points;

            // New maximum health is granted as current health too, keeping the same missing amount.
            Health = Health.Of(Math.Max(0, MaxHealth - missingHealth), MaxHealth);
            Resource = Resource.WithMaximum(MaxResource);
        }

        public LearnedAbility? FindAbility(string abilityId)
        {
            return _abilities.FirstOrDefault(ability => ability.Definition.Id == abilityId);
        }

        /// <summary>True when an ability point could be spent on the ability right now.</summary>
        public bool CanImproveAbility(AbilityDefinition ability)
        {
            Guard.AgainstNull(ability, nameof(ability));

            if (UnspentAbilityPoints < 1 || Level.CompareTo(ability.UnlockLevel) < 0)
            {
                return false;
            }

            var learned = FindAbility(ability.Id);
            return learned is null || !learned.IsMaxRank;
        }

        /// <summary>
        /// Spends one ability point to learn the ability or, if already known, to raise its rank.
        /// </summary>
        /// <exception cref="DomainException">No points, level too low, or already at the maximum rank.</exception>
        public void ImproveAbility(string abilityId)
        {
            var definition = Class.GetAbility(abilityId);

            if (UnspentAbilityPoints < 1)
            {
                throw new DomainException("No ability points available.");
            }

            if (Level.CompareTo(definition.UnlockLevel) < 0)
            {
                throw new DomainException($"{definition.Name} requires level {definition.UnlockLevel}.");
            }

            var learned = FindAbility(abilityId);
            if (learned is null)
            {
                _abilities.Add(new LearnedAbility(definition));
            }
            else
            {
                learned.Upgrade();
            }

            UnspentAbilityPoints--;
        }

        // ---------------------------------------------------------------
        // Items
        // ---------------------------------------------------------------

        /// <exception cref="BusinessRuleViolationException">The inventory is full.</exception>
        public void Store(Item item)
        {
            Inventory.Add(item);
        }

        public void Discard(Guid itemId)
        {
            Inventory.Remove(itemId);
        }

        /// <summary>
        /// Equips an item from the inventory. Whatever was in the slot goes back
        /// to the inventory, taking the place the new item just left.
        /// </summary>
        /// <exception cref="DomainException">The item is not in the inventory or is not equipment.</exception>
        public void Equip(Guid itemId)
        {
            var item = Inventory.Find(itemId);
            if (item is null)
            {
                throw new DomainException("The item is not in the inventory.");
            }

            var equipment = item as Equipment;
            if (equipment is null)
            {
                throw new DomainException($"'{item.Name}' cannot be equipped.");
            }

            Inventory.Remove(itemId);
            var previous = Loadout.Equip(equipment);
            if (previous is not null)
            {
                Inventory.Add(previous);
            }

            RefreshMaximums();
        }

        /// <exception cref="BusinessRuleViolationException">The inventory is full.</exception>
        /// <exception cref="DomainException">The slot is empty.</exception>
        public void Unequip(EquipmentSlot slot)
        {
            CheckRule(new InventoryMustHaveSpaceRule(Inventory));

            Inventory.Add(Loadout.Unequip(slot));
            RefreshMaximums();
        }

        public void AddGold(int amount)
        {
            Gold += Guard.AgainstNegative(amount, nameof(amount));
        }

        // ---------------------------------------------------------------
        // Combat
        // ---------------------------------------------------------------

        public void TakeDamage(int amount)
        {
            Health = Health.TakeDamage(amount);
        }

        public void Heal(int amount)
        {
            Health = Health.Heal(amount);
        }

        public void SpendResource(int cost)
        {
            Resource = Resource.Spend(cost);
        }

        public void RestoreResource(int amount)
        {
            Resource = Resource.Restore(amount);
        }

        public void StartGuarding()
        {
            IsGuarding = true;
        }

        public void StopGuarding()
        {
            IsGuarding = false;
        }

        /// <summary>Restores a fraction of maximum health and the whole resource pool.</summary>
        public void Rest(double healthFraction)
        {
            if (double.IsNaN(healthFraction) || healthFraction < 0.0 || healthFraction > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(healthFraction), healthFraction, "Must be between 0 and 1.");
            }

            Heal((int)Math.Round(MaxHealth * healthFraction, MidpointRounding.AwayFromZero));
            Resource = Resource.Refill();
        }

        public void RecoverFully()
        {
            Health = Health.Full(MaxHealth);
            Resource = Resource.Full(MaxResource);
            IsGuarding = false;
        }

        private void RefreshMaximums()
        {
            Health = Health.WithMaximum(MaxHealth);
            Resource = Resource.WithMaximum(MaxResource);
        }
    }
}
