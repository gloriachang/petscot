using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class TaskGenerator : MonoBehaviour
{
    private string apiKey = "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";
    private string apiUrl = "https://api.anthropic.com/v1/messages";

    public TextMeshProUGUI task1Text;
    public TextMeshProUGUI task2Text;
    public TextMeshProUGUI task3Text;
    [Header("Pet Name")]
    public TMP_InputField petNameInputField;
    public TextMeshProUGUI petNameDisplay;

    [Header("Major")]
    public TMP_InputField majorInputField;

    public Button startButton;



    void Start()
    {

        // Save pet name and major when user confirms onboarding
        startButton.onClick.AddListener(() =>
        {
            if (petNameInputField.text.Length > 0)
                PlayerData.petName = petNameInputField.text;

            if (majorInputField.text.Length > 0)
                PlayerData.major = majorInputField.text;

            if (petNameDisplay)
                petNameDisplay.text = PlayerData.petName;

            StartCoroutine(GenerateTasks());
        });
    }

    public IEnumerator GenerateTasks()
    {
        string affirmationContext = PlayerData.affirmations.Length > 0
            ? " The student has these personal goals and affirmations: " + PlayerData.affirmations + "."
            : "";
        string prompt = "Generate exactly 3 daily tasks for a college student majoring in " + PlayerData.major + ". " + affirmationContext +
                    "One task must be academic and specific to " + PlayerData.major + ", one self-care, and one professional development relevant to their field. " +
                    "Must be able to be planned and achieved within one day. Hard maximum of 20 characters per task. Use this exact format:\\nTASK1: [task]\\nTASK2: [task]\\nTASK3: [task]";

        string jsonBody = "{\"model\": \"claude-haiku-4-5-20251001\", \"max_tokens\": 200, \"messages\": [{\"role\": \"user\", \"content\": \"" + prompt + "\"}]}";;

        var request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-api-key", apiKey);
        request.SetRequestHeader("anthropic-version", "2023-06-01");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ParseAndDisplayTasks(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("API Error: " + request.error);
        }
    }

    void ParseAndDisplayTasks(string jsonResponse)
    {
        // Quick and dirty parse for hackathon
        int contentIndex = jsonResponse.IndexOf("\"text\":\"") + 8;
        int contentEnd = jsonResponse.IndexOf("\"", contentIndex);
        string content = jsonResponse.Substring(contentIndex, contentEnd - contentIndex);
        content = content.Replace("\\n", "\n");

        string[] lines = content.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("TASK1:") && task1Text) task1Text.text = line.Replace("TASK1:", "").Trim();
            if (line.StartsWith("TASK2:") && task2Text) task2Text.text = line.Replace("TASK2:", "").Trim();
            if (line.StartsWith("TASK3:") && task3Text) task3Text.text = line.Replace("TASK3:", "").Trim();
        }
    }
}
