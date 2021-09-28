/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class GameManager : MonoBehaviour
{

    [Header("Arena Objects")]
    private GameObject playerTank;

    [Header("Game UI")]
    public GameObject loadingScreen;
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject GameUIScreen;
    public string arenaName = "GameService Demo";

    public Text CurrentPlayerNameText;
    public Text TextCurrentScore;

    public string UserOpenId;
    public string UserName;

    [Space]
    private const int ADD_SCORE = 100;
    private bool isPaused = false;
    public static int WeakWallCount = 0;
    //private bool isLoginFinished = false;

    private int m_CurrentScore = 0;
    private SendUserMessage m_PicoUserMessage;
    private GameServiceManager m_GameService;

    void Start()
    {
        m_PicoUserMessage = GameObject.Find("PicoPayment").GetComponent<SendUserMessage>();
        m_GameService = GameObject.Find("GameServiceManager").GetComponent<GameServiceManager>();
        playerTank = GameObject.FindGameObjectWithTag("Player");
        m_PicoUserMessage.DELEGATE_GET_USER_INFO_RESULT += LoginToGameService;

        loadingScreen.SetActive(true);
        GameUIScreen.SetActive(false);

        WeakWallCount = GameObject.FindGameObjectsWithTag("WeakWall").Length;

        // first login to Pico account
        Unity.XR.PXR.LoginSDK.Login();


        // login the with Pico openID, currently the input value is fake one
        m_GameService.DELEGATE_LOGIN_SUCCESS += this.UpdatePlayerName;
        m_GameService.DELEGATE_GET_USER_SCORE_RESULT += this.UpdatePlayerScore;
        
    }
    

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (isPaused)
        //    {
        //        GameUIScreen.SetActive(true);
        //        pauseScreen.SetActive(false);
        //        isPaused = false;
        //        Time.timeScale = 1.0f;
        //    }
        //    else
        //    {
        //        GameUIScreen.SetActive(false);
        //        pauseScreen.SetActive(true);
        //        isPaused = true;
        //        Time.timeScale = 0.0f;
        //    }
        //}
    }

    public void LoginToGameService(string openid, string name)
    {
        this.UserOpenId = openid;
        this.UserName = name;
        CurrentPlayerNameText.text = this.UserName;
        m_GameService.LoginPlayer(this.UserOpenId);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("MainScene"); 
        Time.timeScale = 1.0f;
        Rigidbody playerRB = playerTank.GetComponent<Rigidbody>();
        playerRB.isKinematic = true;
        playerRB.isKinematic = false;
    }

    private void UpdatePlayerName(LoginResult result)
    {
        loadingScreen.SetActive(false);
        GameUIScreen.SetActive(true);

        //string display_name = null;
        Debug.Log("result.InfoResultPayload.PlayerProfile = " + result.InfoResultPayload.PlayerProfile);
        //if (result.InfoResultPayload.PlayerProfile != null)
        //{
        //    display_name = result.InfoResultPayload.PlayerProfile.DisplayName;
        //}
        
        //if (display_name == null)
        //{
            
        //}

        Debug.Log("PlayerProfile is null");
        //Display name must be between 3 and 25 characters
        m_GameService.SubmitDisplayName(CurrentPlayerNameText.text);

    }

    private void UpdatePlayerScore()
    {
        m_CurrentScore = 0;
        for (int i = 0; i < m_GameService.CurrentUserStatistics.Count; i++)
        {
            if (m_GameService.CurrentUserStatistics[i].StatisticName == GameServiceManager.STATISTIC_HIGH_SCORE)
            {
                m_CurrentScore = m_GameService.CurrentUserStatistics[i].Value;
                break;
            }
        }
        TextCurrentScore.text = "Score:" + m_CurrentScore;
    }

    //add score for the current player
    public void AddScore()
    {
        WeakWallCount--;
        if (WeakWallCount ==0)
        {
            //m_GameService.UpdateLevelAchievement("Tank");
        }

        m_CurrentScore += ADD_SCORE;
        TextCurrentScore.text = "Score:" + m_CurrentScore;
        m_GameService.UpdateUserScore(m_CurrentScore);
    }

    public int GetCurrentScore()
    {
        return m_CurrentScore;
    }



}
