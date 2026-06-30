
public class PlayerInteractor : MonoBehaviour
{
    private CharacterIdentity _characterIdentity;

    private InteractionBase _target;
    void Awake()
    {
        _characterIdentity = GetComponent<CharacterIdentity>();
    }

    void Update()
    {
        if(_target is null || _target.CachedCommandInfo.Count == 0) return;
        var input = InputSysTemController.Instance;
        if (input is null) return;
        if (input.GetPlayerConfirmPressed() && _target != null)
        {
            _target.Interact(_characterIdentity.CharacterDefinition as AllyDefinitionSO);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out InteractionBase interactionBase))
        {
            _target = interactionBase;
            interactionBase.OnFocus(_characterIdentity.CharacterDefinition as AllyDefinitionSO);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out InteractionBase interactionBase))
        {
            interactionBase.OnLoseFocus(_characterIdentity.CharacterDefinition as AllyDefinitionSO);
        }
        _target = null;
    }

}
