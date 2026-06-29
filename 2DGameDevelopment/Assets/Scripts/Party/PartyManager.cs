
[RequireComponent(typeof(PartyFieldController))]
public class PartyManager : Singleton<PartyManager>
{
    private PartyFieldController fieldController;
    [Header("Initial Party")]
    [SerializeField] private CharacterDefinitionSO  playDefinition;
    [SerializeField] private List<CharacterRuntimeData> partyMembers = new List<CharacterRuntimeData>();
    public List<CharacterRuntimeData> PartyMembers => partyMembers;

    protected override void Awake()
    {
        base.Awake();
        InitParty();
        fieldController = GetComponent<PartyFieldController>();
    }

    private void InitParty()
    {
        if(partyMembers.Count == 0)
        {
            partyMembers.Add(new CharacterRuntimeData(playDefinition));
        }
    }

    private void AddMember(CharacterDefinitionSO characterDefinition)
    {
        partyMembers.Add(new CharacterRuntimeData(characterDefinition));
        RefreshFieldFollowers();
    }

    public void RecruitMember(CharacterDefinitionSO newCharacter)
    {
        AddMember(newCharacter);
        GameModeManager.Instance.RequesChangeMode(GameMode.Explore);
    }

    private void RefreshFieldFollowers()
    {
        List<CharacterDefinitionSO> defs = new(partyMembers.Count);
        foreach (var member in partyMembers)
        {
            defs.Add(member.Definition);
        }
        fieldController.UpdateFollowers(defs);
    }

}
