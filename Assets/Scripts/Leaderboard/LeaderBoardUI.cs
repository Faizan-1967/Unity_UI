using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LeaderBoardUI : MonoBehaviour
{
    public GameObject tilePrefab; // Tile Prefab
    public GameObject Content; // Scrolable View Content

    public GameObject leaderboardObject; // Reference to the leaderboard panel
    public GameObject ChatObject;
    public GameObject scoreObject; // Score Input Panel Reference
    public InputField scoreInputField; // Score Input Field Reference

    private int finalScore = 50; // Initial value is taken as 0
    public ChatScript ChatObj;

    public async void Post_Score_On_Leaderboard()
    {
        try
        {
            if (scoreInputField.text.Equals(""))
                Debug.Log("Enter Your Score");
            finalScore = int.Parse(scoreInputField.text);

            PostScore scoreObj = new PostScore();
            // Else set the score
            await scoreObj.AddScore(finalScore);
        }
        catch (Exception e)
        {
            Debug.Log("There is error while entering score" + e.Message);
        }
    }
    //Click on LeaderBoard Button on Main Menu
    public async void On_Click_Leaderboard()
    {
        try
        {
            await GenerateLeaderBoard();
            SetPanelActive(leaderboardObject, true);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void SetPanelActive(GameObject panel, bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
        else
        {
            Debug.Log("Give a panel in funtion parameter");
        }
    }

    //This will generate LeaderBoard at run time.

    private async Task GenerateLeaderBoard()
    {
        try
        {
            LeaderBoard Lobj = new LeaderBoard();
            RootResponseLeaderboard res = await Lobj.FetchRecords(AllConstant.LeaderBoardName);

            Debug.Log(res);

            // Check if res and res.data are valid
            if (res == null || res.data == null || res.data.records.Count == 0)
            {
                Debug.LogError("Records fetched Succefully but you have no record at this time in leaaderboard");
                return;
            }

            //To delete previous Tiles

            ClearOldTiles();
            float verticalSpacing = 120f; // Adjust this value to set the spacing between tiles
            float startY = 500f; // Start position for the first tile

            // Loop through the leaderboard records
            for (int i = 0; i < res.data.records.Count; i++)
            {
                // Instantiate a new tile
                GameObject tile = GameObject.Instantiate(tilePrefab, Content.transform);

                // Set the position of the tile
                RectTransform tileRect = tile.GetComponent<RectTransform>();
                tileRect.anchoredPosition = new Vector2(0, startY - (i * verticalSpacing));

                // Ensure the tile prefab has the expected child structure
                Transform tileTransform = tile.transform;
                if (tileTransform.childCount < 1)
                {
                    Debug.LogError("Tile prefab does not have the expected child structure.");
                    continue;
                }

                Transform background = tileTransform.GetChild(0);
                if (background.childCount < 3)
                {
                    Debug.LogError("Tile prefab background does not have the expected child structure.");
                    continue;
                }

                // Set the username
                Text usernameText = background.GetChild(1).GetComponent<Text>();
                if (usernameText != null)
                {
                    usernameText.text = res.data.records[i].username;
                }
                else
                {
                    Debug.LogError("Username text component not found.");
                }

                // Set the score
                Text scoreText = background.GetChild(2).GetComponent<Text>();
                if (scoreText != null)
                {
                    scoreText.text = res.data.records[i].score.ToString();
                }
                else
                {
                    Debug.LogError("Score text component not found.");
                }

                // Set the rank
                Text rankText = background.GetChild(0).GetComponent<Text>();
                if (rankText != null)
                {
                    rankText.text = res.data.records[i].rank.ToString();
                }
                else
                {
                    Debug.LogError("Rank text component not found.");
                }
                
            }
        }
        catch (Exception E)
        {
            Debug.Log(E.Message);
            Debug.Log("Error in menu");
            SceneManager.LoadScene("Home");
        }
    }

    private void ClearOldTiles()
    {
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
