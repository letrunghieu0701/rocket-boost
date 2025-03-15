using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    private Rigidbody _rigid;
    [SerializeField] private float _thrustForce = 1200f;
    [SerializeField] private float _rotationSpeed = 300f;

    private AudioSource _audioSource;
    private ParticleSystem _mainEngineEffect;
    private ParticleSystem _leftThrusterEffect;
    private ParticleSystem _rightThrusterEffect;

    private void Start()
    {
        _rigid = this.transform.GetComponent<Rigidbody>();

        _audioSource = this.transform.Find("Sound").GetComponent<AudioSource>();

        _mainEngineEffect = this.transform.Find("Effects/EngineThruster").GetComponent<ParticleSystem>();
        _leftThrusterEffect = this.transform.Find("Effects/LeftSideThruster").GetComponent<ParticleSystem>();
        _rightThrusterEffect = this.transform.Find("Effects/RightSideThruster").GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        ProcessThrusting();
        ProcessRotation();
    }

    private void ProcessThrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void ProcessRotation()
    {
        float rotation = Input.GetAxis("Horizontal");
        if (rotation < 0) // Rotate to left
        {
            RotateLeft();
        }
        else if (rotation > 0) // Rotate to right
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    private void StartThrusting()
    {
        _rigid.AddRelativeForce(Vector3.up * _thrustForce * Time.deltaTime);
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
        if (!_mainEngineEffect.isPlaying)
        {
            _mainEngineEffect.Play();
        }
    }

    public void StopThrusting()
    {
        _audioSource.Stop();
        _mainEngineEffect.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(_rotationSpeed);
        if (!_rightThrusterEffect.isPlaying)
        {
            _rightThrusterEffect.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-_rotationSpeed);
        if (!_leftThrusterEffect.isPlaying)
        {
            _leftThrusterEffect.Play();
        }
    }

    private void ApplyRotation(float rotationForce)
    {
        // Rotate transform
        _rigid.freezeRotation = true; // Make physics system stop rotating the object so that we can manually rotate the object like how we want
        this.transform.Rotate(Vector3.forward * rotationForce * Time.deltaTime);
        _rigid.freezeRotation = false; // Give back control to the physics system
    }

    public void StopRotating()
    {
        _leftThrusterEffect.Stop();
        _rightThrusterEffect.Stop();
    }
}
