using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
public class InteractRaycaster : MonoBehaviour
{
    [SerializeField][Range(0.1f, 30f)] private float _interactDistance;
    private GameObject _mainCamera;
    private Interactable _interactable;
    private GameObject _currentHit;
    private RaycastHit _hit;
    [SerializeField] private Text _guideText;
    [Header("Tab mode ref")] public UnityEngine.UI.Image TabBG;
    [SerializeField] private UnityEngine.UI.Image _defaultBG;
    public static bool InTabMode = false;
    private bool _canInteracted = true;

    // Переменная для хранения последнего интерактивного объекта
    private Interactable _lastInteractable;

    private void Awake()
    {
        InTabMode = false;
        _mainCamera = this.gameObject;
    }

    private void Start()
    {
        TabBG.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (PauseMenu.IsPaused)
        {
            DisabledTabMode();
            return;
        }
        
        if (Input.GetKey(KeyCode.Tab))
        {
            EnabledTabMode();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            DisabledTabMode();
        }

        // Проверка луча
        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out _hit, _interactDistance))
        {
            _currentHit = _hit.collider.gameObject;
            _interactable = _currentHit.GetComponent<Interactable>();
            
            if (_interactable != null)
            {
                // Сохраняем ссылку на последний интерактивный объект
                if (_lastInteractable != _interactable)
                {
                    // Если это новый объект, вызываем OnFocus
                    _lastInteractable?.OnLoseFocus(); // Вызываем OnLoseFocus для предыдущего объекта
                    _interactable.OnFocus(); // Вызываем OnFocus для нового объекта
                    _guideText.text = _interactable.InteractText;

                    // Обновляем ссылку на последний интерактивный объект
                    _lastInteractable = _interactable;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (_canInteracted)
                    {
                        StartCoroutine(C_InteractCooldown());
                        _interactable.OnInteract();
                    }
                }
            }
            else
            {
                // Если интерактивный объект не найден, сбрасываем текст и вызываем OnLoseFocus
                ResetInteraction();
            }
        }
        else
        {
            // Если луч не попадает ни в один объект, сбрасываем взаимодействие
            ResetInteraction();
        }
    }

    private void ResetInteraction()
    {
        if (_lastInteractable != null)
        {
            // Вызываем OnLoseFocus для последнего интерактивного объекта
            _lastInteractable.OnLoseFocus();
            _lastInteractable = null; // Сбрасываем ссылку на последний интерактивный объект
        }

        _guideText.text = ""; // Сбрасываем текст подсказки
    }

    private void EnabledTabMode()
    {
        if (!MeatBeggar.IsShopping || !PauseMenu.IsPaused)
        {
            InTabMode = true;
            TabBG.gameObject.SetActive(true);

            Color tempColor = _defaultBG.color;
            _defaultBG.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.314f);
        }
    }

    private void DisabledTabMode()
    {
        InTabMode = false;
        TabBG.gameObject.SetActive(false);

        Color tempColor = _defaultBG.color;
        _defaultBG.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.133f);
    }

    private IEnumerator C_InteractCooldown()
    {
        _canInteracted = false;
        yield return new WaitForSeconds(0.5f);
        _canInteracted = true;
    }
}
