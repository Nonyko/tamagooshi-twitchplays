using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
/*
This class manages the authentication form.
*/
public class AuthFormController : MonoBehaviour{
  
    public TextMeshProUGUI username, channelName, AuthToken;
    
    //It checks if theres information loaded.
    void Start(){
        
        if( SaveSystem.LoadSave()!=null){
            username.text = SaveSystem.saveData.username;
            channelName.text = SaveSystem.saveData.channelName;
            AuthToken.text = SaveSystem.saveData.password;
        }else{
           EnterConfigInfo();
        }
    }

    //It saves the information with the SaveSystem
    void EnterConfigInfo(){      
       SaveSystem.Save(username.text, AuthToken.text, channelName.text);
    }

    //It saves the info and load the main scene
    public void AuthAndStartApp(){
        EnterConfigInfo();
        SceneManager.LoadScene("TamagooshiScene");
    }

    
}
