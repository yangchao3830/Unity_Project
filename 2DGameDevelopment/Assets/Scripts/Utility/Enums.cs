public enum GameMode
{
    Explore,
    InteractionMenu,
    Battle,
    Puase
}

public enum ActiveMap
{
    Player,
    UI,
    Battle,
    None
}

public enum CameraView
{
    Explore,
    Battle,
    BattleResult
}

public enum PlayerJob
{
    Any,
    /// <summary>
    /// 剑士
    /// </summary>
    Warrior,
    /// <summary>
    /// 药师
    /// </summary>
    Apothecary,
    /// <summary>
    /// 神官
    /// </summary>
    Cleric,
    /// <summary>
    /// 舞娘
    /// </summary>
    Dancer,
    /// <summary>
    /// 猎人
    /// </summary>
    Hunter,
    /// <summary>
    /// 商人
    /// </summary>
    Merchant,
    /// <summary>
    /// 学者
    /// </summary>
    Scholar,
    /// <summary>
    /// 盗贼
    /// </summary>
    Thief,
}

/// <summary>
/// 成长等级
/// </summary>
public enum GrowthRank { S, A, B, C, D }
