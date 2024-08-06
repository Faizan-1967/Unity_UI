using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nakama.TinyJson;
using UnityEngine;

public class LeaderBoard
{
    NakamaConnection nakma;
    public LeaderBoard()
    {
        this.nakma = NakamaConnection.Instance;
    }

    // Function to get all leaderboard records
    public async Task<RootResponseLeaderboard> FetchRecords(string table)
    {
        try
        {
            LeaderBoardData1 tableID = new LeaderBoardData1
            {
                leaderBoardId = table
            };
            string jsonPayload = JsonUtility.ToJson(tableID);

            // Call the custom RPC
            var response = await nakma.client.RpcAsync(nakma.UserSession, "getleaderboardrpc", jsonPayload);
            RootResponseLeaderboard RObj = JsonParser.FromJson<RootResponseLeaderboard>(response.Payload);
            return RObj;
        }
        catch (Exception e)
        {
            System.Console.WriteLine("RPC does not exist:" + e.Message);
            throw e;
        }
    }


}
