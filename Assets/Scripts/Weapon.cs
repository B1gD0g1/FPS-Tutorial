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
    private const string ANIMATOR_ISADS = "isADS";
    private const string ANIMATOR_ENTERADS = "enterADS";
    private const string ANIMATOR_EXITADS = "exitADS";
    private const string ANIMATOR_RECOIL_ADS = "RECOIL_ADS";

    
    //�ж��Ƿ���������
    public bool isActiveWeapon;

    [Header("���")]
    //���
    public bool isShooting;
    public bool readyToShoot;
    public bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("����")]
    //����ģʽ
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    [Header("��ɢ")]
    //��ɢ Spread
    public float spreadIntersity;
    public float hipSpreadIntersity;//���侫׼��
    public float adsSpreadIntersity;//������׼��

    [Header("�ӵ�")]
    //�ӵ�
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;


    public GameObject muzzleEffect;
    internal Animator animator;

    [Header("����")]
    //���� reloading
    public float reloadTime;
    public int magazineSize;
    public int BulletsLeft;
    public bool isReloading;

    //�洢��һ�˳�����������
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    //��¼����ԭ�ȵ�scaleֵ
    public Vector3 spawmScale;

    private bool isADS;

    public enum WeaponModel
    {
        PistolM1911,
        M4,
        USP45,
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

        spreadIntersity = hipSpreadIntersity;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            EnterADS();
        }

        if (Input.GetMouseButtonUp(1))
        {
            ExitADS();
        }



        if (isActiveWeapon)
        {

            GetComponent<Outline>().enabled = false;


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


            if (Input.GetKeyDown(KeyCode.R) && BulletsLeft < magazineSize && isReloading == false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
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

        }
    }


    private void FireWeapon()
    {

        BulletsLeft--;


        //ǹ�ڿ�ǹ��Ч
        muzzleEffect.GetComponent<ParticleSystem>().Play();

        //����������
        if (isADS)
        {
            //��������������
            animator.SetTrigger(ANIMATOR_RECOIL_ADS);
        }
        else
        {
            //�������������
            animator.SetTrigger(ANIMATOR_RECOIL);
        }


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
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //һ��ʱ���ݻ��ӵ�
        StartCoroutine(DestroyBullerAfterTime(bullet, bulletPrefabLifeTime));


        //�������Ƿ���������
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //����ģʽ
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)//�ڼ��֮ǰ�����Ѿ����һ����
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
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

        if (WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            BulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(BulletsLeft, thisWeaponModel);
        }
        else
        {
            BulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(BulletsLeft, thisWeaponModel);
        }

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
        if (Physics.Raycast(ray, out hit))
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

        float x = UnityEngine.Random.Range(-spreadIntersity, spreadIntersity);
        float y = UnityEngine.Random.Range(-spreadIntersity, spreadIntersity);

        //��������������ɢ
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBullerAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    //���뿪��ģʽ
    private void EnterADS()
    {
        animator.SetTrigger(ANIMATOR_ENTERADS);
        isADS = true;
        HUDManager.Instance.middleDot.SetActive(false);
        spreadIntersity = adsSpreadIntersity;
    }

    //ȡ������ģʽ
    private void ExitADS()
    {
        animator.SetTrigger(ANIMATOR_EXITADS);
        isADS = false;
        HUDManager.Instance.middleDot.SetActive(true);
        spreadIntersity = hipSpreadIntersity;
    }

}
