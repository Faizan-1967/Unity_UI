using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainObject; // Reference to the main menu panel
    public InputField newNameField;
    public InputField newAvatarNumber;

    [SerializeField] private Text UserName;
    [SerializeField] private Image profilePic;

    public Sprite[] ProfilePic;

    
    // Start is called before the first frame update
   void Start()
    {
        SetPanelActive(mainObject, true);
    }

    void Update()
    {
        try
        {
            if (NakamaConnection.Instance.UserSession.IsExpired)
                SceneManager.LoadScene("Login");
     
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            SceneManager.LoadScene("Login");
        }
    }
    private void SetPanelActive(GameObject panel, bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
    }

    public async void ChangeName()
    {
        var newName = newNameField.text;
        var newAvatar = newAvatarNumber.text;

        if (newName.Length<2|| newName.Length>20 || (int.Parse(newAvatar) > 6 || int.Parse(newAvatar) <0 )|| newAvatar=="")
        {
            Debug.Log("Name should be greater than lenghth 2 and Avatar value should be between 1 and 6");
           
        }
        else { 
        try
        {
                UserName.text = newName;
                var num = int.Parse(newAvatar);
                profilePic.sprite = ProfilePic[num - 1];
                await NakamaConnection.Instance.client.UpdateAccountAsync(NakamaConnection.Instance.UserSession, newName, newName, newAvatar);
        }
        catch (Exception ex)
        {
            Debug.Log("Error in Try catch" + ex);

        }
            }
       
    }
         public void Logout()
        {
            SceneManager.LoadScene("Login");
        }
 }

