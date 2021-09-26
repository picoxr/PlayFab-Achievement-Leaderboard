using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public Text[] PlayerNames;
    public Text[] Scores;

    //public Dictionary<string, int> ScoreDic = new Dictionary<string, int>();

    private const int MAX_LEADERBOARD_COUNT = 10;
    private const float UPDATE_TIME_GAP = 10.0f;
    private GameServiceManager m_GameService;
    private List<PlayerLeaderboardEntry> LeaderboardRetrived = new List<PlayerLeaderboardEntry>();


    // Start is called before the first frame update
    void Start()
    {
        PlayerNames = new Text[MAX_LEADERBOARD_COUNT];
        Scores = new Text[MAX_LEADERBOARD_COUNT];

        //init Leaderboard gameobjects
        for (int i = 0; i < MAX_LEADERBOARD_COUNT; i++)
        {
            PlayerNames[i] = GameObject.Find("Content" + i).transform.Find("Player").GetComponent<Text>();
            Scores[i] = GameObject.Find("Content" + i).transform.Find("Score").GetComponent<Text>();
        }

        //init gameservice
        m_GameService = GameObject.Find("GameServiceManager").GetComponent<GameServiceManager>();
        m_GameService.DELEGATE_GET_LEADERBOARD_RESULT += UpdateLeaderboard;
        //start request leaderboard
        m_GameService.RequestLeaderboard(GameServiceManager.STATISTIC_HIGH_SCORE, MAX_LEADERBOARD_COUNT);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void UpdateLeaderboard(GetLeaderboardResult result)
    {
        //Debug.Log("UpdateLeaderboard");
        List<PlayerLeaderboardEntry> LeaderboardRetrived = result.Leaderboard;

        int max_count = Math.Min(LeaderboardRetrived.Count, PlayerNames.Length);

        for (int i = 0; i < PlayerNames.Length; i++)
        {
            if (i < LeaderboardRetrived.Count)
            {
                PlayerNames[i].text = LeaderboardRetrived[i].DisplayName != null ? LeaderboardRetrived[i].DisplayName : LeaderboardRetrived[i].PlayFabId;
                Scores[i].text = LeaderboardRetrived[i].StatValue.ToString();
            }
            else
            {
                PlayerNames[i].text = "";
                Scores[i].text = "";
            }


            //if (!ScoreDic.ContainsKey(LeaderboardRetrived[i].PlayFabId))
            //{
            //    ScoreDic.Add(LeaderboardRetrived[i].PlayFabId, LeaderboardRetrived[i].StatValue);
            //}
            //else
            //{
            //    ScoreDic[LeaderboardRetrived[i].PlayFabId] = LeaderboardRetrived[i].StatValue;
            //}
        }

        Invoke("OnUpdateLeaderboard", UPDATE_TIME_GAP);
    }

    private void OnUpdateLeaderboard()
    {
        m_GameService.RequestLeaderboard(GameServiceManager.STATISTIC_HIGH_SCORE, MAX_LEADERBOARD_COUNT);
    }


    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

}
