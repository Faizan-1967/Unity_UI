using UnityEngine;
using Nakama;
using System;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;

public class Authentication
{
    public NakamaConnection instance;
 
    public Authentication(NakamaConnection authObj)
    {
        this.instance = authObj;
    }

   public async Task AuthenticateClient()
    {
        try
        {
            // Authentication logic
            instance.UserSession = await instance.client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
            await instance.CreateSocket();
            SceneManager.LoadScene("Home");
        }
        catch(Exception E)
        {
            throw E;

        }      
    }
}
