using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SliddingDoor : Mechanism
{
    Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if( _isActivated) _rigidbody.AddForce(new Vector2(0, 1000 * Time.deltaTime), ForceMode2D.Force);
    }
}
