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

    float _sfxLoopInitVolume;


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

    public void PlaySfxLoop (AudioClip clip)
    {
        if (!_listSfxAudioSource[10].isPlaying)
        {
            float timeStart = Random.Range(0, clip.length);
            float randomPitch = Random.Range(_lowSFXPitchRange, _highSFXPitchRange);
            if(_listSfxAudioSource[10].volume != 0) _sfxLoopInitVolume = _listSfxAudioSource[10].volume;

            _listSfxAudioSource[10].pitch = randomPitch;
            _listSfxAudioSource[10].volume = 0;
            _listSfxAudioSource[10].clip = clip;
            _listSfxAudioSource[10].time = timeStart;
            _listSfxAudioSource[10].Play();

            StartCoroutine(FadeAudioSourceInVolume(_listSfxAudioSource[10], 0.1f, _sfxLoopInitVolume, false));
        }
    }

    public void StopSfxLoop()
    {
        if (_listSfxAudioSource[10].isPlaying)
        {
            StartCoroutine(FadeAudioSourceInVolume(_listSfxAudioSource[10], 0.1f, 0, true));
        }
    }


    private IEnumerator FadeAudioSourceInVolume(AudioSource audioSource, float duration ,float targetVolume ,bool isStopingAfterFade = false)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null; // Wait until the next frame
        }

        // Ensure the final volume is the target volume
        audioSource.volume = targetVolume;

        if(isStopingAfterFade) audioSource.Stop();
    }



}