using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [HideInInspector] public char timeGrade;
    [HideInInspector] public char damageGrade;
    [HideInInspector] public char killsGrade;
    [HideInInspector] public char CommonGrade;

    [Header("State")]
    public float TimeElapsed;
    public float ReceivedDamage;
    public float AmountOfKills;

    [Header("Conditions")]
    [SerializeField] private float _timeS;
    [SerializeField] private float _damageS;
    [SerializeField] private float _killedS;


    private bool _gameStarted;

    private void Awake()
    {
        GlobalEventManager.OnLevelStart.AddListener(StartGame);
        GlobalEventManager.OnLevelComplete.AddListener(EndGame);
        Instance = this;
    }

    private void Update()
    {
        if (!_gameStarted)
        {
            return;
        }
        TimeElapsed += Time.deltaTime;
    }


    private void StartGame()
    {
        _gameStarted = true;
        TimeElapsed = 0;
    }

    private void CalculateStats()
    {
        float timeCoefficient;

        if (TimeElapsed <= _timeS)
        {
            timeCoefficient = 1;
        }
        else
        {
            timeCoefficient = Mathf.Clamp01(1 - (TimeElapsed / _timeS));
        }

        float damageCoefficient;
        if (ReceivedDamage <= 0)
        {
            damageCoefficient = 1f;
        }
        else
        {
            damageCoefficient = Mathf.Clamp01(1 - (ReceivedDamage / _damageS));
        }


        float killCoefficient = Mathf.Clamp01(AmountOfKills / _killedS);

        damageGrade = GetGrade(damageCoefficient);
        killsGrade = GetGrade(killCoefficient);
        timeGrade = GetGrade(timeCoefficient);

        float averageCoefficient = (damageCoefficient + killCoefficient + timeCoefficient) / 3f;
        CommonGrade = GetGrade(averageCoefficient);

    }

    private void EndGame()
    {
        _gameStarted = false;
        CalculateStats();
    }

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



}






