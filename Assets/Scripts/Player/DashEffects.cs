using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class DashEffects : MonoBehaviour
{
    private Vector2 direction;

    private ParticleSystem particleSys;

    private PlayerControls inputActions;

    private Vector2 inputDir;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new PlayerControls();
        inputActions.Player.Enable();
        //inputActions.Player.testParticles.performed += particleEmit;

        pos = new Vector3(0, 0, 0);
        particleSys = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        inputDir = inputActions.Player.Move.ReadValue<Vector2>();
        if (inputDir.x >= 0.7)
        {
            inputActions.Player.testParticles.performed += particleEmit;
            pos = Vector3.left;
        }
        else if (inputDir.x <= -0.7)
        {
            inputActions.Player.testParticles.performed += particleEmit;
            
        }
        else if (inputDir.y >= 0.7)
        {
            inputActions.Player.testParticles.performed += particleEmit;
            
        }
        else if (inputDir.y <= -0.7)
        {
            inputActions.Player.testParticles.performed += particleEmit;
            
        }
        else
        {
            //Debug.Log("No Rotation");
        }
    }

    private void particleEmit(InputAction.CallbackContext context)
    {
        //if (context.performed == false)
        {
            particleSys.transform.position = pos;
            Debug.Log(particleSys.transform.eulerAngles);
            particleSys.Emit(20);
        }
    }



}
