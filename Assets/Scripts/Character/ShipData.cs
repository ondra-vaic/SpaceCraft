using UnityEngine;
[CreateAssetMenu(fileName = "Spaceship Data", menuName = "ScriptableObjects/Spaceship Data", order = 1)]
public class ShipData : ScriptableObject
{

    [Header("Movement")]
    public float thrustAmount;
    [HideInInspector]public float thrustInput;

    public float yawSpeed;
    public float pitchSpeed;
    [HideInInspector] public Vector3 steeringInput;

    public float leanAmount_X;
    public float leanAmount_Y;

    public float resettingAmount;
    
    [HideInInspector] public float resettingInput;

    
    public void UpdateInputData(Vector3 newSteering, float newThrust, float resettingInput)
    {
        steeringInput = newSteering;
        thrustInput = newThrust;
        this.resettingInput = resettingInput;
    }

}