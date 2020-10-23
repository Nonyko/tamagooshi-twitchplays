
/*
Model representation for a message in twitch chat.
*/
public class MessageModel{
public string chatName, message;

public MessageModel(string chatName, string message){
    this.chatName = chatName;
    this.message = message;
}

}