using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SurfaceSlider))]
public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected float _speed;

    private Rigidbody _rigidbody;
    private SurfaceSlider _surfaceSlider;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _surfaceSlider = GetComponent<SurfaceSlider>();
    }

    public void Move(Vector3 direction,int speed,int sideMove)
    {
        Vector3 directionAlongSurface = _surfaceSlider.Project(direction.normalized);
        print("<color=yellow>directionAlongSurface: </color>" + directionAlongSurface);
        Vector3 offset = directionAlongSurface * (_speed * Time.deltaTime);

        //_rigidbody.MovePosition(_rigidbody.position + offset);
        _rigidbody.MovePosition(transform.position + offset + transform.forward * speed + transform.right * sideMove);

    }
}
