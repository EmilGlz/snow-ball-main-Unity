using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendDescendStart : MonoBehaviour
{
    public int maxLevelToCrossTheAD; // ex, =5 => 1,2,3,4,5 can cross the AD. Else, die 

    public Transform endPosBeforeJump;
    public Transform endPosAfterJump;
    public Transform endPosAfterDescend;
    public Transform jumpRadiusPos;
}
