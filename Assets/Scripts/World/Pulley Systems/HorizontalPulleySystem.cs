using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HorizontalPulleySystem : GearMechanism
{


    Vector2 _initialPosition = new Vector2(0, 0);
    [SerializeField] float _maxLeft = 2f;
    [SerializeField] float _maxRight = 2f;
    [SerializeField] float _pulleySpeed = 0.2f;
    [SerializeField] BoxCollider2D _boxCollider;
    float boxWidth;
    float boxHeight;
    float currentPosX;

    public void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _initialPosition = transform.position;

        currentPosX = transform.position.x;


        boxWidth = _boxCollider.size.x * transform.localScale.x;
        boxHeight = _boxCollider.size.y * transform.localScale.y;
    }

    public override void ActivateOnce(int gearRotationDirection)
    {
        base.ActivateOnce(gearRotationDirection);
        if (gearRotationDirection == 1)
        {
            if (currentPosX + boxWidth / 2 < _initialPosition.x + _maxRight + boxWidth / 2) transform.Translate(Vector2.right * _pulleySpeed);
        }
        else if (gearRotationDirection == -1)
        {
            if (currentPosX - boxHeight / 2 > _initialPosition.x - _maxLeft - boxHeight / 2) transform.Translate(Vector2.left * _pulleySpeed);
        }
            

        Debug.Log("gear rot : " + gearRotationDirection);
        currentPosX = transform.position.x;

    }


    private void OnDrawGizmos()
    {
        if (Application.isEditor && _initialPosition == Vector2.zero) _initialPosition = transform.position;
        Gizmos.color = Color.yellow;

        boxWidth = _boxCollider.size.x * transform.localScale.x;
        boxHeight = _boxCollider.size.y * transform.localScale.y;

        float xLineLeft = _initialPosition.x - _maxLeft - boxWidth / 2;
        float yLineOriginLeft = _initialPosition.y + _boxCollider.offset.y  + boxHeight / 2;
        float yLineEndLeft = _initialPosition.y + _boxCollider.offset.y - boxHeight / 2;

        float xLineRight = _initialPosition.x + _maxRight + boxWidth / 2;
        float yLineOriginRight = _initialPosition.y + _boxCollider.offset.y + boxHeight / 2;
        float yLineEndRight = _initialPosition.y + _boxCollider.offset.y - boxHeight / 2;

        // Draw line left max
        Gizmos.DrawLine(new Vector2(xLineLeft + 0.01f, yLineOriginLeft), new Vector2(xLineLeft + 0.01f, yLineEndLeft));
        Gizmos.DrawLine(new Vector2(xLineLeft, yLineOriginLeft), new Vector2(xLineLeft, yLineEndLeft));
        Gizmos.DrawLine(new Vector2(xLineLeft - 0.01f, yLineOriginLeft), new Vector2(xLineLeft - 0.01f, yLineEndLeft));
        // Draw line right
        Gizmos.DrawLine(new Vector2(xLineRight + 0.01f, yLineOriginRight), new Vector2(xLineRight + 0.01f, yLineEndRight));
        Gizmos.DrawLine(new Vector2(xLineRight, yLineOriginRight), new Vector2(xLineRight, yLineEndRight));
        Gizmos.DrawLine(new Vector2(xLineRight - 0.01f, yLineOriginRight), new Vector2(xLineRight - 0.01f, yLineEndRight));


    }


    //void Update() {

    //    currentPosX = transform.position.x;

    //    if (m_isMovingLeft) {
    //        if (currentPosX > _maxLeft) transform.Translate(Vector2.left * _pulleySpeed);
    //    }

    //    else if (m_isMovingRight) {
    //        if (currentPosX < _maxRight) transform.Translate(Vector2.right * _pulleySpeed);
    //    }

    //}
}
