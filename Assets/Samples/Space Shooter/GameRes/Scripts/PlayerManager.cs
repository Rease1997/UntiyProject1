using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D _playRig;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�������������дһ�������2D��Ϸ�е������ƶ��Ĵ���
        //��ȡ��ҵ�����
        float h = Input.GetAxis("Horizontal");
        if (h != 0) {
            //��ȡ��ҵĸ������
            _playRig = GetComponent<Rigidbody2D>();
            //������ҵ��ٶ�
            _playRig.velocity = new Vector2(h * 5, _playRig.velocity.y);
        }
    }
}
