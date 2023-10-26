using UnityEngine;

public class SwerveInputSystem : MonoBehaviour
{
    private float _lastFrameFingerPositionX;
    private float _moveFactorX;
    public float MoveFactorX => _moveFactorX;

    private void Update()
    {
        /*        if (Input.GetMouseButtonDown(0))
                {
                    _lastFrameFingerPositionX = Input.mousePosition.x;
                }
                else if (Input.GetMouseButton(0))
                {
                    _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
                    _lastFrameFingerPositionX = Input.mousePosition.x;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _moveFactorX = 0f;
                }*/

        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }
            if (t.phase == TouchPhase.Moved)
            {
                _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }
            if (t.phase == TouchPhase.Ended)
            {
                _moveFactorX = 0f;
            }
            if (t.phase == TouchPhase.Stationary)
            {
                _moveFactorX = 0f;
            }
        }
        // Debug.LogWarning("_moveFactorX: " + _moveFactorX);
    }
}