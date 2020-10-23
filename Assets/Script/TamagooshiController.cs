using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/*
Core class for gameplay. It has all the "game bussiness logic".
*/
public class TamagooshiController : MonoBehaviour
{

public float hunger = 100f; 
public float happiness = 100f;
Animator animator;
void Start(){
    animator = this.GetComponent<Animator>();
}
void  Update() {
    
}
//It executes the feed action, which increases the hunger and triggers the eat animation
public void feed(){
    if(hunger+20f > 100f){
        hunger = 100f;
    }else{
        hunger += 20f;
    }
    
    animator.SetTrigger("EAT");
}
//It executes the pet action, which increases the happiness and triggers the pet animation
public void pet(){
    if(hunger>20f){
       
        if(happiness+20f>100f){
             happiness = 100f;
        }else{
            happiness += 20f;
        }
    }else{
       
         if(happiness+10f>100f){
             happiness = 100f;
        }else{
            happiness += 10f;
        }
    }
     animator.SetTrigger("PET");
}
//It resets the animation to the default state
public void BackToIdle(){
    animator.SetTrigger("IDLE");
}

//Hunger and Happiness will decrease with time.
void FixedUpdate() {

    if(hunger>0f ){
        hunger -= 0.03f*Time.fixedDeltaTime;
    }else if(hunger<0){
        hunger = 0;
    }

    if(happiness>0f){
        if(hunger>20f){
              happiness -= 0.06f*Time.fixedDeltaTime;
        }else{
              happiness -= 0.1f*Time.fixedDeltaTime;
        }
    }else if(happiness<0){
        happiness = 0;
    }

}

}