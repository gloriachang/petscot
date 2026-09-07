using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [Header("Task Toggles (Checkboxes)")]
    public Toggle task1Toggle;
    public Toggle task2Toggle;
    public Toggle task3Toggle;

    [Header("Task Text")]
    public TextMeshProUGUI task1Text;
    public TextMeshProUGUI task2Text;
    public TextMeshProUGUI task3Text;

    [Header("Points Display")]
    public TextMeshProUGUI pointsText;

    [Header("Level Display")]
    public TextMeshProUGUI levelText;

    private int totalPoints = 0;
    private int currentLevel = 1;


    void Start()
    {
        task1Toggle.isOn = false;
        task2Toggle.isOn = false;
        task3Toggle.isOn = false;
        UpdatePointsDisplay();

        task1Toggle.onValueChanged.AddListener((isOn) => OnTaskToggled(isOn, task1Text, 10));
        task2Toggle.onValueChanged.AddListener((isOn) => OnTaskToggled(isOn, task2Text, 10));
        task3Toggle.onValueChanged.AddListener((isOn) => OnTaskToggled(isOn, task3Text, 10));
    }

    void OnTaskToggled(bool isOn, TextMeshProUGUI text, int points)
    {
        if (isOn)
        {
            totalPoints += points;
            // TMP strikethrough tag
            text.text = "<s>" + text.text + "</s>";
            text.color = new Color(0.6f, 0.6f, 0.6f, 1f);
        }
        else
        {
            totalPoints -= points;
            // Remove strikethrough if unchecked
            text.text = text.text.Replace("<s>", "").Replace("</s>", "");
            text.color = Color.black;
        }

        UpdatePointsDisplay();
    }

    void UpdatePointsDisplay()
    {
        if (pointsText) pointsText.text = totalPoints + " stars";
        int newLevel = (totalPoints / 100) + 1;
        if (newLevel != currentLevel)
        {
            currentLevel = newLevel;
            // Optional: Debug to confirm it's working
            Debug.Log("Level up! Now level " + currentLevel);
        }

        if (levelText) levelText.text = "Level " + currentLevel;
    }

    public void AddPoints(int amount)
    {
        totalPoints += amount;
        UpdatePointsDisplay();
    }
}
