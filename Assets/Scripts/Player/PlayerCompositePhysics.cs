using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerCompositePhysics : MonoBehaviour {

    //public bool m_isGrounded { get; set; }
    public Rigidbody2D m_playerRigidbody { get; private set; }

    [SerializeField] float _groundCheckOffsetLenght = 0.1f;
    [SerializeField] CircleCollider2D _groundCheckerCircleCollider;
    [SerializeField] float _ceilingCheckOffsetLenght = 0.1f;
    [SerializeField] CircleCollider2D _ceilingCheckerCircleCollider;
    [SerializeField] float _wallCheckOffsetLenght = 0.1f;
    [SerializeField] CircleCollider2D _wallCheckerCircleCollider;

    [SerializeField] float _slopeCheckLenghtDistance = 0.1f;

    [SerializeField] LayerMask _plateformLayer;
    [SerializeField] LayerMask _gearLayer;

    bool _isCollidingWithPlateform;
    bool _isOnContactWithGear;
    public bool m_isOnContactWithGearWall;
    public int m_gearWallDirection = 0;


    void Start() {
        m_playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Gear" || collision.tag == "ControllableGear") _isOnContactWithGear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Gear" || collision.tag == "ControllableGear") _isOnContactWithGear = false;
    }

    public bool IsGrounded()
    {
        if ( Physics2D.OverlapCircle(GetGroundCheckerCircleCollider(), _groundCheckerCircleCollider.radius * transform.localScale.x, _plateformLayer)
        || (IsOnSlope() && _isCollidingWithPlateform)
        ) return true;
        else return false;
    }

    public bool IsOnContactWithGear() => _isOnContactWithGear;
    public bool IsOnContactWithGearWall() => m_isOnContactWithGearWall;
    public bool IsCeiling()
    {
        if (Physics2D.OverlapCircle(GetCeilingCheckerCircleCollider(), (_ceilingCheckerCircleCollider.radius - 0.1f) * transform.localScale.x, _plateformLayer)
             || Physics2D.OverlapCircle(GetCeilingCheckerCircleCollider(), (_ceilingCheckerCircleCollider.radius - 0.1f) * transform.localScale.x, _gearLayer)
        ) return true;
        else return false;
    }

    public bool IsOnWall()
    {
        if (Physics2D.OverlapCircle(GetWallCheckerCircleCollider(1), (_wallCheckerCircleCollider.radius - 0.1f) * transform.localScale.x, _plateformLayer)
             // || Physics2D.OverlapCircle(GetWallCheckerCircleCollider(1), (_wallCheckerCircleCollider.radius - 0.1f) * transform.localScale.x, _gearlayer)
             || Physics2D.OverlapCircle(GetWallCheckerCircleCollider(-1), (_wallCheckerCircleCollider.radius - 0.1f) * transform.localScale.x, _plateformLayer)
            // || Physics2D.OverlapCircle(GetWallCheckerCircleCollider(-1), (_wallCheckerCircleCollider.radius - 0.1f) * transform.localScale.x, _gearlayer)
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
       GizmoDrawer.DrawCircle(GetGroundCheckerCircleCollider(), _groundCheckerCircleCollider.radius * transform.localScale.x);
       GizmoDrawer.DrawCircle(GetCeilingCheckerCircleCollider(), (_ceilingCheckerCircleCollider.radius - 0.1f) * transform.localScale.x);
       GizmoDrawer.DrawCircle(GetWallCheckerCircleCollider(1), (_wallCheckerCircleCollider.radius - 0.1f) * transform.localScale.x);
       GizmoDrawer.DrawCircle(GetWallCheckerCircleCollider(-1), (_wallCheckerCircleCollider.radius - 0.1f) * transform.localScale.x);
    }

    Vector2 GetGroundCheckerCircleCollider()
    {
        return (_groundCheckerCircleCollider.transform.position + new Vector3(0, (_groundCheckerCircleCollider.offset.y - _groundCheckOffsetLenght) * transform.localScale.x));
    }
    Vector2 GetCeilingCheckerCircleCollider()
    {
        return (_ceilingCheckerCircleCollider.transform.position + new Vector3(0, (_ceilingCheckerCircleCollider.offset.y + _ceilingCheckOffsetLenght) * transform.localScale.x));
    }
    Vector2 GetWallCheckerCircleCollider(int direction)
    {
        return (_ceilingCheckerCircleCollider.transform.position + new Vector3(_ceilingCheckerCircleCollider.offset.x + direction * _wallCheckOffsetLenght * transform.localScale.x, 0));
    }


    public Vector2 GetSlopePerpendicularNormal()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _slopeCheckLenghtDistance, _plateformLayer);
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

    public Vector2 GetSlopeGroundPointPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _slopeCheckLenghtDistance, _plateformLayer);
        return hit.transform.position;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isCollidingWithPlateform = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isCollidingWithPlateform = false;
    }





}
