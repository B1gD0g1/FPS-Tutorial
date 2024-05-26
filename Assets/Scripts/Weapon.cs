using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    private const string ANIMATOR_RECOIL = "RECOIL";
    private const string ANIMATOR_RELOAD = "RELOAD";

    //判断是否拿着武器
    public bool isActiveWeapon;

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


    //换弹 reloading
    public float reloadTime;
    public int magazineSize;
    public int BulletsLeft;
    public bool isReloading;

    //存储第一人称武器的坐标
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    


    public enum WeaponModel
    {
        PistolM1911,
        M4,
    }

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto,
    }

    public WeaponModel thisWeaponModel;
    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();


        BulletsLeft = magazineSize;


    }

    // Update is called once per frame
    void Update()
    {

        if (isActiveWeapon)
        {
            //空弹匣情况下按射击发出的声音
            if (BulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagazineSoundM1911.Play();
            }


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


            if (Input.GetKeyDown(KeyCode.R) && BulletsLeft < magazineSize && isReloading == false)
            {
                Reload();
            }

            ////空弹匣时自动重新装填
            //if (readyToShoot && isShooting == false && isReloading == false && BulletsLeft <= 0)
            //{
            //    Reload();
            //}


            if (readyToShoot && isShooting && BulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }


            if (AmmoManager.Instance.ammoDisplay != null)
            {
                AmmoManager.Instance.ammoDisplay.text = $"{BulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
            }

        }
    }

    private void FireWeapon()
    {

        BulletsLeft--;


        //枪口开枪特效
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        //后坐力动画
        animator.SetTrigger(ANIMATOR_RECOIL);

        //枪声实例化，单例模式
        //SoundManager.Instance.shootingSoundM1911.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);


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

    private void Reload()
    {
        //换弹声音
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        animator.SetTrigger(ANIMATOR_RELOAD);

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        BulletsLeft = magazineSize;
        isReloading = false;
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
