using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public Vector3 MoveInput;
    [HideInInspector]
    public CharacterController Character;

    [Header("Variables")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _gravity;
    [SerializeField]
    private float _jumpHeight;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundCheckDistance = 0.1f;
    public static Vector3 Velocity;
    public static bool IsGrounded = false;

    private void Awake()
    {
        Character = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (PauseMenu.IsPaused) return;

        float axis = Input.GetAxis("Horizontal");
        float axis2 = Input.GetAxis("Vertical");
        MoveInput = new Vector3(axis, 0f, axis2);
        MoveInput.Normalize();

        Vector3 direction = transform.right * axis + transform.forward * axis2;
        direction = Vector3.ClampMagnitude(direction, 1f);

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Character.Move(direction * _speed * Time.deltaTime);
        }
        else
        {
            MoveInput = Vector3.zero;
            Character.Move(Vector3.zero * Time.deltaTime);
        }

        IsGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }
            else
            {
                Velocity.y = 0;
            }
        }
        else
        {
            Velocity.y += _gravity * Time.deltaTime;
        }

        Character.Move(Velocity * Time.deltaTime);
    }
}
