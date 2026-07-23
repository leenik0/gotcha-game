using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnterNextLevel : MonoBehaviour
{
    public float fadeDuration = 1f;
    public Image screenFader;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FadeRoutine(1f));
        }
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = screenFader.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);

            screenFader.color = new Color(1f, 1f, 1f, newAlpha);

            yield return null;
        }
        screenFader.color = new Color(1f, 1f, 1f, targetAlpha);
    }
}
