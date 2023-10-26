using UnityEngine;

public class Vibrate : MonoBehaviour
{

    public void VibrateTest1()
    {
        Vibrator.Vibrate();
    }

    public void VibrateTest2()
    {
        Vibrator.Vibrate(100);
    }
}
