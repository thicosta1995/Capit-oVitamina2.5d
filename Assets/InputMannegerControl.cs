using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMannegerControl : MonoBehaviour
{
    // Start is called before the first frame update
    public static InputMannegerControl Instance;
    public Vector2 moveInput;
    public Vector2 AimInput;
    public bool JumpInput;
    public bool FireInput;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }
    private void LateUpdate()
    {
       
        JumpInput = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    public void OnAin(InputValue value)
    {
        AimInput = value.Get<Vector2>();
        
    }
    public void OnFire(InputValue value)
    {
        FireInput = value.isPressed;
    }
    public void OnJump(InputValue value)
    {
        JumpInput = value.isPressed;
    }
    public static Vector2 GetMovementInput()
    {
        return Instance.moveInput;
    }
    public static bool GetJumpInput()
    {
        return Instance.JumpInput;
    }
    public static bool GetFireInput()
    {
        return Instance.FireInput;
    }
    
}
