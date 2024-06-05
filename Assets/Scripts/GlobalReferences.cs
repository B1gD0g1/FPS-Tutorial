using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    
    public static GlobalReferences Instance { get; set; }

    //子弹特效
    public GameObject bulletImpactEffertPrefab;
    //手榴弹特效
    public GameObject grenadeExplosionEffect;
    public GameObject smokeGrenadeEffect;

    //流血特效
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
