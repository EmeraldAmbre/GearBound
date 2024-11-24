using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour {

    [SerializeField] string _sceneName;

    [Header("VFX")]
    [SerializeField] ParticleSystem _playerSpawnedParticule;

    [Header("SFX")]
    [SerializeField] AudioClip _sfxPlayerDetected;
    [SerializeField] List<AudioClip> _sfxPlayerRespawn;

    void Start() {
        _sceneName = SceneManager.GetActiveScene().name;
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player.GetComponent<PlayerController>().m_isRespawning == true)
            {
                AudioManager.Instance.PlayRandomSfx(_sfxPlayerRespawn, 8);
                StartCoroutine(MakeRespawn());
            }
            else
            {
                AudioManager.Instance.PlaySfx(_sfxPlayerDetected, 8);
                player.GetComponent<PlayerController>().m_isRespawning = false;


                CheckpointManager.instance.SaveLastCheckpoint(_sceneName, transform.position);
                float x = transform.position.x; PlayerPrefs.SetFloat("checkpoint_pos_x", x);
                float y = transform.position.y; PlayerPrefs.SetFloat("checkpoint_pos_y", y);
                float z = transform.position.z; PlayerPrefs.SetFloat("checkpoint_pos_z", z);
                PlayerPrefs.Save();
                _playerSpawnedParticule.Play();
            }


        }
    }

    private IEnumerator MakeRespawn()
    {
        yield return new WaitForSeconds(1f);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<PlayerController>().m_isRespawning = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.GetComponent<PlayerController>().m_isControllable = true;


        CheckpointManager.instance.SaveLastCheckpoint(_sceneName, transform.position);
        float x = transform.position.x; PlayerPrefs.SetFloat("checkpoint_pos_x", x);
        float y = transform.position.y; PlayerPrefs.SetFloat("checkpoint_pos_y", y);
        float z = transform.position.z; PlayerPrefs.SetFloat("checkpoint_pos_z", z);
        PlayerPrefs.Save();
        _playerSpawnedParticule.Play();
    }

}
