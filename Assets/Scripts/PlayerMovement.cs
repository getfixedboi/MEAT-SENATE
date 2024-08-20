//namespace Player
//{
//    using System.Collections;
//    using System.Collections.Generic;
//    using UnityEngine;
//    using UnityEngine.EventSystems;

//    [RequireComponent(typeof(CharacterController))]
//    public class PlayerMovement : MonoBehaviour
//    {
//        public static PlayerMovement Instance;

//        [SerializeField] private Camera _playerCamera;

//        private CharacterController _characterController;
//        private float _defaultYPos;
//        private float _defaultFOV;
//        private float _runFOV;

//        private bool _preJump;

//        private float _preJumpCancelTimer;


//        [SerializeField] private Vector3 _offset;

//        private Quaternion _initialRotation;


//        [SerializeField] private float _jumpTimer;

//        private bool _inJump;


//        private Vector3 _moveDirection;


//        private float _airTime;
//        private float _fallTime;


//        private float _headbobEndTimer;

//        private float _aftervaultjumpTimer;



//        private Vector2 _currentInputRaw;

//        private bool _canSprint;

//        [SerializeField]
//        private float _gravity = 30f;
//        private float _zoomSpeed;

//        public bool IsSprinting
//        {
//            get
//            {
//                return _canSprint && (Input.GetAxis("Sprint") > 0.4f) && _currentInputRaw != new Vector2(0f, 0f);
//            }
//        }


//        private void Awake()
//        {
//            Instance = this;
//            _playerCamera = GetComponentInChildren<Camera>();
//            _characterController = GetComponent<CharacterController>();

//            _defaultYPos = _characterController.center.y + _characterController.height / 2f + _offset.y;
//            _defaultFOV = _playerCamera.fieldOfView;

//            Cursor.lockState = CursorLockMode.Locked;
//            Cursor.visible = false;

//            _initialRotation = _playerCamera.transform.rotation;
//        }

//        private void Update()
//        {
//            RaycastHit raycastHit;
//            if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, 1.3f) && (Input.GetButtonDown("Jump") && _characterController.velocity.y < 0f))
//            {
//                _preJump = true;
//                _preJumpCancelTimer = 0.4f;
//            }

//            if (!_characterController.isGrounded)
//            {
//                _airTime -= Time.deltaTime;
//                if (_characterController.velocity.y > 0f)
//                {
//                    _fallTime = Mathf.Lerp(_fallTime, 0f, 6f * Time.deltaTime);
//                }
//                else if (_characterController.velocity.y < 0f && _airTime < -0.6f)
//                {
//                    _fallTime += Time.deltaTime;
//                }
//            }
//            else
//            {
//                _airTime = 0.3f;
//                _fallTime = Mathf.Lerp(_fallTime, 0f, 6f * Time.deltaTime);
//            }

//            _headbobEndTimer -= Time.deltaTime;
//            _aftervaultjumpTimer -= Time.deltaTime;
//             _preJumpCancelTimer -= Time.deltaTime;

//            HandleCameraController();
//            this.HandleMouseLook();

//            this.CalculateMovementInput();
//            this.HandleAddingForce();


//            if (this.canMove)
//            {
//                this.HandleMovementInput();
//                if (this.canJump)
//                {
//                    this.HandleJump();
//                }
//                if (this.canUseHeadbob && this.standing)
//                {
//                    this.HandleHeadbob();
//                }
//                ApplyFinalMovements();

//                if (_characterController.isGrounded)
//                {
//                   _moveDirection.y = -0.5f;
//                }

//                if (_characterController.isGrounded)
//                {
//                    _inJump = false;
//                }

//            }
//            _jumpTimer -= Time.deltaTime;
//            if ((Input.GetButtonDown("Jump")) && _characterController.isGrounded)
//            {
//                _jumpTimer = 0.3f;
//            }
//        }
//        private void ApplyFinalMovements()
//        {
//            if (!_characterController.isGrounded)
//            {
//                _moveDirection.y = _moveDirection.y - _gravity * Time.deltaTime;
//            }
//            _characterController.Move(_moveDirection * Time.deltaTime);
//        }

//        private void HandleCameraController()
//        {
//            // Устанавливаем поле зрения в зависимости от состояния бега
//            float num = IsSprinting ? _runFOV : _defaultFOV;

//            if (_playerCamera.fieldOfView != num)
//            {
//                Zoom(num);
//            }

//            // Устанавливаем наклон камеры в зависимости от состояния бега и горизонтального ввода
//            float temptiltAmount = (IsSprinting && this.standing && this.horizontalInputRaw == 1f) ? (-this.tiltAmount) :
//                                   (IsSprinting && this.standing && this.horizontalInputRaw == -1f) ? this.tiltAmount : 0f;

//            if (this.camController)
//            {
//                SideTilt(temptiltAmount);
//            }
//        }

//        private void Zoom(float fov)
//        {
//            _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, fov, _zoomSpeed * Time.deltaTime);
//        }
//        private void SideTilt(float temptiltAmount)
//        {
//            float b = Mathf.Clamp(-Input.GetAxis("Mouse X") * this.rotationAmount, -this.maxRotationAmount, this.maxRotationAmount);
//            this.rotationZ = Mathf.Lerp(this.rotationZ, temptiltAmount, this.tiltSpeed * Time.deltaTime) + Mathf.Lerp(base.transform.localRotation.z, b, Time.deltaTime * this.smoothRotation);
//        }
//    }

//}
