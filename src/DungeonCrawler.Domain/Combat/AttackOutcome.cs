namespace DungeonCrawler.Domain.Combat
{
    /// <summary>What happened when an attack was resolved against a target.</summary>
    public sealed class AttackOutcome
    {
        private AttackOutcome(bool wasEvaded, bool wasCritical, bool wasGuarded, int damage)
        {
            WasEvaded = wasEvaded;
            WasCritical = wasCritical;
            WasGuarded = wasGuarded;
            Damage = damage;
        }

        public bool WasEvaded { get; }

        public bool WasCritical { get; }

        public bool WasGuarded { get; }

        public int Damage { get; }

        public static AttackOutcome Evaded()
        {
            return new AttackOutcome(true, false, false, 0);
        }

        public static AttackOutcome Hit(int damage, bool wasCritical, bool wasGuarded)
        {
            return new AttackOutcome(false, wasCritical, wasGuarded, damage);
        }
    }
}
