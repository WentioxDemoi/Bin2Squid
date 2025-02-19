using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomDisplayManager : MonoBehaviourPunCallbacks
{
    public RoomItem roomItem;
    List<RoomItem> RoomItemList_ = new List<RoomItem>();
    public Transform ContentObject_;
    public float TimeUpdate_ = 5f;
    float NextUpdateTime_;

    public Text PrivateRoomName_;

    public void JoinPrivateRoom()
    {
        PhotonNetwork.JoinRoom(PrivateRoomName_.text);
    }

    public void JoinRoom(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> RoomList)
    {
        // Assurez-vous que la liste est mise à jour immédiatement
        foreach (RoomItem item in RoomItemList_)
        {
            Destroy(item.gameObject);
        }
        RoomItemList_.Clear();
        foreach (RoomInfo room in RoomList)
        {
            if (!room.RemovedFromList)
            {
                // Vérifie si la room est publique avant de l'afficher
                if (room.CustomProperties.TryGetValue("IsRoomPublic", out object isRoomPublic) && (bool)isRoomPublic)
                {
                    RoomItem newroom = Instantiate(roomItem, ContentObject_);
                    newroom.SetRoomName(room.Name);
                    newroom.SetRoomCapacity($"{room.PlayerCount}/{room.MaxPlayers}");
                    newroom.SetRoomLockState(true);

                    if (room.CustomProperties.TryGetValue("RoomAmountofMoney", out object roomAmountofMoney))
                    {
                        newroom.SetRoomCost(roomAmountofMoney.ToString() + "$");
                    }

                    RoomItemList_.Add(newroom);
                }
            }
        }
        Debug.Log("Room List Update");
    }
}
