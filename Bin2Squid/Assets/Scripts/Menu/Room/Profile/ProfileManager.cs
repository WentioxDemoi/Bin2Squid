using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Text Username_, Money_;
    public void PutUsername(string username)
    {
        Username_.text = username;
    }

    public void PutMoney(string money)
    {
        Money_.text = money;
    }
}
