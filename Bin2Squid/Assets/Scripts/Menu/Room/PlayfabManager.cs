using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;



public class PlayfabManager : MonoBehaviour
{
    public ProfileManager ProfileManager_;

    private void Start()
    {
        //GetMoney
        var requestMoney = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(requestMoney, OnGetMoney, error => Debug.Log("GetMoneyError"));

        //GetUsername
        var requestUsername = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(requestUsername, OnGetUsername, error => Debug.Log("GetUsernameError"));
    }

    private void OnGetMoney(GetUserDataResult result)
    {
        Debug.Log("Money: " + result.Data["Money"].Value);
        ProfileManager_.PutMoney("$ " + result.Data["Money"].Value);
    }

    private void OnGetUsername(GetAccountInfoResult  result)
    {
        Debug.Log("Username: " + result.AccountInfo.Username);
        ProfileManager_.PutUsername("@" + result.AccountInfo.Username);
    }
}

