using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public ShipData data;
    public Rigidbody spaceshipRigidbody;
    public Transform shipModel;

    private void Update()
    {
        ResetShip();
    }

    void FixedUpdate()
    {
        MoveSpaceship();
        TurnSpaceship();
    }

    void ResetShip()
    {
        if (data.resettingInput <= 0.1)
        {
            spaceshipRigidbody.rotation =
                Quaternion.Slerp(spaceshipRigidbody.rotation, Quaternion.Euler(new Vector3(0, transform.localEulerAngles.y, 0)), 0.3f * Time.deltaTime * data.resettingAmount);
            return;
        }
   
        
        spaceshipRigidbody.rotation =
            Quaternion.Slerp(spaceshipRigidbody.rotation, Quaternion.Euler(new Vector3(0, transform.localEulerAngles.y, 0)), data.resettingInput * Time.deltaTime * data.resettingAmount);

       // data.UpdateInputData(Vector3.zero, 0, data.resettingInput);
    }

    void MoveSpaceship()
    {
        spaceshipRigidbody.velocity = transform.forward * data.thrustAmount * (Mathf.Max(data.thrustInput,-.5f));
    }

    void TurnSpaceship()
    {

        Vector3 newTorque = new Vector3(data.steeringInput.x * data.pitchSpeed, -data.steeringInput.z * data.yawSpeed, 0);
        spaceshipRigidbody.AddRelativeTorque(newTorque);

        VisualSpaceshipTurn();
    }

    void VisualSpaceshipTurn()
    {
        shipModel.localEulerAngles = new Vector3(data.steeringInput.x * data.leanAmount_Y
            , shipModel.localEulerAngles.y, data.steeringInput.z * data.leanAmount_X);
    }
}
