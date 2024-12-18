using Photon.Pun;
using UnityEngine;

public class BlocItem : MonoBehaviourPun
{
    public bool selected = false;
    public bool isClickable = false;
    public TextMesh CapacityText;

    private static BlocItem currentlySelectedBloc = null;
    public int playerCount = 0;

    private void Start() {
    }

    public void StartCapacityText() {
        CapacityText.text = 0 + "/" + PhotonNetwork.PlayerList.Length;
    }

    public void SetColor(Color newColor) 
    {
        gameObject.GetComponent<Renderer>().material.color = newColor;
    }

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

    [PunRPC]
    private void UpdateCapacityText(int change, string position)
    {
        string side = gameObject.name == "PlatformItemRight" ? "right" : "left";
        if (position == side) {
            playerCount += change;
            CapacityText.text = playerCount.ToString() + "/" + PhotonNetwork.PlayerList.Length;
        }
    }

    public int IsFull() {
        return playerCount;
    }

    private void Deselect()
    {
        selected = false;
        transform.localScale -= new Vector3(0, 0.1f, 0);
        string position = gameObject.name == "PlatformItemRight" ? "right" : "left";
        photonView.RPC("UpdateCapacityText", RpcTarget.All, -1, position);
    }
}