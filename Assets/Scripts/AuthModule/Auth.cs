using UnityEngine;
using Nakama;
using System;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;

public class Authentication
{
    public NakamaConnection instance;
    public Authentication(NakamaConnection obj)
    {
        this.instance = obj;
    }

   public async Task AuthenticateClient()
    {
        try
        {
            // Authentication logic
            instance.UserSession = await instance.client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
            //If Authenticated 
            SceneManager.LoadScene("Home");
        }
        catch(Exception E)
        {
            SceneManager.LoadScene("Login");
            throw E;

        }      
    }
}
