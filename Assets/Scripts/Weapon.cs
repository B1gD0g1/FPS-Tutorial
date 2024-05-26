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

    //�ж��Ƿ���������
    public bool isActiveWeapon;

    //���
    public bool isShooting;
    public bool readyToShoot;
    public bool allowReset = true;
    public float shootingDelay = 2f;

    //����ģʽ
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //ɢ�� Spread
    public float spreadIntersity; 

    //�ӵ�
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;


    public GameObject muzzleEffect;
    private Animator animator;


    //���� reloading
    public float reloadTime;
    public int magazineSize;
    public int BulletsLeft;
    public bool isReloading;

    //�洢��һ�˳�����������
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
            //�յ�ϻ����°��������������
            if (BulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagazineSoundM1911.Play();
            }


            if (currentShootingMode == ShootingMode.Auto)
            {
                //���������������ӵ�
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                //�㰴�����������ӵ�
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }


            if (Input.GetKeyDown(KeyCode.R) && BulletsLeft < magazineSize && isReloading == false)
            {
                Reload();
            }

            ////�յ�ϻʱ�Զ�����װ��
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


        //ǹ�ڿ�ǹ��Ч
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        //����������
        animator.SetTrigger(ANIMATOR_RECOIL);

        //ǹ��ʵ����������ģʽ
        //SoundManager.Instance.shootingSoundM1911.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);


        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;


        //ʵ�����ӵ�
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);


        //���ӵ���׼�������
        bullet.transform.forward = shootingDirection;


        //����ӵ�
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity , ForceMode.Impulse);

        //һ��ʱ���ݻ��ӵ�
        StartCoroutine(DestroyBullerAfterTime(bullet,bulletPrefabLifeTime));


        //�������Ƿ���������
        if (allowReset)
        {
            Invoke("ResetShot",shootingDelay);
            allowReset = false;
        }

        //����ģʽ
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)//�ڼ��֮ǰ�����Ѿ����һ����
        {
            burstBulletsLeft--;
            Invoke("FireWeapon",shootingDelay);
        }
    }

    private void Reload()
    {
        //��������
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
        //����Ļ�м���������������ָ��λ��
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray,out hit))
        {
            //����һЩ����
            targetPoint = hit.point;
        }
        else
        {
            //��������
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntersity,spreadIntersity);
        float y = UnityEngine.Random.Range(-spreadIntersity,spreadIntersity);

        //��������������ɢ
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBullerAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
