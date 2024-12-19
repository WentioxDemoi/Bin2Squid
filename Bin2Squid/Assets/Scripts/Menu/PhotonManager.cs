using UnityEngine;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public GameObject RoomPanel_, WaitingRoomPanel_;
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
         PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        RoomPanel_.SetActive(false);
        WaitingRoomPanel_.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        

        // Vérification du coût de la salle
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RoomAmountofMoney", out object roomCostString))
        {
            if (int.TryParse(roomCostString.ToString(), out int roomCost))
            {
                PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
                {
                    int playerBalance = 0;
                    if (result.Data != null && result.Data.ContainsKey("Money"))
                    {
                        playerBalance = int.Parse(result.Data["Money"].Value);
                    }

                    if (playerBalance < roomCost)
                    {
                        Debug.Log("Not enough balance to join the room!");
                        PhotonNetwork.LeaveRoom();
                        return;
                    }
                }, error =>
                {
                    Debug.LogError("Error retrieving user data: " + error.ErrorMessage);
                });
            }
            else
            {
                Debug.Log("Invalid room cost format");
                PhotonNetwork.LeaveRoom();
                return;
            }
        }
        else
        {
            Debug.Log("Room cost not found");
            PhotonNetwork.LeaveRoom();
            return;
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("GameState", out object gameState))
        {
            if (gameState.ToString() == "en attente")
            {
                RoomPanel_.SetActive(false);
                WaitingRoomPanel_.SetActive(true);
            }
            else
            {
                Debug.Log("This room is already in game !");
                PhotonNetwork.LeaveRoom();
                return;
            }
        }
        else
        {
            Debug.Log("No Info on room state");
            return;
        }

        Debug.Log("Room joined : " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnLeftRoom() {
        RoomPanel_.SetActive(true);
        WaitingRoomPanel_.SetActive(false);
    }
}
