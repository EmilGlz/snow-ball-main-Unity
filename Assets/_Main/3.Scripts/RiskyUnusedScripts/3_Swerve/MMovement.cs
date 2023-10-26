using UnityEngine;
using System.Collections;

public class MMovement : MonoBehaviour
{
    [SerializeField] protected float _speed;

    private Rigidbody _rigidbody;
    private SurfaceSlider _surfaceSlider;
    [SerializeField] SwerveController swerve;
    [SerializeField] Grow grow;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _surfaceSlider = GetComponent<SurfaceSlider>();
    }

    public float GetSpeed()
    {
        return _speed;
    }
    public IEnumerator SetSpeed( float speed, float delay )
    {
        yield return new WaitForSeconds(delay);
        _speed = speed;
    }

    public void Move(Vector3 direction, float sideMove)
    {
        /// moveDirection = 0, going forward
        /// moveDirection = 1, going right
        /// moveDirection = 2, going back
        /// moveDirection = 3, going left

        //_rigidbody.MovePosition(_rigidbody.position + offset);
        //_rigidbody.MovePosition(transform.position + offset + transform.forward * forwardSpeed + transform.right * sideMove);
        if (!swerve.onRail) // not on the rail
        {
            Vector3 directionAlongSurface = _surfaceSlider.Project(direction.normalized);
            Vector3 offset = directionAlongSurface * (_speed * Time.deltaTime);
            Vector3 nextPos = transform.position + offset + transform.right * sideMove;
            nextPos.y = grow.startPosY + grow.transform.localScale.y * grow.growingHeightConstant;
            _rigidbody.MovePosition(nextPos);
        }
        else
        {
            Vector3 directionAlongSurface = _surfaceSlider.Project(direction.normalized);
            Vector3 offset = directionAlongSurface * (_speed * Time.deltaTime);
            _rigidbody.MovePosition(transform.position + offset + transform.right * sideMove);
        }

        //Vector3 offset = transform.forward * (_speed * Time.deltaTime);
        //_rigidbody.MovePosition(transform.position + offset + transform.right * sideMove);

    }
}
