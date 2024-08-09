using UnityEngine;

[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
public class InteractRaycaster : MonoBehaviour
{
    [SerializeField][Range(0.1f, 30f)] private float _interactDistance;
    private GameObject _mainCamera;
    private Interactable _interactable;
    private GameObject _currentHit;
    private RaycastHit _hit;
    private void Awake()
    {
        _mainCamera = this.gameObject;
    }
    private void Update()
    {
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

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        _interactable.OnInteract();
                    }
                }
            }
            else
            {
                _interactable?.OnLoseFocus();
                _interactable = null;
            }
        }
        else
        {
            _interactable?.OnLoseFocus();
            _interactable = null;
        }
    }
}