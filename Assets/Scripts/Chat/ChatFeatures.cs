using UnityEngine;
using System.Collections;
using Unity;
using Nakama;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nakama.TinyJson;
using System;

public class ChatFeatures 
{
    public IChannel channel;
    NakamaConnection instance;

    private static Action<IApiChannelMessage> _messageHandler;

    public ChatFeatures()
    {
        this.instance = NakamaConnection.Instance;
    }

    public async Task CreateRoom(string name, Action<IApiChannelMessage>MessageCallBack)
    {
        try
        {
            var roomname = name;
            var persistence = true;
            var hidden = false;
            // Join the chat room
            channel = await instance.socket.JoinChatAsync(roomname, ChannelType.Room, persistence, hidden);
            NakamaConnection.Instance.socket.ReceivedChannelMessage += message =>
            {
                MessageCallBack?.Invoke(message);
            };

        }
        catch (Exception ex)
        {
            Debug.Log($"General error occurred: {ex.Message}");
        }
    }

    public async Task SendMessege(string message)
    {
        try
        {
            var content = new Dictionary<string, string> { { "message", message } }.ToJson();
            _ = await instance.socket.WriteChatMessageAsync(channel, content);
        }
        catch(Exception ex)
        {
            Debug.Log("Error While Sending Message" + ex.Message);
        }
    }
   
}
