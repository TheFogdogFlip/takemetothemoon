using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    // Use this for initialization
    private static AudioManager instance;


    public float BGMVolume;
    public float SFXVolume;
    public bool SFXMuted = false;
    public bool BGMMuted = false;

    public Dictionary<string, AudioSource> AudioSources;
    Dictionary<string, float> BGMInitVolumes;

    public GameObject SFX;
    public GameObject BGM;

    AudioSource currentBGM;
    
    public static AudioManager Instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.
            if (instance == null)
                instance = GameObject.FindObjectOfType<AudioManager>();
            return instance;
            
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
     
        AudioSources = new Dictionary<string, AudioSource>();
        BGMInitVolumes = new Dictionary<string, float>();

        AudioSource[] sfxes = SFX.GetComponentsInChildren<AudioSource>();
        for (int i = 0; i < sfxes.Length; i++)
        {
            AudioSources[sfxes[i].gameObject.name] = sfxes[i];
        }

        AudioSource[] bgms = BGM.GetComponentsInChildren<AudioSource>();
        for (int i = 0; i < bgms.Length; i++)
        {
            bgms[i].ignoreListenerVolume = true;
            AudioSources[bgms[i].gameObject.name] = bgms[i];
            BGMInitVolumes[bgms[i].gameObject.name] = bgms[i].volume;
        }
	}

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0f, 1.0f);
        if (!SFXMuted)
        {
            AudioListener.volume = volume;
        }
        SFXVolume = volume;
    }

    public void ChangeCurrentBGM(string name)
    {
        currentBGM.Stop();
        currentBGM = AudioSources[name];
        currentBGM.Play();
    }

    public void SetBGMVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0f, 1.0f);
        if (!BGMMuted)
        {
            AudioSources[currentBGM.gameObject.name].volume = BGMInitVolumes[currentBGM.gameObject.name] * volume;
        }
        BGMVolume = volume;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
