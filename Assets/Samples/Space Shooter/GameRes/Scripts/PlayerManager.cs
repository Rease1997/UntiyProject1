using System;
using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using UnityEngine.UIElements;
using static SceneEventDefine;

public class PlayerManager : MonoBehaviour,IEventMessage
{
    Rigidbody2D _playRig;
    Animator _playAnim;
    //玩家碰到地面的
    bool _isJump = false;
    //敌人父节点
    Transform enemyParent;
    //敌人是否移动
    bool _isenemy = true;
    //用来存储敌人脚本
    List<Enemy> enemyList = new List<Enemy>();
    //相机
    Camera _mainCamera;
    //相机脚本
    MarioCameraFollow marioCamera = new MarioCameraFollow();
    //玩家碰撞框
    EdgeCollider2D edge; BoxCollider2D boxd;

    public float jumpHeight = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //获取玩家的刚体组件
        _playRig = GetComponent<Rigidbody2D>();
        //获取玩家的动画组件
        _playAnim = GetComponent<Animator>();
        //查找敌人位置，还有初始化敌人脚本
        EnemyInit();
        //获取相机并初始化相机脚本
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        marioCamera.Init(_mainCamera.transform, transform);
        //获取玩家碰撞框
        boxd = transform.GetChild(1).Find("SmallMarioCollider").GetComponent<BoxCollider2D>();
        edge = transform.GetChild(1).Find("SmallMarioCollider").GetComponent<EdgeCollider2D>();
    }

    /// <summary>
    ///查找敌人位置，还有初始化敌人脚本
    /// </summary>
    private void EnemyInit()
    {
        //敌人预制体父节点
        enemyParent = GameObject.Find("Goombas").transform;
        if (enemyParent != null)
        {
            for (int i = 0; i < enemyParent.childCount; i++)
            {
                // 获取第 i 个子节点的 Transform
                Transform childTransform = enemyParent.GetChild(i);
                Enemy enemy = new Enemy();
                enemy.Init(childTransform);
                enemyList.Add(enemy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //玩家移动
        PlayerMove();
        EnemyMove();
    }
    
    private void LateUpdate()
    {
        marioCamera.lateUpdate();
    }
    /// <summary>
    /// 敌人移动方法
    /// </summary>
    private void EnemyMove()
    {
        foreach (Enemy enemy in enemyList)
        {
            if (_isenemy)
                enemy.Update();
        }
    }

    /// <summary>
    /// 玩家移动跳跃
    /// </summary>
    private void PlayerMove()
    {
        if (_playAnim == null)
            return;
        //在这个方法里面写一个玩家在2D游戏中的左右移动的代码
        //获取玩家的输入
        float h = Input.GetAxis("Horizontal");
        if (h != 0)
        {
            //设置玩家的速度
            _playRig.velocity = new Vector2(h * 5, _playRig.velocity.y);
            _playAnim.SetBool("speed", true);
            if (h < 0)
            {
                //在这个判断里面写一个玩家旋转180度的代码
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            _playAnim.SetBool("speed", false);
        }

        if (_isJump)//判断条件是否达成
        {
            //写一个玩家点击空格跳起来的代码
            if (Input.GetKeyDown(KeyCode.W))
            {
                //在这修改玩家的位置向上移动3
                _playRig.velocity = new Vector2(_playRig.velocity.x, 24);
                _playAnim.SetBool("jump", true);
                _isJump = false;
            }
            else
            {
                _playAnim.SetBool("jump", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //玩家跳跃判断
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Brick")||
            collision.collider.CompareTag("Pipe")|| collision.collider.CompareTag("Stone"))
        {
            _isJump = true;
        }
        //敌人头部碰撞框
        if (collision.collider.CompareTag("EnemyHead"))
        {
            GameObject enemyobj = collision.collider.transform.parent.gameObject;
            if (enemyobj != null)
            {
                Destroy(enemyobj);
            }
        }
        //敌人身体部分碰撞框
        if (collision.collider.CompareTag("EnemyBody"))
        {
            _playAnim.SetBool("Dle", true);
            _isenemy = false;
            GameObject enemyobj = collision.collider.transform.parent.gameObject;
            if(enemyobj != null)
            {
                EnemyColliderClose(enemyobj);
            }
        }
        //玩家碰撞方块出金币
        if (collision.collider.CompareTag("Untagged")|| collision.collider.CompareTag("PowerBrick"))
        {
            GameObject coinBrick = collision.collider.gameObject;
            if(coinBrick != null)
            {
                Vector3 currentPosition = coinBrick.transform.position;
                currentPosition.y += jumpHeight;
                coinBrick.transform.position = currentPosition;
                StartCoroutine(Positionrestoration(coinBrick));
            }
        }

    }

    private IEnumerator Positionrestoration(GameObject coinBrick)
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 currentPosition = coinBrick.transform.position;
        currentPosition.y -= jumpHeight;
        coinBrick.transform.position = currentPosition;
    }

    private void EnemyColliderClose(GameObject enemyobj)
    {
        foreach(var item in enemyList)
        {
            item.CloseAllCollider(true);
        }
        StartCoroutine(CloseAllPlayerCollider());
    }

    //关闭玩家身上的碰撞框并广播消息
    private IEnumerator CloseAllPlayerCollider()
    {
        //关闭玩家碰撞框
        if (boxd != null && edge != null)
        {
            boxd.enabled = false;
            edge.enabled = false;
        }
        yield return new WaitForSeconds(0.3f);
        var msg = new PlayerManager();
        UniEvent.SendMessage(msg);
    }

    //打开敌人和玩家身上的碰撞框
    internal void openAllCollider()
    {
        _playAnim.SetBool("Dle", false);
        _isenemy = true;
        //打开敌人碰撞框   
        foreach (var item in enemyList)
        {
            item.CloseAllCollider(false);
            item.EnemyPos();
        }
        //打开玩家碰撞框
        if (boxd != null && edge != null)
        {
            boxd.enabled = true;
            edge.enabled = true;
        }
    }
}
