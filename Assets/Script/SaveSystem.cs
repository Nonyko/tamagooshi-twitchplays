using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

/*
System that manages the saves of the app.
*/
public static class SaveSystem
{
 public static SaveData saveData;

 public static string SaveName = "save"; 
 public static void CreateSave(){
     BinaryFormatter formatter = new BinaryFormatter();
     
     string path = Application.persistentDataPath + "/"+SaveName+".bin";

     FileStream stream = new FileStream(path, FileMode.Create);
     formatter.Serialize(stream, saveData);
     stream.Close();
 }

 public static void Save(string username, string password, string channelName){
     BinaryFormatter formatter = new BinaryFormatter();
     
     string path = Application.persistentDataPath + "/"+SaveName+".bin";
     FileStream stream = new FileStream(path, FileMode.Create);

  
  
        saveData = new SaveData(username, password,  channelName);
      

        formatter.Serialize(stream, saveData);
        stream.Close();
 }

 public static SaveData LoadSave(){
     string path = Application.persistentDataPath + "/"+SaveName+".bin";
     if(File.Exists(path)){
           BinaryFormatter formatter = new BinaryFormatter();
           FileStream stream = new FileStream(path, FileMode.Open);
           saveData = formatter.Deserialize(stream) as SaveData;
           stream.Close();
           //Cleaning the strings loaded
            saveData.username = new string(SaveSystem.saveData.username.Where(c => char.IsLetter(c) || char.IsDigit(c) || c=='_').ToArray());
            saveData.password = new string(SaveSystem.saveData.password.Where(c => char.IsLetter(c) || char.IsDigit(c) || c==':').ToArray());
            saveData.channelName = new string(SaveSystem.saveData.channelName.Where(c => char.IsLetter(c) || char.IsDigit(c) || c=='_').ToArray());
        
           Debug.Log("Info Loaded!");
            
           return saveData;
     }else{
         Debug.LogError("Theres no file in the path "+path);
         return null;
     }
 }

}

[System.Serializable]
public class SaveData
{
     public string username, password, channelName;

    public  SaveData(string username, string password, string channelName){
        this.username = username;
        this.password = password;
        this.channelName = channelName;
    }
    
}