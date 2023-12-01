using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    ///////////////////////////////////////////////
    // Settings
    ///////////////////////////////////////////////

    public GameObject coinsUI;
    public GameObject scalesRegularUI;
    public GameObject scalesFireUI;
    public GameObject scalesWaterUI;
    public GameObject scalesGoldenUI;
    public GameObject timerUI;
    public GameObject queueUI;

    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
        } 
        else 
        { 
            Destroy(this); 
        } 
    }

    public void SetCoins(int coins)
    {
        coinsUI.GetComponent<TextMeshProUGUI>().text = coins.ToString("D4");
    }

    public void SetScales(GameObject obj, int scales)
    {
        obj.GetComponent<TextMeshProUGUI>().text = scales.ToString("D2");
    }

    public void SetScalesRegular(int scales)
    {
        SetScales(scalesRegularUI, scales);
    }

    public void SetScalesFire(int scales)
    {
        SetScales(scalesFireUI, scales);
    }

    public void SetScalesWater(int scales)
    {
        SetScales(scalesWaterUI, scales);
    }

    public void SetScalesGolden(int scales)
    {
        SetScales(scalesGoldenUI, scales);
    }

    public void SetTimer(float percent)
    {
        timerUI.GetComponent<Slider>().value = percent;
    }

    public void SetQueue(List<QueuedDragon> dragons)
    {
        // coinsUI.SetActive(value);
    }

}