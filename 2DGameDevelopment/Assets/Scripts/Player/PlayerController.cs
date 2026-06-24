
/// <summary>
/// 实现人物移动和动画效果
/// </summary>
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    [SerializeField] private float speed = 5f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -25f;
    [SerializeField] private float groundSnapFace = -2f;
    [SerializeField] private float maxVelocity = -50f;


    private Vector2 movementInput;
    private float verticalVelocity;
    private bool isMoving;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Movement();
        SetAnimation();
    }

    private void Movement()
    {
        var input = InputSysTemController.Instance;

        if (input == null) return;

        movementInput = input.GetMovementInpt();

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
        Vector3 velocity = new Vector3(movementInput.x, 0, movementInput.y) * speed;
        velocity.y = verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void SetAnimation()
    {
        if (animator == null) return;
        isMoving = movementInput.magnitude > 0.1;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("moveX", movementInput.x);
            animator.SetFloat("moveY", movementInput.y);
        }
    }
}
