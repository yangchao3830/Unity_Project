
public class RecruitAction : ActionBase
{
    public CharacterDefinitionSO CurrentCharacter { get; private set; }
    void Awake()
    {
        CurrentCharacter = GetComponent<CharacterIdentity>().CharacterDefinition;
    }

    public override void TriggerAction(AllyDefinitionSO interactor)
    {
        base.TriggerAction(interactor);
        EvnetBus.Publish(new PanelRequestEvent(this));
    }

    public override void Execute(object contexData = null)
    {
        PartyManager.Instance.RecruitMember(CurrentCharacter);
        HideSceneNPC();
    } 

    private void HideSceneNPC()
    {
        this.gameObject.SetActive(false);
    }
}
