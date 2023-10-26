using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private float _lastFrameMousePositionX;
    private float _moveFactorX;
    public float MoveFactorX => _moveFactorX;

    private void Update()
    {
        /*        if (Input.GetMouseButtonDown(0))
                {
                    _lastFrameMousePositionX = Input.mousePosition.x;
                }
                else if (Input.GetMouseButton(0))
                {
                    _moveFactorX = Input.mousePosition.x - _lastFrameMousePositionX;
                    _lastFrameMousePositionX = Input.mousePosition.x;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _moveFactorX = 0;
                }*/

        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                _lastFrameMousePositionX = Input.mousePosition.x;
            }
            if (t.phase == TouchPhase.Moved)
            {
                _moveFactorX = Input.mousePosition.x - _lastFrameMousePositionX;
                _lastFrameMousePositionX = Input.mousePosition.x;

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

    }
}
