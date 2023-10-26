using UnityEngine;

public class BonusLevelScript : MonoBehaviour
{
    [SerializeField] GameObject[] humanPrefab;
    [SerializeField] Transform beginPoint;
    [SerializeField] float distanceBetween;
    [SerializeField] float roadWidth;
    [SerializeField] int peopleCount;

    public void GeneratePeople()
    {
        System.Random random = new System.Random();
        CommonData.Instance.peopleAtEnd = new GameObject[peopleCount];
        for (int i = 0; i < peopleCount; i++)
        {
            CommonData.Instance.peopleAtEnd[i] = Instantiate(humanPrefab[random.Next(3)], beginPoint.position + distanceBetween * i * beginPoint.forward + beginPoint.right * Random.Range(-1*roadWidth,roadWidth), beginPoint.rotation);
        }
    }
}
