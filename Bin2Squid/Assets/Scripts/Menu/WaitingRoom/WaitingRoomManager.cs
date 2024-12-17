using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

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


    private void Start()
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
        ExitGames.Client.Photon.Hashtable roomState = new ExitGames.Client.Photon.Hashtable() { { "GameState", "in game" } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomState);
        photonView.RPC("RPC_StartGame", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_StartGame()
    {
        PhotonNetwork.LoadLevel("InGame");
    }
}