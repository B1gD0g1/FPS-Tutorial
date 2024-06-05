using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    
    public static GlobalReferences Instance { get; set; }

    //�ӵ���Ч
    public GameObject bulletImpactEffertPrefab;
    //������Ч
    public GameObject grenadeExplosionEffect;
    public GameObject smokeGrenadeEffect;

    //��Ѫ��Ч
    public GameObject bloodSprayEffect;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

}
