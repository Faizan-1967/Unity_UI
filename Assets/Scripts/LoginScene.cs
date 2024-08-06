using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoginScene : MonoBehaviour
{
    public GameObject loadingPanel; // Panel for loading state
    public GameObject loginPanel; // Panel for login state
    public GameObject errorPanel; // Panel for error state

    private NakamaConnection nakma;

  
    void Start()
    {
        SetPanelActive(loginPanel, true); 
        nakma = NakamaConnection.Instance;
    }

    //Function to active panels
    private void SetPanelActive(GameObject panel, bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
    }

    public void ShowError()
    {

        SetPanelActive(errorPanel, false);
        SetPanelActive(loginPanel, true);
        SetPanelActive(loadingPanel, false);
    }

    // Login Button OnClick Handler
    public async void Login()
    {
        try
        {
            // Show loading panel
            SetPanelActive(loginPanel, false);
            SetPanelActive(loadingPanel, true);

            // Creating an Auth Object for authentication
            Authentication authObj = new Authentication(nakma);
            await authObj.AuthenticateClient();
        }
        catch (Exception e)
        {
            
            SetPanelActive(errorPanel, true);
            SetPanelActive(loadingPanel, false);
            SetPanelActive(loginPanel, false);

            Debug.Log("Exception in Login Button: " + e.Message);
        }
    }

    void Update()
    {
    }
}
