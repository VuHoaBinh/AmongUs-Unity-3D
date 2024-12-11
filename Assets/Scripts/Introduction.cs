using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Introduction : MonoBehaviour
{
    public VideoPlayer videoPlayer; 

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video đã chạy xong!");
    }


    public void NextScene()
    {
        StartCoroutine(LoadSceneAsync("OptionMap"));
    }


    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
         while (!asyncLoad.isDone)
        {
            yield return null; 
        }
    }
}
