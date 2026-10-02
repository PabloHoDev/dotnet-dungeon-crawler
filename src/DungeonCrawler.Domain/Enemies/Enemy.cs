using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Enemies
{
    /// <summary>
    /// A concrete enemy in an encounter: a definition scaled to the player's
    /// level and to the floor, with its own health and cooldowns.
    /// Created by <see cref="EnemyFactory"/>.
    /// </summary>
    public sealed class Enemy : Entity<Guid>, ICombatant
    {
        private readonly Dictionary<EnemyAbility, int> _cooldowns = new Dictionary<EnemyAbility, int>();

        internal Enemy(
            Guid id,
            EnemyDefinition definition,
            string name,
            EnemyCategory category,
            int maxHealth,
            int attack,
            int defense,
            int experienceReward,
            int goldReward)
            : base(id)
        {
            Definition = definition;
            Name = name;
            Category = category;
            Health = Health.Full(maxHealth);
            Attack = attack;
            Defense = defense;
            ExperienceReward = experienceReward;
            GoldReward = goldReward;
        }

        public EnemyDefinition Definition { get; }

        public string Name { get; }

        public EnemyCategory Category { get; }

        public Health Health { get; private set; }

        public int Attack { get; }

        public int Defense { get; }

        public int ExperienceReward { get; }

        public int GoldReward { get; }

        public bool IsGuarding { get; private set; }

        /// <summary>How many turns this enemy has already acted in the current combat.</summary>
        public int TurnsTaken { get; private set; }

        public int Speed
        {
            get { return Definition.Speed; }
        }

        public Probability Evasion
        {
            get { return Probability.Never; }
        }

        public bool IsDefeated
        {
            get { return Health.IsDepleted; }
        }

        public EnemyIntent DecideIntent(ICombatant target, IRandomSource random)
        {
            return Definition.Behavior.Decide(this, target, random);
        }

        public bool IsReady(EnemyAbility ability)
        {
            Guard.AgainstNull(ability, nameof(ability));
            return !_cooldowns.TryGetValue(ability, out var remaining) || remaining == 0;
        }

        /// <summary>The first special ability that is off cooldown, or null.</summary>
        public EnemyAbility? FirstReadyAbility()
        {
            foreach (var ability in Definition.Abilities)
            {
                if (IsReady(ability))
                {
                    return ability;
                }
            }

            return null;
        }

        /// <summary>Raw damage of an attack with the given multiplier, before the target's defense.</summary>
        public int AttackDamage(double powerMultiplier)
        {
            return EnemyScaling.Scale(Attack, powerMultiplier);
        }

        public void TakeDamage(int amount)
        {
            Health = Health.TakeDamage(amount);
        }

        public void Heal(int amount)
        {
            Health = Health.Heal(amount);
        }

        public void StartGuarding()
        {
            IsGuarding = true;
        }

        public void StopGuarding()
        {
            IsGuarding = false;
        }

        /// <summary>Registers that the enemy acted: cooldowns advance and the used ability starts its own.</summary>
        public void CompleteTurn(EnemyIntent intent)
        {
            Guard.AgainstNull(intent, nameof(intent));

            foreach (var ability in new List<EnemyAbility>(_cooldowns.Keys))
            {
                _cooldowns[ability] = Math.Max(0, _cooldowns[ability] - 1);
            }

            if (intent.Ability is not null)
            {
                _cooldowns[intent.Ability] = intent.Ability.Cooldown;
            }

            TurnsTaken++;
        }
    }
}
