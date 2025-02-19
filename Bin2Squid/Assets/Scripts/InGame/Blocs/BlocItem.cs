using Photon.Pun;
using UnityEngine;

public class BlocItem : MonoBehaviourPun
{
    public bool selected = false;
    public bool isClickable = false;
    public TextMesh CapacityText;

    private static BlocItem currentlySelectedBloc = null;
    public int playerCount = 0;

    // Initialisation du bloc au démarrage
    private void Start() {
    }

    // Initialise le texte de capacité avec 0 joueurs sur le nombre total de joueurs
    public void StartCapacityText() {
        CapacityText.text = 0 + "/" + PhotonNetwork.PlayerList.Length;
    }

    // Change la couleur du bloc
    public void SetColor(Color newColor) 
    {
        gameObject.GetComponent<Renderer>().material.color = newColor;
    }

    // Gère la sélection/désélection du bloc lors d'un clic de souris
    private void OnMouseDown()
    {
        if (!isClickable)
            return;

        if (currentlySelectedBloc != null && currentlySelectedBloc != this)
        {
            currentlySelectedBloc.Deselect();
        }

        selected = !selected;

        if (selected)
        {
            currentlySelectedBloc = this;
            transform.localScale += new Vector3(0, 0.1f, 0);

            string position = gameObject.name == "PlatformItemRight" ? "right" : "left";
            photonView.RPC("UpdateCapacityText", RpcTarget.All, 1, position);
        }
        else
        {
            currentlySelectedBloc = null;
            transform.localScale -= new Vector3(0, 0.1f, 0);

            string position = gameObject.name == "PlatformItemRight" ? "right" : "left";
            photonView.RPC("UpdateCapacityText", RpcTarget.All, -1, position);
        }
    }

    // Met à jour le texte de capacité du bloc en réseau
    [PunRPC]
    private void UpdateCapacityText(int change, string position)
    {
        string side = gameObject.name == "PlatformItemRight" ? "right" : "left";
        if (position == side) {
            playerCount += change;
            CapacityText.text = playerCount.ToString() + "/" + PhotonNetwork.PlayerList.Length;
        }
    }

    // Retourne le nombre de joueurs actuellement sur le bloc
    public int IsFull() {
        return playerCount;
    }

    // Désélectionne le bloc et met à jour son apparence
    private void Deselect()
    {
        selected = false;
        transform.localScale -= new Vector3(0, 0.1f, 0);
        string position = gameObject.name == "PlatformItemRight" ? "right" : "left";
        photonView.RPC("UpdateCapacityText", RpcTarget.All, -1, position);
    }
}