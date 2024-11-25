using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour {

    [SerializeField] string _sceneName;

    [Header("VFX")]
    [SerializeField] ParticleSystem _playerSpawnedParticule;

    [Header("SFX")]
    [SerializeField] AudioClip _sfxPlayerDetected;
    [SerializeField] List<AudioClip> _sfxPlayerRespawn;

    [SerializeField] bool _isPlayingEffectOnTrigger = true;

    void Start() {
        _sceneName = SceneManager.GetActiveScene().name;
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (RoomData.Instance.m_isPlayerRespawning == true)
            {
                AudioManager.Instance.PlayRandomSfx(_sfxPlayerRespawn, 8);
                StartCoroutine(MakeRespawn());

                RoomData.Instance.m_isPlayerRespawning = false;
            }
            else
            {
                if(_isPlayingEffectOnTrigger)
                {
                    AudioManager.Instance.PlaySfx(_sfxPlayerDetected, 8);
                    _playerSpawnedParticule.Play();
                }
                player.GetComponent<PlayerManager>().m_playerLife = player.GetComponent<PlayerManager>().m_maxLife;
                CheckpointManager.instance.SaveLastCheckpoint(_sceneName, transform.position);
                float x = transform.position.x; PlayerPrefs.SetFloat("checkpoint_pos_x", x);
                float y = transform.position.y; PlayerPrefs.SetFloat("checkpoint_pos_y", y);
                float z = transform.position.z; PlayerPrefs.SetFloat("checkpoint_pos_z", z);
                PlayerPrefs.Save();
            }


        }
    }

    private IEnumerator MakeRespawn()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(1f);

        RoomData.Instance.m_isPlayerRespawning = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.GetComponent<PlayerController>().m_isControllable = true;


        CheckpointManager.instance.SaveLastCheckpoint(_sceneName, transform.position);
        float x = transform.position.x; PlayerPrefs.SetFloat("checkpoint_pos_x", x);
        float y = transform.position.y; PlayerPrefs.SetFloat("checkpoint_pos_y", y);
        float z = transform.position.z; PlayerPrefs.SetFloat("checkpoint_pos_z", z);
        PlayerPrefs.Save();
        if (_isPlayingEffectOnTrigger)  _playerSpawnedParticule.Play();
    }

}
