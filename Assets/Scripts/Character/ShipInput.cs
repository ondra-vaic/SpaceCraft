using UnityEngine;
using UnityEngine.InputSystem;

public class ShipInput : MonoBehaviour
{
    public InputAction wsad;
    public InputAction thrustInput;
    public InputAction resetingInput;
    
    [Header("Input Smoothing")]
    //Steering
    public float steeringSmoothing;
    [SerializeField] private Vector3 rawInputSteering;
    [SerializeField] private Vector3 smoothInputSteering;

    //Thrust
    public float thrustSmoothing;
    private float rawInputThrust;
    private float smoothInputThrust;

    //Shooting
    [SerializeField] private float rawResetingInput;

    [Header("Data Output")]
    public ShipData spaceshipData;

    private void OnEnable()
    {
        wsad.Enable();
        thrustInput.Enable();
        resetingInput.Enable();
    }

    private void OnDisable()
    {
        wsad.Disable();
        thrustInput.Disable();
        resetingInput.Disable();
    }

    public void steering()
    {
        Vector2 rawInput = wsad.ReadValue<Vector2>();
        rawInputSteering = new Vector3(rawInput.y, 0, -rawInput.x);
    }

    public void thrust()
    {
        rawInputThrust = thrustInput.ReadValue<float>();
    }

    public void reseting()
    {
        rawResetingInput = resetingInput.ReadValue<float>();
    }
    
    void Update()
    {
        thrust();   
        steering();
        reseting();
        InputSmoothing();
        SetInputData();
    }

    void InputSmoothing()
    {
        //Steering
        smoothInputSteering = Vector3.Lerp(smoothInputSteering, rawInputSteering, Time.deltaTime * steeringSmoothing);

        //Thrust
        smoothInputThrust = Mathf.Lerp(smoothInputThrust, rawInputThrust, Time.deltaTime * thrustSmoothing);
        
        if(smoothInputSteering.magnitude < 0.02)
            smoothInputSteering = Vector3.zero;

        if (Mathf.Abs(smoothInputThrust) < 0.01)
            smoothInputThrust = 0;
    }

    void SetInputData()
    {
        spaceshipData.UpdateInputData(smoothInputSteering, smoothInputThrust, rawResetingInput);
    }

}