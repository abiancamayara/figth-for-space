using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioObserver
{
    public static event Action<string>playSFXEvent;
        
    public static void OnplaySFXEvent(string obj)
    {
        playSFXEvent?.Invoke(obj);
    }
}
