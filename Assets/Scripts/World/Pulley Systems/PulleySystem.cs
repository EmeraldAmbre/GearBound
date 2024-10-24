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

    private void Start()
    {
        _initialPosition = transform.position;
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void ActivateOnce(int gearRotationDirection)
    {
        base.ActivateOnce(gearRotationDirection);
        if (gearRotationDirection == 1)
            if (transform.position.y < _initialPosition.y + _maxHeight) transform.Translate(Vector2.up * _pulleySpeed);
        else if (gearRotationDirection == -1)
            if (transform.position.y > _initialPosition.y - _minHeight) transform.Translate(Vector2.down * _pulleySpeed);
    }

    //void Update() {
    //    if (m_isMovingUp) {
    //        if (transform.position.y < _initialPosition.y + _maxHeight) transform.Translate(Vector2.up * _pulleySpeed);
    //    }

    //    else if (m_isMovingDown) {
    //        if (transform.position.y > _initialPosition.y - _minHeight) transform.Translate(Vector2.down * _pulleySpeed);
    //    }
    //}

    private void OnDrawGizmos()
    {
        if (Application.isEditor && _initialPosition == Vector2.zero) _initialPosition = transform.position;
        Gizmos.color = Color.yellow;

        float doorWidth = _boxCollider.size.x * transform.localScale.x;
        float doorHeight = _boxCollider.size.y * transform.localScale.y;

        float xLineOrigin = _initialPosition.x - doorWidth / 2;
        float xLineEnd = _initialPosition.x + doorWidth / 2;
        float yMaxheight = _initialPosition.y + _maxHeight + doorHeight / 2;
        float yMinheight = _initialPosition.y - _minHeight - doorHeight / 2;

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
