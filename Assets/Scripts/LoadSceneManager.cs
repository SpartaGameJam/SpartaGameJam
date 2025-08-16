using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    MainMenu,
    Ingame,
    Ending,
    None
}

public class LoadSceneManager : MonoBehaviour
{
    private static LoadSceneManager instance;
    public static LoadSceneManager Instance => instance;

    public SceneName curSceneName {  get; private set; }

    private void Awake()
    {
        if (Instance == null) instance = this;
        //LoadScene(SceneName.UI);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(SceneName sn)
    {
        SceneManager.LoadSceneAsync((int)sn, LoadSceneMode.Additive);
        curSceneName = sn;
    }

    public void UnLoadScene(SceneName sn)
    {
        SceneManager.UnloadSceneAsync((int)sn);
    }

    public void ChangeScene(SceneName nextScene, SceneName curScene)
    {
        StartCoroutine(LoadScene(nextScene, curScene));
    }

    public IEnumerator LoadScene(SceneName nextScene, SceneName curScene)
    {
        yield return null;
        AsyncOperation scheduler = SceneManager.LoadSceneAsync((int)nextScene, LoadSceneMode.Additive);
        scheduler.allowSceneActivation = false;
        while(!scheduler.isDone)
        {
            yield return null;
            if(scheduler.progress >= 0.9f)
            {
                scheduler.allowSceneActivation = true;
                yield return null;
            }
        }
        SceneManager.UnloadSceneAsync((int)curScene);

        curSceneName = nextScene;
    }
}
