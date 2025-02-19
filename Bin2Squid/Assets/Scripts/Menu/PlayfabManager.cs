using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    public ProfileManager ProfileManager_;

    // Démarre le processus pour récupérer les données de l'utilisateur depuis PlayFab
    private void Start()
    {
        // Récupération de l'argent de l'utilisateur
        var requestMoney = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(requestMoney, OnGetMoney, error => Debug.Log("GetMoneyError"));

        // Récupération du nom d'utilisateur
        var requestUsername = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(requestUsername, OnGetUsername, error => Debug.Log("GetUsernameError"));
    }

    // Callback pour traiter les données de l'argent de l'utilisateur
    private void OnGetMoney(GetUserDataResult result)
    {
        Debug.Log("Money: " + result.Data["Money"].Value);
        ProfileManager_.PutMoney("$ " + result.Data["Money"].Value);
    }

    // Callback pour traiter les informations du compte de l'utilisateur
    private void OnGetUsername(GetAccountInfoResult result)
    {
        Debug.Log("Username: " + result.AccountInfo.Username);
        ProfileManager_.PutUsername("@" + result.AccountInfo.Username);
    }
}

