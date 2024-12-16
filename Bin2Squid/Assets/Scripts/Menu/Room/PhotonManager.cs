using UnityEngine;
using Photon.Pun;

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

    public override void OnJoinedRoom()
    {
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
            }
        }
        else
        {
            Debug.Log("No Info on room state");
        }

        Debug.Log("Room joined : " + PhotonNetwork.CurrentRoom.Name);
    }
}
