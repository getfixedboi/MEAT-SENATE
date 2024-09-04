using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowItemDescOnUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ItemBehaviour ItemRef;
    public GameObject prefabToShow;  // Префаб, который будет появляться 
    private GameObject instantiatedPrefab;
    public Canvas Canvas;  // Убедитесь, что это ваш Canvas

    public Vector3 offset;  // Смещение префаба относительно курсора

    public void Start()
    {
        GetComponent<Image>().sprite = ItemRef.GetSprite();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Устанавливаем текст в префабе
        prefabToShow.GetComponentInChildren<Text>().text = ItemRef.GetDesc();

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
            ItemBehaviour itemBehaviour = ItemRef;
            if (itemBehaviour != null)
            {
                // Вызываем OnDrop() для текущего объекта
                itemBehaviour.OnDrop();
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
