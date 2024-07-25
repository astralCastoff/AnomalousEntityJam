using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int)SceneIndices.MAIN_MENU, LoadSceneMode.Additive);
    }

    [SerializeField] GameObject loadingScreen;
    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadGame()
    {
        loadingScreen.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndices.MAIN_MENU));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndices.GAMEPLAY, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    [SerializeField] Slider progressBar;
    float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0f;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count);
                progressBar.value = 1f - totalSceneProgress;
                Debug.Log(totalSceneProgress);

                yield return null;
            }
        }
        loadingScreen.SetActive(false);
    }
}
