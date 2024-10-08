using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate m_onCameraXTranslate;
    public ParallaxCameraDelegate m_onCameraYTranslate;

    private float _oldXPosition;
    private float _oldYPosition;

    void Start()
    {
        _oldXPosition = transform.position.x;
        _oldYPosition = transform.position.y;
    }

    void Update()
    {
        if (transform.position.x != _oldXPosition)
        {
            if (m_onCameraXTranslate != null)
            {
                float delta = _oldXPosition - transform.position.x;
                m_onCameraXTranslate(delta);
            }

            _oldXPosition = transform.position.x;
        }
        if (transform.position.y != _oldYPosition)
        {
            if (m_onCameraYTranslate != null)
            {
                float delta = _oldYPosition - transform.position.y;
                m_onCameraYTranslate(delta);
            }

            _oldYPosition = transform.position.y;
        }
    }
}