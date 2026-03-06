using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [Header("Dữ liệu cần lưu/load")]
    public PlayerData playerData;
    public PlaceableObjectsContainer placeableObjectsContainer;
    public JSONStringList jsonStringList;
    public ItemContainer inventory;
    public ItemContainer startingInventoryTemplate;
    public ItemContainer globalStorage;
    public RecipeList knownRecipies;
    public CropsContainer cropsContainer;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (playerData != null)
            {
                SaveGame(playerData.saveSlotId);
                Debug.Log("<color=yellow>ĐÃ LƯU NHANH (QUICKSAVE) VÀO SLOT: " + playerData.saveSlotId + "</color>");
            }
        }
    }

    public void ResetDataForNewGame()
    {
        placeableObjectsContainer.placeableObjects.Clear();

        if (jsonStringList != null && jsonStringList.strings != null)
        {
            jsonStringList.strings.Clear();
        }

        if (inventory != null && startingInventoryTemplate != null)
        {
            inventory.slots.Clear();
            for (int i = 0; i < startingInventoryTemplate.slots.Count; i++)
            {
                ItemSlot newSlot = new ItemSlot();
                newSlot.item = startingInventoryTemplate.slots[i].item;
                newSlot.count = startingInventoryTemplate.slots[i].count;
                inventory.slots.Add(newSlot);
            }
        }

        if (globalStorage != null) globalStorage.slots.Clear();

        if (cropsContainer != null) cropsContainer.crops.Clear();

        Debug.Log("Đã dọn dẹp và set đồ mặc định để bắt đầu New Game!");
    }

    public void SaveGame(int slotId)
    {
        GameSaveData dataToSave = new GameSaveData();

        dataToSave.playerName = playerData.characterName;
        dataToSave.gender = (int)playerData.playerCharacterGender;
        dataToSave.placedObjects = placeableObjectsContainer.placeableObjects;
        dataToSave.jsonStrings = jsonStringList.strings;

        if (inventory != null) dataToSave.inventorySlots = inventory.slots;
        if (globalStorage != null) dataToSave.globalStorageSlots = globalStorage.slots;

        if (knownRecipies != null) dataToSave.savedRecipes = knownRecipies.recipes;
        if (cropsContainer != null) dataToSave.savedCrops = cropsContainer.crops;

        string json = JsonUtility.ToJson(dataToSave, true);

        string filePath = Application.persistentDataPath + "/SaveSlot_" + slotId + ".json";
        File.WriteAllText(filePath, json);

        Debug.Log("ĐÃ LƯU GAME THÀNH CÔNG VÀO: " + filePath);
    }

    public GameSaveData GetSaveDataInfo(int slotId)
    {
        string filePath = Application.persistentDataPath + "/SaveSlot_" + slotId + ".json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
            return data;
        }

        return null;
    }

    public void LoadGame(int slotId)
    {
        string filePath = Application.persistentDataPath + "/SaveSlot_" + slotId + ".json";

        if (File.Exists(filePath))
        {
            // 1. Đọc chuỗi JSON từ ổ cứng
            string json = File.ReadAllText(filePath);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

            // 2. Trả dữ liệu về lại cho các ScriptableObject
            playerData.characterName = data.playerName;

            // Ép kiểu số nguyên về lại Gender enum 
            playerData.playerCharacterGender = (Gender)data.gender;

            placeableObjectsContainer.placeableObjects = data.placedObjects;
            jsonStringList.strings = data.jsonStrings;

            if (inventory != null) inventory.slots = data.inventorySlots;
            if (globalStorage != null) globalStorage.slots = data.globalStorageSlots;

            if (knownRecipies != null && data.savedRecipes != null)
                knownRecipies.recipes = data.savedRecipes;

            if (cropsContainer != null && data.savedCrops != null)
                cropsContainer.crops = data.savedCrops;

            Debug.Log("ĐÃ TẢI GAME THÀNH CÔNG TỪ: " + filePath);
        }
        else
        {
            Debug.LogError("Không tìm thấy file save ở Slot " + slotId);
        }
    }
}

[System.Serializable]
public class GameSaveData
{
    public string playerName;
    public int gender;
    public List<PlaceableObject> placedObjects;
    public List<string> jsonStrings;

    public List<ItemSlot> inventorySlots;
    public List<ItemSlot> globalStorageSlots;

    public List<CraftingRecipe> savedRecipes;
    public List<CropTile> savedCrops;
}