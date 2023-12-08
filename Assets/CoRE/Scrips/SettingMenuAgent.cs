using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingMenuAgent : MonoBehaviour
{
    public Volume blackFilterObject;
    public Image[] settingImage;
    public bool isShow;

    public TargetBotController tbc;
    public ROS2.AutoBot autobot;

    void Start()
    {
        isShow = false;
    }
    void toglleShow()
    {
        isShow = !isShow;
        blackFilterObject.enabled = isShow;
        foreach (var item in settingImage)
        {
            if (item != null)
            {
                item.enabled = isShow;
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            toglleShow();
        }
    }

    public void OnClickGoToSim()
    {
        toglleShow();
    }
    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickReset()
    {
        tbc.resetPos();
        autobot.ResetAutoBot();

        toglleShow();
    }
}
