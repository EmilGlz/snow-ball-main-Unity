using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    private const string PP_PickupsHave = "pick";
    private const string PP_OtherHave = "pick";

    [SerializeField] TMP_Text coin;

    [SerializeField] GameObject pickupsScrollView;
    [SerializeField] GameObject otherScrollView;

    [SerializeField] GameObject[] pickupsPrices;
    [SerializeField] GameObject[] othersPrices;

    private bool[] pickupsHave;
    private bool[] othersHave;

    [SerializeField] Image pickupImage;
    [SerializeField] Image otherOptionImage;
    [SerializeField] Sprite[] pickupSprites;
    [SerializeField] Sprite[] otherOptionSprites;

    private void Start()
    {
        if (!PlayerPrefs.HasKey(PP_PickupsHave)) //If it is the first time
        {
            pickupsHave = new bool[pickupsPrices.Length];
            othersHave = new bool[othersPrices.Length];

            PlayerPrefs.SetString(PP_PickupsHave, BoolArrayToString(pickupsHave));
            PlayerPrefs.SetString(PP_OtherHave, BoolArrayToString(othersHave));
        }
        else
        {
            pickupsHave = StringToArray(PlayerPrefs.GetString(PP_PickupsHave));
            othersHave = StringToArray(PlayerPrefs.GetString(PP_OtherHave));
            //Debug.Log("othersHave: " + othersHave);
            //Debug.Log("othersHave.L: " + othersHave.Length);
        }

        for (int i = 0; i < pickupsHave.Length; i++)
        {
            pickupsPrices[i].SetActive(!pickupsHave[i]);
        }
        for (int i = 0; i < othersHave.Length; i++)
        {
            othersPrices[i].SetActive(!othersHave[i]);
        }
    }

    private string BoolArrayToString(bool[] array)
    {
        return string.Join("|", Array.ConvertAll(array, value => value ? "1" : "0"));
    }

    private bool[] StringToArray(string str)
    {
        return Array.ConvertAll(str.Split('|'), value => value == "1");
    }

    public void OtherOption_OnClick()
    {
        pickupImage.sprite = pickupSprites[0];
        otherOptionImage.sprite = otherOptionSprites[1];
        pickupsScrollView.SetActive(false);
        otherScrollView.SetActive(true);
    }
    
    public void Pickups_OnClick()
    {
        otherOptionImage.sprite = otherOptionSprites[0];
        pickupImage.sprite = pickupSprites[1];
        otherScrollView.SetActive(false);
        pickupsScrollView.SetActive(true);
    }

}