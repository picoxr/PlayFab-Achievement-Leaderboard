using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class AchievementManager : MonoBehaviour
{
    //public Text TextAchievement;
    public Image ImageAchievementScore;
    public Image ImageAchievementLevel;

    private Dictionary<string, bool> m_AchievementList = new Dictionary<string, bool>();

    private const string ACHIEVEMENT_HIGH_SCORE_100 = "achievement_highscore_100";
    private const string ACHIEVEMENT_LEVEL_COMPLETE = "achievement_levelcompleted";
    private bool NeedUpdateAchievement = false;

    private GameServiceManager m_GameService;

    private void Start()
    {
        //init gameservice
        m_GameService = GameObject.Find("GameServiceManager").GetComponent<GameServiceManager>();
        
        m_GameService.DELEGATE_GET_ACHIEVEMENT_RESULT += UpdateUserAchievement;

        //Get user achievement data
        m_GameService.GetUserAchievement();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        m_GameService.DELEGATE_GET_ACHIEVEMENT_RESULT -= UpdateUserAchievement;
    }

    private void UpdateUserAchievement(GetUserDataResult result)
    {
        string achievementJsonRetrieved = result.Data["UserAchievementList"].Value;
        m_AchievementList = JsonConvert.DeserializeObject<Dictionary<string, bool>>(achievementJsonRetrieved);
        //this.GetComponent<AchievementScript>().AchievementList_local = JsonConvert.DeserializeObject<Dictionary<string, bool>>(achievementJsonRetrieved);
        //TextAchievement.text = "Current Achievements: \n";

        if (m_AchievementList.Count > 0)
        {
            foreach (var item in m_AchievementList)
            {
                //TextAchievement.text += item.Key + " = " + item.Value + "\n";

                if (item.Key == ACHIEVEMENT_HIGH_SCORE_100)
                {
                    ImageAchievementScore.color = item.Value ?Color.white : Color.grey;
                }
                if (item.Key == ACHIEVEMENT_LEVEL_COMPLETE)
                {
                    ImageAchievementLevel.color = item.Value ? Color.white : Color.grey;
                }
            }
        }
    }
    
    private void OnAchievementUpdated(ExecuteCloudScriptResult result)
    {
        for (int i = 0; i < result.Logs.Count; i++)
        {
            Debug.Log("LogLevel: " + result.Logs[i].Level + " LogMessage: " + result.Logs[i].Message + "\n");
        }

        NeedUpdateAchievement = true;
    }

}
