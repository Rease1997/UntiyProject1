using System;
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
    //������������
    bool _isJump = false;
    //�ж��Ƿ�رյ��˽ű���Update����
    bool _isenemy = true;
    Transform enemyParent;
    //��¼���˵ĳ�ʼλ��
    List<Transform> enemyTrans = new List<Transform>();
    List<Enemy> enemyList = new List<Enemy>();
    //��������ű�
    MarioCameraFollow maricamera = new MarioCameraFollow();
    //��ȡ���˵���ײ��
    BoxCollider2D enemycollider;
    //���Ѫ��
    bool _isplayerHp;
    // Start is called before the first frame update
    void Start()
    {
        //��ȡ��ҵĸ������
        _playRig = GetComponent<Rigidbody2D>();
        //��ȡ��ҵĶ������
        _playAnim = GetComponent<Animator>();
        //����Ԥ���常�ڵ�
        enemyParent = GameObject.Find("Goombas").transform;
        //��ȡ�����е������
        _playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        maricamera.Init(_playerCamera.transform, transform);

        //�����˳�ʼ��
        EnemyInit();
    }
    /// <summary>
    /// �����˳�ʼ���ű�
    /// </summary>
    private void EnemyInit()
    {
        if (enemyParent != null)
        {
            for (int i = 0; i < enemyParent.childCount; i++)
            {
                // ��ȡ�� i ���ӽڵ�� Transform
                Transform childTransform = enemyParent.GetChild(i);
                Enemy enemy = new Enemy();
                enemy.Init(childTransform);
                enemyList.Add(enemy);
                enemyTrans.Add(childTransform.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //����ƶ�
        PlayerMove();
        //���õ��˽ű���Update����
        EnemyUpdate();
    }

    void LateUpdate()
    {
        //����������
        maricamera.lateUpdate();
    }
    /// <summary>
    /// ���õ��˽ű��ķ�����ʹ��������һֱ�ƶ�
    /// </summary>
    private void EnemyUpdate()
    {
        foreach (Enemy enemy in enemyList)
        {
            if (_isenemy)
                enemy.Update();
        }
    }
    public void Enemyentities()
    {
        _playAnim.SetBool("Dle", false);
        _isenemy = true; 
        foreach (var item in enemyList)
        {
            item.EnemycolliderClose(enemycollider, false);
            Debug.Log(enemycollider.name);
        }
    }
    /// <summary>
    /// ����ƶ�
    /// </summary>
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
        //���ͷ����ש��
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Brick"))
        {
            _isJump = true;
        }
        //���˵�������ײ��
        if (collision.collider.CompareTag("EnemyBody"))
        {
            _playAnim.SetBool("Dle", true);
            _isenemy = false;
            // ��ȡ���˵���Ϸ����
            GameObject enemyObject = collision.collider.transform.parent.gameObject;
            //�رյ��˵���ײ��
            EnemyColliderClose(enemyObject);
            StartCoroutine(PlayerCollider());
        }
        //���˵�ͷ����ײ��
        if (collision.collider.CompareTag("EnemyHead"))
        {
            // ��ȡ���˵���Ϸ����
            GameObject enemyObject = collision.collider.transform.parent.gameObject;
            if (enemyObject != null)
            {
                // ���ٵ��˵���Ϸ����
                Destroy(enemyObject);
            }
        }
    }

    private void EnemyColliderClose(GameObject enemyObject)
    {
        if (enemyObject != null)
        {
            enemycollider = enemyObject.transform.Find("Body Collider").GetComponent<BoxCollider2D>();
            if (!enemycollider)
            {
                Debug.Log("�ҵ����˵���ײ��");
                foreach (var item in enemyList)
                {
                    item.EnemycolliderClose(enemycollider, true);
                }
            }
        }
    }

    IEnumerator PlayerCollider()
    {
        BoxCollider2D playcollider = transform.GetChild(1).Find("SmallMarioCollider").GetComponent<BoxCollider2D>();
        EdgeCollider2D edgeplayercollider = transform.GetChild(1).Find("SmallMarioCollider").GetComponent<EdgeCollider2D>();
        if (playcollider != null)
        {
            playcollider.enabled = false;
            edgeplayercollider.enabled = false;
            Debug.Log("��ҵ���ײ��ر���" + playcollider.name);
        }
        else
        {
            Debug.Log("��ҵ���ײ��û���ҵ�");
        }
        var msg = new PlayerManager();
        if (playcollider.enabled == false)
            UniEvent.SendMessage(msg);
        yield break;
    }
}
