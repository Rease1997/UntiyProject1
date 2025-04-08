using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy
{
    public GameObject enemyObj;
    Transform enemy;
    Vector3 startPos;
    //Collider2D enemyCollider;
    public void Init(Transform _enemy)
    {
        this.enemy = _enemy;// ��ȡ�������ϵ���ײ��
        enemyObj = this.enemy.gameObject;
        startPos = this.enemy.localPosition;
    }
    //��ײ��Ĵ򿪹ر�
    public void CloseAllCollider(bool isCollider)
    {
        for (int i = 0; i < enemy.childCount; i++)
        {
            if(isCollider)
            {
                BoxCollider2D boxd = enemy.GetChild(i).transform.GetComponent<BoxCollider2D>();
                boxd.enabled = false;
            }
            else
            {
                BoxCollider2D boxd = enemy.GetChild(i).transform.GetComponent<BoxCollider2D>();
                boxd.enabled = true;
            }
        }
    }
    //����λ�ø�ԭ
    public void EnemyPos()
    {
        if (enemy != null)
        {
            enemy.localPosition = startPos;
        }
    }
    public void Update()
    {
        EnemyMove();
    }

    private void EnemyMove()
    {
        //�������������дһ�����������ƶ��Ĵ���
        if (enemy != null && enemy.gameObject.activeSelf)
        {
            enemy.Translate(Vector3.left * Time.deltaTime * 2);
        }
    }
}
