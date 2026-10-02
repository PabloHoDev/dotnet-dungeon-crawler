using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Progression;

namespace DungeonCrawler.Domain.Combat
{
    /// <summary>
    /// Domain service that resolves one attack against one target (RF-011, RF-012).
    /// The order is fixed so results are reproducible: evasion roll, critical roll,
    /// critical multiplier, defense, guard.
    /// </summary>
    public static class AttackResolver
    {
        public static AttackOutcome Resolve(AttackProfile attack, ICombatant target, IRandomSource random)
        {
            Guard.AgainstNull(attack, nameof(attack));
            Guard.AgainstNull(target, nameof(target));
            Guard.AgainstNull(random, nameof(random));

            if (target.Evasion.Roll(random))
            {
                return AttackOutcome.Evaded();
            }

            var isCritical = attack.CriticalChance.Roll(random);
            var damage = isCritical
                ? DamageFormula.Critical(attack.RawDamage, attack.CriticalMultiplier)
                : attack.RawDamage;

            damage = DamageFormula.Mitigate(damage, target.Defense);

            var isGuarded = target.IsGuarding;
            if (isGuarded)
            {
                damage = DamageFormula.Guarded(damage);
            }

            target.TakeDamage(damage);
            return AttackOutcome.Hit(damage, isCritical, isGuarded);
        }
    }
}
