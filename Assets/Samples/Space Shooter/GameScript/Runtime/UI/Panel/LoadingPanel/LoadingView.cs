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
    int hp;
    GameObject player;
    BoxCollider2D playercollider;
    Vector3 playerPos;
    PlayerManager playermanager;
    EdgeCollider2D edgeplayercollider;
    bool _isonEnable = false;
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
        _isonEnable = true;
    }

    int count = 0;
    public override void OnEnable()
    {
        base.OnEnable();
        isCountDown = true;
        countDown = 3;
        if (_isonEnable)
        {
            hp--;
            Debug.Log(hp + "  当前血量");
        }
        count++;
        Debug.Log("计算" + count);
    }

    private void OpenEventMessage(IEventMessage message)
    {
        Debug.Log("打开LoadingPanel界面");
        UIManager.Instance.OpenWindow("LoadingPanel");
    }

    /// <summary>
    /// 玩家生成并附加脚本
    /// </summary>
    private void CreatePlayer()
    {
        GameObject play = YooAssets.LoadAssetSync<GameObject>("Player").AssetObject as GameObject;
        player = GameObject.Instantiate(play);
        //获取玩家生成时的位置
        playerPos = player.transform.localPosition;
        playermanager = player.AddComponent<PlayerManager>();
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
                        //在这让玩家复活 还原玩家的初始位置
                        player.transform.position = playerPos;
                        playermanager.openAllCollider();
                    }
                    else 
                    {
                        //关闭LoadingPanel界面
                        UIManager.Instance.CloseWindow("LoadingPanel");
                        Debug.Log("死翘翘了");
                        //回到主场景
                        SceneEventDefine.StartingScene.SendEventMessage();
                    }
                }
            }
        }
    }
}
