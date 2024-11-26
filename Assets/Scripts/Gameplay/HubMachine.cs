using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HubMachine : MonoBehaviour {

    [SerializeField] GameObject _spriteUpgradeDash;
    [SerializeField] GameObject _spriteUpgradeMagnet;
    [SerializeField] GameObject _spriteUpgradeRotation;
    [SerializeField] GameObject _spriteUpgradePossesion;

    [SerializeField] GameObject _txtOpenHubDoor;
    [SerializeField] Animator _anim;
    [SerializeField] AudioClip _sfxDoorOpening;

    [SerializeField] string _creditsMenuScene;

    GameTimer _timerScript;
    PlayerInputAction _input;

    void InitInput() {
        _input = new();
        _input.Player.HubMachine.performed += OnPerformHubMachineActivated;
        _input.Enable();
    }

    void OnPerformHubMachineActivated(InputAction.CallbackContext context) {
        if (_txtOpenHubDoor.activeSelf && !_anim.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor")) {
            _anim.Play("OpenDoor");
            AudioManager.Instance.PlaySfx(_sfxDoorOpening, 9);
            _txtOpenHubDoor.SetActive(false);
        }
    }

    void Awake() {
        InitInput();
        _timerScript = FindObjectOfType<GameTimer>();
    }

    void Update() {
        HandleShowingUpgradeSprite();
    }

    void OnDestroy() {
        _input.Player.HubMachine.performed -= OnPerformHubMachineActivated;
        _input.Player.Disable();
    }

    private void HandleShowingUpgradeSprite() {
        if (PlayerPrefs.GetInt("dash") == 1) _spriteUpgradeDash.SetActive(true);
        else _spriteUpgradeDash.SetActive(false);
        if (PlayerPrefs.GetInt("magnet") == 1) _spriteUpgradeMagnet.SetActive(true);
        else _spriteUpgradeMagnet.SetActive(false);
        if (PlayerPrefs.GetInt("rotation") == 1) _spriteUpgradeRotation.SetActive(true);
        else _spriteUpgradeRotation.SetActive(false);
        if (PlayerPrefs.GetInt("possession") == 1) _spriteUpgradePossesion.SetActive(true);
        else _spriteUpgradePossesion.SetActive(false);
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
