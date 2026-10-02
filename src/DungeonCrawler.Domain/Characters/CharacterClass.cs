using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler.Domain.Abilities;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Characters
{
    /// <summary>
    /// The archetype of a character. A class is data plugged into
    /// <see cref="Character"/> by composition; there is no subclass of
    /// Character per class. The three MVP classes are exposed as static members.
    /// </summary>
    public sealed class CharacterClass
    {
        public static readonly CharacterClass Warrior = new CharacterClass(
            ClassId.Warrior,
            "Warrior",
            new Attributes(strength: 8, vitality: 8, agility: 4, intelligence: 2, luck: 3),
            baseHealth: 70,
            primaryAttribute: AttributeType.Strength,
            resourceName: "Energy",
            baseResource: 40,
            resourcePerIntelligence: 0,
            resourceRegeneration: 5,
            baseCriticalChance: Probability.Of(0.05),
            criticalMultiplier: 1.5,
            abilities: new[]
            {
                new AbilityDefinition(
                    "power-strike", "Power Strike", Level.Of(1), 10,
                    new DamageAbilityEffect(8, AttributeType.Strength, 2.2, 6)),
                new AbilityDefinition(
                    "second-wind", "Second Wind", Level.Of(3), 15,
                    new HealAbilityEffect(15, AttributeType.Vitality, 3.0, 8)),
                new AbilityDefinition(
                    "crushing-blow", "Crushing Blow", Level.Of(5), 20,
                    new DamageAbilityEffect(15, AttributeType.Strength, 2.6, 10)),
            });

        public static readonly CharacterClass Mage = new CharacterClass(
            ClassId.Mage,
            "Mage",
            new Attributes(strength: 2, vitality: 5, agility: 5, intelligence: 10, luck: 3),
            baseHealth: 54,
            primaryAttribute: AttributeType.Intelligence,
            resourceName: "Mana",
            baseResource: 20,
            resourcePerIntelligence: 2,
            resourceRegeneration: 5,
            baseCriticalChance: Probability.Of(0.05),
            criticalMultiplier: 1.5,
            abilities: new[]
            {
                new AbilityDefinition(
                    "arcane-bolt", "Arcane Bolt", Level.Of(1), 8,
                    new DamageAbilityEffect(12, AttributeType.Intelligence, 2.0, 6)),
                new AbilityDefinition(
                    "mend", "Mend", Level.Of(3), 14,
                    new HealAbilityEffect(10, AttributeType.Intelligence, 1.5, 6)),
                new AbilityDefinition(
                    "fireball", "Fireball", Level.Of(5), 20,
                    new DamageAbilityEffect(18, AttributeType.Intelligence, 2.6, 12)),
            });

        public static readonly CharacterClass Rogue = new CharacterClass(
            ClassId.Rogue,
            "Rogue",
            new Attributes(strength: 6, vitality: 5, agility: 9, intelligence: 2, luck: 3),
            baseHealth: 40,
            primaryAttribute: AttributeType.Agility,
            resourceName: "Energy",
            baseResource: 30,
            resourcePerIntelligence: 0,
            resourceRegeneration: 6,
            baseCriticalChance: Probability.Of(0.10),
            criticalMultiplier: 1.7,
            abilities: new[]
            {
                new AbilityDefinition(
                    "quick-strike", "Quick Strike", Level.Of(1), 8,
                    new DamageAbilityEffect(6, AttributeType.Agility, 1.6, 5, criticalChanceBonus: 0.10)),
                new AbilityDefinition(
                    "adrenaline", "Adrenaline", Level.Of(3), 12,
                    new HealAbilityEffect(8, AttributeType.Agility, 0.9, 5)),
                new AbilityDefinition(
                    "backstab", "Backstab", Level.Of(5), 18,
                    new DamageAbilityEffect(12, AttributeType.Agility, 1.9, 9, criticalChanceBonus: 0.20)),
            });

        private CharacterClass(
            ClassId id,
            string name,
            Attributes baseAttributes,
            int baseHealth,
            AttributeType primaryAttribute,
            string resourceName,
            int baseResource,
            int resourcePerIntelligence,
            int resourceRegeneration,
            Probability baseCriticalChance,
            double criticalMultiplier,
            IReadOnlyList<AbilityDefinition> abilities)
        {
            Id = id;
            Name = name;
            BaseAttributes = baseAttributes;
            BaseHealth = baseHealth;
            PrimaryAttribute = primaryAttribute;
            ResourceName = resourceName;
            BaseResource = baseResource;
            ResourcePerIntelligence = resourcePerIntelligence;
            ResourceRegeneration = resourceRegeneration;
            BaseCriticalChance = baseCriticalChance;
            CriticalMultiplier = criticalMultiplier;
            Abilities = abilities;
        }

        public static IReadOnlyList<CharacterClass> All
        {
            get { return new[] { Warrior, Mage, Rogue }; }
        }

        public ClassId Id { get; }

        public string Name { get; }

        public Attributes BaseAttributes { get; }

        public int BaseHealth { get; }

        /// <summary>The attribute the basic attack scales with.</summary>
        public AttributeType PrimaryAttribute { get; }

        public string ResourceName { get; }

        public int BaseResource { get; }

        public int ResourcePerIntelligence { get; }

        /// <summary>Resource recovered at the end of every combat round.</summary>
        public int ResourceRegeneration { get; }

        public Probability BaseCriticalChance { get; }

        public double CriticalMultiplier { get; }

        /// <summary>The abilities of the class, ordered by unlock level.</summary>
        public IReadOnlyList<AbilityDefinition> Abilities { get; }

        public static CharacterClass Get(ClassId id)
        {
            switch (id)
            {
                case ClassId.Warrior: return Warrior;
                case ClassId.Mage: return Mage;
                case ClassId.Rogue: return Rogue;
                default: throw new ArgumentOutOfRangeException(nameof(id), id, "Unknown class.");
            }
        }

        /// <exception cref="DomainException">The class has no ability with that id.</exception>
        public AbilityDefinition GetAbility(string abilityId)
        {
            var ability = Abilities.FirstOrDefault(candidate => candidate.Id == abilityId);
            if (ability is null)
            {
                throw new DomainException($"{Name} has no ability '{abilityId}'.");
            }

            return ability;
        }
    }
}
