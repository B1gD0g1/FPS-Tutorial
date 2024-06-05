using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieHand zombieHand;

    public int zombieDamage;

    public float playZombieSoundArea = 40f;

    private void Start()
    {
        zombieHand.damage = zombieDamage;
    }

}
