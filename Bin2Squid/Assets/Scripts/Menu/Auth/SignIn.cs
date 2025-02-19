using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;

public class SignIn : MonoBehaviour
{
    public InputField Email_, Username_, Password_;

    public GameObject MenuPanel_, RoomPanel_;


    /// Gère l'inscription d'un nouvel utilisateur dans PlayFab.
    /// - Enregistre l'utilisateur avec son email, nom d'utilisateur et mot de passe
    /// - Définit le surnom Photon pour le multijoueur
    /// - Initialise le compte avec 100 de monnaie (temporaire)
    public void SignIn_()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = Email_.text,
            Username = Username_.text.ToLower(),
            Password = Password_.text,
            RequireBothUsernameAndEmail = false
        };
        Username_.text = "";
        Password_.text = "";
        Email_.text = "";

        PlayFabClientAPI.RegisterPlayFabUser(request, result =>
        {
            Debug.Log("SignIn success");
            MenuPanel_.SetActive(false);
            RoomPanel_.SetActive(true);

            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), accountInfoResult =>
            {
                var playerName = accountInfoResult.AccountInfo.Username;
                PhotonNetwork.NickName = playerName;
                Debug.Log("Player nickname set to: " + PhotonNetwork.NickName);
            }, accountInfoError =>
            {
                Debug.Log("Error retrieving account info: " + accountInfoError.ErrorMessage);
            });
            //A supprimer lorsque le serveur sera prêt
            var updateUserDataRequest = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { "Money", "100" }
                }
            };

            PlayFabClientAPI.UpdateUserData(updateUserDataRequest, updateResult =>
            {
                Debug.Log("Money updated successfully.");
            }, updateError =>
            {
                Debug.Log("Error updating Money: " + updateError.ErrorMessage);
            });
        }, error =>
        {
            Debug.Log("Error while SignIn : " + error.ErrorMessage);
        });
    }
}
