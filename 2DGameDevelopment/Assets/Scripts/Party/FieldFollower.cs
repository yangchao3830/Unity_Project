

public class FieldFollower : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Animator Params")]
    [SerializeField] private string isMovingParam = "isMoving";
    [SerializeField] private string moveXparam = "moveX";
    [SerializeField] private string moveYparam = "moveY";

    [Header("最小位移阈值")]
    [SerializeField] private float movementThreshold = 0.001f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -25f;
    [SerializeField] private float groundSnapFace = -2f;
    [SerializeField] private float maxVelocity = -50f;

    private float verticalVelocity;

    #region 对外接口
    public void SetUpFollower(CharacterDefinitionSO definition)
    {
        animator.runtimeAnimatorController = definition.fieldAnimator;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = definition.Portrait;
    }

    public void MoveTo(Vector3 targetPos, float speed)
    {
        Vector3 toTarget = targetPos - transform.position;
        toTarget.y = 0;
        Vector3 horizantalStep = Vector3.ClampMagnitude(toTarget, Mathf.Max(0f, speed) * Time.deltaTime);

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

        Vector3 movement = horizantalStep;
        movement.y = verticalVelocity * Time.deltaTime;

        characterController.Move(movement);
        UpdateAnimation(horizantalStep);
    }
    #endregion

    private void UpdateAnimation(Vector3 step)
    {
        bool isMoveing = step.magnitude > movementThreshold * movementThreshold;
        animator.SetBool(isMovingParam, isMoveing);

        if (isMoveing)
        {
            animator.SetFloat(moveXparam, step.x);
            animator.SetFloat(moveYparam, step.y);
        }
    }

    public void SnapTo(Vector3 position)
    {
        bool enable = characterController.enabled;
        characterController.enabled = false;
        transform.position = position;
        characterController.enabled = enable;

        verticalVelocity = 0f;
    }

}
