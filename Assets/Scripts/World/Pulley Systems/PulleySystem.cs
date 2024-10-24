using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleySystem : GearMechanism
{
    [SerializeField] float _maxHeight;
    [SerializeField] float _minHeight;
    [SerializeField] float _pulleySpeed;

    Vector2 _initialPosition = new Vector2(0,0);
    float currentHeight;
    [SerializeField] BoxCollider2D _boxCollider;
    float pulleyHeight;
    float pulleyWidth;
    float yMaxheight;
    float yMinheight;

    private void Start()
    {
        _initialPosition = transform.position;
        _boxCollider = GetComponent<BoxCollider2D>();

        pulleyWidth = _boxCollider.size.x * transform.localScale.x;
        pulleyHeight = _boxCollider.size.y * transform.localScale.y;
        yMaxheight = _initialPosition.y + _maxHeight + pulleyHeight / 2;
        yMinheight = _initialPosition.y - _minHeight - pulleyHeight / 2;


        Debug.Log("Max height drawed : " + yMaxheight);
    }

    public override void ActivateOnce(int gearRotationDirection)
    {
        base.ActivateOnce(gearRotationDirection);
        if (gearRotationDirection == 1)
        {
            if (transform.position.y + pulleyHeight / 2 < yMaxheight) transform.Translate(Vector2.up * _pulleySpeed);
            Debug.Log("transform y : " + transform.position.y + pulleyHeight / 2 + "    limit up transform y : " + yMaxheight);
        }
        else if (gearRotationDirection == -1)
        {
            if (transform.position.y - pulleyHeight / 2 > yMinheight) transform.Translate(Vector2.down * _pulleySpeed);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isEditor && _initialPosition == Vector2.zero) _initialPosition = transform.position;
        Gizmos.color = Color.yellow;

        pulleyWidth = _boxCollider.size.x * transform.localScale.x;
        pulleyHeight = _boxCollider.size.y * transform.localScale.y;

        yMaxheight = _initialPosition.y + _maxHeight + pulleyHeight / 2;
        yMinheight = _initialPosition.y - _minHeight - pulleyHeight / 2;

        float xLineOrigin = _initialPosition.x - pulleyWidth / 2;
        float xLineEnd = _initialPosition.x + pulleyWidth / 2;

        // Draw line max height
        Gizmos.DrawLine(new Vector2(xLineOrigin, yMaxheight + 0.01f), new Vector2(xLineEnd, yMaxheight + 0.01f));
        Gizmos.DrawLine(new Vector2(xLineOrigin, yMaxheight), new Vector2(xLineEnd, yMaxheight));
        Gizmos.DrawLine(new Vector2(xLineOrigin, yMaxheight - 0.01f), new Vector2(xLineEnd, yMaxheight - 0.01f));
        // Draw line min height
        Gizmos.DrawLine(new Vector2(xLineOrigin, yMinheight + 0.01f), new Vector2(xLineEnd, yMinheight + 0.01f));
        Gizmos.DrawLine(new Vector2(xLineOrigin, yMinheight), new Vector2(xLineEnd, yMinheight));
        Gizmos.DrawLine(new Vector2(xLineOrigin, yMinheight - 0.01f), new Vector2(xLineEnd, yMinheight - 0.01f));
    }

}
