using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaController : MonoBehaviour
{
    public TMP_Text promptText;

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
        //
    }
}
