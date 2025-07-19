using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    private const string LastQuitTimeKey = "LastQuitTime";
    public TimeSpan elapsedTime;
    public GameScript gameScript;

    void OnApplicationQuit()
    {
        SaveCurrentTime();
    }

    void OnApplicationPause(bool pauseStatus)
    {        
        if (pauseStatus)
        {
            SaveCurrentTime();
        }
    }

    void SaveCurrentTime()
    {
        PlayerPrefs.SetString("cash", gameScript.cashAmount.ToString("F2"));
        DateTime currentTime = DateTime.Now;
        PlayerPrefs.SetString(LastQuitTimeKey, currentTime.ToString());
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("cash"));
    }

    public TimeSpan GetElapsedTime()
    {
        if (PlayerPrefs.HasKey(LastQuitTimeKey))
        {
            string lastQuitTimeString = PlayerPrefs.GetString(LastQuitTimeKey);
            DateTime lastQuitTime = DateTime.Parse(lastQuitTimeString);
            TimeSpan elapsedTime = DateTime.Now - lastQuitTime;
            return elapsedTime;
        }
        else
        {
            return TimeSpan.Zero; // Eğer önceki zaman kaydedilmemişse
        }
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        elapsedTime = GetElapsedTime();
        Debug.Log("Geçen süre: " + elapsedTime.TotalSeconds.ToString("F1") + " saniye.");
    }
}
