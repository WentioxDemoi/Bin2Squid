using UnityEngine;
using Photon.Pun;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class WinLoseCondition : MonoBehaviour
{
    public GameObject WinPanel;
    public GameObject LosePanel;

    bool trigger = false;

    public void Win() {
        if (trigger) return;
        trigger = true;
        WinPanel.SetActive(true);

        // Parse the room cost from string to int
        int roomCost = int.Parse((string)PhotonNetwork.CurrentRoom.CustomProperties["RoomAmountofMoney"]);
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        int currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        
        // Calculer le coût par joueur
        int costPerPlayer = (roomCost * maxPlayers) / currentPlayers;

        // Mettre à jour l'argent du joueur sur PlayFab
        UpdatePlayerCurrency(costPerPlayer);
        
        StartCoroutine(WinRoutine());

        // Récupérer le coût de la room depuis les paramètres de la room
        

        PhotonNetwork.LeaveRoom();
    }

    private void UpdatePlayerCurrency(int amount) {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            int currentMoney = 0;
            if (result.Data != null && result.Data.ContainsKey("Money"))
            {
                currentMoney = int.Parse(result.Data["Money"].Value);
            }

            int newMoney = currentMoney + amount;

            var updateUserDataRequest = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { "Money", newMoney.ToString() }
                }
            };

            PlayFabClientAPI.UpdateUserData(updateUserDataRequest, updateResult =>
            {
                Debug.Log("Money updated successfully. New balance: " + newMoney);
            }, updateError =>
            {
                Debug.LogError("Error updating Money: " + updateError.ErrorMessage);
            });

        }, error =>
        {
            Debug.LogError("Error retrieving user data: " + error.ErrorMessage);
        });
    }

    private IEnumerator WinRoutine() {
        
        yield return new WaitForSeconds(2f);
        
    }

    public void Lose() {
        if (trigger) return;
        trigger = true;
        PhotonNetwork.LeaveRoom();
        LosePanel.SetActive(true);
    }

    public void Back() {
        PhotonNetwork.LoadLevel("Menu");
    }
}
