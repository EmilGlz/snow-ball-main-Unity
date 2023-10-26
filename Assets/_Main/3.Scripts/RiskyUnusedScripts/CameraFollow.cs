using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] Vector3 offSet;


    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 newOffset = offSet;
        //newOffset.y += 2 * player.localScale.y;
        //newOffset.z -= 4 * player.localScale.z;
        transform.position = Vector3.Lerp(transform.position, player.position + offSet, 0.8f);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
