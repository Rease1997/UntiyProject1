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
        this.enemy = _enemy;// 获取敌人身上的碰撞框
        enemyObj = this.enemy.gameObject;
        startPos = this.enemy.localPosition;
    }
    //碰撞框的打开关闭
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
    //敌人位置复原
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
        //在这个方法里面写一个敌人向左移动的代码
        if (enemy != null && enemy.gameObject.activeSelf)
        {
            enemy.Translate(Vector3.left * Time.deltaTime * 2);
        }
    }
}
