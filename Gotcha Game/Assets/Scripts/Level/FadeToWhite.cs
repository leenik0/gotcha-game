using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeToWhite : MonoBehaviour
{
    public Image fadeImage;

    private void Awake()
    {
        if (!fadeImage) fadeImage = GetComponent<Image>();
        Color c = fadeImage.color;
        c.a = 0;
        fadeImage.color = c;
    }

    public IEnumerator FadeIntoWhite(float duration)
    {
        yield return StartCoroutine(FadeRoutine(0f, 1f, duration));
    }

    public IEnumerator FadeFromWhite(float duration)
    {
        yield return StartCoroutine(FadeRoutine(1f, 0f, duration));
    }

    private IEnumerator FadeRoutine(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
}
