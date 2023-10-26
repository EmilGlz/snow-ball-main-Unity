using UnityEngine;

public class RandomPatterns : MonoBehaviour
{
    [SerializeField] GameObject Parent;
    void Start()
    {
        int randomChildIdx = Random.Range(0, Parent.transform.childCount);
        Parent.transform.GetChild(randomChildIdx).gameObject.SetActive(true);
    }


}
