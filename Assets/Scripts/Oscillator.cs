using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Oscillator : MonoBehaviour
{
    private int _ID;
    private Vector3 _startingPoint;
    private Vector3 _travelVector;
    [SerializeField] private Vector3 endingPoint;
    private float offsetMoving;
    [SerializeField] private float period = 2f;

    private const float TAU = 2 * Mathf.PI; // name of the value equal to 2*PI

    void Start()
    {
        string[] nameTokens = this.transform.name.Split('_');
        if (nameTokens.Length != 2)
        {
            Debug.LogError($"Oscillator: Wrong naming format, must be ObjectName_<ID>");
            return;
        }
        _ID = int.Parse(this.transform.name.Split('_')[1]);
        string activeSceneName = SceneManager.GetActiveScene().name;
        MovingObstacleData configData = GameManager.Instance.GetConfigData<string, MovingObstacleData>($"{activeSceneName}_{_ID}");
        if (configData == null)
        {
            Debug.LogError($"Oscillator: Can find {typeof(MovingObstacleData).Name} by ID: {_ID}");
            return;
        }

        _startingPoint = configData.StartPoint;
        this.transform.localPosition = _startingPoint;
        endingPoint = configData.EndPoint;
        period = configData.Period;
        _travelVector = endingPoint - _startingPoint;
    }

    void Update()
    {
        if (period <= Mathf.Epsilon) // Check if period <= a very tiny floating number, to avoid Time.time / 0
        {
            return;
        }

        float cycles = Time.time / period; // calculate how many cycles has been, used for the sin wave
        float sinWaveValue = Mathf.Sin(cycles * TAU);
        offsetMoving = (sinWaveValue + 1) / 2; // recalculate to make the sin wave's value in the range [0, 1]
        this.transform.position = _startingPoint + offsetMoving * _travelVector;
    }
}
