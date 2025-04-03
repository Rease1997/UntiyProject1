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
        //�������
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
        Debug.Log("��LoadingPanel����");
        UIManager.Instance.OpenWindow("LoadingPanel");
        hp--;
        Debug.Log(hp + "  ��ǰѪ��");
    }

    /// <summary>
    /// ������ɲ����ӽű�
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
                        //�ر�LoadingPanel����
                        UIManager.Instance.CloseWindow("LoadingPanel");
                        //�ٿ�һ�� ��ʼ�����λ��
                    }
                    else
                    {
                        Debug.Log("��������");
                        //�ص�������
                    }
                }
            }
        }
    }
}
