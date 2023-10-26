using UnityEngine;

public class CommonData : MonoBehaviour
{
    #region Singleton
    private static CommonData _instance;
    public static CommonData Instance { get { return _instance; } }
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    [HideInInspector] public int coin;

    [HideInInspector] public GameObject[] peopleAtEnd;

    ///SHOP VARIABLES
    [HideInInspector] public GameObject currentPickup;

    // Coins gained from game when you lose/win
    [HideInInspector] public int collectedCoins;

    //InGameInfos
    [HideInInspector] public bool pause;
    //

    //LevelInfo
    [HideInInspector] public int currentLevel;
    [HideInInspector] public float currentLevelTime;
    //
}
