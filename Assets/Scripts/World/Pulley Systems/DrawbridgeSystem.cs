using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawbridgeSystem : GearMechanism
{

    [SerializeField] float _minPivot = 0f;
    [SerializeField] float _maxPivot = 75f;
    [SerializeField] float _rotationSpeed = 0.05f;

    [SerializeField] BoxCollider2D _boxCollider;

    float _currentPivot;

    [SerializeField] Transform _pivotTransform;

    void Start()
    {
        _currentPivot = _pivotTransform.rotation.z;
        if (_currentPivot < _minPivot)
        {
            _currentPivot = _minPivot;
            _pivotTransform.rotation.Set(0, 0, _currentPivot, 0);
        }
        else if (_currentPivot > _maxPivot)
        {
            _currentPivot = _maxPivot;
            _pivotTransform.rotation.Set(0, 0, _currentPivot, 0);
        }

    }
    public override void ActivateOnce(int gearRotationDirection)
    {
        base.ActivateOnce(gearRotationDirection);
        RotatePivot(gearRotationDirection);
    }


    public override void ActivateOnce(int gearRotationDirection, float gearRotationScale)
    {
        base.ActivateOnce(gearRotationDirection , gearRotationScale);

        RotatePivot(gearRotationDirection , gearRotationScale);
    }


    private void RotatePivot(int gearRotationDirection, float gearRotationScale = 1)
    {
        float rotationToApply = _rotationSpeed * gearRotationScale * Time.deltaTime * gearRotationDirection;
        if (gearRotationDirection == 1)
        {
            if (_currentPivot < _maxPivot)
            {
                _currentPivot += rotationToApply;
                _pivotTransform.Rotate(new(0, 0, rotationToApply));
            }
        }
        else if (gearRotationDirection == -1)
        {
            if (_currentPivot > _minPivot)
            {
                _currentPivot += rotationToApply;
                _pivotTransform.Rotate(new(0, 0, rotationToApply));
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;


        float lineLenght = _boxCollider.size.x * _boxCollider.gameObject.transform.localScale.x;

        float angleMinPivotInRadians = (_minPivot + transform.localEulerAngles.z) * Mathf.Deg2Rad;
        Vector2 directionMinpivotEndPoint = new Vector2(Mathf.Cos(angleMinPivotInRadians), Mathf.Sin(angleMinPivotInRadians));
        Vector2 pivotMinLineEndPointPosition = directionMinpivotEndPoint * lineLenght;

        float angleMaxPivotInRadians = (_maxPivot + transform.localEulerAngles.z) * Mathf.Deg2Rad;
        Vector2 directionMaxpivotEndPoint = new Vector2(Mathf.Cos(angleMaxPivotInRadians), Mathf.Sin(angleMaxPivotInRadians));
        Vector2 pivotMaxLineEndPointPosition = directionMaxpivotEndPoint * lineLenght;

        // Draw max pivot line
        Gizmos.DrawLine((Vector2)transform.position + new Vector2(0.01f, 0), (Vector2)transform.position + pivotMaxLineEndPointPosition + new Vector2(0.01f,0));
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + pivotMaxLineEndPointPosition);
        Gizmos.DrawLine((Vector2)transform.position - new Vector2(0.01f, 0), (Vector2)transform.position + pivotMaxLineEndPointPosition - new Vector2(0.01f, 0));
        // Draw min pivot line
        Gizmos.DrawLine((Vector2)transform.position + new Vector2(0.01f, 0), (Vector2)transform.position + pivotMinLineEndPointPosition + new Vector2(0.01f, 0));
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + pivotMinLineEndPointPosition);
        Gizmos.DrawLine((Vector2)transform.position - new Vector2(0.01f, 0), (Vector2)transform.position + pivotMinLineEndPointPosition - new Vector2(0.01f, 0));


    }
}
