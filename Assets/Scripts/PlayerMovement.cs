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
        //������ check ground
        isGrouned = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);
        //����Ĭ���ٶ�
        if (isGrouned && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //����������
        float x = Input.GetAxis("Horizontal");//����
        float z = Input.GetAxis("Vertical");//ǰ��

        //�����ƶ�ʸ��
        Vector3 move = transform.right * x + transform.forward * z;//(�� - ���ᣬ��ǰ - ����)

        //ʵ���ƶ����
        controller.Move(move * speed * Time.deltaTime);


        //�������Ƿ������Ծ
        if (Input.GetButtonDown("Jump") && isGrouned)
        {
            //���� Going up
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Falling down ����
        velocity.y += gravity * Time.deltaTime;

        //ִ����Ծ  Exectuting the jump
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
