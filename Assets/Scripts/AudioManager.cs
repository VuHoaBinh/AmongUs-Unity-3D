using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel{Master, Sfx, Music};
    float masterVolumePercentage = .2f;
    float sfxVolumePercentage = 1f;
    float musicVolumePercentage = 1f;


    AudioSource sfx2DSource;
    int activeMusicSourceIndex = 0;
    AudioSource[] musicSources;

    public static AudioManager instance;

    Transform audioListener;
    Transform playerT;

    SoundLibrary library;

    void Awake()
    {
        if (instance != null){
            Destroy(gameObject);
        }else{

            instance = this;
            DontDestroyOnLoad(gameObject);

            library = GetComponent<SoundLibrary>();
            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music player " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;

            }
            GameObject newSfx2Dsource = new GameObject ("2D sfx source");
			sfx2DSource = newSfx2Dsource.AddComponent<AudioSource> ();
			newSfx2Dsource.transform.parent = transform;


            audioListener = FindObjectOfType<AudioListener>().transform;
            playerT = FindObjectOfType<Player>().transform;


            masterVolumePercentage = PlayerPrefs.GetFloat("masterVolume", masterVolumePercentage);
            sfxVolumePercentage = PlayerPrefs.GetFloat("sfxVolume", sfxVolumePercentage);
            musicVolumePercentage = PlayerPrefs.GetFloat("musicVolume", musicVolumePercentage);
        }

    }

    void Update(){
        if(playerT != null){
            audioListener.position = playerT.position;
        }
    }


    public void SetVolume(float volumePercentage, AudioChannel channel){

        switch(channel){
            case AudioChannel.Music:
                musicVolumePercentage = volumePercentage;
                break;
            case AudioChannel.Sfx:
                sfxVolumePercentage = volumePercentage;
                break;
            case AudioChannel.Master:
                masterVolumePercentage = volumePercentage; 
                break;
        }

        musicSources[0].volume = musicVolumePercentage * masterVolumePercentage;
        musicSources[1].volume = musicVolumePercentage * masterVolumePercentage;

        PlayerPrefs.SetFloat("masterVolume", masterVolumePercentage);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolumePercentage);
        PlayerPrefs.SetFloat("musicVolume", musicVolumePercentage);



    }



    public void PlayMusic(AudioClip clip, float fadeDuration = 1f)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }
    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, masterVolumePercentage * sfxVolumePercentage);
        }
    }


    public void PlaySound(string soundName, Vector3 pos)
    {
        PlaySound (library.GetClipFromName (soundName), pos);
    }


    public void PlaySound2D(string soundName){
        sfx2DSource.PlayOneShot(library.GetClipFromName(soundName), sfxVolumePercentage * masterVolumePercentage);


    }    
    
    
    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercentage * masterVolumePercentage, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercentage * masterVolumePercentage, 0, percent);
            yield return null;
        }
    }
}
