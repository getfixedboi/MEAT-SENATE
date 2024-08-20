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

    private Vector3 _velocity;

    private void Awake()
    {
        Character = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float axis = Input.GetAxis("Horizontal");
        float axis2 = Input.GetAxis("Vertical");
        MoveInput = new Vector3(axis, 0f, axis2);
        MoveInput.Normalize();
        Vector3 vector2 = transform.right * axis + transform.forward * axis2;
        vector2 = Vector3.ClampMagnitude(vector2, 1f);
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Character.Move(vector2 * _speed * Time.deltaTime);
            _velocity.y = _velocity.y + _gravity * Time.deltaTime;
            Character.Move(_velocity * Time.deltaTime);
        }
        else
        {
            MoveInput = Vector3.zero;
            Character.Move(Vector3.zero * _speed * Time.deltaTime);
            _velocity.y = 0;
            Character.Move(_velocity * Time.deltaTime);
        }
    }
}
