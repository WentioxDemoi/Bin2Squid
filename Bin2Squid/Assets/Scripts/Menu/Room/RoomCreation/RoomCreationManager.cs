using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomCreationManager : MonoBehaviour
{
    public InputField RoomName_, RoomNumberOfPlayers_, RoomAmountofMoney_;
    private bool IsRoomPublic_ = true;
    public Button PublicButton_, PrivateButton_;
    public ProfileManager profileManager;

    // Initialise les valeurs par défaut et configure les listeners pour les champs de saisie
    private void Start()
    {
        RoomNumberOfPlayers_.text = "1";
        RoomAmountofMoney_.text = "1";
        PublicButton_.transform.localScale *= 1.2f;

        RoomAmountofMoney_.onEndEdit.AddListener(delegate { ValidateRoomAmount(); });
        RoomNumberOfPlayers_.onEndEdit.AddListener(delegate { ValidateRoomNumberOfPlayers(); });
    }

    // Vérifie que le montant de la room ne dépasse pas l'argent du joueur et est positif
    private void ValidateRoomAmount()
    {
        if (float.TryParse(RoomAmountofMoney_.text, out float amountOfMoney))
        {
            float playerMoney = float.Parse(profileManager.Money_.text.Replace("$ ", ""));
            if (amountOfMoney > playerMoney)
            {
                RoomAmountofMoney_.text = playerMoney.ToString();
            } else if (amountOfMoney <= 0) {
                RoomAmountofMoney_.text = "1";
            }
        }
    }

    // Vérifie que le nombre de joueurs est entre 1 et 50
    private void ValidateRoomNumberOfPlayers()
    {
        if (int.TryParse(RoomNumberOfPlayers_.text, out int numberOfPlayers))
        {
            if (numberOfPlayers > 50)
            {
                RoomNumberOfPlayers_.text = "50";
            }
            else if (numberOfPlayers < 1)
            {
                RoomNumberOfPlayers_.text = "1";
            }
        }
    }

    // Active le mode public pour la room
    public void PublicButton()
    {
        if (IsRoomPublic_ != true)
        {
            PublicButton_.transform.localScale *= 1.2f;
            PrivateButton_.transform.localScale /= 1.2f;
            IsRoomPublic_ = true;
        }
    }

    // Active le mode privé pour la room
    public void PrivateButton()
    {
        if (IsRoomPublic_ != false)
        {
            PrivateButton_.transform.localScale *= 1.2f;
            PublicButton_.transform.localScale /= 1.2f;
            IsRoomPublic_ = false;
        }
    }

    // Augmente le nombre de joueurs de 1
    public void PlusButton()
    {
        int numberOfPlayers = int.Parse(RoomNumberOfPlayers_.text);
        numberOfPlayers++;
        RoomNumberOfPlayers_.text = numberOfPlayers.ToString();
        ValidateRoomNumberOfPlayers();
    }

    // Diminue le nombre de joueurs de 1
    public void MinusButton()
    {
        int numberOfPlayers = int.Parse(RoomNumberOfPlayers_.text);
        numberOfPlayers--;
        RoomNumberOfPlayers_.text = numberOfPlayers.ToString();
        ValidateRoomNumberOfPlayers();
    }

    // Crée une nouvelle room avec les paramètres spécifiés
    public void CreateRoom()
    {
        if (int.TryParse(RoomNumberOfPlayers_.text, out int numberOfPlayers) && numberOfPlayers >= 1 &&
            int.TryParse(RoomAmountofMoney_.text, out int amountOfMoney) && amountOfMoney > 0 &&
            !string.IsNullOrEmpty(RoomName_.text))
        {
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = (byte)int.Parse(RoomNumberOfPlayers_.text) };
            roomOptions.CleanupCacheOnLeave = false;

            if (RoomName_.text.Length >= 1)
            {
                ExitGames.Client.Photon.Hashtable options = new ExitGames.Client.Photon.Hashtable()
            {
                { "GameState", "en attente" },
                { "MaxPlayers", int.Parse(RoomNumberOfPlayers_.text) },
                { "IsRoomPublic", IsRoomPublic_ },
                { "RoomAmountofMoney", RoomAmountofMoney_.text },
            };

                roomOptions.CustomRoomProperties = options;
                roomOptions.CustomRoomPropertiesForLobby = new string[] {"GameState", "MaxPlayers", "IsRoomPublic", "RoomAmountofMoney" };

                PhotonNetwork.CreateRoom(RoomName_.text, roomOptions);
            }
        }
    }

}