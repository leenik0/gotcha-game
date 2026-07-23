using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GachaController : MonoBehaviour, Interactable
{
    public TMP_Text promptText;
    public GachaReward[] rewards;
    public int coinAmountNeeded = 5;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip leverClip;
    public AudioClip rattleClip;
    public AudioClip fanfare;
    public AudioClip cheerClip;

    [Header("Reward Menu")]
    public GameObject rewardPanel;
    public Image panelImage;
    public TMP_Text title;
    public TMP_Text rarity;
    public Image sprite;

    private bool canGacha = true;
    private PlayerInventory inventory;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        inventory = FindAnyObjectByType<PlayerInventory>();
    }
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
            panelImage.color = reward.GetColor();
            StartCoroutine(ShowReward());
        }
    }

    public void Interact()
    {
        if(inventory.GetCoinBalance() >= coinAmountNeeded)
        {
            inventory.SpendCoins(coinAmountNeeded);
            Gacha();

        }
    }

    private IEnumerator ShowReward()
    {
        // Click SFX
        if(leverClip)
        {
            audioSource.clip = leverClip;
            audioSource.Play();
            yield return new WaitUntil(() => audioSource.time >= leverClip.length);
        }

        // Rattle SFX
        if(rattleClip)
        {
            audioSource.clip = rattleClip;
            audioSource.Play();
            yield return new WaitUntil(() => audioSource.time >= rattleClip.length);
            yield return new WaitForSeconds(0.5f);
        }

        // Animation
        if(fanfare)
        {
            audioSource.clip = fanfare;
            audioSource.Play();
        }
        rewardPanel.transform.localScale = Vector3.zero;
        rewardPanel.transform.localRotation = Quaternion.identity;
        rewardPanel.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
        rewardPanel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        rewardPanel.SetActive(true);
        
        if(fanfare)
            yield return new WaitUntil(() => audioSource.time >= fanfare.length);
        
        if(cheerClip)
        {
            audioSource.clip = cheerClip;
            audioSource.Play();
        }

        yield return new WaitForSeconds(3f);

        rewardPanel.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
        rewardPanel.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => { rewardPanel.SetActive(false); });
        canGacha = true;
    }
}
