using System;
using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using YooAsset;

public class LoadingView : ViewBase
{
    bool isCountDown = false;//�Ƿ�������ʱ
    float countDown;//����ʱ
    bool isFirst;//�Ƿ��һ��������� ���ǵ�һ�εĻ��������
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
        //�������
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
            Debug.Log(hp + "  ��ǰѪ��");
        }
        count++;
        Debug.Log("����" + count);
    }

    private void OpenEventMessage(IEventMessage message)
    {
        Debug.Log("��LoadingPanel����");
        UIManager.Instance.OpenWindow("LoadingPanel");
    }

    /// <summary>
    /// ������ɲ����ӽű�
    /// </summary>
    private void CreatePlayer()
    {
        GameObject play = YooAssets.LoadAssetSync<GameObject>("Player").AssetObject as GameObject;
        player = GameObject.Instantiate(play);
        //��ȡ�������ʱ��λ��
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
                        //�ر�LoadingPanel����
                        UIManager.Instance.CloseWindow("LoadingPanel");
                        //��������Ҹ��� ��ԭ��ҵĳ�ʼλ��
                        player.transform.position = playerPos;
                        playermanager.openAllCollider();
                    }
                    else 
                    {
                        //�ر�LoadingPanel����
                        UIManager.Instance.CloseWindow("LoadingPanel");
                        Debug.Log("��������");
                        //�ص�������
                        SceneEventDefine.StartingScene.SendEventMessage();
                    }
                }
            }
        }
    }
}
