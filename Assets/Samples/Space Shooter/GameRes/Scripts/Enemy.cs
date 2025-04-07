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
        this.enemy = _enemy;// 获取敌人身上的碰撞框
        //enemyCollider = enemy.transform.Find("Body Collider").GetComponent<Collider2D>();
        //if (enemyCollider != null)
        //{
        //    Debug.Log("成功获取敌人的碰撞框");
        //}
        //else
        //{
        //    Debug.Log("未能获取敌人的碰撞框");
        //}
    }
    public void EnemycolliderClose(BoxCollider2D enemycollider,bool isClose)
    {
        //关闭敌人碰撞框
        if (isClose)
        {
            Debug.LogError("关闭敌人碰撞框");
            enemycollider.enabled = false;
        }
        else if(isClose == false)
        {
            //开启敌人碰撞框
            Debug.LogError("打开敌人碰撞框");
            enemycollider.enabled = true;
        }
    }
    public void Update()
    {
        EnemyMove();
    }

    private void EnemyMove()
    {
        //在这个方法里面写一个敌人向左移动的代码
        if(enemy != null && enemy.gameObject.activeSelf)
        {
            enemy.Translate(Vector3.left * Time.deltaTime * 2);
        }
    }
}
