using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    private Vector3 startingPoint;
    [SerializeField] Vector3 endingPoint;
    float offsetMoving;
    [SerializeField] float period = 2f;

    const float TAU = 2 * Mathf.PI; // name of the value equal to 2*PI

    void Start()
    {
        startingPoint = this.transform.position;
    }

    void Update()
    {
        if (period <= Mathf.Epsilon) // Check if period <= a very tiny floating number, to avoid Time.time / 0
        {
            return;
        }

        float cycles = Time.time / period; // calculate how many cycles has been, used for the sin wave
        float sinWaveValue = Mathf.Sin(cycles * TAU);
        offsetMoving = (sinWaveValue + 1) / 2; // recalculate to make the sin wave's value in [0, 1]
        this.transform.position = startingPoint + offsetMoving * endingPoint;
    }
}
