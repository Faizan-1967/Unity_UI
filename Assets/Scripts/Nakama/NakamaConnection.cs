using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Nakama;


public class NakamaConnection : MonoBehaviour
{
    public static string host = "13.60.75.218";
    public static string type = "http";
    public static int port = 7350;
    public static string serverKey = "defaultkey";

    public IClient client;
    public ISession UserSession;

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
            client = new Client(type, host, port, serverKey);
        }
        catch (Exception E)
        {
            Debug.Log("Connection Not Created " + E.Message);
        }

    }

}