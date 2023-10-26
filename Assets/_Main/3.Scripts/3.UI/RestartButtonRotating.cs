using UnityEngine;
using UnityEngine.UI;

public class RestartButtonRotating : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    
    private Image image;

    private RectTransform rectTransform;

    private int i = 1;

    private int rotation = 0;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        rectTransform.Rotate(Vector3.forward, -4f);
        rotation += 4;
        if(rotation >= 180)
        {
            rectTransform.Rotate(Vector3.forward, -90f);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y, rectTransform.sizeDelta.x);
            rotation = 0;
            image.sprite = sprites[i];
            i = i == 0 ? 1 : 0;
        }
    }
}
