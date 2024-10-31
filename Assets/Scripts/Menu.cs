using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class Menu : MonoBehaviour 
{

    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;

    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public int[] screenWidths;

    int activeScreenResIndex;

    void Start(){
        // activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
        activeScreenResIndex = Mathf.Clamp(PlayerPrefs.GetInt("screen res index"), 0, resolutionToggles.Length - 1);

        bool isFullScreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;


        volumeSliders[0].value = AudioManager.instance.masterVolumePercentage;
        volumeSliders[1].value = AudioManager.instance.musicVolumePercentage;
        volumeSliders[2].value = AudioManager.instance.sfxVolumePercentage;

        for(int i = 0; i < resolutionToggles.Length; i++){
            resolutionToggles[i].isOn = i == activeScreenResIndex;
        }

        SetFullscreen(isFullScreen);
    }
    public void Play(){

        SceneManager.LoadScene("SampleScene");
    }

    public void Quit(){ 

        Application.Quit();
    }
    public void OptionMenu(){
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void MainMenu(){
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    public void SetScreenResolution(int i) {
		if (resolutionToggles [i].isOn) {
			activeScreenResIndex = i;
			float aspectRatio = 16 / 9f;
			Screen.SetResolution (screenWidths [i], (int)(screenWidths [i] / aspectRatio), false);
			PlayerPrefs.SetInt ("screen res index", activeScreenResIndex);
			PlayerPrefs.Save ();
		}
	}

    public void SetFullscreen(bool isFullscreen) {
		for (int i = 0; i < resolutionToggles.Length; i++) {
			resolutionToggles [i].interactable = !isFullscreen;
		}

		if (isFullscreen) {
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions [allResolutions.Length - 1];
			Screen.SetResolution (maxResolution.width, maxResolution.height, true);
		} else {
			SetScreenResolution (activeScreenResIndex);
		}

		PlayerPrefs.SetInt ("fullscreen", ((isFullscreen) ? 1 : 0));
		PlayerPrefs.Save ();
	}

    public void SetMasterVolume(float vol){
        AudioManager.instance.SetVolume(vol, AudioManager.AudioChannel.Master); 
    }

    public void SetSFXVolume(float vol){
        AudioManager.instance.SetVolume(vol, AudioManager.AudioChannel.Sfx); 

    }

    public void SetMusicVolume(float vol){
        AudioManager.instance.SetVolume(vol, AudioManager.AudioChannel.Music); 

    }
}
