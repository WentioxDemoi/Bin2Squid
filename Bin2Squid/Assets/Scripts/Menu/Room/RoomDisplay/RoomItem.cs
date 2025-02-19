using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text RoomName_, RoomCapacity_, RoomCost_;

    public GameObject LockOpen_, LockClosed_;

    public RoomDisplayManager roomDisplayManager;

    // Sets the room name text in the UI
    public void SetRoomName(string RoomName)
    {
        RoomName_.text = RoomName;
    }

    // Sets the room capacity text in the UI
    public void SetRoomCapacity(string capacity)
    {
        RoomCapacity_.text = capacity;
    }

    // Sets the room cost text in the UI
    public void SetRoomCost(string cost)
    {
        RoomCost_.text = cost;
    }

    // Updates the lock state of the room, showing either open or closed lock
    public void SetRoomLockState(bool isOpen)
    {
        LockOpen_.SetActive(isOpen);
        LockClosed_.SetActive(!isOpen);
    }

    // Handles the event when the room item is clicked, joining the room
    public void OnClickRoomItem()
    {
        roomDisplayManager.JoinRoom(RoomName_.text);
    }

}
