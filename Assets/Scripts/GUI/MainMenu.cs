using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string nameEssentialScene;
    [SerializeField] string nameNewGameStartScene;

    [SerializeField] PlayerData playerData;

    public Gender selectedGender;
    public TMPro.TMP_Text genderText;
    public TMPro.TMP_InputField nameInputField;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject characterCreatePanel;
    public GameObject slotSelectionPanel;

    [Header("Slot UI Texts")]
    public TMPro.TMP_Text slot0Text;
    public TMPro.TMP_Text slot1Text;
    public TMPro.TMP_Text slot2Text;

    private enum MenuFlow { None, NewGame, LoadGame }
    private MenuFlow currentFlow = MenuFlow.None;
    private int pendingSlotId = -1;

    private void Start()
    {
        SetGenderFemale();
        // Mới vào game chỉ bật Main Menu
        mainMenuPanel.SetActive(true);
        characterCreatePanel.SetActive(false);
        slotSelectionPanel.SetActive(false);
    }

    public void ExitGame() { Application.Quit(); }

    public void SetGenderMale() { selectedGender = Gender.Male; genderText.text = "Male"; }
    public void SetGenderFemale() { selectedGender = Gender.Female; genderText.text = "Female"; }

    public void OnNewGameButtonClicked()
    {
        currentFlow = MenuFlow.NewGame;
        mainMenuPanel.SetActive(false);
        characterCreatePanel.SetActive(true);
    }

    public void OnLoadButtonClicked()
    {
        currentFlow = MenuFlow.LoadGame;
        mainMenuPanel.SetActive(false);
        slotSelectionPanel.SetActive(true);
        RefreshSlotUI();
    }

    public void OnStartButtonClicked()
    {
        if (currentFlow == MenuFlow.NewGame)
        {
            characterCreatePanel.SetActive(false);
            slotSelectionPanel.SetActive(true);
            RefreshSlotUI();
        }
        else if (currentFlow == MenuFlow.LoadGame)
        {
            CreateNewSaveDataAndStartGame(pendingSlotId);
        }
    }

    public void OnSlotButtonClicked(int slotNum)
    {
        if (currentFlow == MenuFlow.NewGame)
        {
            CreateNewSaveDataAndStartGame(slotNum);
        }
        else if (currentFlow == MenuFlow.LoadGame)
        {
            GameSaveData checkData = SaveManager.instance.GetSaveDataInfo(slotNum);

            if (checkData != null)
            {
                Debug.Log("Đang load game từ Slot " + slotNum);
                playerData.saveSlotId = slotNum;
                SaveManager.instance.LoadGame(slotNum);
                LoadGameScenes();
            }
            else
            {
                Debug.Log("Slot trống! Chuyển sang tạo nhân vật...");
                pendingSlotId = slotNum;
                slotSelectionPanel.SetActive(false);
                characterCreatePanel.SetActive(true);
            }
        }
    }


    private void CreateNewSaveDataAndStartGame(int slotId)
    {
        playerData.saveSlotId = slotId;
        playerData.characterName = nameInputField.text;
        playerData.playerCharacterGender = selectedGender;

        SaveManager.instance.ResetDataForNewGame();
        SaveManager.instance.SaveGame(slotId);

        LoadGameScenes();
    }

    private void LoadGameScenes()
    {
        SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Single);
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
    }

    public void RefreshSlotUI()
    {
        SetupSingleSlotText(0, slot0Text, "Slot 1");
        SetupSingleSlotText(1, slot1Text, "Slot 2");
        SetupSingleSlotText(2, slot2Text, "Slot 3");
    }

    private void SetupSingleSlotText(int slotId, TMPro.TMP_Text textComponent, string defaultTitle)
    {
        if (SaveManager.instance == null || textComponent == null) return;
        GameSaveData data = SaveManager.instance.GetSaveDataInfo(slotId);
        if (data != null)
        {
            string genderStr = (data.gender == (int)Gender.Male) ? "Male" : "Female";
            textComponent.text = $"{defaultTitle}\n{data.playerName} ({genderStr})";
        }
        else
        {
            textComponent.text = $"{defaultTitle}\n<color=gray>Trống (Empty)</color>";
        }
    }
}