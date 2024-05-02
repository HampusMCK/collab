using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    public List<GameObject> DontDestroy;
    public GameObject loadingScreen;
    public Slider LoadingBar;
    public TMP_Text LoadingStatus;
    public GameObject player;
    Vector3 spawn;
    private void Awake()
    {
        foreach (GameObject g in DontDestroy)
        {
            DontDestroyOnLoad(g);
        }
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        GameObject g = GameObject.Find("Button");
        if (g != null)
        {
            g.GetComponent<Button>().onClick.RemoveAllListeners();
            g.GetComponent<Button>().onClick.AddListener(() => loadNextScene(1));
        }
    }

    public void loadNextScene(int SceneToLoad)
    {
        if (SceneToLoad == 2)
            spawn = new Vector3(8, 2, 8);
        if (SceneToLoad == 1)
            spawn = new Vector3(0, 2, 0);
        StartCoroutine(LoadScene(SceneToLoad));
    }

    IEnumerator LoadScene(int SceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToLoad);

        loadingScreen.SetActive(true);

        player.transform.position = spawn;

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBar.value = progressValue;
            LoadingStatus.text = (progressValue * 100).ToString() + "%";

            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}