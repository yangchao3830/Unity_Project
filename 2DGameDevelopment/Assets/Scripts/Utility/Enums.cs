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

public enum ItemType
{
    Equipment = 0,
    Consumable = 1
}

/// <summary>
/// 物品类型枚举
/// </summary>
public enum ItemIconKey
{
    Weapon,//武器类
    Armor,//防具
    Accessory,//饰品
    Healing,//治疗类
    SP,//Sp回复类
    Revive,//复活类
    Cure,//解除状态
    KeyItem//关键物品
}