using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Uses :
//  AudioManager.Instance.PlayMusic(BattleMusic); 
//  AudioManager.Instance.RandomSoundEffect(AttackNoises);


public class AudioManager : MonoBehaviour
{
    // Audio players components.
    [SerializeField] List<AudioSource> _listSfxAudioSource;
    [SerializeField] AudioSource _musicAudioSource;

    // Random pitch adjustment range.
    float _lowSFXPitchRange = .92f;
    float _highSFXPitchRange = 1.08f;

    float _volumeSfxRange;


    // Singleton instance.
    public static AudioManager Instance = null;

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        _musicAudioSource.clip = clip;
        _musicAudioSource.Play();

    }

    // Play a single clip through the sound effects source.
    public void PlaySfx(AudioClip clip, int bus = 0)
    {
        float randomPitch = Random.Range(_lowSFXPitchRange, _highSFXPitchRange);
        float randomVolume = Random.Range(-_volumeSfxRange, _volumeSfxRange);

        _listSfxAudioSource[bus].pitch = randomPitch;
        _listSfxAudioSource[bus].volume += randomVolume;
        _listSfxAudioSource[bus].clip = clip;
        _listSfxAudioSource[bus].Play();
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void PlayRandomSfx(List<AudioClip> listAudioCLip, int bus = 0)
    {
        int randomIndex = Random.Range(0, listAudioCLip.Count());

        float randomPitch = Random.Range(_lowSFXPitchRange, _highSFXPitchRange);
        float randomVolume = Random.Range(-_volumeSfxRange, _volumeSfxRange);

        _listSfxAudioSource[bus].pitch = randomPitch;
        _listSfxAudioSource[bus].volume += randomVolume;
        _listSfxAudioSource[bus].clip = listAudioCLip[randomIndex];
        _listSfxAudioSource[bus].Play();
    }

}