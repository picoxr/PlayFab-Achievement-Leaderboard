using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using LitJson;
using Unity.XR.PXR;
public class SendUserMessage : MonoBehaviour
{
    //private const string IS_SUCCESS = "isSuccess";
    //private const string MSG = "msg";
    //private const string CODE = "code";

    public delegate void DelegateGetUserInfoResult(string openid, string name);
    public DelegateGetUserInfoResult DELEGATE_GET_USER_INFO_RESULT;

    private void Start()
    {

    }

    public void LoginCallback(string LoginInfo)
    {
        //JsonData jsrr = JsonMapper.ToObject(LoginInfo);

        LoginSDK.GetUserAPI();

        Debug.Log("login callback:");
    }

    public void UserInfoCallback(string userInfo)
    {
        Debug.Log("userInfo callback:" + userInfo);

        JsonData jsrr = JsonMapper.ToObject(userInfo);
        JsonData user_data = JsonMapper.ToObject(jsrr["data"].ToJson());        
        string openid = user_data["openid"].ToString();
        string name = user_data["username"].ToString();
        
        this.DELEGATE_GET_USER_INFO_RESULT(openid, name);
    }


    public GameObject GetCurrentGameObject()
    {
        return GameObject.Find("MassageInfo");
    }

    public void ActivityForResultCallback(string activity)
    {
        PicoPaymentSDK.jo.Call("authCallback", activity);
    }
}
