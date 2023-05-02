using Assets.Scripts.Deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{


    private bool repeatMusic = true;
    public AudioSource MusicSource;
    public AudioSource EffectsSource;
    public AudioClip BackgroundMusic;

    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    public float Volumne;

    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject();
                    _instance = gameObject.AddComponent<AudioManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //DontDestroyOnLoad(this);
        this.MusicSource.loop = this.repeatMusic;
        this.PlayMusic(this.BackgroundMusic);
    }

    private void Update()
    {
        this.EffectsSource.volume = Volumne;
        this.MusicSource.volume = Volumne;
    }


    public void Play(AudioClip clip)
    {
        EffectsSource.clip = clip;
        EffectsSource.Play();
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
        EffectsSource.Stop();
        EffectsSource.pitch = randomPitch;
        EffectsSource.clip = clips[randomIndex];
        EffectsSource.Play();
    }



}
