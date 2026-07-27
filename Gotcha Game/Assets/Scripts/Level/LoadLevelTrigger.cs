using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelTrigger : MonoBehaviour
{

    public int nextLevelIndex = -1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (nextLevelIndex == -1)
            nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextLevelIndex);
        else
            Debug.Log("Scene Index Out of Bound: " + nextLevelIndex);
    }
}
