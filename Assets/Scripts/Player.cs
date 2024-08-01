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
        playerHealthUI.text = $"生命值: {HP}";
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            print("玩家死亡！");
            PlayerDead();
            isDead = true;

        }
        else
        {
            print("玩家被攻击！");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"Health: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        //死亡声音
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDie);

        //死亡后禁用鼠标和移动脚本
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        //死亡动画
        GetComponentInChildren<Animator>().enabled = true;
        playerHealthUI.gameObject.SetActive(false);

        //屏幕淡出效果
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
        //判断屏幕是否有这种溅血效果
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible). 设置初始alpha值为1(完全可见)
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;//持续时间
        float elapsedTime = 0f;//运行时间

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp. 使用Lerp计算新的alpha值
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value. 使用新的alpha值更新颜色。
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time. 增加运行时间。
            elapsedTime += Time.deltaTime;

            yield return null; ; // Wait for the next frame.等待下一帧
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
