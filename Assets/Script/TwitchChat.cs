using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;
/*
This class handles the twitch irc server connection, listen it and sends it messages to a Queue
*/
public class TwitchChat : MonoBehaviour
{
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    private  ConfigInfo config;
    string username, password, channelName; //Get password from https://twitchapps.com/tmi/

    public MessageModel messageModel;
    public Queue<MessageModel> messages = new Queue<MessageModel>();
    void Start(){
       loadConfigInfo();
       Connect();
    }
    //it Loads the config info from SaveSystem
    void loadConfigInfo(){
        
      
        SaveData saveData =  SaveSystem.LoadSave();
       

        config = ConfigInfo.GetInstance();
        config.username = SaveSystem.saveData.username;
        config.password = saveData.password;
        config.channelName = saveData.channelName;

      
        username = config.username;
        password = config.password;
        channelName = config.channelName;        
    }


    // Update is called once per frame
    void Update(){
       
        
        if(!twitchClient.Connected){
           
            loadConfigInfo();
            Connect();
        }
         ReadChat();
    }

    //It connects in twitch irc server and send /mods and /vips commands in chat
    private void Connect(){
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS "+password);
        writer.WriteLine("NICK "+username);
        writer.WriteLine("USER "+username+" 8 * :"+ username);
        writer.WriteLine("JOIN #"+channelName);
        
        writer.WriteLine("CAP REQ :twitch.tv/membership");
        writer.WriteLine("CAP REQ :twitch.tv/commands");              
        
        writer.Flush();

        SendPublicChatMessage("/mods");

        SendPublicChatMessage("/vips");

       
    }

    //It handles the /mods and /vips commands response and puts it in a permission list
    private void GetPermissions(string message){
        
        
        //Adds broadcaster in permissions
       
        config.permissionList.Add(config.username);

        // Extracting moderators list from message
        if(message.Contains("moderators") ){
            var splitPoint = message.IndexOf("are:", 1);
            var substringModerators = message.Substring(splitPoint+4);
          
            substringModerators = substringModerators.Trim();
            substringModerators = substringModerators.Replace(".","");
            string[] moderators = substringModerators.Split(',');
           
            foreach(string mod in moderators){
               
                config.permissionList.Add(mod);
            }
        }

        // Extracting VIPs list from message
        if(message.Contains("VIPs") ){
            var splitPoint = message.IndexOf("are:", 1);
            var substringVIPs = message.Substring(splitPoint+4);
             
            
            
            substringVIPs = substringVIPs.Trim();
            substringVIPs = substringVIPs.Replace(".","");
            string[] vips = substringVIPs.Split(',');
       
            foreach(string vip in vips){
              
                config.permissionList.Add(vip);
            }
        }        

    } 

    //It reads twitch chat and handle its messages
    private void ReadChat(){
        if(twitchClient.Available>0){
            var message = reader.ReadLine();
            
            //Users messages only
            if(message.Contains("PRIVMSG")){

                //GET users name by spliting it from the string
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                //Get the users message by spliting it from the string
                splitPoint  = message.IndexOf(":",1);
                message = message.Substring(splitPoint + 1);

                messageModel = new MessageModel(chatName, message);
                messages.Enqueue(messageModel);
//                print( String.Format("{0}: {1}", chatName, message));
            }
            //It handles the /vips and /mods commands response, calling GetPermissions()
            if(message.StartsWith(":tmi.twitch.tv NOTICE #"+channelName+" :The moderators of this channel are: ")
             && message.Contains("moderators") 
            
            || (message.StartsWith(":tmi.twitch.tv NOTICE #"+channelName+" :The VIPs of this channel are: "))
             && message.Contains("VIPs") ){
               
                    GetPermissions(message);
                   
            }
           
        }
    }

    //it sends an irc message
     public void SendIrcMessage(string message)
        {
            try
            {
                writer.WriteLine(message);
                writer.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    //it sends a public message
        public void SendPublicChatMessage(string message)
        {
            try
            {
                SendIrcMessage(":" + username + "!" + username + "@" + username +
                ".tmi.twitch.tv PRIVMSG #" + channelName + " :" + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
}
