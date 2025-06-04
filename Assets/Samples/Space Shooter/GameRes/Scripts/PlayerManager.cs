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
    //������������
    bool _isJump = false;
    //���˸��ڵ�
    Transform enemyParent;
    //�����Ƿ��ƶ�
    bool _isenemy = true;
    //�����洢���˽ű�
    List<Enemy> enemyList = new List<Enemy>();
    //���
    Camera _mainCamera;
    //����ű�
    MarioCameraFollow marioCamera = new MarioCameraFollow();
    //�����ײ��
    EdgeCollider2D edge; BoxCollider2D boxd;

    public float jumpHeight = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //��ȡ��ҵĸ������
        _playRig = GetComponent<Rigidbody2D>();
        //��ȡ��ҵĶ������
        _playAnim = GetComponent<Animator>();
        //���ҵ���λ�ã����г�ʼ�����˽ű�
        EnemyInit();
        //��ȡ�������ʼ������ű�
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        marioCamera.Init(_mainCamera.transform, transform);
        //��ȡ�����ײ��
        boxd = transform.GetChild(1).Find("SmallMarioCollider").GetComponent<BoxCollider2D>();
        edge = transform.GetChild(1).Find("SmallMarioCollider").GetComponent<EdgeCollider2D>();
    }

    /// <summary>
    ///���ҵ���λ�ã����г�ʼ�����˽ű�
    /// </summary>
    private void EnemyInit()
    {
        //����Ԥ���常�ڵ�
        enemyParent = GameObject.Find("Goombas").transform;
        if (enemyParent != null)
        {
            for (int i = 0; i < enemyParent.childCount; i++)
            {
                // ��ȡ�� i ���ӽڵ�� Transform
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
        //����ƶ�
        PlayerMove();
        EnemyMove();
    }
    
    private void LateUpdate()
    {
        marioCamera.lateUpdate();
    }
    /// <summary>
    /// �����ƶ�����
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
    /// ����ƶ���Ծ
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
        
        //�����Ծ�ж�
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Brick")||
            collision.collider.CompareTag("Pipe")|| collision.collider.CompareTag("Stone"))
        {
            _isJump = true;
        }
        //����ͷ����ײ��
        if (collision.collider.CompareTag("EnemyHead"))
        {
            GameObject enemyobj = collision.collider.transform.parent.gameObject;
            if (enemyobj != null)
            {
                Destroy(enemyobj);
            }
        }
        //�������岿����ײ��
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
        //�����ײ��������
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

    //�ر�������ϵ���ײ�򲢹㲥��Ϣ
    private IEnumerator CloseAllPlayerCollider()
    {
        //�ر������ײ��
        if (boxd != null && edge != null)
        {
            boxd.enabled = false;
            edge.enabled = false;
        }
        yield return new WaitForSeconds(0.3f);
        var msg = new PlayerManager();
        UniEvent.SendMessage(msg);
    }

    //�򿪵��˺�������ϵ���ײ��
    internal void openAllCollider()
    {
        _playAnim.SetBool("Dle", false);
        _isenemy = true;
        //�򿪵�����ײ��   
        foreach (var item in enemyList)
        {
            item.CloseAllCollider(false);
            item.EnemyPos();
        }
        //�������ײ��
        if (boxd != null && edge != null)
        {
            boxd.enabled = true;
            edge.enabled = true;
        }
    }
}
