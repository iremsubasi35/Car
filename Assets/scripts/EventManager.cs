using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static UnityAction OnStartGame;
    public static UnityAction OnStopGame;

    public static UnityAction OnPlayerFinish;
}