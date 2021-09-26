using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    private string achievement_strength_10 = "achievement_strength_10";
    private string achievement_levelcompleted = "achievement_levelcompleted";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //// Build the request object and access the API
    //private static void StartCloudHelloWorld()
    //{
    //    PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
    //    {
    //        FunctionName = "helloWorld", // Arbitrary function name (must exist in your uploaded cloud.js file)
    //        FunctionParameter = new { inputValue = "YOUR NAME" }, // The parameter provided to your function
    //        GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
    //    }, OnCloudHelloWorld, OnErrorShared); 
    //}

    //public void AcquireReward()
    //{
    //    PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
    //    {
    //        FunctionName = "completedLevel",
    //        FunctionParameter = new { levelName = "Loby", monstersKilled = "10"},
    //        GeneratePlayStreamEvent = true,
    //    }, OnAcquireAward, OnErrorShared);
    //}

    private void AchievementAccomplished(string achievementId)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "accomplishAchievement",
            FunctionParameter = new { achievementName = achievementId },
        }, OnAcquireAchievement, OnErrorShared);
    }

    public void AchievementAccomplished_strength10()
    {
        AchievementAccomplished(achievement_strength_10);
    }

    public void AchievementAccomplished_levelCompleted()
    {
        AchievementAccomplished(achievement_levelcompleted);
    }

    // For test only
    private void RevokeAchievement(string achievementId)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "revokeAchievement",
            FunctionParameter = new { achievementName = achievementId },
        }, OnRevokeAchievement, OnErrorShared);
    }

    // For test only
    public void RevokeAchievement_strength10()
    {
        AchievementAccomplished(achievement_strength_10);
    }

    // For test only
    public void RevokeAchievement_levelCompleted()
    {
        AchievementAccomplished(achievement_levelcompleted);
    }

    //private static void OnCloudHelloWorld(ExecuteCloudScriptResult result)
    //{
    //    for(int i = 0; i < result.Logs.Count; i++)
    //    {
    //        Debug.Log("LogLevel: " + result.Logs[i].Level + " LogMessage: " + result.Logs[i].Message + "\n");
    //    }
    //}

    //private static void OnAcquireAward(ExecuteCloudScriptResult result)
    //{
    //    for (int i = 0; i < result.Logs.Count; i++)
    //    {
    //        Debug.Log("LogLevel: " + result.Logs[i].Level + " LogMessage: " + result.Logs[i].Message + "\n");
    //    }
    //}

    private static void OnAcquireAchievement(ExecuteCloudScriptResult result)
    {
        for (int i = 0; i < result.Logs.Count; i++)
        {
            Debug.Log("LogLevel: " + result.Logs[i].Level + " LogMessage: " + result.Logs[i].Message + "\n");
        }
    }
    private static void OnRevokeAchievement(ExecuteCloudScriptResult result)
    {
        for (int i = 0; i < result.Logs.Count; i++)
        {
            Debug.Log("LogLevel: " + result.Logs[i].Level + " LogMessage: " + result.Logs[i].Message + "\n");
        }
    }

    private static void OnErrorShared(PlayFabError error)
    {

    }
}
