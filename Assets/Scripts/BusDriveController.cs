using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusDriveController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;

    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _instantStopTime;

    private Coroutine instantStopCoroutine;

    public bool CanMove = true;

    private void FixedUpdate()
    {
        if (!CanMove)
        {
            if (instantStopCoroutine == null)
            {
                frontLeftWheelCollider.motorTorque = 0;
                frontRightWheelCollider.motorTorque = 0;
                instantStopCoroutine = StartCoroutine(InstantStop());
            }
            return;
        }

        if (instantStopCoroutine != null)
        {
            StopCoroutine(instantStopCoroutine);
            instantStopCoroutine = null;
        }

        //If you wanna test it on Editor delete Comment on GetInputFunction
        //GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    #region ButtonInputs
    //This functions using for play with buttons
    public void SetHorizontalInput(int value)
    {
        horizontalInput = value;
    }

    public void SetVerticalInput(int value)
    {
        verticalInput = value;
    }

    public void SetIsBreaking(bool state)
    {
        isBreaking = state;
    }
    //////////////////////////////////////

    #endregion

    //Applying motor torque to wheel to move
    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    //Applying break torque to wheel to stop
    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    //Steering wheels to turn right and left
    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }


    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    //Just for animation
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    //When arrive to a bus station it needs to be stopped to Get and Drop passengers
    private IEnumerator InstantStop()
    {
        var currentSpeed = _rb.velocity;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * _instantStopTime;
            _rb.velocity = Vector3.Lerp(currentSpeed, Vector3.zero, t);
            yield return null;
        }

        _rb.velocity = Vector3.zero;
    }
}

