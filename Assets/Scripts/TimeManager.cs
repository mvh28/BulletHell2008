using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static Action changeScene;
    public static int Seconds{get; private set;}
    public static int stageTime = 10;

    private float minuteToRealTime = 1f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        Seconds = -3;
        timer = minuteToRealTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0){
            Seconds++;
            if (Seconds == 0){
                changeScene?.Invoke();
            }
            else if (Seconds >= stageTime){
                changeScene?.Invoke();
                Seconds = 0;
            }
            timer = minuteToRealTime;
        }
    }
}
