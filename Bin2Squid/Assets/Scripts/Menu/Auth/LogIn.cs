using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using System;

public class LogIn : MonoBehaviour
{
    public InputField Email_, Password_;

    public GameObject MenuPanel_, RoomPanel_;

    // Vérifie au démarrage si l'utilisateur est déjà connecté et gère la connexion au lobby Photon
    void Start() {
        Debug.Log(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
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

    // Gère la connexion de l'utilisateur avec son email et mot de passe via PlayFab
    // Si les champs sont vides, utilise des identifiants par défaut
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

    // Vérifie si une chaîne de caractères correspond à un format d'email valide
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        // Expression régulière pour vérifier une adresse e-mail
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    // Envoie un email de récupération de mot de passe à l'adresse email spécifiée
    public void RecoveryPassword_() {
        if (IsValidEmail(Email_.text))
        {

            var request = new SendAccountRecoveryEmailRequest
            {
                Email = Email_.text,
                TitleId = PlayFabSettings.TitleId // Assurez-vous d'avoir configuré votre Title ID
            };

            PlayFabClientAPI.SendAccountRecoveryEmail(request, null, null);
        }
    }

}
