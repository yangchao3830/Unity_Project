
public abstract class CharacterDefinitionSO : ScriptableObject
{
    [Header("Identity")]
    public string ID;
    public string Name;
    public Sprite Portrait;

    [Header("Stats")]
    public int BaseLevel = 1;
    public StatBlock BaseStats;

    [Header("Animator")]
    public AnimatorOverrideController fieldAnimator;
    public AnimatorOverrideController battleAnimator;
}

[System.Serializable]
public struct StatBlock
{
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int MaxHP;
    /// <summary>
    /// 最大法力值
    /// </summary>
    public int MaxSP;
    /// <summary>
    /// 物理攻击
    /// </summary>
    public int PAtk;
    /// <summary>
    /// 物理防御力
    /// </summary>
    public int PDef;
    /// <summary>
    /// 属性攻击
    /// </summary>
    public int MAtk;
    /// <summary>
    /// 属性防御
    /// </summary>
    public int MDef;
    /// <summary>
    /// 速度
    /// </summary>
    public int Speed;
    /// <summary>
    /// 命中率
    /// </summary>
    public int Accuracy;
    /// <summary>
    /// 闪避率
    /// </summary>
    public int Evastion;

    public static StatBlock Zero = new StatBlock();//零值

    public static StatBlock operator +(StatBlock a, StatBlock b)
    {
        return new()
        {
            MaxHP = a.MaxHP + b.MaxHP,
            MaxSP = a.MaxSP + b.MaxSP,
            PAtk = a.PAtk + b.PAtk,
            PDef = a.PDef + b.PDef,
            MAtk = a.MAtk + b.MAtk,
            MDef = a.MDef + b.MDef,
            Speed = a.Speed + b.Speed,
            Accuracy = a.Accuracy + b.Accuracy,
            Evastion = a.Evastion + b.Evastion
        };
    }

}
