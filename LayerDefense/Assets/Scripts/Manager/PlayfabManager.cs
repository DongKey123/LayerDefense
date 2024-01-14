using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using UnityEngine;

public class PlayfabManager : MonoSingleton<PlayfabManager>
{
    public enum EPlatform
    {
        Guest,
        Google,
        Apple,
        Facebook,
    }

    public static event Action<LoginResult> OnLoginSuccess;
    public static event Action<PlayFabError> OnLoginFailed;

    [SerializeField] GetPlayerCombinedInfoRequestParams _infoRequestParams;

    #region :   Public

    public static string SerializePlayfabJson<T>(T target)
    {
        return PlayFabSimpleJson.SerializeObject(target);
    }

    public static T DeserializePlayfabJson<T>(string json)
    {
        return PlayFabSimpleJson.DeserializeObject<T>(json);
    }

    public static void SendRequest(string funcName, object parameter, Action<ExecuteCloudScriptResult> success, Action<PlayFabError> error)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = funcName,
            FunctionParameter = parameter,
            GeneratePlayStreamEvent = true

        }, success, error);
    }

    public void Login(int platform)
    {
        PlayerPrefs.SetInt("Platform", platform);

        switch ((EPlatform)platform)
        {
            default:
            case EPlatform.Guest:
                break;
            case EPlatform.Google:
                break;
            case EPlatform.Apple:
                break;
            case EPlatform.Facebook:
                break;
        }
    }

    #endregion

    #region :   Protected

    protected override void OnInitialize()
    {
        base.OnInitialize();

        OnLoginSuccess += OnLoginSuccessEvent;
        OnLoginFailed += OnLoginFailedEvent;
    }

    protected override void OnRelease()
    {
        base.OnRelease();

        OnLoginSuccess -= OnLoginSuccessEvent;
        OnLoginFailed -= OnLoginFailedEvent;
    }

    #endregion

    #region :   Private

    private void LoginToGuest()
    {
        var request = new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = _infoRequestParams,
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailed);
    }

    private void OnLoginSuccessEvent(LoginResult result)
    {

    }

    private void OnLoginFailedEvent(PlayFabError error)
    {

    }

    #endregion
}
