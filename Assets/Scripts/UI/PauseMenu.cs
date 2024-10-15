using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerCameraMovement _cameraMovement;
    [Space]
    [Header("Canvas references")]
    [SerializeField] private Canvas _playerInterface;
    [SerializeField] private Canvas _shopInterface;
    [SerializeField] private GameObject _selfRef;
    [Header("Settings elements references")]
    [SerializeField] private Slider _volumeSlider; // Добавили ссылку на слайдер громкости
    [SerializeField] private Slider _mouseSensitivitySlider; // Добавили ссылку на слайдер чувствительности мыши
    [SerializeField] private Toggle _vsyncToggle;

    public static bool IsPaused;

    private void Awake()
    {
        IsPaused = false;
        _selfRef.gameObject.SetActive(false);
        _volumeSlider.value = AudioListener.volume;
        _mouseSensitivitySlider.value = _cameraMovement.MouseSensivity;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) { return; }

        if (IsPaused)
        {
            ClosePauseMenu();
        }
        else if (!_shopInterface.gameObject.activeInHierarchy)
        {
            OpenPauseMenu();
        }
    }

    private void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        IsPaused = true;

        _playerInterface.gameObject.SetActive(false);
        _selfRef.gameObject.SetActive(true);

    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        _playerInterface.gameObject.SetActive(true);
        _selfRef.gameObject.SetActive(false);

        if (ShowItemDescOnUI.prefab)
        {
            Destroy(ShowItemDescOnUI.prefab);
            ShowItemDescOnUI.prefab = null;
        }
        if (ShowModifierDescOnUI.prefab)
        {
            Destroy(ShowModifierDescOnUI.prefab);
            ShowModifierDescOnUI.prefab = null;
        }
    }


    public void QuitGame()
    {
        ClosePauseMenu();
        Application.Quit(1);
    }

    public void ToggleVSync()
    {
        QualitySettings.vSyncCount = _vsyncToggle.isOn ? 1 : 0;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = _volumeSlider.value;
    }

    public void ChangeMouseSensitivity()
    {
        _cameraMovement.MouseSensivity = _mouseSensitivitySlider.value;
    }
}
