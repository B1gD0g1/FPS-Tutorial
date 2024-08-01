using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;

    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    public bool isDead;


    private void Start()
    {
        playerHealthUI.text = $"����ֵ: {HP}";
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            print("���������");
            PlayerDead();
            isDead = true;

        }
        else
        {
            print("��ұ�������");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"Health: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        //��������
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDie);

        //��������������ƶ��ű�
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        //��������
        GetComponentInChildren<Animator>().enabled = true;
        playerHealthUI.gameObject.SetActive(false);

        //��Ļ����Ч��
        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOverUI());

    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
    }

    private IEnumerator BloodyScreenEffect()
    {
        //�ж���Ļ�Ƿ������ֽ�ѪЧ��
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible). ���ó�ʼalphaֵΪ1(��ȫ�ɼ�)
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;//����ʱ��
        float elapsedTime = 0f;//����ʱ��

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp. ʹ��Lerp�����µ�alphaֵ
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value. ʹ���µ�alphaֵ������ɫ��
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time. ��������ʱ�䡣
            elapsedTime += Time.deltaTime;

            yield return null; ; // Wait for the next frame.�ȴ���һ֡
        }

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {

            if (isDead == false)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }
}
