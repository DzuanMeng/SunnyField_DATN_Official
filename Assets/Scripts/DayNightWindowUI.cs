using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DayNightWindowUI : MonoBehaviour
{
    [Header("Tham chiếu")]
    [SerializeField] private DayTimeController timeController;
    [SerializeField] private Image windowDisplay;

    [Header("Dữ liệu hình ảnh")]
    [SerializeField] private List<Sprite> spritesByHour;

    private void Update()
    {
        if (timeController == null || windowDisplay == null || spritesByHour.Count == 0) return;

        float currentHour = timeController.Hours;

        int index = Mathf.FloorToInt(currentHour);

        index = Mathf.Clamp(index, 0, spritesByHour.Count - 1);

        // Cập nhật Sprite
        if (windowDisplay.sprite != spritesByHour[index])
        {
            windowDisplay.sprite = spritesByHour[index];
        }
    }
}