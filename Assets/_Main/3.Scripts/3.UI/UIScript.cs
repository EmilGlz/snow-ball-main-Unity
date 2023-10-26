using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    #region Singleton
    private static UIScript _instance;
    public static UIScript Instance { get { return _instance; } }
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    private const string PP_Coin = "coin";
    private const string PP_Sound = "snd";
    private const string PP_Vibration = "vbr";

    [SerializeField] private GameObject introCanvas;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject inGame;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject gameWin;
    [SerializeField] private GameObject getCoins;
    [SerializeField] private GameObject shop;

    //Start menu
    [SerializeField] private float introTime = 2f;
    [SerializeField] private TMP_Text startMenuCoin;
    [SerializeField] private TMP_Text shopCoin;
    //

    //InGame
    [SerializeField] private Image levelFiller;
    [SerializeField] private RectTransform levelHandle;
    [SerializeField] private float handleLocalStartX;
    [SerializeField] private float handleLocalEndX;
    [SerializeField] private TMP_Text levelInfoText;
    //

    //Options
    [SerializeField] private RectTransform activeSettingsPanel;
    [SerializeField] private Image soundOptionImage;
    [SerializeField] private Image vibrationOptionImage;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Sprite vibrationOnSprite;
    [SerializeField] private Sprite vibrationOffSprite;
    //

    //Win game
    [SerializeField] private RectTransform[] stars;
    [SerializeField] private TMP_Text winScreenCoin;
    //

    //TEMPTEST
    private int numOfScreenShots;
    //

    private bool optionsOpen = false;
    private bool soundOn = true;
    private bool vibrationOn = true;

    private bool isMainMenu;

    private System.Random random;

    void Start()
    {
        random = new System.Random();

        if (!PlayerPrefs.HasKey(PP_Sound)) //First time
        {
            PlayerPrefs.SetInt(PP_Sound, 1);
            PlayerPrefs.SetInt(PP_Vibration, 1);
        }
        else
        {
            soundOn = PlayerPrefs.GetInt(PP_Sound) == 1;
            vibrationOn = PlayerPrefs.GetInt(PP_Vibration) == 1;

            soundOptionImage.sprite = soundOn ? soundOnSprite : soundOffSprite;
            vibrationOptionImage.sprite = vibrationOn ? vibrationOnSprite : vibrationOffSprite;
        }

        if (!PlayerPrefs.HasKey(PP_Coin)) // First Time
        {
            CommonData.Instance.coin  = 0;
            PlayerPrefs.SetInt(PP_Coin, CommonData.Instance.coin);
        }
        else
        {
            CommonData.Instance.coin = PlayerPrefs.GetInt(PP_Coin);
        }

        startMenuCoin.text = CommonData.Instance.coin.ToString();
        shopCoin.text = CommonData.Instance.coin.ToString();

        //startMenu.SetActive(false);
        Debug.Log("Setting in game false...");
        inGame.SetActive(false);
        gameOver.SetActive(false);
        pause.SetActive(false);
        gameWin.SetActive(false);
        getCoins.SetActive(false);
        shop.SetActive(false);

        //StartCoroutine(Intro());

        //TEMPSCRRENSHOT
        if (!PlayerPrefs.HasKey("ss"))
        {
            PlayerPrefs.SetInt("ss", 0);
            numOfScreenShots = 0;
        }
        else
        {
            numOfScreenShots = PlayerPrefs.GetInt("ss");
        }
        //
        AudioListener.volume = 1f;
    }

    public void ResetUI()
    {
        soundOn = PlayerPrefs.GetInt(PP_Sound) == 1;
        vibrationOn = PlayerPrefs.GetInt(PP_Vibration) == 1;

        soundOptionImage.sprite = soundOn ? soundOnSprite : soundOffSprite;
        vibrationOptionImage.sprite = vibrationOn ? vibrationOnSprite : vibrationOffSprite;
    }

    public void SetStartMenuUI()
    {
        startMenu.SetActive(true);
        inGame.SetActive(false);
        gameOver.SetActive(false);
        pause.SetActive(false);
        gameWin.SetActive(false);
        getCoins.SetActive(false);
    }


    private IEnumerator Intro()
    {
        yield return new WaitForSeconds(introTime);
        introCanvas.SetActive(false);
        startMenu.SetActive(true);
    }

    public void TapToPlay_OnClick()
    {
        startMenu.SetActive(false);
        inGame.SetActive(true);
        levelInfoText.text = string.Format("{0} - {1}", CommonData.Instance.currentLevel, CommonData.Instance.currentLevel + 1); 
        StartCoroutine(LevelInfoUpdate(CommonData.Instance.currentLevelTime));
    }
    public IEnumerator LevelInfoUpdate(float levelTime)
    {
        float currentTime = 0;
        while (currentTime < levelTime)
        {
            if (CommonData.Instance.pause)
            {
                yield return null;
                continue;
            }
            else
            {
                SetLevelFiller(currentTime / levelTime);
                currentTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void GameOver()
    {
        inGame.SetActive(false);
        gameOver.SetActive(true);
    }

    public void WinLevel(int growLevel)
    {
        for (int i = 0; i < 3; i++)
        {
            stars[i].gameObject.SetActive(false);
        }
        inGame.SetActive(false);
        gameWin.SetActive(true);

        int numOfStars, rand;

        if (growLevel < 4)
        {
            numOfStars = 1;
            rand = random.Next(250, 301);
        }
        else if(growLevel < 7)
        {
            numOfStars = 2;
            rand = random.Next(301, 381);
        }
        else
        {
            numOfStars = 3;
            rand = random.Next(381, 421);
        }

        StartCoroutine(EndGameStars(numOfStars));

        CommonData.Instance.collectedCoins = rand;
        winScreenCoin.text = rand.ToString();
        CommonData.Instance.coin += rand;
        PlayerPrefs.SetInt(PP_Coin, CommonData.Instance.coin);
        shopCoin.text = CommonData.Instance.coin.ToString();
        startMenuCoin.text = CommonData.Instance.coin.ToString();

    }

    private IEnumerator EndGameStars(int numOfStars)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < numOfStars; i++)
        {
            stars[i].gameObject.SetActive(true);
            LeanTween.scale(stars[i], 1.3f * Vector2.one, .4f).setEasePunch();
            yield return new WaitForSeconds(.4f);
        }
        //Need to deactivate stars on next level click
    }

    public void NextLevel_OnClick()
    {
        for (int i = 0; i < 3; i++)
        {
            stars[i].gameObject.SetActive(false);
        }
    }

    public void Options_OnClick()
    {
        if (!optionsOpen) LeanTween.moveY(activeSettingsPanel, activeSettingsPanel.anchoredPosition.y - activeSettingsPanel.rect.height, .3f);
        else LeanTween.moveY(activeSettingsPanel, activeSettingsPanel.anchoredPosition.y + activeSettingsPanel.rect.height, .3f);
        optionsOpen = !optionsOpen;
    }

    public void SoundOnOff_OnClick()
    {
        soundOn = !soundOn;
        AudioListener.volume = soundOn ? 1 : 0;
        PlayerPrefs.SetInt(PP_Sound, soundOn ? 1 : 0);
        soundOptionImage.sprite = soundOn ? soundOnSprite : soundOffSprite;
    }

    public void Vibration_OnClick()
    {
        vibrationOn = !vibrationOn;
        Vibrator.isOn = vibrationOn;
        PlayerPrefs.SetInt(PP_Vibration, vibrationOn ? 1 : 0);
        vibrationOptionImage.sprite = vibrationOn ? vibrationOnSprite : vibrationOffSprite;
    }

    public void PauseOpen_OnClick()
    {
        inGame.SetActive(false);
        pause.SetActive(true);
    }
    
    public void PauseClose_OnClick()
    {
        pause.SetActive(false);
        inGame.SetActive(true);
    }

    public void CoinsOpen_OnClick(bool isMainMenu)
    {
        this.isMainMenu = isMainMenu;
        if (isMainMenu) startMenu.SetActive(false);
        else shop.SetActive(false);

        getCoins.SetActive(true);
    }
    
    public void CoinsClose_OnClick()
    {
        getCoins.SetActive(false);
        if (isMainMenu) startMenu.SetActive(true);
        else shop.SetActive(true);
    }

    public void ShopOpen_OnClick()
    {
        startMenu.SetActive(false);
        shop.SetActive(true);
    }
    
    public void ShopClose_OnClick()
    {
        shop.SetActive(false);
        startMenu.SetActive(true);
    }

    public void SetLevelFiller(float ratio)
    {
        levelFiller.fillAmount = ratio;
        levelHandle.anchoredPosition = new Vector2(handleLocalStartX + ratio*(handleLocalEndX-handleLocalStartX), levelHandle.localPosition.y);
    }

    public void MainMenu_OnClick()
    {
        pause.SetActive(false);
        startMenu.SetActive(true);
    }

    public void TESTSCREENSHOT()
    {
        Debug.Log("SCREENSHOTCAPTURED");
        ScreenCapture.CaptureScreenshot("SS"+ numOfScreenShots++ + ".png");
        PlayerPrefs.SetInt("ss", numOfScreenShots);
    }
}
