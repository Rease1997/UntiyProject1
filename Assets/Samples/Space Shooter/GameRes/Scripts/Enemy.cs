using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    Transform enemy;
    public void Init(Transform _enemy)
    {
        this.enemy = _enemy;
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
