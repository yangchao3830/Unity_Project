using System;
using Unity.Mathematics;

[Serializable]
public class CharacterRuntimeData
{
    public CharacterDefinitionSO Definition;
    public int Level;
    public int CurrentsHP;
    public int CurrentsSP;
    public int CurrentBP;
    public int CurrentExo;

    public string DisplayName => Definition.name;
    public StatBlock EquipmentStats;

    public CharacterRuntimeData(CharacterDefinitionSO definition)
    {
        Definition = definition;
        EquipmentStats = StatBlock.Zero;

        var stats = GetTotalStats();
        CurrentsHP = stats.MaxHP;
        CurrentsSP = stats.MaxSP;
        CurrentBP = 0;
    }

    public StatBlock GetBaseStats()
    {
        if (Definition is AllyDefinitionSO allyDefinitionSO)
            return allyDefinitionSO.GetStatForLevel(Level);

        if (Definition is EnemyDefinitionSO enemyDefinitionSO)
            return enemyDefinitionSO.BaseStats;

        return Definition != null ? Definition.BaseStats : StatBlock.Zero;
    }

    public StatBlock GetTotalStats() => GetBaseStats() + EquipmentStats;

    #region  数据变化接口
    public void HealFull()
    {
        CurrentsHP = GetTotalStats().MaxHP;
        CurrentsSP = GetTotalStats().MaxSP;
    }

    public void ModifyHP(int amount)
    {
        CurrentsHP += amount;
        CurrentsHP = Mathf.Clamp(CurrentsHP, 0, GetTotalStats().MaxHP);
    }

    public void ModifySP(int amount)
    {
        CurrentsSP += amount;
        CurrentsSP = Mathf.Clamp(CurrentsSP, 0, GetTotalStats().MaxSP);
    }

    public void ResetBattleBP()
    {
        CurrentBP = 0;
    }

    #endregion

}
