
public class RecruitAction : ActionBase
{
    public CharacterDefinitionSO CurrentCharacter { get; private set; }
    void Awake()
    {
        CurrentCharacter = GetComponent<CharacterIdentity>().CharacterDefinition;
    }

    public override void TriggerAction(AllyDefinitionSO interactor)
    {
        EvnetBus.Publish(new PanelRequestEvent(this));
    }
}
