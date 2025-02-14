using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTriggers : MonoBehaviour
{
    [SerializeField] private GameObject _startTrigger;
    [SerializeField] private GameObject _endTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _startTrigger)
        {
            GlobalEventManager.SendLevelStart();
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject == _endTrigger)
        {
            GlobalEventManager.SendLevelComplete();
            other.gameObject.SetActive(false);
        }
    }
}
