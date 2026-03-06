using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuPanel;

    [Header("Scene Names")]
    public string mainMenuSceneName = "MainMenuScene";

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void SaveGame()
    {
        if (SaveManager.instance != null && SaveManager.instance.playerData != null)
        {
            SaveManager.instance.SaveGame(SaveManager.instance.playerData.saveSlotId);
            Debug.Log("<color=green>ĐÃ LƯU GAME THÀNH CÔNG VÀO SLOT: " + SaveManager.instance.playerData.saveSlotId + "</color>");
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Debug.Log("Đang thoát game ra Desktop...");
        Application.Quit();
    }
}