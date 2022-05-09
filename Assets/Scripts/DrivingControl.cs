using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrivingControl : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxBrakeTorque;
    public float maxSteeringAngle;
    public bool isParked = false;

    public float magnitude;

    bool braking;
    
    AudioSource enginSound;
    

    void Start()
    {
        enginSound = GetComponent<AudioSource>();
    }

     
    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
     
    public void FixedUpdate()
    {
        if (isParked)
            return;
        
        float vertical = Input.GetAxis("Vertical");
        float motor = maxMotorTorque * vertical;
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        float brake = braking ? maxBrakeTorque : 0;
        float motorRange = (GetComponent<Rigidbody>().velocity.magnitude - 1f) / (2f - 1f);
        enginSound.pitch = motorRange;
        enginSound.pitch = Mathf.Clamp(enginSound.pitch, .7f, 3f);

        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            
                if (axleInfo.motor) {
                    // --- acceleration
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
                
                if (axleInfo.brake)
                {
                    // --- brake
                    axleInfo.leftWheel.brakeTorque = maxBrakeTorque * brake;
                    axleInfo.rightWheel.brakeTorque = maxBrakeTorque * brake;
            }
            
            if(Input.GetKeyDown(KeyCode.P))
                if(axleInfo.leftWheel.rpm >-1 && 
                axleInfo.leftWheel.rpm < 1 && 
                axleInfo.rightWheel.rpm > -1 &&
                axleInfo.rightWheel.rpm < 1)
                    isParked = true;

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

    }

    void Update()
    {
        braking = Input.GetKey(KeyCode.Space);
    }
}


[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool brake;
    public bool steering;
}
 