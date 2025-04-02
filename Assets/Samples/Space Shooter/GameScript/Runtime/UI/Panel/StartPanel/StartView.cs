using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class StartView : ViewBase
{
    Button _startBtn;
    public override void Init(UIWindow uiBase)
    {
        base.Init(uiBase);
       
        _startBtn = uiBase.transform.Find("StartButton").GetComponent<Button>();
        _startBtn.onClick.AddListener(() =>
        {
            //跳转到第一关 Scene1_1
            SceneEventDefine.Scene1_1.SendEventMessage();
            UIManager.Instance.CloseWindow("StartPanel");
        });
    }
}
