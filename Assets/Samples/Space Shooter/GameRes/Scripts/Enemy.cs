using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{

    Transform enemy;
    //Collider2D enemyCollider;
    public void Init(Transform _enemy)
    {
        this.enemy = _enemy;// ��ȡ�������ϵ���ײ��
        //enemyCollider = enemy.transform.Find("Body Collider").GetComponent<Collider2D>();
        //if (enemyCollider != null)
        //{
        //    Debug.Log("�ɹ���ȡ���˵���ײ��");
        //}
        //else
        //{
        //    Debug.Log("δ�ܻ�ȡ���˵���ײ��");
        //}
    }
    public void EnemycolliderClose(BoxCollider2D enemycollider,bool isClose)
    {
        //�رյ�����ײ��
        if (isClose)
        {
            Debug.LogError("�رյ�����ײ��");
            enemycollider.enabled = false;
        }
        else if(isClose == false)
        {
            //����������ײ��
            Debug.LogError("�򿪵�����ײ��");
            enemycollider.enabled = true;
        }
    }
    public void Update()
    {
        EnemyMove();
    }

    private void EnemyMove()
    {
        //�������������дһ�����������ƶ��Ĵ���
        if(enemy != null && enemy.gameObject.activeSelf)
        {
            enemy.Translate(Vector3.left * Time.deltaTime * 2);
        }
    }
}
