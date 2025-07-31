using System.Collections;
using UnityEngine;

public class TonearmController : MonoBehaviour
{
    [Header("Tonearm Setup")]
    public Transform tonearm;

    [Header("Wave Angles (degrees)")]
    public float[] waveAngles = { 0f, -90f, -180f, -270f };

    [Header("Rotation Settings")]
    public float rotationDuration = 1f;

    private int currentWave = 0;
    private Coroutine rotationCoroutine;

    public void NextWave()
    {
        if (currentWave >= waveAngles.Length)
        {
            Debug.Log("No more wave");
            return;
        }

        float targetAngle = waveAngles[currentWave];

        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        rotationCoroutine = StartCoroutine(RotateTonearm(targetAngle));

        currentWave++;
    }

    private IEnumerator RotateTonearm(float targetZAngle)
    {
        float startAngle = tonearm.localEulerAngles.z;
        float time = 0f;

        while (time < rotationDuration)
        {
            float angle = Mathf.LerpAngle(startAngle, targetZAngle, time / rotationDuration);
            tonearm.rotation = Quaternion.Euler(0f, 0f, angle);
            time += Time.deltaTime;
            yield return null;
        }

        tonearm.rotation = Quaternion.Euler(0f, 0f, targetZAngle);
    }

}
