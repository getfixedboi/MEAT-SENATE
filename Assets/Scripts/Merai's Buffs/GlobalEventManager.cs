using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager : MonoBehaviour
{
    public static UnityEvent OnLevelComplete = new UnityEvent();
    public static UnityEvent OnLevelStart = new UnityEvent();

    public static void SendLevelComplete()
    {
        OnLevelComplete.Invoke();
    }
    public static void SendLevelStart()
    {
        OnLevelStart.Invoke();

    }
}
