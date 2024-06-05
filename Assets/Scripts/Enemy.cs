using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;


    private NavMeshAgent navAgent;

    //”√”⁄≈–∂œµ–»À «∑ÒÀ¿Õˆ
    public bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            //ÀÊª˙ ˝
            int randomValue = Random.Range(0,2); // 0 or 1


            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }

            isDead = true;

            //À¿Õˆ“Ù–ß
            SoundManager.Instance.zombieHurtChannel.PlayOneShot(SoundManager.Instance.zombieDeath);

        }
        else
        {
            animator.SetTrigger("DAMAGE");

            //±ªª˜÷–“Ù–ß
            SoundManager.Instance.zombieHurtChannel.PlayOneShot(SoundManager.Instance.zombieHurt);

        }
    }


    private void OnDrawGizmos()
    {
        //π•ª˜∑∂Œß
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 8f);

        //◊∑÷∑∂Œß
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f);

        //Õ£÷π◊∑÷∑∂Œß∑∂Œß
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 30f);

        //≤•∑≈Ω© ¨“Ù–ß«¯
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 40f);
    }

}
