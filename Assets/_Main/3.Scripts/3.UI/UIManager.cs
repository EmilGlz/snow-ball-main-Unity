using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] MMovement mMovement;
    [SerializeField] SwerveController swerve;
    [SerializeField] Grow grow;
    [SerializeField] GameObject player;

    [SerializeField] RandomLevel randomLevel;
    [SerializeField] Image blackImage_WinMenu;
    [SerializeField] Image blackImage_StartMenu;
    [SerializeField] AudioSource buttonClickSound;
    [SerializeField] Material testMat;
    const string currentLevel = "";

    private void Start()
    {
        Debug.Log(testMat.name);
    }

    public void StartGame_OnClick()
    {
        Time.timeScale = 1f;
        StartCoroutine(mMovement.SetSpeed(5f, 0f));
        swerve.canSwerve = true;
    }



    public void NextLevel_OnClick()
    {
        blackImage_WinMenu.gameObject.SetActive(true);
        blackImage_StartMenu.gameObject.SetActive(true);
        LeanTween.value(0f, 1f, .5f).setOnUpdate(OnUpdate_WinMenu).setOnComplete(() =>
        {
            grow.playerCollider.isTrigger = true;
            grow.SetForwardCamDampingX(0f);
            ResetPlayer();
            randomLevel.GenerateNextLevel( GetLevelPieceCountByLevel(CommonData.Instance.currentLevel + 1) );
            StartCoroutine(SetCMForwardDampingAfterOneFrame());
            UIScript.Instance.SetStartMenuUI();
            LeanTween.value(1f, 0f, .5f).setOnUpdate(OnUpdate_StartMenu).setOnComplete(() =>
            {
                blackImage_WinMenu.gameObject.SetActive(false);
                blackImage_StartMenu.gameObject.SetActive(false);
            });
        });
    }

    private void ResetPlayer()
    {
        player.transform.position = swerve.gameStartPos;
        player.transform.rotation = Quaternion.identity;
        swerve.movingDirection = 0;
        swerve.canMoveNormally = true;
        swerve.canSwerve = false;
        swerve.circularMove = 0;
        swerve.currentAngle = 0f;
        swerve.maxRoadPosLeft = swerve.maxRoadPosLeftAtStart;
        swerve.maxRoadPosRight = swerve.maxRoadPosRightAtStart;
        grow.ResetHumanCorpsPositions();
        grow.SetGrowLevel(1);
    }

    IEnumerator SetCMForwardDampingAfterOneFrame()
    {
        yield return null;
        grow.SetForwardCamDampingX(grow.cmForwardDampingX_Start);
        //blackImage.gameObject.SetActive(false);
    }

    void OnUpdate_WinMenu(float a)
    {
        blackImage_WinMenu.color = new Color(blackImage_WinMenu.color.r, blackImage_WinMenu.color.g, blackImage_WinMenu.color.b, a);
    }

    void OnUpdate_StartMenu(float a)
    {
        blackImage_WinMenu.color = new Color(blackImage_WinMenu.color.r, blackImage_WinMenu.color.g, blackImage_WinMenu.color.b, a);
    }


    public void PauseGame_OnClick()
    {
        swerve.canSwerve = false;
        StartCoroutine(mMovement.SetSpeed(0f,0f));
    }
    public void ResumeGame_OnClick()
    {
        swerve.canSwerve = true;
        StartCoroutine(mMovement.SetSpeed(5f, 0f));
    }

    public void MainMenu_OnClick()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }
    
    public void RestartCurrentScene_OnClick()
    {
        Time.timeScale = 1f;
        grow.playerCollider.isTrigger = true;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex).completed += (value) => 
        {
            Debug.Log("Log after sceneload");
            UIScript.Instance.ResetUI(); 
        };
        
    }

    public int GetLevelPieceCountByLevel(int level)
    {
        if (level < 8)
        {
            return 2 + level;
        }
        else
        {
            return 10;
        }
    }

    public void ButtonClick()
    {
        buttonClickSound.Play();
    }

}
