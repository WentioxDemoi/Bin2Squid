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

    private void Start()
    {
        RoomNumberOfPlayers_.text = "1";
        RoomAmountofMoney_.text = "1";
        PublicButton_.transform.localScale *= 1.2f;

        RoomAmountofMoney_.onEndEdit.AddListener(delegate { ValidateRoomAmount(); });
        RoomNumberOfPlayers_.onEndEdit.AddListener(delegate { ValidateRoomNumberOfPlayers(); });
    }

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

    public void PublicButton()
    {
        if (IsRoomPublic_ != true)
        {
            PublicButton_.transform.localScale *= 1.2f;
            PrivateButton_.transform.localScale /= 1.2f;
            IsRoomPublic_ = true;
        }
    }

    public void PrivateButton()
    {
        if (IsRoomPublic_ != false)
        {
            PrivateButton_.transform.localScale *= 1.2f;
            PublicButton_.transform.localScale /= 1.2f;
            IsRoomPublic_ = false;
        }
    }

    public void PlusButton()
    {
        int numberOfPlayers = int.Parse(RoomNumberOfPlayers_.text);
        numberOfPlayers++;
        RoomNumberOfPlayers_.text = numberOfPlayers.ToString();
        ValidateRoomNumberOfPlayers();
    }

    public void MinusButton()
    {
        int numberOfPlayers = int.Parse(RoomNumberOfPlayers_.text);
        numberOfPlayers--;
        RoomNumberOfPlayers_.text = numberOfPlayers.ToString();
        ValidateRoomNumberOfPlayers();
    }

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