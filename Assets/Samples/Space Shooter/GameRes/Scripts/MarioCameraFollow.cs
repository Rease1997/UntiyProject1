using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioCameraFollow
{
    //����ű�
    Transform transform; //���
    Transform _transPlayer; //���
    public float smoothSpeed = 0.125f;  // ƽ�������ٶ�
    public float offsetY = 5f;  // �����Y��Ĺ̶�ƫ����
    public float offsetZ = -10f;  // �����Z��Ĺ̶�ƫ����
    public void Init(Transform transCamera,Transform transPlayer)
    {
        this.transform = transCamera;
        this._transPlayer = transPlayer;
    }

    public void lateUpdate()
    {
        // ֻ��ȡ�����X���ϵ�λ�ã����������Y���Z���ƫ��
        float desiredX = _transPlayer.position.x;

        // �����µ����λ�ã�����Y��Z�᲻��
        Vector3 desiredPosition = new Vector3(desiredX, offsetY, offsetZ);

        // ƽ���ƶ���Ŀ��λ��
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // �������λ��
        transform.position = smoothedPosition;
    }
}
