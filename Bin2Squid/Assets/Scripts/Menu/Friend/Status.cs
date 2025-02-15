using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class Status : MonoBehaviour
{

    public void Start()
    {
        SaveStatus("online");
    }

    public void SaveStatus(string status)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Status", status },
            },
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request, null, null);
    }

    private void OnApplicationQuit()
    {
        SaveStatus("offline");
    }
}