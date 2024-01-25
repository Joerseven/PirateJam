using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashEffects : MonoBehaviour
{
    private Vector2 direction;

    private ParticleSystem particleSys;

    private PlayerControls inputActions;

    private Vector2 inputDir;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new PlayerControls();
        inputActions.Player.Enable();
        inputActions.Player.testParticles.performed += particleEmit;

        particleSys = GetComponentInChildren<ParticleSystem>();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        inputDir = inputActions.Player.Move.ReadValue<Vector2>();
        if (inputDir.x >= 0.7)
        {
            rotation.Set(0f, 0f, -190f, 0f);
        }
        else if (inputDir.x <= -0.7)
        {
            rotation.Set(0f, 0f, -10f, 0f);
        }
        else if (inputDir.y >= 0.7)
        {
            rotation.Set(0f, 0f, -100f, 0f);
        }
        else if (inputDir.y <= -0.7)
        {
            rotation.Set(0f, 0f, -280f, 0f);
        }
        else
        {
            rotation.Set(0f, 0f, -100f, 0f);
        }
    }

    private void particleEmit(InputAction.CallbackContext context)
    {
        particleSys.shape.rotation.Set(rotation.x, rotation.y, rotation.z);
        Debug.Log(rotation);
        particleSys.Emit(50);
    }



}
