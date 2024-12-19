using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class WaitingRoomManager : MonoBehaviour
{
    public Text RoomName_, RoomCapacity_, RoomCost_;

    public Transform ContentObject_;
    public PlayerItem playerItem;
    public PhotonView photonView;

    List<PlayerItem> PlayerItemList_ = new List<PlayerItem>();

    public GameObject StartGameButton_;

    public float TimeUpdate_ = 1f;
    float NextUpdateTime_;


    private void OnEnable()
    {
        GetRoomName();
        GetRoomCapacity();
        GetRoomCost();
        AddPlayerList();
    }

    private void GetRoomName()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            RoomName_.text = PhotonNetwork.CurrentRoom.Name;
        }
        else
        {
            RoomName_.text = "Salle inconnue"; // Valeur par défaut si la salle n'est pas trouvée
        }
    }

    private void GetRoomCapacity()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
            int currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
            RoomCapacity_.text = $"{currentPlayers}/{maxPlayers}";
        }
        else
        {
            RoomCapacity_.text = "Capacité de la salle inconnue"; // Valeur par défaut si la salle n'est pas trouvée
        }
    }

    private void GetRoomCost()
    {
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("RoomAmountofMoney"))
        {
            RoomCost_.text = $"${PhotonNetwork.CurrentRoom.CustomProperties["RoomAmountofMoney"]}";
        }
        else
        {
            RoomCost_.text = "Coût de la salle inconnu"; // Valeur par défaut si le coût n'est pas trouvé
        }
    }

    private void Update()
    {
        if (Time.time >= NextUpdateTime_)
        {
            DeletePlayerList();
            GetRoomCapacity();
            NextUpdateTime_ = Time.time + TimeUpdate_;
        }

        // Vérification des conditions pour afficher le bouton
        if (PhotonNetwork.CurrentRoom != null)
        {
            int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
            int currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

            if (currentPlayers == maxPlayers && PhotonNetwork.IsMasterClient)
            {
                StartGameButton_.SetActive(true);
            }
            else
            {
                StartGameButton_.SetActive(false);
            }
        }
    }

    public void DeletePlayerList()
    {
        foreach (PlayerItem Item in PlayerItemList_)
        {
            Destroy(Item.gameObject);
        }

        PlayerItemList_.Clear();
        AddPlayerList();
    }

    public void AddPlayerList()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        
        foreach (Photon.Realtime.Player player in players)
        {
            PlayerItem NewPlayer_ = Instantiate(playerItem, ContentObject_);
            NewPlayer_.SetUsername(player.NickName);
            
            PlayerItemList_.Add(NewPlayer_);
        }
    }

    public void StartGame()
    {
        photonView.RPC("UpdateMoney", RpcTarget.All);
        ExitGames.Client.Photon.Hashtable roomState = new ExitGames.Client.Photon.Hashtable() { { "GameState", "in game" } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomState);
        photonView.RPC("RPC_StartGame", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_StartGame()
    {
        PhotonNetwork.LoadLevel("InGame");
    }

    [PunRPC]
    private void UpdateMoney() {
        // Check if the custom property exists and is a string
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RoomAmountofMoney", out object roomCostObj) && roomCostObj is string roomCostStr) {
            if (int.TryParse(roomCostStr, out int roomCost)) {
                // Fetch the current money from PlayFab
                PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result => {
                    if (result.Data != null && result.Data.ContainsKey("Money")) {
                        int currentMoney = int.Parse(result.Data["Money"].Value);

                        // Subtract the room cost
                        int newMoney = currentMoney - roomCost;

                        // Update the money on PlayFab
                        var updateUserDataRequest = new UpdateUserDataRequest {
                            Data = new Dictionary<string, string> {
                                { "Money", newMoney.ToString() }
                            }
                        };

                        PlayFabClientAPI.UpdateUserData(updateUserDataRequest, updateResult => {
                            Debug.Log("Money updated successfully.");
                        }, error => {
                            Debug.LogError("Error updating money: " + error.GenerateErrorReport());
                        });
                    }
                }, error => {
                    Debug.LogError("Error fetching user data: " + error.GenerateErrorReport());
                });
            } else {
                Debug.LogError("RoomAmountofMoney is not a valid integer string.");
            }
        } else {
            Debug.LogError("RoomAmountofMoney property not found or is not a string.");
        }
    }

    public void Back() {
        PhotonNetwork.LeaveRoom();
    }
}
