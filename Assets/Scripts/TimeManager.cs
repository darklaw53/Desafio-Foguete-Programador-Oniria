using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public AudioSource audioC;

    public void Pause()
    {
        Time.timeScale = 0;
        audioC.pitch = 0;
    }
    
    public void Play()
    {
        Time.timeScale = 1;
        audioC.pitch = 1;
    }

    public void TimesTwo()
    {
        Time.timeScale = 2;
        audioC.pitch = 2;
    }

    public void TimesFour()
    {
        Time.timeScale = 4;
        audioC.pitch = 4;
    }
}
