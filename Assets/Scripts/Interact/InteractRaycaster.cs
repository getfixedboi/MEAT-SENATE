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

        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out _hit, _interactDistance))
        {
            _currentHit = _hit.collider.gameObject;
            _interactable = _currentHit.GetComponent<Interactable>();

            if (_interactable != null)
            {
                if (_interactable.IsLastInteracted)
                {
                    _interactable = null;
                }
                else
                {
                    _interactable.OnFocus();
                    _guideText.text = _interactable.InteractText;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (_canInteracted)
                        {
                            StartCoroutine(C_InteractCooldown());
                            _interactable.OnInteract();
                        }
                    }
                }
            }
            else
            {
                _guideText.text = "";
                _interactable?.OnLoseFocus();
                _interactable = null;
            }
        }
        else
        {
            _guideText.text = "";
            _interactable?.OnLoseFocus();
            _interactable = null;
        }
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
