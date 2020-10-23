using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
Controller for the Hungry Slider
*/
public class HungryBarController : MonoBehaviour
{
    
    public Slider slider;
    public Image fill;
    public TamagooshiController Tamagooshi;

    public Color goodState;
    public Color badState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Tamagooshi.hunger;
        if(Tamagooshi.hunger<=20f){
            fill.color = badState;
        }else{
              fill.color = goodState;
        }
    }
}
