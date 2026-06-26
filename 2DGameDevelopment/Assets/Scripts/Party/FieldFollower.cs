

public class FieldFollower : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Animator Params")]
    [SerializeField] private string isMovingParam = "isMoving";
    [SerializeField] private string moveXparam = "moveX";
    [SerializeField] private string moveZparam = "moveZ";

    [Header("Gravity")]
    [SerializeField] private float gravity = -25f;
    [SerializeField] private float groundSnapFace = -2f;
    [SerializeField] private float maxVelocity = -50f;

    private float verticalVelocity;

    #region 对外接口
    public void SetUpFollower(CharacterDefinitionSO definition)
    {
        animator.runtimeAnimatorController = definition.fieldAnimator;
    }

    public void MoveTo()
    {
        bool isGrounded = characterController.isGrounded;

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = groundSnapFace;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            if (verticalVelocity < maxVelocity)
            {
                verticalVelocity = maxVelocity;
            }
        }
    }
    #endregion

}
