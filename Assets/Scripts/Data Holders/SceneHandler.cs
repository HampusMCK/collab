using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider LoadingBar;
    public TMP_Text LoadingStatus;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(loadingScreen);
    }

    public void loadNextScene(int SceneToLoad)
    {
        StartCoroutine(LoadScene(SceneToLoad));
    }

    IEnumerator LoadScene(int SceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToLoad);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBar.value = progressValue;
            LoadingStatus.text = (progressValue * 100).ToString() + "%";

            yield return null;
        }
    }
}