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


    //�洢Ѳ��"��תվ"����Ե�
    List<Transform> waypointsList = new List<Transform>();


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // --- ��ʼ�� Initialization --- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = patrolSpead;
        timer = 0f;

        // --- �ҵ����к�·�㣬�ƶ�����һ����·�� --- //

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

        //�����Zombie�ľ���
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);


        //����Zombie������Ч��  
        if (distanceFromPlayer < GetPlayZombieSoundArea())
        {
            //������Ч
            if (SoundManager.Instance.zombieChannel.isPlaying == false)
            {
                SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
                //�ӳٲ���
                SoundManager.Instance.zombieChannel.PlayDelayed(1f);
            }
        }
        


        // --- ������˵��ﺽ·�㣬���ƶ�����һ����·�� --- //
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
        }

        // --- �л�������״̬ --- //
        timer += Time.deltaTime;
        if (timer >patrolingTimer)
        {
            animator.SetBool("isPatroling", false);
        }

        //��ҽ�����˼�ⷶΧ������׷��״̬

        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ֹͣ�ƶ�
        agent.SetDestination(agent.transform.position);


        //״̬�˳�ֹͣ����
        SoundManager.Instance.zombieChannel.Stop();

    }

    private float GetPlayZombieSoundArea()
    {
        float playZombieSoundArea = GameObject.Find("Zombie").GetComponent<Zombie>().playZombieSoundArea;
        return playZombieSoundArea;
    }
}
