using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarksManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabsForBuffs;


    private char GetGrade(float coefficient) // .|.
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
