using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivity;
    [SerializeField]
    private Transform _playerBody;

    private float _xRotation;

    [Header("BobbingEffect")]
    [SerializeField]
    private float _walkingBobbingSpeed;
    [SerializeField]
    private float _bobbingAmount;


    private float _x;
    private float _y;
    private float _defaultPosY = 0;
    private float _timer = 0;
    private Quaternion _origRotation;

    private float _sliderMultiplier = 0.01f;
    public float MouseSensivity
    {
        get
        {
            return _sliderMultiplier * _mouseSensitivity;
        }
        set
        {
            _mouseSensitivity = value / _sliderMultiplier;
        }
    }
    public static GameObject Instance;

    [Header("Weapon sway params")]
    [SerializeField] private GameObject _hands;
    private float _handsDefaultPosY = 0;
    private Vector3 _handsDefaultLocalPos;
    [SerializeField][Range(0.01f, 1f)] private float _handsSwayMultiplier;
    private void Awake()
    {
        Instance = this.gameObject;
    }
    private void Start()
    {
        _defaultPosY = transform.localPosition.y;
        _origRotation = transform.rotation;

        _handsDefaultPosY = _hands.transform.localPosition.y;
        _handsDefaultLocalPos = _hands.transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (InteractRaycaster.InTabMode)
        {
            // добавить хуй 2 
            return;
        }
        HeadBod();
        _x = Input.GetAxisRaw("Horizontal");
        _y = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        _xRotation -= y;
        _xRotation = Mathf.Clamp(_xRotation, -70f, 70f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, _x);
        _playerBody.Rotate(Vector3.up * x);

        //_hands.transform.localPosition = _handsDefaultLocalPos + x * _hands.transform.right * Time.deltaTime * _handsSwayMultiplier;
        //_hands.transform.localPosition = Vector3.Lerp(_handsDefaultLocalPos,_handsDefaultLocalPos + x * _hands.transform.right * Time.deltaTime * _handsSwayMultiplier,Time.deltaTime);
    }

    private void HeadBod()
    {
        if (_x != 0 || _y != 0)
        {
            _timer += Time.deltaTime * _walkingBobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, _defaultPosY + Mathf.Sin(_timer) * _bobbingAmount, transform.localPosition.z);
            _hands.transform.localPosition = new Vector3(_hands.transform.localPosition.x, _handsDefaultPosY + (1 - Mathf.Cos(_timer)) * _bobbingAmount, _hands.transform.localPosition.z);
        }
        else
        {
            _timer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, _defaultPosY, transform.localPosition.z), Time.deltaTime * _walkingBobbingSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, _origRotation, Time.deltaTime * _walkingBobbingSpeed);
            _hands.transform.localPosition = Vector3.Lerp(_hands.transform.localPosition, new Vector3(_hands.transform.localPosition.x, _handsDefaultPosY, _hands.transform.localPosition.z), Time.deltaTime * _walkingBobbingSpeed);
        }
    }

}
