using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaController : MonoBehaviour, Interactable
{
    public TMP_Text promptText;
    public GachaReward[] rewards;

    [Header("Reward Menu")]
    public GameObject canvas;
    public TMP_Text title;
    public TMP_Text rarity;
    public Image sprite;

    private bool canGacha = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            promptText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            promptText.gameObject.SetActive(false);
        }
    }

    public void Gacha()
    {
        if (canGacha)
        {
            canGacha = false;
            GachaReward reward = rewards[Random.Range(0, rewards.Length)];
            title.text = reward.GetName();
            rarity.text = reward.GetRarity();
            sprite.sprite = reward.GetSprite();
            StartCoroutine(ShowReward());
        }
    }

    public void Interact()
    {
        Gacha();
    }

    private IEnumerator ShowReward()
    {
        canvas.transform.localScale = Vector3.zero;
        canvas.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        canvas.SetActive(true);
        yield return new WaitForSeconds(3f);
        canvas.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => { canvas.SetActive(false); });
        canGacha = true;
    }
}
