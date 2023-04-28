using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class EventManager
{
    public static event Action OnStartGame;
    public static void InvokeOnStartCounting() => OnStartGame?.Invoke();
}
