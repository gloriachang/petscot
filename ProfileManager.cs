using UnityEngine;
using TMPro;

public class ProfileManager : MonoBehaviour
{
    public TMP_InputField affirmationsInput;
    public TextMeshProUGUI profileNameText;

    [Header("Generator References")]
    public TaskGenerator taskGenerator;
    public QuestGenerator questGenerator;

    void Start()
    {

        // Auto-regenerate when user finishes typing affirmations
        affirmationsInput.onEndEdit.AddListener((text) =>
        {
            PlayerData.affirmations = text;

            if (taskGenerator != null)
                taskGenerator.StartCoroutine(taskGenerator.GenerateTasks());

            if (questGenerator != null)
                questGenerator.StartCoroutine(questGenerator.GenerateQuests());
        });
    }
}