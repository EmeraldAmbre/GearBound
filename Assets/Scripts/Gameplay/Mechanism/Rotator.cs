using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool m_isActive = true;
    public int m_rotationDirection = 1;
    [SerializeField] float _rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isActive)
        {
           transform.Rotate(new Vector3(0,0, _rotationSpeed * Time.deltaTime * m_rotationDirection));
        }
    }
}
