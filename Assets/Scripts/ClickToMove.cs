using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    private NavMeshAgent navAgent;



    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //创建一条从摄像机到鼠标位置的光线 create a ray from the camera to the mouse posision
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //检查光线是否到达地面(自带寻路) check if the ray hits the ground (Navmesh)
            if (Physics.Raycast(ray,out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                //将特工移动到点击的位置上 move the agent to the clicked position 
                navAgent.SetDestination(hit.point);
            }
        }
    }

}
