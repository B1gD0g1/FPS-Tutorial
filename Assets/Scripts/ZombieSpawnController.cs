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


    public float spawnDeplay = 0.5f;//在每一波中产生每个僵尸之间的延迟

    public int currentWave = 0;
    public float waveCooldown = 10.0f;//每一波之间的时间

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
        currentWaveUI.text = "回合: " + currentWave.ToString();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            //生成指定范围内的随机偏移量
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
            Vector3 spawnPosition = gameObject.transform.position + spawnOffset;

            //实例化僵尸
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            //获得敌人(Enemy)脚本
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            //监控僵尸数量
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDeplay);

        }
    }

    private void Update()
    {
        //获得死去的僵尸
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        //实际移除所有死亡的僵尸
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();

        //如果所有僵尸都死了，开始冷却
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            //开始下一波冷却
            StartCoroutine(WaveCooldown());
        }

        //运行冷却倒计时
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            //重置冷却倒计时
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

        //下一波数量是上一波的两倍
        currentZombiesPerWave *= 2;

        StartNextWave();
    }
}
