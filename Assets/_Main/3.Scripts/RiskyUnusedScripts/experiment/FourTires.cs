using UnityEngine;

public class FourTires : MonoBehaviour
{
    public Transform backLeft;
    public Transform backRight;
    public Transform frontLeft;
    public Transform frontRight;
    public RaycastHit lr;
    public RaycastHit rr;
    public RaycastHit lf;
    public RaycastHit rf;
    public Vector3 upDir;

    void Update()
    {
        Physics.Raycast(backLeft.position + Vector3.up, Vector3.down, out lr);
        Physics.Raycast(backRight.position + Vector3.up, Vector3.down, out rr);
        Physics.Raycast(frontLeft.position + Vector3.up, Vector3.down, out lf);
        Physics.Raycast(frontRight.position + Vector3.up, Vector3.down, out rf);


        // Get the vectors that connect the raycast hit points

        Vector3 a = rr.point - lr.point;
        Vector3 b = rf.point - rr.point;
        Vector3 c = lf.point - rf.point;
        Vector3 d = rr.point - lf.point;

        // Get the normal at each corner

        Vector3 crossBA = Vector3.Cross(b, a);
        Vector3 crossCB = Vector3.Cross(c, b);
        Vector3 crossDC = Vector3.Cross(d, c);
        Vector3 crossAD = Vector3.Cross(a, d);

        // Calculate composite normal

        //  transform.up = (crossBA + crossCB + crossDC + crossAD).normalized;

        Vector3 newUp = (crossBA + crossCB + crossDC + crossAD).normalized;

        transform.up = Vector3.Lerp(transform.up, newUp, Time.deltaTime);

    }
}
