using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public decimal time;
    private Text timerText;

    void Awake()
    {
        timerText = GetComponent<Text>();//This code will grab and cache a reference to the Text component on the same GameObject the script exists on.
    }

    void Update()
    {
        time = System.Math.Round((decimal)Time.timeSinceLevelLoad, 2);
        timerText.text = time.ToString();
    }
}