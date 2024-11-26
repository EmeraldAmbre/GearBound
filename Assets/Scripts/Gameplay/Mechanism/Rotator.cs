using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool m_isActive = true;
    public int m_rotationDirection = 1;
    [SerializeField] bool _isInteractingWithPlayer = false;
    [SerializeField] float _rotationSpeed;


    // Update is called once per frame
    void Update()
    {
        if(m_isActive && !_isInteractingWithPlayer)
        {
           transform.Rotate(new Vector3(0,0, _rotationSpeed * Time.deltaTime * m_rotationDirection));
        }
    }

    private void FixedUpdate()
    {
        if(m_isActive && _isInteractingWithPlayer)
        {
            transform.Rotate(new Vector3(0, 0, _rotationSpeed * Time.deltaTime * m_rotationDirection));
        }
    }
}
