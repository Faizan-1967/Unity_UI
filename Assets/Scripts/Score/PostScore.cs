using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nakama.TinyJson;
using UnityEngine;

public class PostScore
{

    public async Task<RootScoreResponse> AddScore(int score)
    {
        try
        {
            var userState = new Dictionary<string, int>
            {   {"xp" ,score },
                { "level", 1},
                {"coins", 1 }
            };

            var data = new Dictionary<string, Dictionary<string, int>>
            {
                { "userState", userState}
            };


            //Json Payload
            string jsonPayload = data.ToJson();


            var res = await NakamaConnection.Instance.client.RpcAsync(NakamaConnection.Instance.UserSession, "matchendrpc", jsonPayload);
            RootScoreResponse RObj = JsonParser.FromJson<RootScoreResponse>(res.Payload);

            return RObj;

        }
        catch (Exception E)
        {
            throw E;
        }

    }


}
