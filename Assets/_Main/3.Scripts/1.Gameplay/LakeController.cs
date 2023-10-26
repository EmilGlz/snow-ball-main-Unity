using UnityEngine;
using System.Collections;

public class LakeController : MonoBehaviour
{
    public int levelToBeBroken;
    [SerializeField] GameObject fullLake;
    [SerializeField] GameObject brokenLake;
    //[SerializeField] Grow playerGrow;
    [SerializeField] ParticleSystem lakeBreakeVFXPrefab;
    //[SerializeField] Rigidbody playerRB;
    //[SerializeField] SwerveElgun swerve;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter: " + other.gameObject.layer);
        if (other.gameObject.layer == 3)
        {
            Grow playerGrow = other.GetComponent<Grow>();
            SwerveController swerve = other.transform.parent.GetComponent<SwerveController>();
            Rigidbody playerRB = other.transform.parent.GetComponent<Rigidbody>();
            if (playerGrow.growLevel >= levelToBeBroken)
            {
                // break ice
                BreakIce(playerGrow, swerve, playerRB);
            }
        }
    }

    public void BreakIce(Grow playerGrow, SwerveController swerve, Rigidbody playerRB)
    {
        StartCoroutine(LakeDie(swerve, playerGrow, 1f));
        swerve.canSwerve = false;
        swerve.circularMove = -1;
        playerGrow.playerCollider.isTrigger = false;
        playerRB.useGravity = true;
        Debug.Log("break ice");
        fullLake.SetActive(false);
        brokenLake.SetActive(true);
        lakeBreakeVFXPrefab.gameObject.SetActive(true);
        lakeBreakeVFXPrefab.Play();
    }

    IEnumerator LakeDie( SwerveController swerve, Grow playerGrow, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        swerve.circularMove = -1;
        playerGrow.DIE();
    }

}
