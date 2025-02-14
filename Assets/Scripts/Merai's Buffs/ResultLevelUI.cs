using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultLevelUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Text KillsText;
    public Text DamageText;
    public Text TimeText;
    public Text KillsGradeText;
    public Text DamageGradeText;
    public Text TimeGradeText;
    public Text CommonGrade;
    public GameObject ResultsPanel;

    public GameObject BuffPanel;
    public Text BuffText;


    public List<Buffs> BuffsList;

    public GameObject Content;
    private bool _isAnimating = false;

    private void Start()
    {
        GlobalEventManager.OnLevelComplete.AddListener(ShowResults);
    }
    public void ShowResults()
    {
        ResultsPanel.SetActive(true);

        StartCoroutine(AnimateResultsSequentially());
    }

    private IEnumerator AnimateResultsSequentially()
    {
        yield return StartCoroutine(AnimateResult(LevelManager.Instance.AmountOfKills, LevelManager.Instance.killsGrade, KillsText, KillsGradeText, "kills"));
        yield return StartCoroutine(AnimateResult(LevelManager.Instance.ReceivedDamage, LevelManager.Instance.damageGrade, DamageText, DamageGradeText, "damage"));
        yield return StartCoroutine(AnimateResult(LevelManager.Instance.TimeElapsed, LevelManager.Instance.timeGrade, TimeText, TimeGradeText, "time"));
        CommonGrade.text = LevelManager.Instance.CommonGrade.ToString();
        CommonGrade.color = GetGradeColor(LevelManager.Instance.CommonGrade);
        yield return new WaitForSeconds(2f);
        ResultsPanel.SetActive(false);
        StartCalculateBuffs();
    }

    private IEnumerator AnimateResult(float targetValue, char grade, Text valueText, Text gradeText, string type)
    {
        while (_isAnimating)
        {
            yield return null;
        }

        _isAnimating = true;

        if (valueText != null && gradeText != null)
        {
            gradeText.text = "";

            if (targetValue == 0)
            {
                yield return new WaitForSeconds(1f);
                valueText.text = "0";
                yield return new WaitForSeconds(1f);
                gradeText.text = grade.ToString();
                gradeText.color = GetGradeColor(grade);
            }
            else
            {
                float increment = targetValue / 50;

                for (float i = 0; i <= targetValue; i += increment)
                {
                    if (type == "time")
                    {
                        valueText.text = i.ToString("F2");
                    }
                    else
                    {
                        valueText.text = i.ToString("F0");
                    }

                    yield return new WaitForSeconds(0.02f);
                }
                yield return new WaitForSeconds(1f);
                valueText.text = targetValue.ToString(type == "time" ? "F2" : "F0");
                gradeText.text = grade.ToString();
                gradeText.color = GetGradeColor(grade);
            }
        }
        else
        {
            Debug.LogError($"{type}Text или {type}GradeText не назначены в ResultsDisplay!");
        }

        _isAnimating = false;
    }

    private Color GetGradeColor(char grade)
    {
        switch (grade)
        {
            case 'S': return Color.red;
            case 'A': return Color.green;
            case 'B': return Color.blue;
            case 'C': return Color.yellow;
            case 'D': return Color.gray;
            default: return Color.white;
        }
    }

    private void StartCalculateBuffs()
    {
        StartCoroutine(CalculateBuffsAmount());
    }

    private IEnumerator CalculateBuffsAmount()
    {
        BuffPanel.SetActive(true);
        BuffText.text = "Your buffs:\n";

        int numberOfBuffs = 0;

        switch (LevelManager.Instance.CommonGrade)
        {
            case 'S':
                numberOfBuffs = 3;
                break;
            case 'A':
                numberOfBuffs = 2;
                break;
            case 'B':
                numberOfBuffs = 1;
                break;
            default:
                numberOfBuffs = 0;
                break;
        }

        if (numberOfBuffs > 0 && BuffsList != null && BuffsList.Count > 0)
        {
            List<int> selectedIndices = new List<int>();
            List<Buffs> selectedBuffs = new List<Buffs>();

            for (int i = 0; i < numberOfBuffs; i++)
            {
                if (selectedIndices.Count == BuffsList.Count)
                {
                    Debug.LogWarning("Недостаточно разных баффов для создания без повторений.  Уменьшите необходимое количество баффов или добавьте больше различных баффов в BuffsList.");
                    break;
                }

                int randomIndex;
                do
                {
                    randomIndex = UnityEngine.Random.Range(0, BuffsList.Count);
                } while (selectedIndices.Contains(randomIndex)); 

                selectedIndices.Add(randomIndex);
                selectedBuffs.Add(BuffsList[randomIndex]);
            }
            foreach (Buffs buff in selectedBuffs)
            {
                GameObject newBuff = Instantiate(buff.gameObject, Content.transform);
                newBuff.transform.SetParent(Content.transform);

                BuffText.text += "- " + buff.Name + " " + buff.Description + "\n"; 
            }
        }
        else
        {
            BuffText.text = "Didnt get any buffs";
        }
        yield return new WaitForSeconds(3f);
         BuffPanel.SetActive(false);
        BuffText.text = "";
    }
}

