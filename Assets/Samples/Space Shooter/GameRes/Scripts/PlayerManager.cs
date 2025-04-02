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
        //在这个方法里面写一个玩家在2D游戏中的左右移动的代码
        //获取玩家的输入
        float h = Input.GetAxis("Horizontal");
        if (h != 0) {
            //获取玩家的刚体组件
            _playRig = GetComponent<Rigidbody2D>();
            //设置玩家的速度
            _playRig.velocity = new Vector2(h * 5, _playRig.velocity.y);
        }
    }
}
