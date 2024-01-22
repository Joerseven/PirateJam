using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashParticle : MonoBehaviour
{
    [SerializeField] public ParticleSystem dust;
    
    public void Update(){
        // if(Input.GetKeyDown(KeyCode.K)){
        //     CreateDust();
        // }
    }
    
    public void CreateDust(int direction){
        Debug.Log("playing dust");
        dust.Play();
    }
}
