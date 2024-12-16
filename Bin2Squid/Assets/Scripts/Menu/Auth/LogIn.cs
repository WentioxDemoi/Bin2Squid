using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class LogIn : MonoBehaviour
{
    public InputField Email_, Password_;

    public GameObject MenuPanel_, RoomPanel_;

    public void LogIn_()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = Email_.text,
            Password = Password_.text
        };
        Email_.text = "";
        Password_.text = "";

        PlayFabClientAPI.LoginWithEmailAddress(request, result =>
        {
            Debug.Log("LogIn success");
            MenuPanel_.SetActive(false);
            RoomPanel_.SetActive(true);
        }, error =>
        {
            Debug.Log("Error while LogIn : " + error.ErrorMessage);
        });
    }

}
