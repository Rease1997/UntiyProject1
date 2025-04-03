using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using static SceneEventDefine;

public class PlayerManager : MonoBehaviour, IEventMessage
{
    Camera _playerCamera;
    Rigidbody2D _playRig;
    Animator _playAnim;
    //玩家碰到地面的
    bool _isJump = false;
    //判断是否关闭敌人脚本的Update方法
    bool _isenemy = true;
    Transform enemyParent;
    List<Enemy> enemyList = new List<Enemy>();
    //声明相机脚本
    MarioCameraFollow maricamera = new MarioCameraFollow();
    // Start is called before the first frame update
    void Start()
    {
        //获取玩家的刚体组件
        _playRig = GetComponent<Rigidbody2D>();
        //获取玩家的动画组件
        _playAnim = GetComponent<Animator>();
        //敌人预制体父节点
        enemyParent = GameObject.Find("Goombas").transform;
        //获取场景中的摄像机
        _playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        maricamera.Init(_playerCamera.transform, transform);

        //给敌人初始化
        EnemyInit();
    }

    /// <summary>
    /// 给敌人初始化脚本
    /// </summary>
    private void EnemyInit()
    {
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
        //调用敌人脚本的Update方法
        EnemyUpdate();
    }

    void LateUpdate()
    {
        //相机跟随玩家
        maricamera.lateUpdate();
    }
    /// <summary>
    /// 调用敌人脚本的方法，使敌人向左一直移动
    /// </summary>
    private void EnemyUpdate()
    {
        foreach (Enemy enemy in enemyList)
        {
            if (_isenemy)
                enemy.Update();
        }
    }

    /// <summary>
    /// 玩家移动
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
                _playRig.velocity = new Vector2(_playRig.velocity.x, 21);
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
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Untagged"))
        {
            _isJump = true;
        }
        
        if (collision.collider.CompareTag("EnemyBody"))
        {
            _playAnim.SetBool("Dle", true);
            _isenemy = false;
            StartCoroutine(PlayerCollider());
        }
        if (collision.collider.CompareTag("EnemyHead"))
        {
            // 获取敌人的游戏对象
            GameObject enemyObject = collision.collider.transform.parent.gameObject;
            if (enemyObject != null)
            {
                // 销毁敌人的游戏对象
                Destroy(enemyObject);
            }
        }
    }

    IEnumerator PlayerCollider()
    {
        BoxCollider2D playcollider = transform.GetChild(1).Find("SmallMarioCollider").GetComponent<BoxCollider2D>();
        if (playcollider != null)
        {
            playcollider.enabled = false;
        }
        else
        {
            Debug.Log("玩家的碰撞框没有找到");
        }
        yield return new WaitForSeconds(2f);
        var msg = new PlayerManager();
        UniEvent.SendMessage(msg);
    }
}
