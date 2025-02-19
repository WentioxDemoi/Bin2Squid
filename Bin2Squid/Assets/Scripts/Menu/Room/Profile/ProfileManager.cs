using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Text Username_, Money_;

    /// Met à jour le texte affichant le nom d'utilisateur
    public void PutUsername(string username)
    {
        Username_.text = username;
    }

    /// Met à jour le texte affichant le montant d'argent
    public void PutMoney(string money)
    {
        Money_.text = money;
    }
}