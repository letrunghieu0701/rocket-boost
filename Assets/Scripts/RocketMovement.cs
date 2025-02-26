using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField] private float thrustForce = 1200f;
    [SerializeField] private float rotationSpeed = 300f;

    private AudioSource audioSource;
    private ParticleSystem mainEngineParticle;
    private ParticleSystem leftThrusterParticle;
    private ParticleSystem rightThrusterParticle;

    private void Start()
    {
        rigid = this.transform.GetComponent<Rigidbody>();

        audioSource = this.transform.Find("Sound").GetComponent<AudioSource>();

        mainEngineParticle = this.transform.Find("Effects/EngineThruster").GetComponent<ParticleSystem>();
        leftThrusterParticle = this.transform.Find("Effects/LeftSideThruster").GetComponent<ParticleSystem>();
        rightThrusterParticle = this.transform.Find("Effects/RightSideThruster").GetComponent<ParticleSystem>();
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
        if (Input.GetKey(KeyCode.A)) // Rotate to left
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D)) // Rotate to right
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
        rigid.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        if (!mainEngineParticle.isPlaying)
        {
            mainEngineParticle.Play();
        }
    }

    public void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticle.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationSpeed);
        if (!rightThrusterParticle.isPlaying)
        {
            rightThrusterParticle.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationSpeed);
        if (!leftThrusterParticle.isPlaying)
        {
            leftThrusterParticle.Play();
        }
    }

    private void ApplyRotation(float rotationForce)
    {
        // Rotate transform
        rigid.freezeRotation = true; // Make physics system stop rotating the object so that we can manually rotate the object like how we want
        this.transform.Rotate(Vector3.forward * rotationForce * Time.deltaTime);
        rigid.freezeRotation = false; // Give back control to the physics system
    }

    public void StopRotating()
    {
        leftThrusterParticle.Stop();
        rightThrusterParticle.Stop();
    }
}
