using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Slider MouseSenseSlider;
    public TMP_Text mouseSenseTXT;
    public GameObject mainUI;
    public GameObject SettingsUI;

    [Header("Loading Screen References")]
    public GameObject loadingScreen;
    public Image loadingFill;
    public TMP_Text loadingStatus;

    public Settings settings;

    private void Awake()
    {
        if (!File.Exists(Application.dataPath + "/settings.cfg"))
        {
            settings = new Settings();
            string jsonExport = JsonUtility.ToJson(settings);
            File.WriteAllText(Application.dataPath + "/settings.cfg", jsonExport);
        }
        else
        {
            string jsonImport = File.ReadAllText(Application.dataPath + "/settings.cfg");
            settings = JsonUtility.FromJson<Settings>(jsonImport);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SettingsUI.activeSelf)
            UpdateMouseSlider();
    }

    public void SettingsOC(int instegator)
    {
        switch (instegator)
        {
            case 0:
                mainUI.SetActive(false);
                SettingsUI.SetActive(true);
                MouseSenseSlider.value = settings.mouseSensetivity;

                break;
            case 1:
                mainUI.SetActive(true);
                SettingsUI.SetActive(false);
                settings.mouseSensetivity = MouseSenseSlider.value;

                string jsonExport = JsonUtility.ToJson(settings);
                File.WriteAllText(Application.dataPath + "/settings.cfg", jsonExport);

                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play(int SceneID)
    {
        StartCoroutine(LoadSceneAsync(SceneID));
    }

    IEnumerator LoadSceneAsync(int SceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneID);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingFill.fillAmount = progressValue;
            loadingStatus.text = (progressValue * 100).ToString() + "%";

            yield return null;
        }
    }

    public void UpdateMouseSlider()
    {
        mouseSenseTXT.text = "Mouse Sensitivity: " + MouseSenseSlider.value.ToString("F1");
    }
}