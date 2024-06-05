using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int bulletDamage;


    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Target"))
        {
            print("击中" + objectWeHit.gameObject.name + "!");

            CreatBulletImpactEffect(objectWeHit);

            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            print("击中一面墙");

            CreatBulletImpactEffect(objectWeHit);

            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Enemy"))
        {
            print("击中一个敌人");

            if (objectWeHit.gameObject.GetComponent<Enemy>().isDead == false)
            {
                objectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }

            CreateBloodSprayEffect(objectWeHit);

            Destroy(gameObject);
        }

    }

    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);

    }

    private void CreatBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffertPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(objectWeHit.gameObject.transform);


    }

}
