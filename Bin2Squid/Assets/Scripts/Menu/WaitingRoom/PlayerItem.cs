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

    public void SetUsername(string username)
    {
        Username_.text = username;
    }

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

    public void ExcludePlayer()
    {
        PhotonNetwork.CloseConnection(GetPlayerByName());
    }

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