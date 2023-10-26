using UnityEngine;

public class GrowMine : MonoBehaviour
{
    [SerializeField] Transform snowball;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grow"))
        {
            if (snowball.localScale.x < 7f)
            {
                LeanTween.scale(snowball.gameObject, snowball.localScale + Vector3.one / 2, 0.2f).setEaseInSine();
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Grow1"))
        {
            if (snowball.localScale.x > 1.5f)
            {
                LeanTween.scale(snowball.gameObject, snowball.localScale - Vector3.one / 2, 0.2f).setEaseInSine();
            }
            Destroy(other.gameObject);
        }


    }
}
