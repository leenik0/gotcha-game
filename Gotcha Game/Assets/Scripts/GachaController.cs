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
    public GameObject rewardPanel;
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
        rewardPanel.transform.localScale = Vector3.zero;
        rewardPanel.transform.localRotation = Quaternion.identity;
        rewardPanel.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
        rewardPanel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        rewardPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        rewardPanel.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => { rewardPanel.SetActive(false); });
        canGacha = true;
    }
}
