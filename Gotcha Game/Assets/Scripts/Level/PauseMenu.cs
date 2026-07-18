using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObject;
    private bool gamePaused;

    private PlayerMechanics inputActions;

    private void Awake()
    {
        inputActions = new PlayerMechanics();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!pauseMenuObject)
            pauseMenuObject = transform.GetChild(0).gameObject;

        SetPauseMenuState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(inputActions.Default.Pause.triggered)
        {
            Debug.Log("Pause Menu Triggered");
            SetPauseMenuState(!gamePaused);
        }
    }

    private void SetPauseMenuState(bool pause = true)
    {

        if(!pauseMenuObject)
        {
            Debug.Log("[Warning] Pause Menu Object Missing");
            return;
        }

        pauseMenuObject.SetActive(pause);
        Time.timeScale = pause ? 0 : 1;

        gamePaused = pause;
    }

    public void OnClickResume()
    {
        SetPauseMenuState(false);
    }
    public void OnClickSettings()
    {
        // have another menu gameobject lmao
    }
    public void OnClickMainMenu()
    {
        // [TODO: smth smth consent forward design]
        // [TODO: ensure that the main menu is in index 0
        Time.timeScale = 1f;
        LevelManager.Instance.LoadLevel(0);
    }
    public void OnClickQuit()
    {
        // [TODO: smth smth consent forward design]
        Time.timeScale = 1f;
        Application.Quit();
    }
}
