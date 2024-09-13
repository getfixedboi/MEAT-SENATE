using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowModifierDescOnUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ModifierBehaviour ModRef;
    public GameObject prefabToShow;  // Префаб, который будет появляться 
    private GameObject instantiatedPrefab;
    public Canvas Canvas;  // Убедитесь, что это ваш Canvas

    public Vector3 offset;  // Смещение префаба относительно курсора

    public void Start()
    {
        GetComponent<Image>().sprite = ModRef.GetSprite();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Устанавливаем текст в префабе
        prefabToShow.GetComponentInChildren<Text>().text = ModRef.GetDesc();

        // Создаем префаб и позиционируем его с учетом смещения
        instantiatedPrefab = Instantiate(prefabToShow, Canvas.transform);

        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            Canvas.GetComponent<RectTransform>(),
            Input.mousePosition,
            Canvas.worldCamera,
            out worldPoint
        );
        instantiatedPrefab.transform.position = worldPoint + offset;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Уничтожаем префаб, когда курсор покидает область
        if (instantiatedPrefab != null)
        {
            Destroy(instantiatedPrefab);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Проверяем, была ли нажата правая кнопка мыши
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ModifierBehaviour modBehaviour = ModRef;
            if (modBehaviour != null)
            {
                // Вызываем OnDrop() для текущего объекта
                modBehaviour.OnDrop();
            }
        }

        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            if (PlayerStatictics.CurrentModifier)
            {
                if (PlayerStatictics.CurrentModifier != this)
                {
                    PlayerStatictics.CurrentModifier.GetComponentInChildren<UnityEngine.UI.Text>().enabled = false;
                    PlayerStatictics.CurrentModifier.ModRef.DetachProjectileEffect();
                    PlayerStatictics.CurrentModifier = this;
                    PlayerStatictics.CurrentModifier.GetComponentInChildren<UnityEngine.UI.Text>().enabled = true;
                }
                else
                {
                    PlayerStatictics.CurrentModifier.GetComponentInChildren<UnityEngine.UI.Text>().enabled = false;
                    PlayerStatictics.CurrentModifier.ModRef.DetachProjectileEffect();
                    PlayerStatictics.CurrentModifier = null;
                }
            }
            else
            {
                PlayerStatictics.CurrentModifier = this;
                PlayerStatictics.CurrentModifier.GetComponentInChildren<UnityEngine.UI.Text>().enabled = true;
            }
        }
    }

    void Update()
    {
        // Обновляем позицию префаба с учетом смещения
        if (instantiatedPrefab != null)
        {
            Vector3 worldPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                Canvas.GetComponent<RectTransform>(),
                Input.mousePosition,
                Canvas.worldCamera,
                out worldPoint
            );
            instantiatedPrefab.transform.position = worldPoint + offset;
        }
    }

    private void OnDestroy()
    {
        Destroy(instantiatedPrefab);
    }
}
