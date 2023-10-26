using UnityEngine;

public class AlignSurface : MonoBehaviour
{
    //declare the variables that are needed
    public bool grounded;
    private Vector3 posCur;
    private Quaternion rotCur;
    //private Vector3 turnVector;

    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1.5f) == true)
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);

            rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            posCur = new Vector3(transform.position.x, hit.point.y + 0.45f, transform.position.z);

            grounded = true;
        }

        else
        {
            grounded = false;
        }


        if (grounded == true)
        {
            transform.position = Vector3.Lerp(transform.position, posCur, Time.deltaTime * 7);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime * 7);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up * 2f, Time.deltaTime * 7);

            /*            if (transform.eulerAngles.x > 20)
                        {
                            turnVector.x -= Time.deltaTime * 1000;
                        }
                        else if (transform.eulerAngles.x < 20)
                        {
                            turnVector.x += Time.deltaTime * 1000;
                        }
                        rotCur.eulerAngles = Vector3.zero;
                        transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime);*/

        }
    }
}
