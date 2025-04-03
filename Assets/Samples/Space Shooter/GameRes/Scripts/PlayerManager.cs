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
    List<Enemy> enemyList = new List<Enemy>();
    //��������ű�
    MarioCameraFollow maricamera = new MarioCameraFollow();
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
            // ��ȡ���˵���Ϸ����
            GameObject enemyObject = collision.collider.transform.parent.gameObject;
            if (enemyObject != null)
            {
                // ���ٵ��˵���Ϸ����
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
            Debug.Log("��ҵ���ײ��û���ҵ�");
        }
        yield return new WaitForSeconds(2f);
        var msg = new PlayerManager();
        UniEvent.SendMessage(msg);
    }
}
