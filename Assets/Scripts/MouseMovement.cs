using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float mouseSensitivity = 500f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;


    // Start is called before the first frame update
    void Start()
    {
        //�������������Ļ�м䣬��ʹ�䲻�ɼ�
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //��ȡ�������
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //��X����ת �����Ϻ����¿���
        xRotation -= mouseY;

        //���ƣ���������ת
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //��Y����ת ����������ҿ���
        yRotation += mouseX;

        //������ת�ı���ҵ�ʵ������
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);



    }
}
