using UnityEngine;
using Cinemachine;

public class RailStart : MonoBehaviour
{
    //public Transform railStartPos;
    public Transform endPos;
    public int levelToPassTheRail;
    public Collider leftTrackCollider;
    public Collider rightTrackCollider;
    public Collider groundColliderInRail;
    public CinemachineVirtualCamera cmInRail;
}
