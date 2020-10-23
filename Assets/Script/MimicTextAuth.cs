using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
Controller for the label used to hide the real password.
*/
public class MimicTextAuth : MonoBehaviour
{
     public TextMeshProUGUI textToMimic;

     public TextMeshProUGUI textShown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int countLenght = textToMimic.text.Length;
        if(textShown.text.Length!=countLenght){
            textShown.text="";
            for(int i=0; i<countLenght;i++){
                textShown.text+="*";
            }
        }
    }
}
