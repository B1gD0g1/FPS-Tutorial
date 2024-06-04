using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{

    private float timer;
    public float patrolingTimer = 0f;

    private Transform player;
    private NavMeshAgent agent;

    public float detectionArea = 18f;
    public float patrolSpead = 2f;

    //存储巡逻"中转站"，点对点
    List<Transform> waypointsList = new List<Transform>();


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // --- 初始化 Initialization --- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = patrolSpead;
        timer = 0f;

        // --- 找到所有航路点，移动到第一个航路点 --- //

        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in waypointCluster.transform)
        {
            waypointsList.Add(t);
        }

        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        agent.SetDestination(nextPosition);

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // --- 如果敌人到达航路点，就移动到下一个航路点 --- //
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
        }

        // --- 切换到闲置状态 --- //
        timer += Time.deltaTime;
        if (timer >patrolingTimer)
        {
            animator.SetBool("isPatroling", false);
        }

        //玩家进入敌人检测范围，进入追踪状态

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //停止移动
        agent.SetDestination(agent.transform.position);
    }

}
