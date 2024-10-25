using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewChanger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] int _cameraPriority = 20;
    int _initCameraPriority;

    private void Start()
    {
        _initCameraPriority = _camera.Priority;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _camera.Priority = _cameraPriority;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _camera.Priority = _initCameraPriority;
    }
}
