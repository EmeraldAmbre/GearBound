using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubMachine : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteUpgradeDash;
    [SerializeField] SpriteRenderer _spriteUpgradeMagnet;
    [SerializeField] SpriteRenderer _spriteUpgradeRotation;
    [SerializeField] SpriteRenderer _spriteUpgradePossesion;

    [SerializeField] GameObject _txtOpenHubDoor;
    [SerializeField] Animator _anim;

    [SerializeField] string _creditsMenuScene;

    GameTimer _timerScript;

    void Awake() {
        _timerScript = FindObjectOfType<GameTimer>();
    }

    void Update() {
        HandleShowingUpgradeSprite();

        if(_txtOpenHubDoor.activeSelf && !_anim.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor")) {
            if(Input.GetKeyDown(KeyCode.T)) {
                _anim.Play("OpenDoor");
                _txtOpenHubDoor.SetActive(false);
            }
        }
    }

    private void HandleShowingUpgradeSprite() {
        if (PlayerPrefs.GetInt("dash") == 1) _spriteUpgradeDash.enabled = true;
        else _spriteUpgradeDash.enabled = false;
        if (PlayerPrefs.GetInt("magnet") == 1) _spriteUpgradeMagnet.enabled = true;
        else _spriteUpgradeMagnet.enabled = false;
        if (PlayerPrefs.GetInt("rotation") == 1) _spriteUpgradeRotation.enabled = true;
        else _spriteUpgradeRotation.enabled = false;
        if (PlayerPrefs.GetInt("possession") == 1) _spriteUpgradePossesion.enabled = true;
        else _spriteUpgradePossesion.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(PlayerPrefs.GetInt("dash") == 1
            && PlayerPrefs.GetInt("magnet") == 1
            && PlayerPrefs.GetInt("rotation") == 1
            && PlayerPrefs.GetInt("possession") == 1
            )
        {
            _txtOpenHubDoor.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        _txtOpenHubDoor.SetActive(false);
    }

    public void OnDoorOpenAnimationFinished() {
        Debug.Log("Animation Finished!");
        _timerScript.StopTimer();
        StartCoroutine(LoadSceneWithDelay(0.2f, _creditsMenuScene));
    }

    private IEnumerator LoadSceneWithDelay(float delay, string sceneName) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
