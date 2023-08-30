using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManage : MonoBehaviour
{
    public Animator Animator;
    // setting scroll down

   
    bool isclickReversBtnSetting;
    public void ButtonSetting()
    {
        isclickReversBtnSetting = !isclickReversBtnSetting;
        if (isclickReversBtnSetting)
        {
            Animator.Play("setting");

        }
        else if (!isclickReversBtnSetting)
        {
            Animator.Play("settingBack");
        }
    }


}
