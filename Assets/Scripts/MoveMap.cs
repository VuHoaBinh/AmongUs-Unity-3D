using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveMap : MonoBehaviour
{
    
    public void Game1()
    {
        StartCoroutine(LoadSceneAsync("SampleScene"));
    }

    public void Game2()
    {
        StartCoroutine(LoadSceneAsync("TestMap"));
    }

    public void Game3()
    {
        StartCoroutine(LoadSceneAsync("Game3"));
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
