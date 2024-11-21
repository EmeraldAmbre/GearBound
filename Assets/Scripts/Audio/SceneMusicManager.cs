using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] AudioClip _musicScene;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(_musicScene);
    }
}
