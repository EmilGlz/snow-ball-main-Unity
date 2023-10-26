using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EZCameraShake;
using System.Collections;

public class Grow : MonoBehaviour
{
    [SerializeField] SwerveController swerve;
    [SerializeField] Rigidbody snowballRB;
    [SerializeField] Transform snowball;
    public SphereCollider playerCollider;
    [SerializeField] ParticleSystem snowDeathVFX;
    [SerializeField] ParticleSystem snowSmokeVFX;
    [SerializeField] ParticleSystem leftSparkVFX;
    [SerializeField] ParticleSystem rightSparkVFX;
    [SerializeField] CinemachineVirtualCamera cmForward;
    [SerializeField] CinemachineVirtualCamera cmUnder;
    [SerializeField] CinemachineVirtualCamera cmUp;
    [SerializeField] MMovement movement;
    [SerializeField] AudioSource DeathSound;
    [SerializeField] UIManager uIManager;
    [SerializeField] ParticleSystem windVFX;
    [SerializeField] ParticleSystem snowSmokeTrail;
    [SerializeField] MMovement mMovement;
    [SerializeField] List<GameObject> stickedHumans;
    [SerializeField] GameObject[] humanPlacesInSnowballPositions;
    [SerializeField] Texture[] playerTextures;
    [SerializeField] Material[] playerMaterials;
    [SerializeField] Vector3[] humanCorpsPosInMinimumScale;
    [SerializeField] Vector3 forwardCameraOffset;
    [SerializeField] Vector3 forwardCameraRotation;
    [Space(10)]
    public int growLevel = 1;
    public float minimumSnowScale;
    public float maximumSnowScale;
    public float startPosY;
    public float growingHeightConstant;
    public int maxLakeLvl;
    [SerializeField] float corpExtendScale;
    [SerializeField] float growingTime;
    [SerializeField] float growingScale;
    [SerializeField] bool onLake;
    [SerializeField] float humanCorpsExtendPosConstant;
    [HideInInspector] public float cmForwardDampingX_Start;

    LakeController lakeController;
    CinemachineVirtualCamera cmInRail;
    int humanInSnowballCount = 0;

    private void Start()
    {
        cmForwardDampingX_Start = cmForward.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping.x;
        startPosY = transform.parent.position.y;
        humanCorpsPosInMinimumScale = new Vector3[humanPlacesInSnowballPositions.Length];
        for (int i = 0; i < humanPlacesInSnowballPositions.Length; i++)
        {
            humanCorpsPosInMinimumScale[i] = humanPlacesInSnowballPositions[i].transform.localPosition;
        }
    }

    public void SetForwardCamDampingX(float xDamping)
    {
        cmForward.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping.x = xDamping;
    }

    void TurnCameraForward()
    {
        cmForward.Priority = 13;
        cmUnder.Priority = 11;
        cmUp.Priority = 12;
        if (cmInRail != null) cmInRail.Priority = 10;
    }

    void TurnCameraUnder()
    {
        //cmUnder.gameObject.SetActive(true);
        //cmForward.gameObject.SetActive(false);
        //cmUp.gameObject.SetActive(false);
        cmUnder.Priority = 13;
        cmForward.Priority = 11;
        cmUp.Priority = 12;
        if (cmInRail != null)
        {
            cmInRail.Priority = 10;
        }
    }

    void TurnCameraUp()
    {
        //cmUp.gameObject.SetActive(true);
        //cmForward.gameObject.SetActive(false);
        //cmInRail.gameObject.SetActive(false);
        //cmUnder.gameObject.SetActive(false);
        cmUp.Priority = 13;
        cmUnder.Priority = 12;
        cmForward.Priority = 11;
        if (cmInRail != null)
        {
            cmInRail.Priority = 10;
        }
    }

    void TurnCameraInRail()
    {
        //cmInRail.gameObject.SetActive(true);
        //cmUp.gameObject.SetActive(false);
        //cmForward.gameObject.SetActive(false);
        //cmUnder.gameObject.SetActive(false);
        cmUp.Priority = 10;
        cmUnder.Priority = 12;
        cmForward.Priority = 11;
        if (cmInRail != null)
        {
            cmInRail.Priority = 13;
        }
    }

