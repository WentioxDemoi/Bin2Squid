using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;

public class LogIn : MonoBehaviour
{
    public InputField Email_, Password_;

    public GameObject MenuPanel_, RoomPanel_;

    void Start() {
        if (PlayFabClientAPI.IsClientLoggedIn() && PhotonNetwork.IsConnected) { 
            if (PhotonNetwork.InRoom) {
                PhotonNetwork.LeaveRoom();
            }
            MenuPanel_.SetActive(false);
            RoomPanel_.SetActive(true);
        }
        if (PhotonNetwork.InLobby) {
            Debug.Log("Vous êtes dans un lobby.");
        } else {
            Debug.Log("Vous n'êtes pas dans un lobby.");
            PhotonNetwork.JoinLobby();
        }       
    }

    public void LogIn_()
    {
        LoginWithEmailAddressRequest request = null;
        if (Email_.text == " " || Password_.text == "") {
            request = new LoginWithEmailAddressRequest
            {
                Email = "okay@gmail.com",//Email_.text,
                Password = "okay999"//Password_.text
            };
        }
        else {
            request = new LoginWithEmailAddressRequest
            {
                Email = Email_.text,
                Password = Password_.text
            };
        }
        Email_.text = "";
        Password_.text = "";

        PlayFabClientAPI.LoginWithEmailAddress(request, result =>
        {
            Debug.Log("LogIn success");
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
        }, error =>
        {
            Debug.Log("Error while LogIn : " + error.ErrorMessage);
        });
    }

}
