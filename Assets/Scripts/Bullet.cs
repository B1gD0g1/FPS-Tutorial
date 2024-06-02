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

        if (objectWeHit.gameObject.CompareTag("Zombie"))
        {
            print("击中一个敌人");

            objectWeHit.gameObject.GetComponent<Zombie>().TakeDamage(bulletDamage);

            Destroy(gameObject);
        }

    }


    void CreatBulletImpactEffect(Collision objectWeHit)
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
