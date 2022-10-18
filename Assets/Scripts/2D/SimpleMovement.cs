using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class SimpleMovement : MonoBehaviour
{
    [Header("Move")]
    public float movingSpeed = 5;
    public float runningSpeedFactor = 1.5f;
    //public float rotationSpeed = 3;
    public bool holdShiftToRun = false;
    public bool isRunning = false;
    private Vector2 direction;

    [Header("Jump")]
    public float jumpHeight = 5;
    bool jumping;

    [Header("GroundCheck")]
    public float distanceToCheck = 0.5f;
    public bool isGrounded;

    private Animator _animator;
    private Rigidbody rb;

    void Start()
    {
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (direction.magnitude != 0)
        {
            _animator.SetFloat("Speed", isRunning ? 5 : 3);
            float speed = movingSpeed * (isRunning ? runningSpeedFactor : 1);
            transform.Translate(0, 0, speed * direction.y * Time.deltaTime);
            //transform.Rotate(0, rotationSpeed * direction.x * Time.deltaTime, 0);
        }
        else
        {
            _animator.SetFloat("Speed", 0);
        }


        //RandomCheckGround code
        if (Physics.Raycast(transform.position, Vector2.down, distanceToCheck))
        {
            jumping = false;
            isGrounded = true;
        }
        else
        {
            jumping = true;
            isGrounded = false;
        }

        //RandomJump code
        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y));
            rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
    }

    public void Run(InputAction.CallbackContext ctx)
    {
        isRunning = holdShiftToRun && ctx.ReadValueAsButton();
    }

}
