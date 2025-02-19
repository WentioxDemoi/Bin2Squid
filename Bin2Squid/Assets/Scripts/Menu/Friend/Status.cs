using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class Status : MonoBehaviour
{
    // Définit le statut du joueur comme "en ligne" au démarrage du jeu
    public void Start()
    {
        SaveStatus("online");
    }

    // Sauvegarde le statut du joueur (en ligne/hors ligne) dans PlayFab
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

    // Définit le statut du joueur comme "hors ligne" quand le jeu se ferme
    private void OnApplicationQuit()
    {
        SaveStatus("offline");
    }
}