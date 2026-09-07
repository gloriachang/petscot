using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class QuestGenerator : MonoBehaviour
{
    private string apiKey = "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";
    private string apiUrl = "https://api.anthropic.com/v1/messages";

    [Header("Quest Toggles")]
    public Toggle quest1Toggle;
    public Toggle quest2Toggle;
    public Toggle quest3Toggle;
    public Toggle quest4Toggle;

    [Header("Quest Texts")]
    public TextMeshProUGUI quest1Text;
    public TextMeshProUGUI quest2Text;
    public TextMeshProUGUI quest3Text;
    public TextMeshProUGUI quest4Text;

    [Header("Points Reference")]
    public TaskManager taskManager;

    void Start()
    {
        quest1Toggle.isOn = false;
        quest2Toggle.isOn = false;
        quest3Toggle.isOn = false;
        quest4Toggle.isOn = false;

        if (quest1Text) quest1Text.text = "Loading quest...";
        if (quest2Text) quest2Text.text = "Loading quest...";
        if (quest3Text) quest3Text.text = "Loading quest...";
        if (quest4Text) quest4Text.text = "Loading quest...";

        quest1Toggle.onValueChanged.AddListener((isOn) => OnQuestToggled(isOn, quest1Text, 30));
        quest2Toggle.onValueChanged.AddListener((isOn) => OnQuestToggled(isOn, quest2Text, 30));
        quest3Toggle.onValueChanged.AddListener((isOn) => OnQuestToggled(isOn, quest3Text, 30));
        quest4Toggle.onValueChanged.AddListener((isOn) => OnQuestToggled(isOn, quest4Text, 30));

        StartCoroutine(GenerateQuests());
    }

    void OnQuestToggled(bool isOn, TextMeshProUGUI text, int points)
    {
        if (isOn)
        {
            text.text = "<s>" + text.text + "</s>";
            text.color = new Color(0.6f, 0.6f, 0.6f, 1f);
            if (taskManager) taskManager.AddPoints(points);
        }
        else
        {
            text.text = text.text.Replace("<s>", "").Replace("</s>", "");
            text.color = Color.white;
            if (taskManager) taskManager.AddPoints(-points);
        }
    }

    public IEnumerator GenerateQuests()
    {
        yield return new WaitForSeconds(0.5f);

        string major = PlayerData.major.Length > 0 ? PlayerData.major : "undeclared";
        string affirmationContext = PlayerData.affirmations.Length > 0
            ? " The student has these personal goals and affirmations: " + PlayerData.affirmations + "."
            : "";

        string prompt = "Generate exactly 4 long-term quests for a college student majoring in " + PlayerData.major + ". " + affirmationContext +
                        "One quest must be academic, one self-improvement, one professional development, and one social/community goal. " +
                        "Each quest should be achievable within 15-30 days and specific to their field. " +
                        "Keep each quest under 35 characters. " +
                        "Use this exact format with nothing else: " +
                        "QUEST1: [quest] QUEST2: [quest] QUEST3: [quest] QUEST4: [quest]";

        string jsonBody = "{\"model\": \"claude-haiku-4-5-20251001\", \"max_tokens\": 250, \"messages\": [{\"role\": \"user\", \"content\": \"" + prompt + "\"}]}";

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
            ParseAndDisplayQuests(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Quest API Error: " + request.downloadHandler.text);
            if (quest1Text) quest1Text.text = "Complete a research paper";
            if (quest2Text) quest2Text.text = "Build a daily workout habit";
            if (quest3Text) quest3Text.text = "Apply to 3 internships";
            if (quest4Text) quest4Text.text = "Join a club or organization";
        }
    }

    void ParseAndDisplayQuests(string jsonResponse)
    {
        try
        {
            int textIndex = jsonResponse.IndexOf("\"text\":\"") + 8;
            int textEnd = jsonResponse.IndexOf("\"", textIndex);
            string content = jsonResponse.Substring(textIndex, textEnd - textIndex);
            content = content.Replace("\\n", "\n").Replace("\\r", "");

            string[] parts = content.Split(new string[] { "QUEST1:", "QUEST2:", "QUEST3:", "QUEST4:" },
                             System.StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 4)
            {
                if (quest1Text) quest1Text.text = parts[0].Trim();
                if (quest2Text) quest2Text.text = parts[1].Trim();
                if (quest3Text) quest3Text.text = parts[2].Trim();
                if (quest4Text) quest4Text.text = parts[3].Trim();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Parse error: " + e.Message);
            if (quest1Text) quest1Text.text = "Complete a research paper";
            if (quest2Text) quest2Text.text = "Build a daily workout habit";
            if (quest3Text) quest3Text.text = "Apply to 3 internships";
            if (quest4Text) quest4Text.text = "Join a club or organization";
        }
    }
}