namespace DungeonCrawler.Domain.Progression
{
    /// <summary>
    /// Every balancing number of the game in one place (RNF-012, RNF-013).
    /// Formulas read these values; nothing else in the domain hard-codes them.
    /// </summary>
    public static class Balance
    {
        // Progression
        public const int ExperienceCurveFactor = 50;
        public const int AttributePointsPerLevel = 5;
        public const int AbilityPointsPerLevel = 1;
        public const int MaxAbilityRank = 3;

        // Derived statistics
        public const int HealthPerVitality = 8;
        public const int HealthPerLevel = 6;
        public const int DefensePerVitality = 1;
        public const double BasicAttackBaseDamage = 4.0;
        public const double BasicAttackScaling = 1.5;
        public const double CriticalChancePerLuck = 0.01;
        public const double MaxCriticalChance = 0.50;
        public const double EvasionPerAgility = 0.01;
        public const double MaxEvasion = 0.30;

        // Combat
        public const int MitigationConstant = 40;
        public const double GuardDamageFactor = 0.5;
        public const int MinimumDamage = 1;
        public const double EnemyCriticalMultiplier = 1.5;

        // Enemy scaling
        public const double EnemyLevelScalingStep = 0.09;
        public const double EnemyFloorScalingStep = 0.14;

        // Expedition
        public const double RestHealFraction = 0.40;
        public const int InventoryCapacity = 20;
        public const int TreasureGoldPerFloor = 25;

        // Loot
        public const double CommonDropChance = 0.40;
        public const double LuckRarityShiftPerPoint = 0.5;
        public const double MaxLuckRarityShift = 20.0;
    }
}
