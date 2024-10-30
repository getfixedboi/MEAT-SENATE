using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private PlayerStatictics _playerStats;
    [SerializeField] private GameObject _startTrigger;
    [SerializeField] private GameObject _endTrigger;

    private static float _receivedDamage;
    public static float ReceivedDamage
    {
        get
        {
            return _receivedDamage;
        }
        set
        {
            if (_gameStarted && !_gameEnded)
            {
                _receivedDamage = value;
            }
        }
    }

    private static float _killCount;
    public static float KillCount
    {
        get
        {
            return _killCount;
        }
        set
        {
            if (_gameStarted && !_gameEnded)
            {
                _killCount = value;
            }
        }
    }

    private static float _timeElapsed;
    public static float TimeElapsed
    {
        get
        {
            return _timeElapsed;
        }
        set
        {
            if (_gameStarted && !_gameEnded)
            {
                _timeElapsed = value;
            }
        }
    }

    private static bool _gameStarted = false;
    private static bool _gameEnded = false;
    [Space]
    [SerializeField] private float _maxReceivedDamage;
    [SerializeField] private float _maxKillCount;
    [SerializeField] private float _maxTime;
    [SerializeField] private float _timeSGrade;

    [SerializeField] private UnityEngine.UI.Text _buffTextOutput;

    private List<Tuple<string, Action>> _lowTierBuffs = new List<Tuple<string, Action>>();
    private List<Tuple<string, Action>> _midTierBuffs = new List<Tuple<string, Action>>();
    private List<Tuple<string, Action>> _highTierBuffs = new List<Tuple<string, Action>>();

    private static List<Tuple<string, Action>> _playerBuffs = new List<Tuple<string, Action>>();

    private void Awake()
    {
        _playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStatictics>(); // бля сам ты тупл

        _lowTierBuffs.Add(new Tuple<string, Action>("+5 мясных кусочков", () => PlayerSkills.MeatPieceCount += 5));

        _midTierBuffs.Add(new Tuple<string, Action>("Увеличена скорость снаряда", () => UnityEngine.Debug.Log("aboba")));

        _highTierBuffs.Add(new Tuple<string, Action>("Полное лечение", () => _playerStats.Heal(1000)));
    }

    private void Update()
    {
        if (!_gameStarted) { return; }
        _timeElapsed += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, в какой триггер вошел игрок
        if (other.gameObject == _startTrigger)
        {
            _gameStarted = true;
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject == _endTrigger)
        {
            _gameEnded = true;
            other.gameObject.SetActive(false);
            ShowResult();
        }
    }

    private void ShowResult()
    {
        // Рассчитываем коэффициенты от 0 до 1 для каждого параметра
        float damageCoefficient = Mathf.Clamp01(1 - (ReceivedDamage / _maxReceivedDamage));
        float killCoefficient = Mathf.Clamp01(KillCount / _maxKillCount);
        float timeCoefficient = Mathf.Clamp01(1 - (TimeElapsed / _maxTime));

        if (TimeElapsed <= _timeSGrade)
        {
            timeCoefficient = 1;
        }
        else
        {
            timeCoefficient = Mathf.Clamp01(1 - (TimeElapsed / _maxTime));
        }

        // Присваиваем оценку в зависимости от коэффициента
        char damageGrade = GetGrade(damageCoefficient);
        char killGrade = GetGrade(killCoefficient);
        char timeGrade = GetGrade(timeCoefficient);

        // Рассчитываем средний коэффициент для общей оценки
        float averageCoefficient = (damageCoefficient + killCoefficient + timeCoefficient) / 3f;
        char totalGrade = GetGrade(averageCoefficient);

        // Вывод в консоль сначала истинного значения, затем оценки
        UnityEngine.Debug.Log($"Полученный урон: {ReceivedDamage}, Оценка за урон: {damageGrade}");
        UnityEngine.Debug.Log($"Количество убийств: {KillCount}, Оценка за убийства: {killGrade}");
        UnityEngine.Debug.Log($"Время: {TimeElapsed}, Оценка за время: {timeGrade}");
        UnityEngine.Debug.Log($"Общая оценка: {totalGrade}");

        if (averageCoefficient == 1) // Высший тир (S)
        {
            AddBuffFromTier(_highTierBuffs);
        }
        else if (averageCoefficient > 0.6f) // Средний тир (A или B)
        {
            AddBuffFromTier(_midTierBuffs);
        }
        else // Низкий тир (C и ниже)
        {
            AddBuffFromTier(_lowTierBuffs);
        }
        DisplayPlayerBuffs();
    }

    private void AddBuffFromTier(List<Tuple<string, Action>> tierBuffs)
    {
        // Выбираем случайный бафф из указанного тира
        var randomIndex = UnityEngine.Random.Range(0, tierBuffs.Count);
        var selectedBuff = tierBuffs[randomIndex];

        // Добавляем бафф к списку баффов игрока
        _playerBuffs.Add(selectedBuff);

        // Выводим описание выбранного баффа в консоль и применяем его действие
        UnityEngine.Debug.Log($"Игрок получил бафф: {selectedBuff.Item1}");
        selectedBuff.Item2.Invoke(); // Применение баффа
    }
    // Функция, возвращающая оценку в зависимости от коэффициента
    private char GetGrade(float coefficient)
    {
        if (coefficient >= 1f)
            return 'S';
        else if (coefficient >= 0.75f)
            return 'A';
        else if (coefficient >= 0.5f)
            return 'B';
        else if (coefficient >= 0.25f)
            return 'C';
        else
            return 'D';
    }
    private void DisplayPlayerBuffs()
    {
        // Очищаем текстовое поле перед выводом
        _buffTextOutput.text = "";

        // Проходим по всем баффам игрока и добавляем описание каждого через пустую строку
        foreach (var buff in _playerBuffs)
        {
            _buffTextOutput.text += buff.Item1 + "\n\n"; // Добавляем описание баффа с отступом
        }
    }
}
