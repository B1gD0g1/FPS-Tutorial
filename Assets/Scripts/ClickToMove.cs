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
            //����һ��������������λ�õĹ��� create a ray from the camera to the mouse posision
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //�������Ƿ񵽴����(�Դ�Ѱ·) check if the ray hits the ground (Navmesh)
            if (Physics.Raycast(ray,out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                //���ع��ƶ��������λ���� move the agent to the clicked position 
                navAgent.SetDestination(hit.point);
            }
        }
    }

}
