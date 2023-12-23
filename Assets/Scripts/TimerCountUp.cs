using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountUp : MonoBehaviour
{
    Text timerText;

    // Start call hoy first frame update er agea
    void Start()
    {
        timerText = GetComponentInChildren<Text>();
    }

    // Update call hoy once per frame
    void Update()
    {
        float t = Time.timeSinceLevelLoad;
        int seconds = (int)(t % 60);
        t /= 60;
        int minutes = (int)(t % 60);

        timerText.text = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00") );

    }
}
