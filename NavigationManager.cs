using UnityEngine;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{
    [Header("Pages")]
    public GameObject startPage;
    public GameObject onboardingPage;
    public GameObject mainPage;
    public GameObject questsPage;
    public GameObject friendsPage;
    public GameObject profilePage;

    [Header("Navigation Bar")]
    public GameObject navigationBar;

    [Header("Flow Buttons")]
    public Button startButton;      // Button on StartPage
    public Button onboardingButton; // "Let's Go" button on OnboardingPage

    [Header("Nav Bar Buttons")]
    public Button mainNavButton;
    public Button questsNavButton;
    public Button friendsNavButton;
    public Button profileNavButton;

    void Start()
    {
        // Start with StartPage only
        startPage.SetActive(true);
        onboardingPage.SetActive(false);
        mainPage.SetActive(false);
        questsPage.SetActive(false);
        friendsPage.SetActive(false);
        profilePage.SetActive(false);
        navigationBar.SetActive(false);

        // Flow buttons
        startButton.onClick.AddListener(GoToOnboarding);
        onboardingButton.onClick.AddListener(GoToMain);

        // Nav bar buttons
        mainNavButton.onClick.AddListener(() => ShowMainPage(mainPage));
        questsNavButton.onClick.AddListener(() => ShowMainPage(questsPage));
        friendsNavButton.onClick.AddListener(() => ShowMainPage(friendsPage));
        profileNavButton.onClick.AddListener(() => ShowMainPage(profilePage));
    }

    void GoToOnboarding()
    {
        startPage.SetActive(false);
        onboardingPage.SetActive(true);
        navigationBar.SetActive(false);
    }

    void GoToMain()
    {
        onboardingPage.SetActive(false);
        mainPage.SetActive(true);
        navigationBar.SetActive(true);
    }

    void ShowMainPage(GameObject pageToShow)
    {
        mainPage.SetActive(false);
        questsPage.SetActive(false);
        friendsPage.SetActive(false);
        profilePage.SetActive(false);

        pageToShow.SetActive(true);
    }
}