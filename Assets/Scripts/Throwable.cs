using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float delay = 3f;
    [SerializeField] private float damageRadius = 20f;
    [SerializeField] private float explosionForce = 2000f;


    private float countdown;

    private bool hasExplosed = false;
    public bool hasBeenThrown = false;


    public enum ThrowableType
    {
        None,
        Grenade,
        Smoke_Grenade
    }

    public ThrowableType throwableType;



    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExplosed)
            {
                Explode();
                hasExplosed = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();

        Destroy(this.gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
            case ThrowableType.Smoke_Grenade:
                SmokeGrenadeEffect();
                break;
        }
    }

    private void SmokeGrenadeEffect()
    {

        //视觉效果 Visual Effect
        GameObject smokeEffect = GlobalReferences.Instance.smokeGrenadeEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);

        //声音
        SoundManager.Instance.throwableChannel.PlayOneShot(SoundManager.Instance.grenadeSound);


        //物理效果 Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //使敌人暂时失明

            }
        }
    }

    private void GrenadeEffect()
    {
        //视觉效果 Visual Effect
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //声音
        SoundManager.Instance.throwableChannel.PlayOneShot(SoundManager.Instance.grenadeSound);


        //物理效果 Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            //同时对范围内的敌人造成伤害

        }

    }

}
