using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerGear : MonoBehaviour
{
    public bool m_isCollidingWithGear = false;
    public UnityEvent OnExittingCollisionWitGear;
    // Start is called before the first frame update
    void Start()
    {
        OnExittingCollisionWitGear = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_isCollidingWithGear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_isCollidingWithGear = false;
        OnExittingCollisionWitGear.Invoke();
    }

    private void OnTri(Collision2D collision)
    {
        if(collision.collider.tag == "Gear") m_isCollidingWithGear = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Gear")
        {
        }
    }
}
