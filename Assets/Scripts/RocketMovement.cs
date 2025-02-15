using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField] private float thrustForce = 5f;
    [SerializeField] private float rotationSpeed = 5f;


    private void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody>();
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
            rigid.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
        }
    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationSpeed);
        }
    }

    private void ApplyRotation(float rotationForce)
    {
        this.transform.Rotate(Vector3.forward * rotationForce * Time.deltaTime);
    }
}
