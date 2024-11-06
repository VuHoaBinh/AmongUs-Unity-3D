using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme;
    public AudioClip menuTheme;
    string sceneName;

    // Start is called before the first frame update
    void Start(){
        OnLevelWasLoaded(0);
    }
    void OnLevelWasLoaded(int level)
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if(newSceneName != sceneName){
            sceneName = newSceneName;
            Invoke("PlayMusic", .2f);
        }

    }

    void PlayMusic(){
        AudioClip clipToPlay = null;

        if(sceneName == "Menu"){
            clipToPlay = menuTheme;
        }else if(sceneName == "SampleScene"){
            clipToPlay = mainTheme;
        }

        if(clipToPlay != null){
            AudioManager.instance.PlayMusic(clipToPlay, 2);
            Invoke("PlayMusic", clipToPlay.length);
        }
    }
}
