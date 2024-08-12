using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Nakama;


public class NakamaConnection : MonoBehaviour
{
   

    public IClient client;
    public ISession UserSession;
    public ISocket socket;

    private static NakamaConnection instance;
    //Singleton
    public static NakamaConnection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NakamaConnection>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject("NakmaConnection");
                    instance = singleton.AddComponent<NakamaConnection>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }
    public async Task CreateSocket()
    {
        socket = instance.client.NewSocket();
        bool appearOnline = true;
        int connectionTimeout = 30;
        await socket.ConnectAsync(UserSession, appearOnline, connectionTimeout);
    }

    // Awake ????
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        try
        {  
            client = new Client(AllConstant.type, AllConstant.host, AllConstant.port, AllConstant.serverKey);
        }
        catch (Exception E)
        {
            Debug.Log("Connection Not Created " + E.Message);
        }

    }

}