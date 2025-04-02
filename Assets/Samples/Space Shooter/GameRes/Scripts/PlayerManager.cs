using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D _playRig;
    Animator _playAnim;
    //������������
    bool _isJump = false;
    Transform enemyParent;
    bool isenemy = true;
    List<Enemy> enemyList = new List<Enemy>();
    Enemy enemy = new Enemy();
    // Start is called before the first frame update
    void Start()
    {
        //��ȡ��ҵĸ������
        _playRig = GetComponent<Rigidbody2D>();
        //��ȡ��ҵĶ������
        _playAnim = GetComponent<Animator>();
        //����Ԥ���常�ڵ�
        enemyParent = GameObject.Find("Goombas").transform;
        if (enemyParent != null)
        {
            for (int i = 0; i < enemyParent.childCount; i++)
            {
                // ��ȡ�� i ���ӽڵ�� Transform
                Transform childTransform = enemyParent.GetChild(i);
                enemy.Init(childTransform);
                enemyList.Add(enemy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //����ƶ�
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
        //�������������дһ�������2D��Ϸ�е������ƶ��Ĵ���
        //��ȡ��ҵ�����
        float h = Input.GetAxis("Horizontal");
        if (h != 0)
        {
            //������ҵ��ٶ�
            _playRig.velocity = new Vector2(h * 5, _playRig.velocity.y);
            _playAnim.SetBool("speed", true);
            if (h < 0)
            {
                //������ж�����дһ�������ת180�ȵĴ���
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

        if (_isJump)//�ж������Ƿ���
        {
            //дһ����ҵ���ո��������Ĵ���
            if (Input.GetKeyDown(KeyCode.W))
            {
                //�����޸���ҵ�λ�������ƶ�3
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
