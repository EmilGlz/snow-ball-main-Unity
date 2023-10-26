using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    private SwerveInputSystem _swerveInputSystem;
    [SerializeField] private float swerveSpeed = 0.5f;
    [SerializeField] private float maxSwerveAmount = 1f;
    [SerializeField] Rigidbody rb;

    public float swerveAmount;
    float lastSwerveAmount = 0;

    float currentSwerveVelocity;

    private void Awake()
    {
        _swerveInputSystem = GetComponent<SwerveInputSystem>();
    }

    private void Update()
    {
        swerveAmount = Time.deltaTime * swerveSpeed * _swerveInputSystem.MoveFactorX;
        //swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);





        //transform.Translate(swerveAmount, 0, 0);


        Vector3 targetPosition = transform.TransformPoint(new Vector3(swerveAmount, 0, 0));

        //transform.position = Mathf.SmoothDamp(transform.position, targetPosition, ref movingSphere.velocity, 0.3f);
        currentSwerveVelocity = Mathf.SmoothDamp(lastSwerveAmount, swerveAmount, ref currentSwerveVelocity, 1f);
        rb.AddForce(transform.right.normalized * currentSwerveVelocity, ForceMode.Impulse);
        lastSwerveAmount = swerveAmount;
    }
}