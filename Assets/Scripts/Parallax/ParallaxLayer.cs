using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float m_xParallaxFactor;
    public float m_yParallaxFactor;

    public void MoveX(float delta)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.x -= delta * m_xParallaxFactor;

        transform.localPosition = newPosition;
    }

    public void MoveY(float delta)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y -= delta * m_yParallaxFactor;

        transform.localPosition = newPosition;
    }
}
