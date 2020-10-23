using System.Collections;
using System.Collections.Generic;
/*
This class is a singleton that holds information used in authentication and permission steps.
*/
[System.Serializable]
public  class ConfigInfo{
public string username, password, channelName; //Get password from https://twitchapps.com/tmi/
public List<string> permissionList;
public static ConfigInfo _instance;

        private  ConfigInfo(){
            
            this.username =  "";
            this.password = "";
            this.channelName = "";
            permissionList = new List<string>();
        }

        public static ConfigInfo GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ConfigInfo();
            }
            return _instance;
        }
}
