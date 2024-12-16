using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text RoomName_, RoomCapacity_, RoomCost_;

    public GameObject LockOpen_, LockClosed_;

    public RoomDisplayManager roomDisplayManager;

    public void SetRoomName(string RoomName)
    {
        RoomName_.text = RoomName;
    }

    public void SetRoomCapacity(string capacity)
    {
        RoomCapacity_.text = capacity;
    }

    public void SetRoomCost(string cost)
    {
        RoomCost_.text = cost;
    }

    public void SetRoomLockState(bool isOpen)
    {
        LockOpen_.SetActive(isOpen);
        LockClosed_.SetActive(!isOpen);
    }

    public void OnClickRoomItem()
    {
        roomDisplayManager.JoinRoom(RoomName_.text);
    }

}
