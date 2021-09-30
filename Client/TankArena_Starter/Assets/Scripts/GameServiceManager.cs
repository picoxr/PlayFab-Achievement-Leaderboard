using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;

public class GameServiceManager : MonoBehaviour
{
    public const string STATISTIC_HIGH_SCORE = "HighScore";
    public const string ACHIEVENTMENT_SCORE_100 = "achievement_highscore_100";
    public const string ACHIEVENTMENT_LEVEL_COMPLETE = "achievement_levelcompleted";

    public delegate void DelegateGetLeaderboardResult(GetLeaderboardResult result);
    public DelegateGetLeaderboardResult DELEGATE_GET_LEADERBOARD_RESULT;

    public delegate void DelegateLoginSuccess(LoginResult result);
    public DelegateLoginSuccess DELEGATE_LOGIN_SUCCESS;

    //public delegate void DelegateOnStatisticUpdate();
    //public DelegateOnStatisticUpdate DELEGATE_ON_STATISTIC_UPDATE;

    public delegate void DelegateGetAchievenmentResult(GetUserDataResult result);
    public DelegateGetAchievenmentResult DELEGATE_GET_ACHIEVEMENT_RESULT;

    public delegate void DelegateGetUserScoreResult();
    public DelegateGetUserScoreResult DELEGATE_GET_USER_SCORE_RESULT;

    public LoginResult CurrentUserLoginInfo;
    public List<StatisticValue> CurrentUserStatistics = new List<StatisticValue>();

    public Dictionary<string, bool> AchievementList = new Dictionary<string, bool>();

    private void Start()
    {
        CurrentUserLoginInfo = new LoginResult();
    }

    private void Update()
    {

    }

    public void LoginPlayer(string name)
    {
        Debug.Log("LoginPlayer " + name);
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            //Substitute your own titleId in the PlayFab console
            PlayFabSettings.staticSettings.TitleId = "FB462";
        }
        var request = new LoginWithCustomIDRequest {
            CustomId = name,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
                GetPlayerProfile = true
            }
        };
        //var request_email = new LoginWithEmailAddressRequest { Email = "zhaiguanxun@hotmail.com",  CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

        // Should use other login method in the future, especially how to use Pico account for PlayFab
    }

    /// <summary>
    /// Get current User Score
    /// </summary>
    public void GetUserStatistic()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), OnGetUserStatisticResult, OnErrorCallback);
    }

    private void OnGetUserStatisticResult(GetPlayerStatisticsResult result)
    {
        CurrentUserStatistics = result.Statistics;
        Debug.Log("Received the following Statistics:");
        if (DELEGATE_GET_USER_SCORE_RESULT != null)
            this.DELEGATE_GET_USER_SCORE_RESULT();
    }

    /// <summary>
    /// Submit Score to Service
    /// </summary>
    /// <param name="score">Player current score</param>
    public void UpdateUserScore(int score)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = STATISTIC_HIGH_SCORE,
                Value = score
            }
        }
        }, result => OnUpdatePlayerScoreResult(result), FailureCallback);
    }

    private void OnUpdatePlayerScoreResult(UpdatePlayerStatisticsResult updateResult)
    {
        Debug.Log("Successfully submitted high score");
        GetUserAchievement();
    }
    
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("OnLoginSuccess");

        //After login success, get user account info
        CurrentUserLoginInfo = result;
        this.DELEGATE_LOGIN_SUCCESS(result);
        //var request = new GetAccountInfoRequest();
        //PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoResult, OnGetAccountInfoError);
        this.GetUserStatistic();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void SubmitDisplayName(string name)
    {
        Debug.Log("SubmitDisplayName = " + name);
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name 
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateUserDisplayName, OnErrorCallback);       
    }

    private void OnUpdateUserDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("OnUpdateUserDisplayName " + result.DisplayName);
    }

    private void OnGetAccountInfoResult(GetAccountInfoResult result)
    {
        //Debug.Log("OnGetAccountInfoSuccess " + result);        
        
        //CurrentPlayerName.text = result.AccountInfo.CustomIdInfo.CustomId;
    }

    private void OnGetAccountInfoError(PlayFabError error)
    {
        Debug.LogError("OnGetAccountInfoError " + error);
    }

    //Leaderboard related interfaces
    public void RequestLeaderboard(string statisticName, int maxCount, int startPosition = 0)
    {
        Debug.Log("RequestLeaderboard " + statisticName);
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = statisticName,
            StartPosition = 0,
            MaxResultsCount = 10
        }, OnGetLeaderboardResult, FailureCallback);
    }

    private void OnGetLeaderboardResult(GetLeaderboardResult result)
    {
        //Debug.Log("OnGetLeaderboardResult");
        DELEGATE_GET_LEADERBOARD_RESULT(result);
    }

    /// <summary>
    /// PlayFab Failure callback
    /// </summary>
    /// <param name="error"></param>
    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    //Achievement Related interfaces
    public void GetUserAchievement()
    {
        PlayFabClientAPI.GetUserReadOnlyData(new GetUserDataRequest
        {
            PlayFabId = CurrentUserLoginInfo.PlayFabId,
            Keys = new List<string>
            {
                "UserAchievementList"
            }
        }, OnGetUserAchievementResult, OnErrorCallback);
    }
    
    private void OnGetUserAchievementResult(GetUserDataResult result)
    {
        DELEGATE_GET_ACHIEVEMENT_RESULT?.Invoke(result);
    }

    public void UpdateLevelAchievement(string name)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "checkLevelCompleteForAchievement",
            FunctionParameter = new { LevelName = name },
        }, OnUpdateGameLevel, OnErrorCallback);
    }

    private void OnUpdateGameLevel(ExecuteCloudScriptResult result)
    {
        Debug.Log("Update Game Level Success!");
        this.GetUserAchievement();
    }

    private static void OnErrorCallback(PlayFabError error)
    {
        Debug.LogError("OnErrorCallback " + error.ErrorMessage);
    }

}
