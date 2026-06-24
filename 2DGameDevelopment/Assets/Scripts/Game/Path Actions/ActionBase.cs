
public abstract class ActionBase : MonoBehaviour
{
    public PlayerJob MathJob = PlayerJob.Any;
    public ActionCommandInfo ConmmandInfo;

    public virtual bool CanShow(AllyDefinitionSO interactor)
    {
        return IsJobMatch(interactor);
    }

    public virtual bool CanExecute(AllyDefinitionSO interactor)
    {
        return true;
    }

    protected virtual bool IsJobMatch(AllyDefinitionSO interactor)
    {
        if(MathJob == PlayerJob.Any) return true;

        return interactor.Job == MathJob;
    }

    public virtual void TriggerAction(AllyDefinitionSO interactor)
    {
        Execute(interactor);
    }

    public virtual void Execute(object contexData =null)
    {
        
    }


}

[System.Serializable]
public struct ActionCommandInfo
{
    public string DisplayName;
    public Sprite Icon;
    public int Order;
}
