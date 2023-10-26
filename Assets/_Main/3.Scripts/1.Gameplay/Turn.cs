using UnityEngine;

public class Turn : MonoBehaviour
{
    [SerializeField] Transform maxRightPos;
    [SerializeField] Transform maxLeftPos;
    [SerializeField] Transform TurningPos;

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
        //else if (movingDirection == 2) // going forward
        //{
        //    maxRoadWidthRight = maxRightPos.position.x;
        //}
        //else if (movingDirection == 3) // going forward
        //{
        //    maxRoadWidthRight = maxRightPos.position.z;
        //}
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
        //else if (movingDirection == 3) // turning left-forward
        //{
        //    maxRoadWidthLeft = maxLeftPos.position.x;
        //}
        //else if (movingDirection == 4) // turning left
        //{
        //    maxRoadWidthLeft = maxLeftPos.position.z;
        //}
        return maxRoadWidthLeft;
    }

    public Vector3 GetTurningPosition()
    {
        return TurningPos.position;
    }

    /// <summary>
    /// returns the turning pos to get the radius of the circular motion
    /// </summary>
    /// <param name="leftRight_forward"> if player turns left or right, it is 1. else if player turns forward, it is 1</param>
    /// <returns></returns>
    public float GetTurningPoss(int leftRight_forward)
    {
        float res = 0;
        if (leftRight_forward == 0) // left or right
        {
            res = TurningPos.position.x;
        }
        else if (leftRight_forward == 1) // right-forward or left-forward
        {
            res = TurningPos.position.z;
        }
        return res;
    }

}
