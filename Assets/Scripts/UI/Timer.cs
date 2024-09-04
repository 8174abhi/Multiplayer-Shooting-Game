using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class Timer : MonoBehaviour
{
    public TMP_Text timertext;
    float TimeRemaining = 600f;
    void UpdateTimerText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(TimeRemaining);
        timertext.text = string.Format("{0:D2}:{1:D2}",timeSpan.Minutes,timeSpan.Seconds);  
    }
    private void Update()
    {
        if(TimeRemaining>0)
        {
            TimeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            TimeRemaining = 0;
        }
    }
    public void QuitGame()
    {
        Application.Quit();

        Cursor.visible = true;
    }
}
