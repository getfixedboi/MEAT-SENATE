using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private GameObject _player;
    private float _distanceToPlayer;
    public bool IsRightSide;
    public string Text;
    // Компоненты и параметры для изменения цвета текста
    private Text _textComponent;  // Компонент текста
    private Color _startColor = new Color(1, 0, 0, 0);  // Начальный цвет (красный, полностью прозрачный)
    private Color _endColor = Color.black;  // Конечный цвет (черный)

    public float Duration = 1.0f;  // Время жизни текста для изменения цвета
    private float _elapsedTime = 0f;  // Время с начала перехода
    //text spread
    private float _textRushTime;

    private float _yRushMultliplier;
    private float _xRushMultliplier;
    private float _yCalmMultliplier;

    private float _yRushRngMultliplier;
    private float _xRushRngMultliplier;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
        _textComponent = GetComponent<Text>();
        _textComponent.text = Text;
        _startColor = new Color(_startColor.r, _startColor.g, _startColor.b, 0);
        _textComponent.color = _startColor;
        SpreadParams();
    }

    void FixedUpdate()
    {
        // Увеличиваем прошедшее время
        _elapsedTime += Time.deltaTime;

        // Рассчитываем процент завершения (от 0 до 1) для ускоренного перехода цвета
        float t = Mathf.Clamp01(_elapsedTime / Duration * 1.5f);

        // Плавно делаем текст видимым и меняем его цвет (включая альфа-канал для плавного появления)
        if (_textComponent != null)
        {
            Color currentColor = Color.Lerp(_startColor, _endColor, t);
            _textComponent.color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Min(t * 2, 1));  // Увеличение альфа для плавного появления
        }

        //text spread
        if (_elapsedTime < _textRushTime)
        {
            transform.localPosition += Vector3.up * _yRushMultliplier * (0.05f - UnityEngine.Random.Range(-_yRushRngMultliplier, _yRushRngMultliplier));
            transform.localPosition += Vector3.right * _xRushMultliplier * (IsRightSide ? -1 : 1) * (0.05f - UnityEngine.Random.Range(-_xRushRngMultliplier, _xRushRngMultliplier));
        }
        else
        {
            transform.localPosition += Vector3.up * _yCalmMultliplier;
        }
    }

    private void SpreadParams()
    {
        if (_distanceToPlayer < 2f) // 1
        {
            _textComponent.fontSize = 4;

            _textRushTime = 0.15f;

            _xRushMultliplier = 0.45f; // Увеличено на дополнительные 50%
            _yRushMultliplier = 0.9f; // Увеличено на дополнительные 50%
            _yCalmMultliplier = 0.0045f; // Увеличено на дополнительные 50%

            _yRushRngMultliplier = 0.02925f; // Увеличено на дополнительные 50%
            _xRushRngMultliplier = 0.3375f; // Увеличено на дополнительные 50%
        }
        else if (_distanceToPlayer >= 2f && _distanceToPlayer <= 4f) // 2
        {
            _textComponent.fontSize = 5;

            _textRushTime = 0.15f;

            _xRushMultliplier = 1.0125f; // Увеличено на дополнительные 50%
            _yRushMultliplier = 1.35f; // Увеличено на дополнительные 50%
            _yCalmMultliplier = 0.00675f; // Увеличено на дополнительные 50%

            _yRushRngMultliplier = 0.03375f; // Увеличено на дополнительные 50%
            _xRushRngMultliplier = 0.3375f; // Увеличено на дополнительные 50%
        }
        else if (_distanceToPlayer > 4f && _distanceToPlayer <= 5f) // 3
        {
            _textComponent.fontSize = 6;

            _textRushTime = 0.15f;

            _xRushMultliplier = 1.125f; // Увеличено на дополнительные 50%
            _yRushMultliplier = 1.575f; // Увеличено на дополнительные 50%
            _yCalmMultliplier = 0.009f; // Увеличено на дополнительные 50%

            _yRushRngMultliplier = 0.0405f; // Увеличено на дополнительные 50%
            _xRushRngMultliplier = 0.405f; // Увеличено на дополнительные 50%
        }
        else /////////////////////////////////////////////////////////////4
        {
            _textComponent.fontSize = 9;

            _textRushTime = 0.15f;

            _xRushMultliplier = 1.35f; // Увеличено на дополнительные 50%
            _yRushMultliplier = 1.8f; // Увеличено на дополнительные 50%
            _yCalmMultliplier = 0.009f; // Увеличено на дополнительные 50%

            _yRushRngMultliplier = 0.045f; // Увеличено на дополнительные 50%
            _xRushRngMultliplier = 0.45f; // Увеличено на дополнительные 50%
        }
    }
}
