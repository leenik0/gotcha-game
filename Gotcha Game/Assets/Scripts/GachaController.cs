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

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip leverClip;
    public AudioClip rattleClip;
    public AudioClip fanfare;
    public AudioClip cheerClip;

    [Header("Reward Menu")]
    public GameObject rewardPanel;
    public TMP_Text title;
    public TMP_Text rarity;
    public Image sprite;

    private bool canGacha = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            StartCoroutine(ShowReward());
        }
    }

    public void Interact()
    {
        Gacha();
    }

    private IEnumerator ShowReward()
    {
        // Click SFX
        audioSource.clip = leverClip;
        audioSource.Play();
        yield return new WaitUntil(() => audioSource.time >= leverClip.length);

        // Rattle SFX
        audioSource.clip = rattleClip;
        audioSource.Play();
        yield return new WaitUntil(() => audioSource.time >= rattleClip.length);
        yield return new WaitForSeconds(0.5f);

        // Animation
        audioSource.clip = fanfare;
        audioSource.Play();
        rewardPanel.transform.localScale = Vector3.zero;
        rewardPanel.transform.localRotation = Quaternion.identity;
        rewardPanel.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
        rewardPanel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        rewardPanel.SetActive(true);
        
        yield return new WaitUntil(() => audioSource.time >= fanfare.length);
        audioSource.clip = cheerClip;
        audioSource.Play();

        yield return new WaitForSeconds(3f);

        rewardPanel.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
        rewardPanel.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => { rewardPanel.SetActive(false); });
        canGacha = true;
    }
}
