using UnityEngine;
using DG.Tweening;

public class PopupAnimation : MonoBehaviour
{
    private void OpenPopup()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }

    public void ClosePopup()
    {
        transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
