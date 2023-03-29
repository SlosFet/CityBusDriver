using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image LoadingBarFill;
    public TextMeshProUGUI LoadingText;


    private void Start()
    {
        //Usually I leave it 60 but my phone has 120hz screen.So with this way I get better performance.For old devices 60 is enough
        Application.targetFrameRate = 144;
    }

    /// <summary>
    /// This loads next scene on build settings
    /// </summary>
    public void NextScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    //Basic loader scene coroutine
    IEnumerator LoadSceneAsync(int sceneId)
    {
        LoadingScreen.SetActive(true);
        LoadingBarFill.fillAmount = 0;
        LoadingText.text = "LOADING %0";
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingText.text = "LOADING %" + progressValue * 100;
            LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
        LoadingBarFill.fillAmount = 1;
        LoadingText.text = "LOADING %100";

    }
}