using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;


    private NavMeshAgent navAgent;

    //�����жϵ����Ƿ�����
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
            //�����
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

            //������Ч
            SoundManager.Instance.zombieHurtChannel.PlayOneShot(SoundManager.Instance.zombieDeath);

        }
        else
        {
            animator.SetTrigger("DAMAGE");

            //��������Ч
            SoundManager.Instance.zombieHurtChannel.PlayOneShot(SoundManager.Instance.zombieHurt);

        }
    }


    private void OnDrawGizmos()
    {
        //������Χ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 8f);

        //׷��Χ
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f);

        //ֹͣ׷��Χ��Χ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 30f);

        //���Ž�ʬ��Ч��
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 40f);
    }

}
