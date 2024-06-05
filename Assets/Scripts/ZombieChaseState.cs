using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{

    private Transform player;
    private NavMeshAgent agent;
    private Zombie zombie;

    public float chaseSpread = 6f;
    public float stopChasingDistance = 21f;
    public float attackingDistance = 2.5f;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // --- ��ʼ�� Initialization --- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chaseSpread;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //��������ľ���
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer < GetPlayZombieSoundArea())
        {
            //׷����Ч
            if (SoundManager.Instance.zombieChannel.isPlaying == false)
            {
                SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);
            }
        }


        //׷����Ҳ�һֱ�������
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);


        // --- ����Ƿ�Ӧ��ֹͣ׷�� --- //

        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        // --- ����Ƿ�Ӧ�ù��� --- //

        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);

        //״̬�˳�ֹͣ����
        SoundManager.Instance.zombieChannel.Stop();
    }

    private float GetPlayZombieSoundArea()
    {
        float playZombieSoundArea = GameObject.Find("Zombie").GetComponent<Zombie>().playZombieSoundArea;
        return playZombieSoundArea;
    }

}
