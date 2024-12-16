
using UnityEngine;
using UnityEngine.UI;

public class Utils : MonoBehaviour
{
    public InputField Email_, Username_, Password_;

    public GameObject MenuPanel_, RoomPanel_;

    public void ClosePannelAfterAuth()
    {
        MenuPanel_.SetActive(false);
        RoomPanel_.SetActive(true);
    }
}
