using UnityEngine;

public class RailMovement : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;

    void Update()
    {
        transform.position = Vector3.Lerp(start.position, end.position, 1000);
    }
}
