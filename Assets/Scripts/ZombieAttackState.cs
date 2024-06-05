using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    public float stopAttackingDistance = 2.5f;



    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // --- ��ʼ�� Initialization --- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //�����Zombie�ľ���
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        //Zombie��Ч��Ч��
        if (distanceFromPlayer < GetPlayZombieSoundArea())
        {
            //������Ч
            if (SoundManager.Instance.zombieChannel.isPlaying == false)
            {
                SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieAttack);
            }
        }


        LookAtPlayer();

        // --- ����Ƿ�Ӧ��ֹͣ���� --- //


        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //״̬�˳�ֹͣ����
        SoundManager.Instance.zombieChannel.Stop();
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.rotation.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    private float GetPlayZombieSoundArea()
    {
        float playZombieSoundArea = GameObject.Find("Zombie").GetComponent<Zombie>().playZombieSoundArea;
        return playZombieSoundArea;
    }

}
