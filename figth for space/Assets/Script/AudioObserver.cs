using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioObserver
{
    public static event Action<string, float>PlaySFXEvent;
    public static event Action PlayMusicEvent;
    public static event Action StopMusicEvent;
    
        
    public static void OnplaySFXEvent(string obj, float volume)
    {
        PlaySFXEvent?.Invoke(obj, volume);
    }
    public static void OnplayMusicEvent()
    {
        PlayMusicEvent?.Invoke();
    }
    public static void OnStopMusicEvent()
    {
        StopMusicEvent?.Invoke();
    }
}
