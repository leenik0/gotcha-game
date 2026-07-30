using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadNextScene : MonoBehaviour
{
    public FadeToWhite fadeToWhite;
    public int nextScene;
    private LevelManager levelManager;
    private bool isTransitioning = false;

    void Start()
    {
        GameObject parentObj = transform.parent.gameObject;
        levelManager = parentObj.GetComponent<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Fading to white");
        StartCoroutine(TransitionToNextLevel());
    }

    private IEnumerator TransitionToNextLevel()
    {
        Debug.Log("Fading to white");
        yield return StartCoroutine(fadeToWhite.FadeIntoWhite(1f));
        levelManager.LoadLevel(nextScene);
    }
}
