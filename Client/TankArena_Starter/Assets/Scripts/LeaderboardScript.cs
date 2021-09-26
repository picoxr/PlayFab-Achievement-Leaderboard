using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour
{
    public Text LeaderboardResult;
    public Dictionary<string, int> ScoreDic = new Dictionary<string, int>();
    //public bool HasALogin = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitScore(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "HighScore",
                Value = playerScore
            }
        }
        }, result => OnStatisticsUpdated(result), FailureCallback);
    }

    public void AddOneRecord()
    {
        SubmitScore(0);
    }

    public void RequestLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        }, result => DisplayLeaderboard(result), FailureCallback);
    }

    public void DisplayLeaderboard(GetLeaderboardResult result)
    {
        LeaderboardResult.text = "";
        List<PlayerLeaderboardEntry> LeaderboardRetrived = result.Leaderboard;
        for (int i = 0; i < LeaderboardRetrived.Count; i++)
        {
            LeaderboardResult.text +=
                "DisplayName: " + LeaderboardRetrived[i].DisplayName + "  " +
                "PlayFabId: " + LeaderboardRetrived[i].PlayFabId + "  " +
                "Position: " + LeaderboardRetrived[i].Position + "  " +
                "LastLogin: " + LeaderboardRetrived[i].Profile.LastLogin + " " +
                "StatValue: " + LeaderboardRetrived[i].StatValue + "  \n";

            if (!ScoreDic.ContainsKey(LeaderboardRetrived[i].PlayFabId))
            {
                ScoreDic.Add(LeaderboardRetrived[i].PlayFabId, LeaderboardRetrived[i].StatValue);
            }
            else
            {
                ScoreDic[LeaderboardRetrived[i].PlayFabId] = LeaderboardRetrived[i].StatValue;
            }
            
        }

        LeaderboardResult.text += 
            "NextReset: " + result.NextReset.ToString() + "\n" +
            "Version" + result.Version.ToString() + "\n";
    }

    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
    {
        Debug.Log("Successfully submitted high score");
    }

    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

}
