using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private const string ANIMATOR_RECOIL = "RECOIL";

    //射击
    public bool isShooting;
    public bool readyToShoot;
    public bool allowReset = true;
    public float shootingDelay = 2f;

    //连发模式
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //散射 Spread
    public float spreadIntersity; 

    //子弹
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;


    public GameObject muzzleEffect;
    private Animator animator;


    public enum ShootingMode
    {
        Single,
        Burst,
        Auto,
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            //长按鼠标左键发射子弹
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            //点按鼠标左键发射子弹
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }


        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        //枪口开枪特效
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        //后坐力动画
        animator.SetTrigger(ANIMATOR_RECOIL);

        //枪声实例化，单例模式
        SoundManager.Instance.shootingSoundM1911.Play();


        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;


        //实例化子弹
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);


        //将子弹对准射击方向
        bullet.transform.forward = shootingDirection;


        //射出子弹
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity , ForceMode.Impulse);

        //一段时间后摧毁子弹
        StartCoroutine(DestroyBullerAfterTime(bullet,bulletPrefabLifeTime));


        //检查玩家是否完成了射击
        if (allowReset)
        {
            Invoke("ResetShot",shootingDelay);
            allowReset = false;
        }

        //连发模式
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)//在检查之前我们已经射击一次了
        {
            burstBulletsLeft--;
            Invoke("FireWeapon",shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        //从屏幕中间射击来检查我们所指的位置
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray,out hit))
        {
            //击中一些东西
            targetPoint = hit.point;
        }
        else
        {
            //向空中射击
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntersity,spreadIntersity);
        float y = UnityEngine.Random.Range(-spreadIntersity,spreadIntersity);

        //返回射击方向和扩散
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBullerAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
