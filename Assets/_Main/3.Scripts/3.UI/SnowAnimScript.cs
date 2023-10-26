using UnityEngine;

public class SnowAnimScript : MonoBehaviour
{
    [SerializeField] int startRandomWidth;
    [SerializeField] float animHeight;
    [SerializeField] float endHeight;

    [SerializeField] RectTransform[] rects;

    RectTransform rectTransform;

    private System.Random random;
    private void Start()
    {
        random = new System.Random();
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        foreach(RectTransform rect in rects)
        {
            if (rect.position.y >= rectTransform.position.y + endHeight) rect.position = new Vector2(rect.position.x, rect.position.y - 4f);
            else
            {
                rect.Rotate(Vector3.forward, random.Next(360));
                rect.position = new Vector2(rectTransform.position.x + random.Next(startRandomWidth) - startRandomWidth / 2, rect.position.y + animHeight);
            }
        }
    }
}
