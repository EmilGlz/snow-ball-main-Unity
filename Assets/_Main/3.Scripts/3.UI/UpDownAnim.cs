using UnityEngine;

public class UpDownAnim : MonoBehaviour
{
    [SerializeField] RectTransform animObject;
    [SerializeField] float animHeight;
    [SerializeField] float speed;
    [SerializeField] bool isOpposite;
    [SerializeField] bool isMoving;
    private float changeHeight;
    private bool goesDown = false;

    private void Start()
    {
        if (isOpposite)
        {
            changeHeight = 0;
            goesDown = !goesDown;
        }
        else
        {
            changeHeight = animHeight;
            if (isMoving) speed *= 2;
        }
    }

    private void FixedUpdate()
    {
        if (goesDown)
        {
            animObject.anchoredPosition = new Vector2(animObject.anchoredPosition.x, animObject.anchoredPosition.y - speed); ;
            changeHeight -= speed;
            if (changeHeight <= 0) goesDown = !goesDown;
        }
        else
        {
            animObject.anchoredPosition = new Vector2(animObject.anchoredPosition.x, animObject.anchoredPosition.y + speed); ;
            changeHeight += speed;
            if (changeHeight >= animHeight) goesDown = !goesDown;
        }
    }
}
