using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevel : MonoBehaviour
{
    private const char SPRTR = '|';
    private const string PP_Level = "lvlarray";
    private const string PP_LevelCount = "lvlNum";
    private const float BEGINPIECETIME = 2.25f;

    [SerializeField] private GameObject beginningPiece;

    //Obstacles with Given Conditions
    private const int LEFTTURNID = 199;
    private const int RIGHTTURNID = 200;
    private const int RAILSID = 201;
    private const int numberOfObstaclesWithConditions = 2;
    [SerializeField] private GameObject leftTurn;
    [SerializeField] private GameObject rightTurn;
    [SerializeField] private GameObject rails;
    //

    [SerializeField] private GameObject[] otherObstacles;

    [SerializeField] private GameObject finishAndBonusPiece;

    [SerializeField] private int NumberOfObstacles = 14;

    private int[] currentLevel;

    private System.Random random;

    [SerializeField] List<GameObject> mapObjects;
    [SerializeField] Material[] skyboxes;
    private void Start()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        random = new System.Random();
        if (PlayerPrefs.HasKey(PP_Level))
        {
            GenerateFromSavedArray(ParseString(PlayerPrefs.GetString(PP_Level)));
            CommonData.Instance.currentLevel = PlayerPrefs.GetInt(PP_LevelCount);
        }
        else //First Time
        {
            GenerateRandomLevel(NumberOfObstacles);
            CommonData.Instance.currentLevel = 1;
            PlayerPrefs.SetInt(PP_LevelCount, 1);
        }
        RenderSettings.skybox = skyboxes[(CommonData.Instance.currentLevel + 1) % skyboxes.Length];
    }

    public void GenerateNextLevel(int obstacleCount)
    {
        
        random = new System.Random();
        GenerateRandomLevel(obstacleCount);
        RenderSettings.skybox = skyboxes[(CommonData.Instance.currentLevel + 1) % skyboxes.Length];
    }

    private void GenerateRandomLevel(int _obstacleCount)
    {
        DestroyAllMapObjects();
        mapObjects = new List<GameObject>();
        int obstacleCount = otherObstacles.Length + numberOfObstaclesWithConditions;

        int[] countingArray = new int[obstacleCount];

        int[] shuffleArray = new int[obstacleCount];
        for (int j = 0; j < obstacleCount; j++) shuffleArray[j] = j;

        currentLevel = new int[_obstacleCount];

        GameObject lastLevelPiece = Instantiate(beginningPiece, Vector3.zero, Quaternion.identity);
        CommonData.Instance.currentLevelTime = BEGINPIECETIME;
        mapObjects.Add(lastLevelPiece);
        int endCheck = obstacleCount - 1;
        int rand;

        for (int i = 0; i < _obstacleCount; i++)
        {
            rand = random.Next(endCheck);

            if (i == 5 && countingArray[obstacleCount - 1] == 0)
            {
                int leftOrRight = random.Next(2);
                lastLevelPiece = Instantiate(leftOrRight == 0 ? rightTurn : leftTurn, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
                CommonData.Instance.currentLevelTime += lastLevelPiece.GetComponent<LevelPieceInfo>().spendTime;
                currentLevel[i] = (leftOrRight == 0) ? RIGHTTURNID : LEFTTURNID;
                
                countingArray[obstacleCount - 1]++;

                int indexInShuffleArray = Array.IndexOf(shuffleArray, obstacleCount - 1);

                int temp = shuffleArray[indexInShuffleArray];
                shuffleArray[indexInShuffleArray] = shuffleArray[endCheck];
                shuffleArray[endCheck] = temp;
                mapObjects.Add(lastLevelPiece);
                continue;
            }
            else if (i == 7 && countingArray[obstacleCount - 2] == 0)
            {
                lastLevelPiece = Instantiate(rails, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
                CommonData.Instance.currentLevelTime += lastLevelPiece.GetComponent<LevelPieceInfo>().spendTime;
                currentLevel[i] = RAILSID;

                countingArray[obstacleCount - 2]++;

                int indexInShuffleArray = Array.IndexOf(shuffleArray, obstacleCount - 2);

                int temp = shuffleArray[indexInShuffleArray];
                shuffleArray[indexInShuffleArray] = shuffleArray[endCheck];
                shuffleArray[endCheck] = temp;
                mapObjects.Add(lastLevelPiece);
                continue;
            }

            if (shuffleArray[rand] == obstacleCount - 1 || shuffleArray[rand] == obstacleCount - 2)
            {
                if (countingArray[shuffleArray[rand]] == 2) // Limit reached
                {
                    for (int j = rand; j < endCheck; j++)
                    {
                        shuffleArray[j] = shuffleArray[j + 1];
                    }
                    endCheck--;
                    i--;
                }
                else
                {
                    if (shuffleArray[rand] == obstacleCount - 1)
                    {
                        int leftOrRight = random.Next(2);
                        lastLevelPiece = Instantiate(leftOrRight == 0 ? rightTurn : leftTurn, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
                        CommonData.Instance.currentLevelTime += lastLevelPiece.GetComponent<LevelPieceInfo>().spendTime;
                        currentLevel[i] = (leftOrRight == 0) ? RIGHTTURNID : LEFTTURNID;
                    }
                    else
                    {
                        lastLevelPiece = Instantiate(rails, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
                        CommonData.Instance.currentLevelTime += lastLevelPiece.GetComponent<LevelPieceInfo>().spendTime;
                        currentLevel[i] = RAILSID;
                    }
                    mapObjects.Add(lastLevelPiece);

                    countingArray[shuffleArray[rand]]++;

                    int temp = shuffleArray[rand];
                    shuffleArray[rand] = shuffleArray[endCheck];
                    shuffleArray[endCheck] = temp;
                }
            }
            else
            {
                if (countingArray[shuffleArray[rand]] == 3) // Limit reached
                {
                    for (int j = rand; j < endCheck; j++)
                    {
                        shuffleArray[j] = shuffleArray[j + 1];
                    }
                    endCheck--;
                    i--;
                }
                else
                {
                    lastLevelPiece = Instantiate(otherObstacles[shuffleArray[rand]], lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
                    CommonData.Instance.currentLevelTime += lastLevelPiece.GetComponent<LevelPieceInfo>().spendTime;
                    currentLevel[i] = shuffleArray[rand];
                    mapObjects.Add(lastLevelPiece);
                    countingArray[shuffleArray[rand]]++;

                    int temp = shuffleArray[rand];
                    shuffleArray[rand] = shuffleArray[endCheck];
                    shuffleArray[endCheck] = temp;
                }
            }
        }
        PlayerPrefs.SetString(PP_Level, FormatArray(currentLevel));
        PlayerPrefs.SetInt(PP_LevelCount, ++CommonData.Instance.currentLevel);
        GameObject finishLine = Instantiate(finishAndBonusPiece, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
        finishLine.GetComponent<BonusLevelScript>().GeneratePeople();
        Debug.Log("Random People Located");
        mapObjects.Add(finishLine);
    }

    void DestroyAllMapObjects()
    {
        for (int i = 0; i < mapObjects.Count; i++)
        {
            Destroy(mapObjects[i]);
        }
    
    }

    private void GenerateFromSavedArray(int[] array)
    {
        DestroyAllMapObjects();
        mapObjects = new List<GameObject>();
        GameObject lastLevelPiece = Instantiate(beginningPiece, Vector3.zero, Quaternion.identity);
        CommonData.Instance.currentLevelTime = BEGINPIECETIME;
        mapObjects.Add(lastLevelPiece);

        for (int i = 0; i < array.Length; i++)
        {
            if(array[i] == LEFTTURNID)
            {
                lastLevelPiece = Instantiate(leftTurn, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
                CommonData.Instance.currentLevelTime += lastLevelPiece.GetComponent<LevelPieceInfo>().spendTime;
            }
            else if(array[i] == RIGHTTURNID)
            {
                lastLevelPiece = Instantiate(rightTurn, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
                CommonData.Instance.currentLevelTime += lastLevelPiece.GetComponent<LevelPieceInfo>().spendTime;
            }
            else if(array[i] == RAILSID)
            {
                lastLevelPiece = Instantiate(rails, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
                CommonData.Instance.currentLevelTime += lastLevelPiece.GetComponent<LevelPieceInfo>().spendTime;
            }
            else
            {
                lastLevelPiece = Instantiate(otherObstacles[array[i]], lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
                CommonData.Instance.currentLevelTime += lastLevelPiece.GetComponent<LevelPieceInfo>().spendTime;
            }
            mapObjects.Add(lastLevelPiece);
        }
        GameObject finishLine = Instantiate(finishAndBonusPiece, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.position, lastLevelPiece.GetComponent<LevelPieceInfo>().endPoint.rotation);
        finishLine.GetComponent<BonusLevelScript>().GeneratePeople();
        mapObjects.Add(finishLine);
    }

    private string FormatArray(int[] array)
    {
        return string.Join(SPRTR.ToString(), array);
    }

    private int[] ParseString(string formatted)
    {
        return Array.ConvertAll(formatted.Split(SPRTR), int.Parse);
    }
}