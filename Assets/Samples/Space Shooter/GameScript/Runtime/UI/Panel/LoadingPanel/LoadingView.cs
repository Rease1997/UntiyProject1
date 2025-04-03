using System;
using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using YooAsset;

public class LoadingView : ViewBase
{
    bool isCountDown = false;//是否开启倒计时
    float countDown;//倒计时
    bool isFirst;//是否第一次生成玩家 不是第一次的话复活玩家
    int hp = 3;
    public override void Init(UIWindow uiBase)
    {
        base.Init(uiBase);
        isFirst = true;
        hp = 3;
    }

    public void Init()
    {
        if (!isFirst) return;
        //生成玩家
        CreatePlayer();
        GameManager.Instance._eventGroup.AddListener<PlayerManager>(OpenEventMessage);
        UIManager.Instance.CloseWindow("LoadingPanel");
        isFirst = false;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        isCountDown = true;
        countDown = 3;
    }

    private void OpenEventMessage(IEventMessage message)
    {
        Debug.Log("打开LoadingPanel界面");
        UIManager.Instance.OpenWindow("LoadingPanel");
        hp--;
        Debug.Log(hp + "  当前血量");
    }

    /// <summary>
    /// 玩家生成并附加脚本
    /// </summary>
    private static void CreatePlayer()
    {
        GameObject play = YooAssets.LoadAssetSync<GameObject>("Player").AssetObject as GameObject;
        GameObject player = GameObject.Instantiate(play);
        player.AddComponent<PlayerManager>();
    }

    public override void Update()
    {
        base.Update();
        if (isCountDown)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0)
            {
                isCountDown = false;
                if (isFirst)
                    Init();
                else
                {
                    if(hp > 0)
                    {
                        //关闭LoadingPanel界面
                        UIManager.Instance.CloseWindow("LoadingPanel");
                        //再开一次 初始化玩家位置
                    }
                    else
                    {
                        Debug.Log("死翘翘了");
                        //回到主场景
                    }
                }
            }
        }
    }
}
