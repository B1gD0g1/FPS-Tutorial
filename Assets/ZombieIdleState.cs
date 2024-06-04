using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{

    private float timer;
    public float idleTimer = 0f;

    private Transform player;

    public float detectionAreaRadius = 18f;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // --- ÇÐ»»µ½Ñ²Âß×´Ì¬ transition to Protrol State --- //
        timer += Time.deltaTime;
        if (timer > idleTimer)
        {
            animator.SetBool("isPatroling", true);
        }

        // --- ÇÐ»»µ½×·Öð×´Ì¬ transition to Chase State --- //

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }

}
