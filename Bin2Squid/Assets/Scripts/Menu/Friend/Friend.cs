using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{
    // Start is called before the first frame update
    public Text usernameText;

    FriendsManager manager;
    public GameObject Disconnected;
    public GameObject Connected;

    // Initialise le composant en trouvant le gestionnaire d'amis dans la scène
    public void Start()
    {
        manager = FindObjectOfType<FriendsManager>();
    }

    // Supprime l'ami de la liste en appelant la fonction correspondante du gestionnaire
    public void OnClickDelete()
    {
        manager.OnClickRemoveButton(usernameText.text);
    }

    // Définit le nom d'utilisateur affiché pour cet ami
    public void SetUsername(string username)
    {
        usernameText.text = username;
    }

    // Met à jour l'état de connexion de l'ami (en ligne/hors ligne)
    // et affiche l'icône correspondante
    public void SetState(string state)
    {
        if (state == "online") {
            Disconnected.SetActive(false);
            Connected.SetActive(true);
        } else {
            Disconnected.SetActive(true);
            Connected.SetActive(false);
        }
    }
}
