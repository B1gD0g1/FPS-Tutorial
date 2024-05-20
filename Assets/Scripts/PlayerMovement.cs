using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrouned;
    bool isMoveing;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //检查地面 check ground
        isGrouned = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);
        //重置默认速度
        if (isGrouned && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //获得玩家输入
        float x = Input.GetAxis("Horizontal");//左右
        float z = Input.GetAxis("Vertical");//前后

        //创建移动矢量
        Vector3 move = transform.right * x + transform.forward * z;//(右 - 红轴，向前 - 蓝轴)

        //实际移动玩家
        controller.Move(move * speed * Time.deltaTime);


        //检查玩家是否可以跳跃
        if (Input.GetButtonDown("Jump") && isGrouned)
        {
            //上升 Going up
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Falling down 掉落
        velocity.y += gravity * Time.deltaTime;

        //执行跳跃  Exectuting the jump
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrouned == true)
        {
            isMoveing = true;
        }
        else
        {
            isMoveing = false;
        }

        lastPosition = gameObject.transform.position;



    }
}
