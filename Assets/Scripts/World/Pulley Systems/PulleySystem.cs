using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PulleySystem : GearMechanism
{
    [SerializeField] float _maxHeight;
    [SerializeField] float _minHeight;
    [SerializeField] float _pulleySpeed;

    Vector2 _initialPosition = new Vector2(0,0);
    float currentHeight;
    [SerializeField] BoxCollider2D _boxCollider;
    Rigidbody2D _body;
    float pulleyHeight;
    float pulleyWidth;
    float yMaxheight;
    float yMinheight;
    float yMaxHeightLockThreshold;
    float yMinHeightLockThreshold;
    [SerializeField] bool _isLockingAfterThreshold;
    [SerializeField] float yMaxLockThreshold = 0;
    [SerializeField] float yMinLockThreshold = 0;
    bool _isLocked = false;

    bool _isPulleyWithDynamicRigidBody = false;
    [SerializeField]  bool _isAddingConstantVelocityToRigidbody = false;
    Rigidbody2D _dynamicRigidBody;


    Vector2 _forceToApplyOnRigidbody = Vector2.zero;

    private void Start()
    {
        _initialPosition = transform.position;
        if(_boxCollider == null) _boxCollider = GetComponent<BoxCollider2D>();
        _body = GetComponent<Rigidbody2D>();

        pulleyWidth = _boxCollider.size.x * transform.localScale.x;
        pulleyHeight = _boxCollider.size.y * transform.localScale.y;
        yMaxheight = _initialPosition.y + _maxHeight + pulleyHeight / 2;
        yMinheight = _initialPosition.y - _minHeight - pulleyHeight / 2;

        yMaxHeightLockThreshold = _initialPosition.y + yMaxLockThreshold + pulleyHeight / 2;
        yMinHeightLockThreshold = _initialPosition.y - yMinLockThreshold - pulleyHeight / 2;

        if (GetComponent<Rigidbody2D>() != null)
        {
            if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
            {
                _isPulleyWithDynamicRigidBody = true;
                _dynamicRigidBody = GetComponent<Rigidbody2D>();
            }
        }
            
            
    }

    public override void ActivateOnce(int gearRotationDirection, float gearRotationScale = 1)
    {
        if(_isLocked == false)
        {
            base.ActivateOnce(gearRotationDirection, gearRotationScale);
            if(_dynamicRigidBody != null) ApplyForce(gearRotationDirection, gearRotationScale);
            else Move(gearRotationDirection, gearRotationScale);
        }
        handleThresholdCheck();
    }

    public override void ActivateOnce(int gearRotationDirection)
    {
        if (_isLocked == false)
        {
            base.ActivateOnce(gearRotationDirection, 1);
            if (_dynamicRigidBody != null) ApplyForce(gearRotationDirection, 1);
            else Move(gearRotationDirection, 1);
        }
        handleThresholdCheck();
    }

    private void Move(int gearRotationDirection, float gearRotationScale)
    {
        
        if (gearRotationDirection == 1)
        {
            if (transform.position.y + pulleyHeight / 2 < yMaxheight)
            {
                transform.Translate(Vector2.up * _pulleySpeed * gearRotationScale * Time.deltaTime);
            }
        }
        else if (gearRotationDirection == -1)
        {
            if (transform.position.y - pulleyHeight / 2 > yMinheight)
            {
                transform.Translate(Vector2.down * _pulleySpeed * gearRotationScale * Time.deltaTime);
            } 
        }
    }

    private void ApplyForce(int gearRotationDirection, float gearRotationScale)
    {
        if (gearRotationDirection == 1)
        {
            if (transform.position.y + pulleyHeight / 2 < yMaxheight)
            {
                _forceToApplyOnRigidbody = Vector2.up * _pulleySpeed * gearRotationScale;
            }
        }
        else if (gearRotationDirection == -1)
        {
            if (transform.position.y - pulleyHeight / 2 > yMinheight)
            {
                _forceToApplyOnRigidbody = Vector2.up * _pulleySpeed * gearRotationScale;
            }
        }
    }

    private void handleThresholdCheck()
    {
        if (_isLockingAfterThreshold && _isLocked == false)
        {
            if (yMaxLockThreshold != 0 && (transform.position.y - pulleyHeight / 2 > yMaxHeightLockThreshold))
            {
                _body.constraints = RigidbodyConstraints2D.FreezeAll;
                _isLocked = true;
            }
            if (yMinLockThreshold != 0 && (transform.position.y + pulleyHeight / 2 < yMinHeightLockThreshold))
            {
                _body.constraints = RigidbodyConstraints2D.FreezeAll;
                _isLocked = true;
            }

        }
    }


    private void FixedUpdate()
    {
        if (_forceToApplyOnRigidbody != Vector2.zero && _dynamicRigidBody != null && _forceToApplyOnRigidbody.y  > 0)
        {
            if (_isAddingConstantVelocityToRigidbody is true) _dynamicRigidBody.velocity = _forceToApplyOnRigidbody;
            else _dynamicRigidBody.AddForce(_forceToApplyOnRigidbody ,ForceMode2D.Force) ;
        }
        _forceToApplyOnRigidbody = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        if (Application.isEditor && _initialPosition == Vector2.zero) _initialPosition = transform.position;
        Gizmos.color = Color.yellow;

        pulleyWidth = _boxCollider.size.x * transform.localScale.x;
        pulleyHeight = _boxCollider.size.y * transform.localScale.y;

        yMaxheight = _initialPosition.y + _maxHeight + pulleyHeight / 2;
        yMinheight = _initialPosition.y - _minHeight - pulleyHeight / 2;


        yMaxHeightLockThreshold = _initialPosition.y + yMaxLockThreshold + pulleyHeight / 2;
        yMinHeightLockThreshold = _initialPosition.y - yMinLockThreshold - pulleyHeight / 2;

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


        if(_isLockingAfterThreshold)
        {
            Gizmos.color = Color.red;
            if (yMaxLockThreshold != 0)
            {
                Gizmos.DrawLine(new Vector2(xLineOrigin, yMaxHeightLockThreshold + 0.01f), new Vector2(xLineEnd, yMaxHeightLockThreshold + 0.01f));
                Gizmos.DrawLine(new Vector2(xLineOrigin, yMaxHeightLockThreshold), new Vector2(xLineEnd, yMaxHeightLockThreshold));
                Gizmos.DrawLine(new Vector2(xLineOrigin, yMaxHeightLockThreshold - 0.01f), new Vector2(xLineEnd, yMaxHeightLockThreshold - 0.01f));
            }
            if (yMinLockThreshold != 0)
            {
                Gizmos.DrawLine(new Vector2(xLineOrigin, yMinHeightLockThreshold + 0.01f), new Vector2(xLineEnd, yMinHeightLockThreshold + 0.01f));
                Gizmos.DrawLine(new Vector2(xLineOrigin, yMinHeightLockThreshold), new Vector2(xLineEnd, yMinHeightLockThreshold));
                Gizmos.DrawLine(new Vector2(xLineOrigin, yMinHeightLockThreshold - 0.01f), new Vector2(xLineEnd, yMinHeightLockThreshold - 0.01f));
            }
        }
    }

}
