using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using Nakama.TinyJson;
using System.Linq;
using Unity.VisualScripting;

public class ChatScript : MonoBehaviour
{
    public GameObject sendertilePrefab; // Tile Prefab
    public GameObject selftilePrefab; // Tile Prefa
    public GameObject chatContent; // Scrolable View Content
    public InputField chatInputField;

    private User userObj;
    private ChatFeatures ChatObj;

    Queue<IApiChannelMessage> messageQueue;
   
    public Sprite[] icons;
    private string message = "";
  
    async void Start()
    {
        try
        {
            userObj = new();
            this.messageQueue = new Queue<IApiChannelMessage>();
            await StartChat();
        }
        catch(Exception e)
        {
            
            Debug.Log("Start"+e.Message);
        }
    }

   void Update()
    {
        try
        {
            if (messageQueue != null)
            {
                lock (messageQueue)
                {
                    while (messageQueue.Count > 0)
                    {
                       PopulateChatMessages(messageQueue.Peek());
                        messageQueue.Dequeue();
                    }
                }
            }
        }
        catch(Exception e)
        {
           
            Debug.Log("update" + e.Message);
        }
    }

    public async void PopulateChatMessages(IApiChannelMessage message) 
    {
        try
        {
             string messageContent = JsonParser.FromJson<ChatPayload>(message.Content).message;
             var userAccount = await userObj.getUserAccount(message.SenderId);
           

            if (message.SenderId.Equals(NakamaConnection.Instance.UserSession.UserId))
            {
               
                GameObject chattile = GameObject.Instantiate(selftilePrefab, chatContent.transform);

                Transform tileTransform = chattile.transform;

                Text Message = tileTransform.GetChild(0).GetComponent<Text>();
  
                if (Message != null)
                {
                    Message.text = messageContent;
                    
                }

                foreach (var u in userAccount.Users)
                {
                    Text name = tileTransform.GetChild(1).GetChild(0).GetComponent<Text>();
                    if (name != null)
                    {
                        name.text = u.Username;
                    }
                    chattile.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = GameObject.Find("MenuManager").GetComponent<MenuManager>().ProfilePic[int.Parse(u.AvatarUrl) - 1];
                }
            }
            else
            {
                GameObject chattile = GameObject.Instantiate(sendertilePrefab, chatContent.transform);
                Transform tileTransform = chattile.transform;

                Text Message = tileTransform.GetChild(0).GetComponent<Text>();
                if (Message != null)
                {
                    Message.text = messageContent;
                }
                foreach (var u in userAccount.Users)
                {
                    Text name = tileTransform.GetChild(1).GetChild(0).GetComponent<Text>();
                    if (name != null)
                    {
                        name.text = u.Username;
                    }
                    chattile.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = GameObject.Find("MenuManager").GetComponent<MenuManager>().ProfilePic[int.Parse(u.AvatarUrl)-1];
                }
            }
            //Function to go to latest recieved Message.
            ScrollToBottom();

        }catch(Exception e)
        {
            Debug.Log("populate" + e.Message);
        }
    }
    private void ScrollToBottom()
    {
        ScrollRect scrollRect = chatContent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = -1000f;
        }
    }
    void ChatHandler(IApiChannelMessage message)
    {
        try
        {
            lock (messageQueue)
            {
                messageQueue.Enqueue(message);
                Debug.Log(message);
            }
        }catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public async void on_click_send()
    {

        message = chatInputField.text;
        if (message.Length >1)
        {
            await ChatObj.SendMessege(message);
            chatInputField.text = "";
            
        }
        else
        {
            Debug.Log("Message can't be empty.");
        }
    }
    private async Task PopulateOldMessages(string channelId)
    {
        try
        {
            var result = await NakamaConnection.Instance.client.ListChannelMessagesAsync(NakamaConnection.Instance.UserSession, channelId, 100, true);

            lock (messageQueue)
            {
                foreach (var message in result.Messages)
                {
                    messageQueue.Enqueue(message);
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task StartChat()
    {
        try
        {
            this.ChatObj = new();
            await ChatObj.CreateRoom("GlobalRoom", ChatHandler);


            string channelID = ChatObj.channel.Id;

            await PopulateOldMessages(channelID);

        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}



