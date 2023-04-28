using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class EventManager
{
    public static event Action OnStartPlay;
    public static void InvokeOnStartPlay() => OnStartPlay?.Invoke();

    public static event Action<int> OnUpdatePlayerLifes;
    public static void InvokeOnUpdatePlayerLifes(int playerLifes) => OnUpdatePlayerLifes?.Invoke(playerLifes);

    public static event Action<int> OnUpdatePlayerPoints;
    public static void InvokeOnUpdatePlayerPoints(int points) => OnUpdatePlayerPoints?.Invoke(points);
}


