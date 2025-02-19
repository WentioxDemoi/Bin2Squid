using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public Text Username_;

    public GameObject ExcludeButton_;

    private void Start()
    {
        PhotonNetwork.EnableCloseConnection = true;
    }

    // Sets the username text for the player item.
    public void SetUsername(string username)
    {
        Username_.text = username;
    }

    // Toggles the visibility of the ExcludeButton if the current client is the MasterClient.
    public void DisplayPlayerManager()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (ExcludeButton_.activeSelf)
            {
                ExcludeButton_.SetActive(false);
            }
            else
            {
                ExcludeButton_.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Not able to display PlayerManager because not MasterClient !");
        }
    }

    // Excludes a player from the room by closing their connection.
    public void ExcludePlayer()
    {
        PhotonNetwork.CloseConnection(GetPlayerByName());
    }

    // Retrieves a Player object from the PhotonNetwork.PlayerList based on the username.
    public Player GetPlayerByName()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == Username_.text)
            {
                return player;
            }
        }
        Debug.Log("Problème avec GetPlayerByName");
        return null;
    }
}