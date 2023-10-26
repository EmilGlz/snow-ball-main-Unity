using UnityEngine;

public class IceCrack : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] int levelToBeCracked;
    AudioSource crackSound;
    [SerializeField] GameObject normalIcePiece;
    [SerializeField] GameObject crackedIcePiece;

    private void Start()
    {
        crackSound = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // collision with player
        {
            Grow playerGrow = other.GetComponent<Grow>();
            if (playerGrow.growLevel >= levelToBeCracked)
            {
                // crack
                normalIcePiece.SetActive(false);
                crackedIcePiece.SetActive(true);
                crackSound.Play();
            }
        }
    }
}
