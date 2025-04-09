using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Oscillator : MonoBehaviour
{
    private int _ID;
    private MovingObstacleData _configData;

    [SerializeField] private Vector3 _startingPoint;
    [SerializeField] private Vector3 _endingPoint;
    [SerializeField] private float _SinPeriod;

    void Start()
    {
        string[] nameTokens = this.transform.name.Split('_');
        if (nameTokens.Length != 2)
        {
            Debug.LogError($"Oscillator: Wrong naming format, must be ObjectName_<ID>");
            return;
        }

        _ID = int.Parse(nameTokens[1]);
        string activeSceneName = SceneManager.GetActiveScene().name;
        _configData = GameManager.Instance.GetConfigData<string, MovingObstacleData>($"{activeSceneName}_{_ID}");
        if (_configData == null)
        {
            Debug.LogError($"Oscillator: Can find {typeof(MovingObstacleData).Name} by ID: {_ID}");
            return;
        }

        _startingPoint = _configData.StartPoint;
        _endingPoint = _configData.EndPoint;
        this.transform.localPosition = _startingPoint;
        _SinPeriod = _configData.Period;
    }

    void Update()
    {
        if (_configData == null)
        {
            Debug.LogError($"Oscillator: Can find {typeof(MovingObstacleData).Name} by ID: {_ID}");
            return;
        }

        if (_SinPeriod <= Mathf.Epsilon) // Check if period <= a very tiny floating number, to avoid Time.time / 0
        {
            return;
        }

        float cycles = Time.time / _SinPeriod; // calculate how many cycles has been, used for the sin wave
        float sinWaveValue = Mathf.Sin(cycles * MathUtils.TAU);
        float _offsetMoving = (sinWaveValue + 1) / 2; // recalculate to make the sin wave's value in the range [0, 1]
        this.transform.position = _startingPoint + _offsetMoving * (_endingPoint - _startingPoint);
    }
}
