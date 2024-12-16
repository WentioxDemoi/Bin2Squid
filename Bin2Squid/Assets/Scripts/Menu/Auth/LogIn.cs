using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections.Generic;

public class LogIn : MonoBehaviour
{
    public InputField Email_, Password_;

    public GameObject MenuPanel_, RoomPanel_;

    public void LogIn_()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = "okay@gmail.com",//Email_.text,
            Password = "okay999"//Password_.text
        };
        Email_.text = "";
        Password_.text = "";

        PlayFabClientAPI.LoginWithEmailAddress(request, result =>
        {
            Debug.Log("LogIn success");
            MenuPanel_.SetActive(false);
            RoomPanel_.SetActive(true);

            //A supprimer lorsque le serveur sera prêt
            var updateUserDataRequest = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { "Money", "10" }
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
            Debug.Log("Error while LogIn : " + error.ErrorMessage);
        });
    }

}
