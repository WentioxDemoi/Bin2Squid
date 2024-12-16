using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    public InputField Email_, Username_, Password_;

    public GameObject MenuPanel_, RoomPanel_;

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
        }, error =>
        {
            Debug.Log("Error while SignIn : " + error.ErrorMessage);
        });
    }
}
