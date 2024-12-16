using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public InputField RoomName_, RoomNumberOfPlayers_, RoomAmountofMoney_;
    private bool IsRoomPublic_ = true;

    public Button PublicButton_, PrivateButton_;

    private void Start()
    {
        RoomNumberOfPlayers_.text = "2";
        PublicButton_.transform.localScale *= 1.2f;

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
    }

    public void MinusButton()
    {
        int numberOfPlayers = int.Parse(RoomNumberOfPlayers_.text);
        numberOfPlayers--;
        RoomNumberOfPlayers_.text = numberOfPlayers.ToString();
    }




    public void CreateRoom()
    {
        if (int.TryParse(RoomNumberOfPlayers_.text, out int numberOfPlayers) && numberOfPlayers >= 2 &&
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
                roomOptions.CustomRoomPropertiesForLobby = new string[] { "mapSeed", "Time", "GameState", "MaxPlayers" };

                PhotonNetwork.CreateRoom(RoomName_.text, roomOptions);
            }
        }
    }

}