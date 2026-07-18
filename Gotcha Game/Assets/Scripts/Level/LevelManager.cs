using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { get; private set; }

    public Transform levelStartPosition;
    private Vector3 respawnPoint;

    public void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (levelStartPosition)
            respawnPoint = levelStartPosition.position;
        else
            respawnPoint = Vector3.zero;
    }

    // returns the respawn point so that the player can respawn in the correct location
    public Vector3 GetRespawnPosition()
    {
        if(respawnPoint != null)
            return respawnPoint;
        return Vector3.zero;
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void SetRespawnPoint(Vector3 respawn)
    {
        respawnPoint = respawn;
    }
}
