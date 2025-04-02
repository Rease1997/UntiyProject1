using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D _playRig;
    Animator _playAnim;
    //玩家碰到地面的
    bool _isJump = false;
    Transform enemyParent;
    bool isenemy = true;
    List<Enemy> enemyList = new List<Enemy>();
    Enemy enemy = new Enemy();
    // Start is called before the first frame update
    void Start()
    {
        //获取玩家的刚体组件
        _playRig = GetComponent<Rigidbody2D>();
        //获取玩家的动画组件
        _playAnim = GetComponent<Animator>();
        //敌人预制体父节点
        enemyParent = GameObject.Find("Goombas").transform;
        if (enemyParent != null)
        {
            for (int i = 0; i < enemyParent.childCount; i++)
            {
                // 获取第 i 个子节点的 Transform
                Transform childTransform = enemyParent.GetChild(i);
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
        foreach (Enemy enemy in enemyList)
        {
            enemy.Update();
        }
    }

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

    }
}
