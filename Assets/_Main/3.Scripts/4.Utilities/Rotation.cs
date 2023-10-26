
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] GameObject Object;
    [SerializeField] float RotationValueX;
    [SerializeField] float RotationValueY;
    [SerializeField] float RotationValueZ;
    [SerializeField] bool spaceWorld;
    void FixedUpdate()
    {
        Object.transform.Rotate(RotationValueX, RotationValueY, RotationValueZ, spaceWorld ? Space.World : Space.Self);
    }
}
