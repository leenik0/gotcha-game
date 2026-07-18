using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    public void LoadGame()
    {
        Debug.Log("Game loaded");
    }

    public void StartNewGame()
    {
        Debug.Log("New game started");
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Game quit");
        Application.Quit();
    }
}
