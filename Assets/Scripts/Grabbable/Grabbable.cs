using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

[DisallowMultipleComponent]
public class Grabbable : Interactable
{
    public static bool LockShooting = false;
    private bool _isGrabbed = false;
    private static readonly float _grabDistanceMultiplier = 1.6f;
    private static readonly float _grabDuration = 0.1f; // Время, за которое объект будет подбираться
    private Rigidbody _rb;
    private Rigidbody _rbCamera;
    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _rbCamera = PlayerCameraMovement.Instance.GetComponent<Rigidbody>();
    }
    public override sealed void OnFocus()
    {
        //Empty
    }

    public override sealed void OnInteract()
    {
        if (_isGrabbed)
        {
            Throw();
        }
        else
        {
            StartCoroutine(Grab());
        }
    }

    public override sealed void OnLoseFocus()
    {
        Throw(true);
    }

    protected IEnumerator Grab()
    {
        LockShooting = true;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Определяем целевую позицию для подбора
        Vector3 targetPosition = PlayerCameraMovement.Instance.transform.position +
                                 PlayerCameraMovement.Instance.transform.forward * _grabDistanceMultiplier;

        // Текущее положение объекта
        Vector3 startPosition = gameObject.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < _grabDuration)
        {
            // Вычисляем интерполяцию
            float t = elapsedTime / _grabDuration;
            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Ждем до следующего кадра
        }

        // Устанавливаем окончательную позицию и флаг схваченности
        gameObject.transform.position = targetPosition;
        gameObject.transform.SetParent(PlayerCameraMovement.Instance.transform);
        _isGrabbed = true;
    }

    private void FixedUpdate()
    {
        if (_isGrabbed)
        {
			_rb.velocity = _rbCamera.velocity;
            // transform.position = PlayerCameraMovement.Instance.transform.position +
            //                      PlayerCameraMovement.Instance.transform.forward * _grabDistanceMultiplier;
        }
    }

    protected void Throw(bool b = false)
    {
        gameObject.transform.SetParent(null);
        // Определяем расстояние, на которое выкинем объект
        float throwDistance = 6f; // Вы можете изменить это значение по своему усмотрению

        // Вычисляем направление броска
        Vector3 throwDirection = PlayerCameraMovement.Instance.transform.forward;

        // Применяем силу к объекту для броска
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            if (!b)
            {
                rb.AddForce(throwDirection * throwDistance, ForceMode.Impulse);
            }
        }

        // Устанавливаем флаг, что объект больше не схвачен
        gameObject.GetComponent<Grabbable>()._isGrabbed = false;
        LockShooting = false;
    }
}