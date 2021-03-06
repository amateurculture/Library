﻿using UnityEngine;

public class TimeController : MonoBehaviour
{
    [HideInInspector] public LightingController lightingController;
    [HideInInspector] FogController fogController;
    
    public float year;
    public float day;
    public float hour;
    public float minute;
    [Space()]
    public float daysInYear;
    public float hoursInDay;
    public float secondsInHour;
    //public int frameRate;

    [Space()]
    public bool stopTime;

    [HideInInspector] float planetaryRotation;
    float currentHour;
    float currentMinute;
    float gameTime;
    float adjustedSecondsInHour;
    float secondsRemainingInMinute;
    bool EndOfDay;
    bool nextDay;

    private void Reset()
    {
        year = 2000;
        day = 1;
        hour = currentHour = 8;
        minute = currentMinute = 0;

        //frameRate = 60;
        daysInYear = 365;
        hoursInDay = 24;
        secondsInHour = 60;

        lightingController = GetComponent<LightingController>();
        fogController = GetComponent<FogController>();
    }

    private void Start()
    {
        lightingController = GetComponent<LightingController>();
        fogController = GetComponent<FogController>();

        planetaryRotation = 15f * ((hour + (minute / 60f)) - 6f);
        gameTime = hour + (minute / 60f);
        secondsRemainingInMinute = Time.time + (secondsInHour / 60);
        adjustedSecondsInHour = secondsInHour / 60;
        currentHour = hour;
        currentMinute = minute;

        RenderSettings.sun.transform.eulerAngles = Vector3.zero;
        RenderSettings.sun.transform.Rotate(new Vector3(15f * ((hour + (minute / 60f)) - 6f), 0, 0));

        UpdateTime();
        UpdateLighting();
        UpdateFog();
    }

    public float GetSecondsInHour()
    {
        return secondsInHour;
    }

    void Update()
    {
        if (RenderSettings.sun != null && RenderSettings.sun.transform.gameObject.activeSelf != false)
        {
            if (!stopTime && Time.time > secondsRemainingInMinute)
            {
                if (hour != currentHour)
                {
                    gameTime += (hour - currentHour) * .0166f;
                    currentHour = hour;
                }
                if (minute != currentMinute)
                {
                    gameTime += (minute - currentMinute) * .0166f;
                    currentMinute = minute;
                }

                secondsRemainingInMinute = Time.time + adjustedSecondsInHour;
                UpdateTime();
                UpdateLighting();
                UpdateFog();
            }
            else if (stopTime && Time.frameCount % 3 == 0) {
                planetaryRotation = 15f * ((hour + (minute / 60f)) - 6f);
                gameTime = hour + (minute / 60f);
                secondsRemainingInMinute = Time.time + (secondsInHour / 60);
                adjustedSecondsInHour = secondsInHour / 60;
                currentHour = hour;
                currentMinute = minute;

                RenderSettings.sun.transform.eulerAngles = Vector3.zero;
                RenderSettings.sun.transform.Rotate(new Vector3(15f * ((hour + (minute / 60f)) - 6f), 0, 0));
            }
        }
    }

    void UpdateTime()
    {
        gameTime += .0166f;
        hour = ((int)gameTime) % hoursInDay;
        minute = ((int)(60f * (gameTime - ((int)gameTime))));
        currentHour = hour;
        currentMinute = minute;
        planetaryRotation += .25f;
        planetaryRotation %= 360;

        if (planetaryRotation > 270 && planetaryRotation < 280) 
            EndOfDay = true;
        else
        {
            nextDay = false;
            EndOfDay = false;
        }

        if (EndOfDay && !nextDay)
        {
            nextDay = true;
            day ++;
        }

        if (day > daysInYear) {
            day = 1;
            year ++;
        }
    }

    void UpdateLighting() { 
        RenderSettings.sun.transform.Rotate(.25f, 0, 0);
        if (lightingController != null) 
            lightingController.UpdateLighting(); 
    }

    void UpdateFog() { if (fogController != null) fogController.UpdateFogColor(); }
}
