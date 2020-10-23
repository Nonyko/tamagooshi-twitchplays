using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
/*
Core class, it manages permissions, commands and call method executions.
*/
public class GameManager : MonoBehaviour{
   
    TwitchChat twitchChat;   
    string[] commands = {"!feed", "!pet"};
    List<string> permissionList;    
    public TamagooshiController Tamagooshi;

    private Queue<MessageModel> commandsToExecute = new Queue<MessageModel>();
    private MessageModel atualCommandToExecute;

    // Start is called before the first frame update
    void Start(){
        ConfigInfo config = ConfigInfo.GetInstance();
       
      
        permissionList = config.permissionList;

        twitchChat = GameObject.Find("TwitchChat").GetComponent<TwitchChat>();
        
    }

    // Update is called once per frame
    void Update(){         
       
        //Check if theres messagens in queue to analyze
        
        if(twitchChat.messages.Count>0){
           
           MessageModel messageModel =  twitchChat.messages.Dequeue();
         

            //Verifies if username is in permission list, otherwise discard it.         
            if( permissionList.Exists(element => (new string(element.Where(c => char.IsLetter(c) || char.IsDigit(c) || c=='_').ToArray())).Equals(messageModel.chatName))){
                //Check if it is vallid command, otherwise discard it.
            
                if(  IsCommandValid(messageModel.message) ){                  
                 PutCommandsInQueue(messageModel); 
                }
            }
           
        }


        // QUEUE TO EXECUTE COMMANDS
        if(commandsToExecute.Count>0){
            if(atualCommandToExecute!=commandsToExecute.Peek()){
                 
                 atualCommandToExecute=commandsToExecute.Peek();
                 StartCoroutine(ExecuteCommand(atualCommandToExecute));                  
            }
        }
       
        
    }

    bool IsCommandValid(string message){
         if( Array.Exists(commands, element => element.Equals(message))){
               return true;
         }
         return false;
    }

 

    //PUT COMMANDS IN QUEUE To Execute
    void PutCommandsInQueue(MessageModel messageModel){
         commandsToExecute.Enqueue(messageModel);
      
    }

     IEnumerator ExecuteCommand(MessageModel command){
          string responseText = "";
            if(command.message.Equals("!feed")){
            Tamagooshi.feed();         
            responseText="Tamagooshi is fed! Now he is "+Tamagooshi.hunger+"% of hungry need";
            }
            if(command.message.Equals("!pet")){
                Tamagooshi.pet();           
                responseText="Tamagooshi received pet! Now he is "+Tamagooshi.happiness+"% of hapinness!";
            }
            StartCoroutine(ShowResultInTextForm(responseText)); 
         yield return new WaitForSeconds(0.8f);

        BackToIdle(Tamagooshi);
       
    }
    IEnumerator ShowResultInTextForm(string responseText){
         yield return new WaitForSeconds(1f);        
          twitchChat.SendPublicChatMessage(responseText);
    }

 
    void BackToIdle(TamagooshiController Tamagooshi){
     
      Tamagooshi.BackToIdle();
      commandsToExecute.Dequeue();
    }
   
}
