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
        //将光标锁定在屏幕中间，并使其不可见
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //绕X轴旋转 （向上和向下看）
        xRotation -= mouseY;

        //限制（锁定）旋转
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //绕Y轴旋转 （向左和向右看）
        yRotation += mouseX;

        //根据旋转改变玩家的实际身体
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);



    }
}
