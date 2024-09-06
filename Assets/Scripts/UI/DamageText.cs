using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public bool IsRightSide;
    public string Text;
    // Компоненты и параметры для изменения цвета текста
    private Text textComponent;  // Компонент текста
    private Color startColor = new Color(1, 0, 0, 0);  // Начальный цвет (красный, полностью прозрачный)
    private Color endColor = Color.black;  // Конечный цвет (черный)

    public float Duration = 1.0f;  // Время жизни текста для изменения цвета
    private float elapsedTime = 0f;  // Время с начала перехода

    void Start()
    {
        textComponent = GetComponent<Text>();
        textComponent.text = Text;

        // Скрываем текст в первый кадр (начальный альфа-канал = 0)
        startColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        textComponent.color = startColor;
    }

    void Update()
    {
        // Увеличиваем прошедшее время
        elapsedTime += Time.deltaTime;

        // Рассчитываем процент завершения (от 0 до 1) для ускоренного перехода цвета
        float t = Mathf.Clamp01(elapsedTime / Duration * 1.5f);

        // Плавно делаем текст видимым и меняем его цвет (включая альфа-канал для плавного появления)
        if (textComponent != null)
        {
            Color currentColor = Color.Lerp(startColor, endColor, t);
            textComponent.color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Min(t * 2, 1));  // Увеличение альфа для плавного появления
        }

        if (elapsedTime < .05)
        {
            transform.localPosition += Vector3.up * .04f;
            transform.localPosition += Vector3.right * (IsRightSide ? -1 : 1) * (.07f - UnityEngine.Random.Range(-.2f, .2f));
        }
        else
        {
            transform.localPosition += Vector3.up * .004f;
        }
    }
}
