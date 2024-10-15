using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCompositePhysics : MonoBehaviour {

    public float _jumpForce = 7.5f;

    //public bool m_isGrounded { get; set; }
    public bool m_isCollidingWithGround { get; private set; }
    public Rigidbody2D m_playerRigidbody { get; private set; }

    [SerializeField] float _groundCheckOffsetLenght = 0.1f;
    [SerializeField] CircleCollider2D _groundCheckerCircleCollider;

    [SerializeField] LayerMask _groundlayer;
    [SerializeField] LayerMask _gearlayer;

    void Start() {
        m_playerRigidbody = GetComponent<Rigidbody2D>();
    }

    public void Jump() {
        m_playerRigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    public bool IsGrounded()
    {

        if ( Physics2D.OverlapCircle(GetGroundCheckerCircleCollider(), _groundCheckerCircleCollider.radius * transform.localScale.x, _groundlayer)
             || Physics2D.OverlapCircle(GetGroundCheckerCircleCollider(), _groundCheckerCircleCollider.radius * transform.localScale.x, _gearlayer)
        ) return true;
        else return false;
    }

    private void OnDrawGizmos()
    {
        DrawCircle(GetGroundCheckerCircleCollider(), _groundCheckerCircleCollider.radius * transform.localScale.x);
    }

    Vector2 GetGroundCheckerCircleCollider()
    {
        return (_groundCheckerCircleCollider.transform.position + new Vector3(0, (_groundCheckerCircleCollider.offset.y - _groundCheckOffsetLenght) * transform.localScale.x)) ;
    }

    void DrawCircle(Vector3 center, float radius)
    {
        Color gizmoColor = Color.yellow; 
        int segments = 100;  // Number of segments to smooth the circle

        float angleStep = 360f / segments;
        Vector3 lastPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep;
            float radian = angle * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(radian) * radius, Mathf.Sin(radian) * radius, 0);
            Gizmos.DrawLine(lastPoint, nextPoint);
            lastPoint = nextPoint;
        }
    }

}
