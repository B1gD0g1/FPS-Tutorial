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

        // --- 初始化 Initialization --- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //玩家与Zombie的距离
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        //Zombie音效有效区
        if (distanceFromPlayer < GetPlayZombieSoundArea())
        {
            //攻击音效
            if (SoundManager.Instance.zombieChannel.isPlaying == false)
            {
                SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieAttack);
            }
        }


        LookAtPlayer();

        // --- 检查是否应该停止攻击 --- //


        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //状态退出停止播放
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
