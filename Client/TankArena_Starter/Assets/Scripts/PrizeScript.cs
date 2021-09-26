using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayerAndPopulateLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Note: This is a recursive function. Invoke it initially with no parameter
    public void CreatePlayerAndPopulateLeaderboard(int playerIndex = 5)
    {
        if (playerIndex <= 0) return;
        const string leaderboardName = "tournamentScore_manual";
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = playerIndex.ToString(),
            CreateAccount = true
        }, result => OnLoggedIn(result, playerIndex, leaderboardName), FailureCallback);
    }

    private void OnLoggedIn(LoginResult loginResult, int playerIndex, string leaderboardName)
    {
        Debug.Log("Player has successfully logged in with " + loginResult.PlayFabId);
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = leaderboardName,
                Value = playerIndex + 100
            }
        }
        }, result => OnStatisticsUpdated(result, playerIndex), FailureCallback);
    }

    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult, int playerIndex)
    {
        Debug.Log("Successfully updated player statistic");
        // Recursively invoke for next player
        CreatePlayerAndPopulateLeaderboard(playerIndex - 1);
    }

    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
