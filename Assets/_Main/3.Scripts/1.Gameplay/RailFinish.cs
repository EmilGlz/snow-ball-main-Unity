using UnityEngine;

public class RailFinish : MonoBehaviour
{
    public Transform destination;
    public Transform maxRightPos;
    public Transform maxLeftPos;

    public float GetMaxRightPos(int movingDirection)
    {
        float maxRoadWidthRight = 0;
        if (movingDirection == 0 || movingDirection == 2) // going forward/backwards
        {
            maxRoadWidthRight = maxRightPos.position.x;
        }
        else if (movingDirection == 1 || movingDirection == 3) // going left/right
        {
            maxRoadWidthRight = maxRightPos.position.z;
        }
        return maxRoadWidthRight;
    }

    public float GetMaxLeftPos(int movingDirection)
    {
        float maxRoadWidthLeft = 0;
        if (movingDirection == 0 || movingDirection == 2) // going forward/backwards
        {
            maxRoadWidthLeft = maxLeftPos.position.x;
        }
        else if (movingDirection == 1 || movingDirection == 3) // going left/right
        {
            maxRoadWidthLeft = maxLeftPos.position.z;
        }
        return maxRoadWidthLeft;
    }

    //public float maxRightPos;
    //public float maxLeftPos;
}
