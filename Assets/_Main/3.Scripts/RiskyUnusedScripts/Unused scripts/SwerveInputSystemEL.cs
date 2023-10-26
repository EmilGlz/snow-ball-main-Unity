using UnityEngine;
using UnityEngine.UI;

public class SwerveInputSystemEL : MonoBehaviour
{
    private float _lastFrameFingerPositionX;
    private float _moveFactorX;
    public float MoveFactorX => _moveFactorX;

    [SerializeField] Text inputText;
    // Swipe
    //[Header("Swipe")]
    //public float swipeRange;
    //public float tapRange;


    //public bool swipedUp = false;
    //Vector2 startTouchPos;
    //Vector2 currentTouchPos;
    //Vector2 endTouchPos;

    private void Update()
    {
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                _lastFrameFingerPositionX = Input.mousePosition.x;
                //startTouchPos = t.position;
            }
            if (t.phase == TouchPhase.Moved)
            {
                _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = Input.mousePosition.x;

                //currentTouchPos = t.position;
                //Vector2 distance = currentTouchPos - startTouchPos;
                //if (!swipedUp)
                //{
                //    if (distance.y > swipeRange)
                //    {
                //        swipedUp = true;
                //        Debug.Log("Up");
                //    }
                //    else if (distance.y < -swipeRange)
                //    {
                //        swipedUp = true;
                //        Debug.Log("Down");
                //    }
                //}
            }
            if (t.phase == TouchPhase.Ended)
            {
                _moveFactorX = 0f;

                //swipedUp = false;
            }
            if (t.phase == TouchPhase.Stationary)
            {
                _moveFactorX = 0f;
            }
        }
        //inputText.text = _moveFactorX.ToString();
    }
}