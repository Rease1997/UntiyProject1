using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioCameraFollow
{
    //相机脚本
    Transform transform; //相机
    Transform _transPlayer; //玩家
    public float smoothSpeed = 0.125f;  // 平滑跟随速度
    public float offsetY = 5f;  // 相机在Y轴的固定偏移量
    public float offsetZ = -10f;  // 相机在Z轴的固定偏移量
    public void Init(Transform transCamera,Transform transPlayer)
    {
        this.transform = transCamera;
        this._transPlayer = transPlayer;
    }

    public void lateUpdate()
    {
        // 只获取玩家在X轴上的位置，保持相机在Y轴和Z轴的偏移
        float desiredX = _transPlayer.position.x;

        // 构造新的相机位置，保持Y和Z轴不变
        Vector3 desiredPosition = new Vector3(desiredX, offsetY, offsetZ);

        // 平滑移动到目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 更新相机位置
        transform.position = smoothedPosition;
    }
}