    public void TurnWindVFX(bool vfxOn)
    {
        if (vfxOn)
        {
            windVFX.Play();
        }
        else
        {
            windVFX.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BG")) // big grow, 1 dene yeyende en boyuk razmer olur
        {
            Debug.Log("BG");
            growLevel = 10;
            SetGrowLevel(growLevel);
            //GrowSnowball(growingScale);
        }
        else if (other.CompareTag("MG")) // middle grow, 3 dene yenende max razmer olur
        {
            Debug.Log("MG");
            IncreaseGrowLevelBy(3);
            SetGrowLevel(growLevel);
            //GrowSnowball(growingScale);
        }
        else if (other.CompareTag("LG")) // little grow, 10 dene yeyende max razmer olur, adamlar
        {
            Debug.Log("LG");
            IncreaseGrowLevelBy(1);
            SetGrowLevel(growLevel);
            //GrowSnowball(growingScale);
        }
        else if (other.CompareTag("BS")) // big shrink, 1 dene yeyende min razmer olur
        {
            Debug.Log("BS");
            growLevel = 1;
            DecreaseGrowLevelBy(10);
            SetGrowLevel(growLevel);
            //ShrinkSnowball(other, growingScale);
        }
        else if (other.CompareTag("LS")) // little shrink, 10 dene yeyende min balaca olur
        {
            Debug.Log("LS");
            DecreaseGrowLevelBy(1);
            SetGrowLevel(growLevel);
            //ShrinkSnowball(other, growingScale);
        }
        else if (other.CompareTag("TRMPLN"))
        {
            swerve.testTime = 0f;
            snowSmokeTrail.Stop();
            swerve.onRail = true;
            //snowballRB.useGravity = true;
            swerve.currentAngle = 0f;
            Trampoline tmp = other.GetComponent<Trampoline>();
            tmp.TrampolineAnimate();
            //StartCoroutine(swerve.AnimateJumpZ(transform.parent.gameObject, tmp.destination, 1.5f));
            snowballRB.isKinematic = true;
            //snowballRB.interpolation = RigidbodyInterpolation.Interpolate;
            swerve.playerPosBeforeJump = transform.parent.position;
            swerve.circularMove = 9;
            swerve.circularMovementRadius = (tmp.destination.position.y - transform.parent.position.y) + 1f;
            TurnCameraUp();
        }
        else if (other.CompareTag("RailS"))
        {
            windVFX.Play();
            Debug.Log("RailS");
            RailStart railStart = other.GetComponent<RailStart>();
            cmInRail = railStart.cmInRail;
            swerve.circularMove = -1;
            StartCoroutine(MakeRBKinematic());
            //snowballRB.isKinematic = false;
            //snowballRB.interpolation = RigidbodyInterpolation.None;
            //LeanTween.move(transform.parent.gameObject, other.gameObject.transform.position, .2f).setOnComplete( ()=>
            // it is moving to position, 0.5 meters from startpos to endpos 
            if (swerve.movingDirection == 0) // forward
            {
                leftSparkVFX.transform.rotation = Quaternion.Euler(0f, 225f, 0f);
                rightSparkVFX.transform.rotation = Quaternion.Euler(0f, 225f, 0f);
                LeanTween.rotateY(transform.parent.gameObject, 45f, 1f);
            }
            else if (swerve.movingDirection == 1) // right
            {
                leftSparkVFX.transform.rotation = Quaternion.Euler(0f, -45f, 0f);
                rightSparkVFX.transform.rotation = Quaternion.Euler(0f, -45f, 0f);
                LeanTween.rotateY(transform.parent.gameObject, 135f, 1f);
            }
            else if (swerve.movingDirection == 2) // back
            {
                leftSparkVFX.transform.rotation = Quaternion.Euler(0f, 45f, 0f);
                rightSparkVFX.transform.rotation = Quaternion.Euler(0f, 45f, 0f);
                LeanTween.rotateY(transform.parent.gameObject, 225f, 1f);
            }
            else if (swerve.movingDirection == 3) // left
            {
                leftSparkVFX.transform.rotation = Quaternion.Euler(0f, 135f, 0f);
                rightSparkVFX.transform.rotation = Quaternion.Euler(0f, 135f, 0f);
                LeanTween.rotateY(transform.parent.gameObject, -45f, 1f);
            }
            //LeanTween.move(transform.parent.gameObject, other.gameObject.transform.position + (railStart.endPos.position - other.gameObject.transform.position).normalized * .5f, .5f).setOnComplete(() =>
            LeanTween.move(transform.parent.gameObject, other.gameObject.transform.position +
                (new Vector3(railStart.endPos.position.x, other.gameObject.transform.position.y, railStart.endPos.position.z) -
                other.gameObject.transform.position).normalized * .5f, .2f).setOnComplete(() =>
                {
                    if (growLevel >= railStart.levelToPassTheRail) // can pass the rail
                    {
                        railStart.leftTrackCollider.isTrigger = true;
                        railStart.rightTrackCollider.isTrigger = true;
                        railStart.groundColliderInRail.enabled = true;
                    }
                    else // you cannot pas the rails, DIE
                    {
                        railStart.leftTrackCollider.isTrigger = false;
                        railStart.rightTrackCollider.isTrigger = false;
                        railStart.groundColliderInRail.enabled = false;
                        StartCoroutine(mMovement.SetSpeed(0, 2f));
                        DIE();
                    }
                    swerve.circularMove = 0;
                    swerve.canMoveNormally = false;
                    swerve.canSwerve = false;
                    swerve.targetPosToReach = railStart.endPos;
                    //Vibrator.Vibrate(150);
                    //snowballRB.useGravity = true;
                    //LeanTween.move(transform.parent.gameObject, railStart.endPos.position, 3f);
                });
        }
        else if (other.CompareTag("CamChange"))
        {
            TurnCameraInRail();
        }
        else if (other.CompareTag("RailF")) // big grow, 1 dene yeyende en boyuk razmer olur
        {
            Debug.Log("RailF");
            swerve.testTime = 0f;
            windVFX.Stop();
            if (swerve.movingDirection == 0) // forward
            {
                LeanTween.rotateY(transform.parent.gameObject, 0f, 1f);
            }
            else if (swerve.movingDirection == 1) // right
            {
                LeanTween.rotateY(transform.parent.gameObject, 90f, 1f);
            }
            else if (swerve.movingDirection == 2) // back
            {
                LeanTween.rotateY(transform.parent.gameObject, 180f, 1f);
            }
            else if (swerve.movingDirection == 3) // left
            {
                LeanTween.rotateY(transform.parent.gameObject, -90f, 1f);
            }
            swerve.currentAngle = 0;
            RailFinish railFinish = other.GetComponent<RailFinish>();
            swerve.maxRoadPosLeft = railFinish.GetMaxLeftPos(swerve.movingDirection);
            swerve.maxRoadPosRight = railFinish.GetMaxRightPos(swerve.movingDirection);
            swerve.targetPosToReach = railFinish.destination;
            swerve.circularMove = 11;
            swerve.playerPosBeforeJump = transform.parent.position;
            swerve.circularMovementRadius = 2.5f;
            //StartCoroutine(movement.SetSpeed(5, 0));
        }
        else if (other.CompareTag("LKS"))
        {
            onLake = true;
            lakeController = other.GetComponent<LakeController>();
            maxLakeLvl = lakeController.levelToBeBroken;
        }
        else if (other.CompareTag("LKF"))
        {
            onLake = false;
        }
        else if (other.CompareTag("AfterRailEnd"))
        {
            TurnCameraForward();
            swerve.onRail = false;
            snowSmokeTrail.Play();
            swerve.circularMove = 0;
            swerve.canMoveNormally = true;
            swerve.canSwerve = true;
        }
        else if (other.CompareTag("ENDLVL"))
        {
            // play humans animations from commondata
            for (int i = 0; i < CommonData.Instance.peopleAtEnd.Length; i++)
            {
                CommonData.Instance.peopleAtEnd[i].GetComponent<Animator>().SetTrigger("Run");
                CommonData.Instance.peopleAtEnd[i].GetComponent<HumanController>().canRun = true;
            }
        }
        else if (other.CompareTag("ENDBNLVL"))
        {
            EndLvl endLvl = other.GetComponent<EndLvl>();
            swerve.canSwerve = false;
            swerve.circularMove = 0;
            swerve.canMoveNormally = false;
            swerve.targetPosToReach = endLvl.targetPosAfterFinish;
        }
        else if (other.CompareTag("ENDGAME"))
        {
            // after ~1.5s delay, show ui, vfx, ...
            other.GetComponent<EndLVL1>().confetiInTheEnd.Play();
            StartCoroutine(GameFinish(1.5f));
        }
        else if (other.CompareTag("LeftTurn"))
        {
            snowballRB.isKinematic = false;
            swerve.currentAngle = 0f;
            Turn turn = other.GetComponent<Turn>();
            swerve.playerPosBeforeJump = transform.parent.position;
            //swerve.circularMovementRadius = Mathf.Abs(transform.position.x - turn.GetTurningPoss(0));
            swerve.circularMovementRadius = Mathf.Abs((transform.position - turn.GetTurningPosition()).magnitude);
            swerve.canSwerve = false;
            if (swerve.movingDirection == 0) // forward to left
            {
                swerve.circularMove = 4;
                swerve.movingDirection = 3;
            }
            else if (swerve.movingDirection == 1) // right to forward
            {
                swerve.circularMove = 2;
                swerve.movingDirection = 0;
            }
            else if (swerve.movingDirection == 2) // back to right
            {
                swerve.circularMove = 6;
                swerve.movingDirection = 1;
            }
            else if (swerve.movingDirection == 3) // left to back
            {
                swerve.circularMove = 8;
                swerve.movingDirection = 2;
            }
            TurnWindVFX(true);
            swerve.maxRoadPosRight = turn.GetMaxRightPos(swerve.movingDirection);
            swerve.maxRoadPosLeft = turn.GetMaxLeftPos(swerve.movingDirection);
        }
        else if (other.CompareTag("RightTurn"))
        {
            snowballRB.isKinematic = false;
            Turn turn = other.GetComponent<Turn>();
            swerve.currentAngle = 0f;
            swerve.playerPosBeforeJump = transform.parent.position;
            swerve.canSwerve = false;
            swerve.circularMovementRadius = Mathf.Abs((transform.position - turn.GetTurningPosition()).magnitude);
            if (swerve.movingDirection == 0) // forward to right
            {
                swerve.circularMove = 1;
                swerve.movingDirection = 1;
            }
            else if (swerve.movingDirection == 1) // right to back
            {
                swerve.circularMove = 5;
                swerve.movingDirection = 2;
            }
            else if (swerve.movingDirection == 2) // back to left
            {
                swerve.circularMove = 7;
                swerve.movingDirection = 3;
            }
            else if (swerve.movingDirection == 3) // left to forward
            {
                swerve.circularMove = 3;
                swerve.movingDirection = 0;
            }
            swerve.maxRoadPosRight = turn.GetMaxRightPos(swerve.movingDirection);
            swerve.maxRoadPosLeft = turn.GetMaxLeftPos(swerve.movingDirection);
        }
        else if (other.CompareTag("ADS")) // ascend-descend Start
        {
            swerve.testTime = 0f;
            swerve.circularMove = 10;
            swerve.canMoveNormally = false;
            swerve.canSwerve = false;
            snowballRB.useGravity = false;
        }
        else if (other.CompareTag("ADJS")) // ascend-descend Jump Start
        {
            windVFX.Play();
        }
        else if (other.CompareTag("ADJF")) // ascend-descend Jump Finish
        {
            windVFX.Stop();
        }
        else if (other.CompareTag("ADF")) // ascend-descend Finish
        {
            snowSmokeTrail.Play();
            snowballRB.useGravity = false;
            swerve.canMoveNormally = true;
            swerve.canSwerve = true;
            swerve.circularMove = 0;
        }
        else if (other.CompareTag("IgloS")) // Iglo start
        {
            Debug.Log("IgloS");
            TurnCameraUnder();
        }
        else if (other.CompareTag("IgloF")) // Iglo finish
        {
            Debug.Log("IgloF");
            TurnCameraForward();
        }
        else if (other.CompareTag("CMB")) // Combiner
        {
            if (growLevel >= other.GetComponent<CombinerController>().snowBallLevelToGameOver)
            {
                // SnowBall dies, game is over
                snowDeathVFX.transform.position = transform.position;
                snowDeathVFX.Play();
                Debug.Log("DIE, combiner");
            }
        }
        else if (other.CompareTag("HMN")) // human
        {
            IncreaseGrowLevelBy(2);
            //GrowSnowball(growingScale);
            SetGrowLevel(growLevel);
            stickedHumans.Add(other.gameObject);
            if (humanInSnowballCount < humanPlacesInSnowballPositions.Length)
            {
                //playerMaterials[humanInSnowballCount].mainTexture = playerTextures[other.gameObject.layer - 10];
                humanPlacesInSnowballPositions[humanInSnowballCount].SetActive(true);
                humanInSnowballCount++;
            }
            else
            {
                humanInSnowballCount = 0;
                //playerMaterials[humanInSnowballCount].mainTexture = playerTextures[other.gameObject.layer - 10];
                humanPlacesInSnowballPositions[humanInSnowballCount].SetActive(true);
                humanInSnowballCount++;
            }
            Destroy(other.gameObject);

        }
        if (other.gameObject.layer >= 10 && other.gameObject.layer <= 16) // we hit human
        {
            IncreaseGrowLevelBy(1);
            //GrowSnowball(growingScale);
            SetGrowLevel(growLevel);
            stickedHumans.Add(other.gameObject);
            if (humanInSnowballCount < humanPlacesInSnowballPositions.Length)
            {
                playerMaterials[humanInSnowballCount].mainTexture = playerTextures[other.gameObject.layer - 10];
                humanPlacesInSnowballPositions[humanInSnowballCount].SetActive(true);
                humanInSnowballCount++;
            }
            else
            {
                humanInSnowballCount = 0;
                playerMaterials[humanInSnowballCount].mainTexture = playerTextures[other.gameObject.layer - 10];
                humanPlacesInSnowballPositions[humanInSnowballCount].SetActive(true);
                humanInSnowballCount++;
            }
        }
        else if (other.gameObject.layer == 17) // tree
        {
            // Die
            DIE();
            Debug.Log("DIE, tree");
        }
    }

    IEnumerator GameFinish(float delay)
    {
        yield return new WaitForSeconds(delay);
        swerve.circularMove = -1;
        UIScript.Instance.WinLevel(growLevel);
        StartCoroutine(mMovement.SetSpeed(0f, 0f));
    }

    IEnumerator MakeRBKinematic()
    {
        yield return null;
        yield return null;
        snowballRB.isKinematic = false;
    }

    public void DIE()
    {
        StartCoroutine(mMovement.SetSpeed(0, 0f));
        swerve.canSwerve = false;
        DieVFX();
        Vibrator.Vibrate(300);
        //DeathSound.Play();
        CameraShaker.Instance.ShakeOnce(3f, 3f, .1f, 1f);
        UIScript.Instance.GameOver();
    }

    private void DieVFX()
    {
        //snowDeathVFX.transform.position = transform.position;
        //snowDeathVFX.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RT")) // little shrink
        {
            rightSparkVFX.transform.position = other.ClosestPoint(transform.position);
            rightSparkVFX.Play();
        }
        if (other.CompareTag("LT")) // little shrink
        {
            leftSparkVFX.transform.position = other.ClosestPoint(transform.position);
            leftSparkVFX.Play();
        }
        if (other.CompareTag("PL")) // colliding with plane
        {
            //snowSmokeVFX.transform.position = transform.position + Vector3.one;
            //snowSmokeVFX.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RT")) // little shrink
        {
            rightSparkVFX.Stop();
        }
        if (other.CompareTag("LT")) // little shrink
        {
            leftSparkVFX.Stop();
        }
        if (other.CompareTag("PL")) // colliding with plane
        {
            snowSmokeVFX.Stop();
        }
    }

    void IncreaseGrowLevelBy(int increaseLevel)
    {
        growLevel += increaseLevel;
        if (growLevel > 10)
        {
            growLevel = 10;
        }
        else if (growLevel < 1)
        {
            growLevel = 1;
        }
    }

    void DecreaseGrowLevelBy(int decreaseLevel)
    {
        growLevel -= decreaseLevel;
        if (growLevel < 1)
        {
            growLevel = 1;
        }
        else if (growLevel > 10)
        {
            growLevel = 10;
        }
    }
    //[SerializeField] bool growingAnimIsOn;
    public void SetGrowLevel(int level)
    {
        Vector3 nextScale = Vector3.one * minimumSnowScale + Vector3.one * (level - 1) * growingScale;
        //bool gettingBigger;
        if (nextScale.x > maximumSnowScale)
        {
            nextScale.x = maximumSnowScale;
            nextScale.y = maximumSnowScale;
            nextScale.z = maximumSnowScale;
        }
        else if (nextScale.x < minimumSnowScale)
        {
            nextScale.x = minimumSnowScale;
            nextScale.y = minimumSnowScale;
            nextScale.z = minimumSnowScale;
        }
        LeanTween.scale(gameObject, nextScale, growingTime);
        playerCollider.radius = nextScale.x / 2f;
        SetHumanCorpPositions(playerCollider.radius);
        transform.parent.position = new Vector3(transform.parent.position.x, startPosY + transform.localScale.y * growingHeightConstant, transform.parent.position.z);
        if (onLake)
        {
            if (growLevel > maxLakeLvl)
            {
                // DIE in the lake
                lakeController.BreakIce(this, swerve, snowballRB);
            }
        }
    }

    void SetHumanCorpPositions(float sphereRadius)
    {
        for (int i = 0; i < humanPlacesInSnowballPositions.Length; i++)
        {
            humanPlacesInSnowballPositions[i].transform.localPosition = humanCorpsPosInMinimumScale[i].normalized * sphereRadius * humanCorpsExtendPosConstant;
        }
    }

    public void ResetHumanCorpsPositions()
    {
        for (int i = 0; i < humanPlacesInSnowballPositions.Length; i++)
        {
            humanPlacesInSnowballPositions[i].transform.localPosition = humanCorpsPosInMinimumScale[i];
            humanPlacesInSnowballPositions[i].SetActive(false);
        }
    }
}




    //private void GrowSnowball(float increasingSize)
    //{
    //    Vector3 nextScale = Vector3.one * minimumSnowScale + Vector3.one * (growLevel - 1) * increasingSize;
    //    if (nextScale.x > maximumSnowScale)
    //    {
    //        nextScale.x = maximumSnowScale;
    //        nextScale.y = maximumSnowScale;
    //        nextScale.z = maximumSnowScale;
    //    }
    //    LeanTween.scale(gameObject, nextScale, growingTime);
    //    playerCollider.radius = nextScale.x / 2f;
    //    ExtendHumanCorpPositions(playerCollider.radius);
    //    transform.parent.position = new Vector3(transform.parent.position.x, startPosY + transform.localScale.y * growingHeightConstant, transform.parent.position.z);
    //    if (onLake)
    //    {
    //        if (growLevel > maxLakeLvl)
    //        {
    //            // DIE
    //            DIE();
    //        }
    //    }
    //}
    //private void ShrinkSnowball(Collider other, float decreasingSize)
    //{
    //    Vector3 nextScale = Vector3.one * minimumSnowScale + Vector3.one * (growLevel - 1) * decreasingSize;
    //    if (nextScale.x < minimumSnowScale)
    //    {
    //        nextScale.x = minimumSnowScale;
    //        nextScale.y = minimumSnowScale;
    //        nextScale.z = minimumSnowScale;
    //    }
    //    LeanTween.scale(gameObject, nextScale, growingTime);
    //    playerCollider.radius = nextScale.x / 2f;
    //    SetHumanCorpPositions(playerCollider.radius);
    //    transform.parent.position = new Vector3(transform.parent.position.x, startPosY + transform.localScale.y * growingHeightConstant, transform.parent.position.z);
    //}

