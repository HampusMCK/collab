using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject PMenu;
    public GameObject SettingsMenu;
    public Slider MouseSense;
    public TMP_Text mouseTXT;

    WorldSC world;
    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.Find("World").GetComponent<WorldSC>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseTXT.text = "Mouse Sensetivity: " + MouseSense.value.ToString("F1");
    }

    public void ResumefromPause()
    {
        gameObject.SetActive(false);
        world.inUI = false;
    }

    public void openSettings()
    {
        PMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void closeSettings()
    {
        SettingsMenu.SetActive(false);
        PMenu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}