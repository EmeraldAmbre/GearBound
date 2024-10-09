using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomConnector : MonoBehaviour
{
    [SerializeField] string _sceneNameToConnect;

    [SerializeField] bool _isSpawningUp;
    [SerializeField] bool _isSpawningDown;
    [SerializeField] bool _isSpawningRight;
    [SerializeField] bool _isSpawningLeft;
    [SerializeField] float _spawningDistance = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            SceneManager.LoadScene(_sceneNameToConnect);
            Debug.Log("Woah");
        }
    }
}
