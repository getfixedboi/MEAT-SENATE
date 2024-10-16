using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowItemDescOnUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ItemBehaviour ItemRef;
    public GameObject prefabToShow;  // Префаб, который будет появляться 
    public GameObject instantiatedPrefab;
    public Canvas Canvas;  // Убедитесь, что это ваш Canvas
    public static GameObject prefab;

    [Space]
    [Range(-100, 100)] public float offsetXPercent; // Офсет по X в процентах
    [Range(-100, 100)] public float offsetYPercent; // Офсет по Y в процентах

    private void Start()
    {
        GetComponent<Image>().sprite = ItemRef.GetSprite();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Устанавливаем текст в префабе
        prefabToShow.GetComponentInChildren<Text>().text = ItemRef.GetDesc();

        // Создаем префаб и позиционируем его с учетом смещения
        instantiatedPrefab = Instantiate(prefabToShow, Canvas.transform);

        PositionPrefab(instantiatedPrefab);
        prefab = instantiatedPrefab;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Уничтожаем префаб, когда курсор покидает область
        if (instantiatedPrefab != null)
        {
            prefab = null;
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
                prefab = null;
            }
        }
    }

    private void Update()
    {
        // Обновляем позицию префаба с учетом смещения
        if (instantiatedPrefab != null)
        {
            PositionPrefab(instantiatedPrefab);
        }
    }

    private void PositionPrefab(GameObject prefab)
    {
        // Получаем размеры экрана
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Вычисляем офсет на основе разрешения экрана
        float offsetX = (offsetXPercent / 100f) * screenWidth;
        float offsetY = (offsetYPercent / 100f) * screenHeight;

        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            Canvas.GetComponent<RectTransform>(),
            Input.mousePosition,
            Canvas.worldCamera,
            out worldPoint
        );

        prefab.transform.position = worldPoint + new Vector3(offsetX, offsetY, 0);
    }

    private void OnDestroy()
    {
        Destroy(instantiatedPrefab);
    }
}
