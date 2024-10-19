using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{

    public static Vibration instance;

    private void Awake()
    {
        instance = this;
    }

    public void StartVibration(float frequency, float amplitude, float duration, OVRInput.Controller controller)
    {
        StartCoroutine(Vibrate(frequency, amplitude, duration, controller));
    }

    private IEnumerator Vibrate(float frequency, float amplitude, float duration, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, controller); // バイブレーションを停止
    }
}
