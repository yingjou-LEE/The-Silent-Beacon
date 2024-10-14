using System;
using Cinemachine;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float forwardAcceleration = 1;
    [SerializeField] private float rotationAcceleration = 12;
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera boatmanCam;
    [SerializeField] private Vector2 cameraFovRange = new Vector2(60, 90);
    [SerializeField] private float maxFovSpeed = 3;
    [SerializeField] private bool fovChange = false;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InputManager.Instance.OnArrowKeys += Move;
    }

    private void Update()
    {
        if (fovChange)
        {
            boatmanCam.m_Lens.FieldOfView =
                Mathf.Lerp(cameraFovRange.x, cameraFovRange.y, _rb.velocity.magnitude / maxFovSpeed);
        }
    }

    private void Move(Vector2 input)
    {
        var forwardSpeed = input.y * forwardAcceleration * Time.deltaTime;
        var rotationSpeed = input.x * rotationAcceleration * Time.deltaTime;
        
        _rb.AddForce(transform.forward * forwardSpeed, ForceMode.Force);
        _rb.AddTorque(Vector3.up * rotationSpeed, ForceMode.Force);
    }
}
