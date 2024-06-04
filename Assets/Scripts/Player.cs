using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;


    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            print("玩家死亡！");

            //游戏结束

            //重生玩家

            //死亡动画

        }
        else
        {
            print("玩家被攻击！");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
        }
    }
}
