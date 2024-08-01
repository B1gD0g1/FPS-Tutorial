using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour
{

    public int initalZombiePerWave = 5;
    public int currentZombiesPerWave;


    public float spawnDeplay = 0.5f;//��ÿһ���в���ÿ����ʬ֮����ӳ�

    public int currentWave = 0;
    public float waveCooldown = 10.0f;//ÿһ��֮���ʱ��

    public bool inCooldown;
    public float cooldownCounter = 0;

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI waveoverUI;
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI currentWaveUI;



    private void Start()
    {
        currentZombiesPerWave = initalZombiePerWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();

        currentWave++;
        currentWaveUI.text = "�غ�: " + currentWave.ToString();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            //����ָ����Χ�ڵ����ƫ����
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
            Vector3 spawnPosition = gameObject.transform.position + spawnOffset;

            //ʵ������ʬ
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            //��õ���(Enemy)�ű�
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            //��ؽ�ʬ����
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDeplay);

        }
    }

    private void Update()
    {
        //�����ȥ�Ľ�ʬ
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        //ʵ���Ƴ����������Ľ�ʬ
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();

        //������н�ʬ�����ˣ���ʼ��ȴ
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            //��ʼ��һ����ȴ
            StartCoroutine(WaveCooldown());
        }

        //������ȴ����ʱ
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            //������ȴ����ʱ
            cooldownCounter = waveCooldown;
        }

        cooldownCounterUI.text = cooldownCounter.ToString("F0");

    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        waveoverUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;
        waveoverUI.gameObject.SetActive(false);

        //��һ����������һ��������
        currentZombiesPerWave *= 2;

        StartNextWave();
    }
}
