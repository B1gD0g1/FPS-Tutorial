using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{

    private Transform player;
    private NavMeshAgent agent;

    public float chaseSpread = 6f;
    public float stopChasingDistance = 21f;
    public float attackingDistance = 2.5f;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // --- 初始化 Initialization --- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chaseSpread;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //追逐玩家并一直看向玩家
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // --- 检查是否应该停止追逐 --- //

        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        // --- 检查是否应该攻击 --- //

        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }

}
