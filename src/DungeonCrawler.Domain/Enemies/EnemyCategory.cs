namespace DungeonCrawler.Domain.Enemies
{
    /// <summary>How dangerous and how rewarding an enemy is (RF-015 to RF-019).</summary>
    public enum EnemyCategory
    {
        Common,
        Elite,
        MiniBoss,
        Boss,
    }

    /// <summary>The predominant style of an enemy.</summary>
    public enum EnemyArchetype
    {
        Melee,
        Ranged,
        Tank,
        Caster,
    }
}
