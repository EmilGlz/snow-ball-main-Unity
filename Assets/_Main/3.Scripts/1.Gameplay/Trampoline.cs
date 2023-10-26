using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public Transform destination;
    [SerializeField] Animation trampolineAnim;
    public void TrampolineAnimate()
    {
        Debug.Log("Spring jump");
        //trampolineAnimator.SetBool("JumpStart", true);
        trampolineAnim.Play("SpringJump");
    }
}