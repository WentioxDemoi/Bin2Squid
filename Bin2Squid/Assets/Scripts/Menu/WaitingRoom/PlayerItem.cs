using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    public Text Username_;

    public void SetUsername(string username)
    {
        Username_.text = username;
    }
}