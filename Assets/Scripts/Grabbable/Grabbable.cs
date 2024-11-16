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
    private static readonly float _grabDistanceMultiplier = 1.5f;
    private static readonly float _grabDuration = 0.1f;
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
        //ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ ХУЙНЯ 
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

        Vector3 targetPosition = PlayerCameraMovement.Instance.transform.position +
                                 PlayerCameraMovement.Instance.transform.forward * _grabDistanceMultiplier;

        Vector3 startPosition = gameObject.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < _grabDuration)
        {
            float t = elapsedTime / _grabDuration;
            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = targetPosition;
        gameObject.transform.SetParent(PlayerCameraMovement.Instance.transform);
        _isGrabbed = true;
    }

    private void FixedUpdate()
    {
        if (_isGrabbed)
        {
            _rb.velocity = _rbCamera.velocity;
            _rb.angularVelocity = _rbCamera.angularVelocity;

            _rb.velocity = _rbCamera.velocity;
        }
    }

    protected void Throw(bool b = false)
    {
        gameObject.transform.SetParent(null);

        float throwDistance = 6f;

        Vector3 throwDirection = PlayerCameraMovement.Instance.transform.forward;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            if (!b)
            {
                rb.AddForce(throwDirection * throwDistance, ForceMode.Impulse);
            }
        }

        gameObject.GetComponent<Grabbable>()._isGrabbed = false;
        LockShooting = false;
    }
}