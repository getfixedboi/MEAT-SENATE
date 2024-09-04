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
    [Header("Tab mode ref")][SerializeField] private UnityEngine.UI.Image _tabBG;
    [SerializeField] private UnityEngine.UI.Image _defaultBG;
    private void Awake()
    {
        _mainCamera = this.gameObject;
        _tabBG.enabled = false;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            EnabledTabMode();
        }
        else
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
                        _interactable.OnInteract();
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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _tabBG.enabled = true;

        Color tempColor = _defaultBG.color;
        _defaultBG.color = new Color(tempColor.r,tempColor.g,tempColor.b,0.314f);
    }
    private void DisabledTabMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _tabBG.enabled = false;

        Color tempColor = _defaultBG.color;
        _defaultBG.color = new Color(tempColor.r,tempColor.g,tempColor.b,0.133f);
    }
}
