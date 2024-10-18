using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

[DisallowMultipleComponent]
public abstract class Grabbable : Interactable
{
    public static bool LockShooting = false;
    protected bool isGrabbed = false;
    private static readonly float grabDistanceMultiplier = 1.6f;
    private static readonly float grabDuration = 0.1f; // Время, за которое объект будет подбираться
    private static GameObject _holdedObj = null;

    public override sealed void OnFocus()
    {
        
    }

    public override sealed void OnInteract()
    {
        if (isGrabbed)
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
        Throw(_holdedObj, true);
    }

    protected IEnumerator Grab()
    {
        LockShooting = true;
        _holdedObj = this.gameObject;

        gameObject.GetComponent<Rigidbody>().useGravity = false;
        // Определяем целевую позицию для подбора
        Vector3 targetPosition = PlayerCameraMovement.Instance.transform.position +
                                 PlayerCameraMovement.Instance.transform.forward * grabDistanceMultiplier;

        // Текущее положение объекта
        Vector3 startPosition = gameObject.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < grabDuration)
        {
            // Вычисляем интерполяцию
            float t = elapsedTime / grabDuration;
            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Ждем до следующего кадра
        }

        // Устанавливаем окончательную позицию и флаг схваченности
        gameObject.transform.position = targetPosition;
        gameObject.transform.SetParent(PlayerCameraMovement.Instance.transform);
        isGrabbed = true;
    }

    protected void Throw(GameObject g = null, bool b = false)
    {
        GameObject obj;
        if (g)
        {
            obj = g;
        }
        else
        {
            g = this.gameObject;
        }

        _holdedObj = null;
        g.transform.SetParent(null);
        // Определяем расстояние, на которое выкинем объект
        float throwDistance = 30f; // Вы можете изменить это значение по своему усмотрению

        // Вычисляем направление броска
        Vector3 throwDirection = PlayerCameraMovement.Instance.transform.forward;

        // Применяем силу к объекту для броска
        Rigidbody rb = g.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            if (!b)
            {
                rb.AddForce(throwDirection * throwDistance, ForceMode.Impulse);
            }
        }

        // Устанавливаем флаг, что объект больше не схвачен
        g.GetComponent<Grabbable>().isGrabbed = false;
        LockShooting = false;
    }
}
