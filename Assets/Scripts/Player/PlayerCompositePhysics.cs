using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerCompositePhysics : MonoBehaviour {

    public float _jumpForce = 7.5f;

    //public bool m_isGrounded { get; set; }
    public bool m_isCollidingWithGround { get; private set; }
    public Rigidbody2D m_playerRigidbody { get; private set; }

    [SerializeField] float _groundCheckOffsetLenght = 0.1f;
    [SerializeField] float _slopeCheckLenghtDistance = 0.1f;
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

    private void Update()
    {
        if(IsOnSlope())
        {
            Debug.Log("Is on slope");
        }
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


    public Vector2 GetSlopePerpendicularNormal()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _slopeCheckLenghtDistance, _groundlayer);
        Debug.DrawRay(transform.position, Vector2.down * _slopeCheckLenghtDistance, Color.yellow);

        if (hit)
        {
            Vector2 slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
            Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            return slopeNormalPerpendicular;
        }
        else return Vector2.left;
    }
    public bool IsOnSlope()
    {
        if (GetSlopePerpendicularNormal() != Vector2.left) return true;
        else return false;

        //if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        //{
        //    canWalkOnSlope = false;
        //}
        //else
        //{
        //    canWalkOnSlope = true;
        //}

        //if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
        //{
        //    rb.sharedMaterial = fullFriction;
        //}
        //else
        //{
        //    rb.sharedMaterial = noFriction;
        //}
    }




}
