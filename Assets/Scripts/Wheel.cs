using UnityEngine;

public class Wheel : MonoBehaviour
{
    WheelCollider wc;
    MeshRenderer ms;


    void Start()
    {
        wc = GetComponent<WheelCollider>();
        ms = GetComponent<MeshRenderer>();
    }


    void Update()
    {

    }

    void ApplyLocalPositionToVisual(WheelCollider wheelCollider)
    {
        if (wheelCollider.transform.childCount == 0)
        {
            return;
        }
        
        Transform visualWheel = wheelCollider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
        
    }
}
