using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    //��ʧʱ��
    public float fadeDuration = 7.0f;

    public void StartFade()
    {
        StartCoroutine(FadeOut());
        fadeImage.gameObject.SetActive(true);
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f); // Black with alpha 1.

        while (timer < fadeDuration)
        {
            // Interpolate the color between start and end over time.����ʱ��������ڿ�ʼ�ͽ���֮�������ɫ��
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the image is completely black at the end. ȷ��ͼ�����������ȫ��ɫ�ġ�
        fadeImage.color = endColor;
    }
}
