using System.Collections;
using Photon.Pun;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class InGameManager : MonoBehaviourPun
{
    public GameObject blocsManagerPrefab;
    public Camera mainCamera;
    public GameObject hudManager;
    public GameObject winLoseCondition;
    private GameObject FirstBloc;

    int i = 0;

    // Initializes the game by creating the first set of blocks if the client is the master
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        for (i = 0; i < 10; i++)
        {
            if (i == 0)
            {
                GameObject firstBloc = PhotonNetwork.Instantiate(blocsManagerPrefab.name, new Vector3(0, 0, i * 4), Quaternion.identity);
                firstBloc.GetComponent<BlocsManager>().isFirst = true;
                photonView.RPC("SetBlocAsFirstRPC", RpcTarget.All, firstBloc.GetComponent<PhotonView>().ViewID);
            }
            else
                PhotonNetwork.Instantiate(blocsManagerPrefab.name, new Vector3(0, 0, i * 4), Quaternion.identity);
        }
        i--;
        StartCoroutine(ManageBlocs());
    }

    // Updates the UI hints based on block selection and checks for single player condition
    private void Update() {
        if (FirstBloc.GetComponent<BlocsManager>().isSelected) {
            hudManager.GetComponent<HudManager>().StopHintAnimation();
            hudManager.GetComponent<HudManager>().SetHint("Nice Choice !");
        }
        else {
            hudManager.GetComponent<HudManager>().AnimateHintText();
            hudManager.GetComponent<HudManager>().SetHint("Click on a tile to select it !");
        }
        if (PhotonNetwork.PlayerList.Length == 1) {
            StartCoroutine(WaitAndWin());
        }
    }

    // Randomly breaks either the left or right side of the current first block
    private void BreakBloc() {
        bool randomBool = Random.value > 0.5f;
        if (randomBool) {
            photonView.RPC("BreakBlocRPC", RpcTarget.All, 0);
        } else {
            photonView.RPC("BreakBlocRPC", RpcTarget.All, 1);
        }
    }

    // RPC function that handles the block breaking logic on all clients
    [PunRPC]
    public void BreakBlocRPC(int side) {
        if (side == 0) {
            FirstBloc.GetComponent<BlocsManager>().BlocLeftItem_.SetColor(Color.black);
            if (FirstBloc.GetComponent<BlocsManager>().BlocLeftItem_.selected) {
                if (FirstBloc.GetComponent<BlocsManager>().BlocRightItem_.playerCount == 0) {
                    StartCoroutine(WaitAndWin());
                } else {
                    StartCoroutine(WaitAndLose());
                }
            }
        } else {
            FirstBloc.GetComponent<BlocsManager>().BlocRightItem_.SetColor(Color.black);
            if (FirstBloc.GetComponent<BlocsManager>().BlocRightItem_.selected) {
                if (FirstBloc.GetComponent<BlocsManager>().BlocLeftItem_.playerCount == 0) {
                    StartCoroutine(WaitAndWin());
                } else {
                    StartCoroutine(WaitAndLose());
                }
            }
        }
    }

    // Triggers the win condition after a delay
    private IEnumerator WaitAndWin() {
        yield return new WaitForSeconds(3f);
        winLoseCondition.GetComponent<WinLoseCondition>().Win();
    }
    // Triggers the lose condition after a delay
    private IEnumerator WaitAndLose() {
        yield return new WaitForSeconds(3f);
        winLoseCondition.GetComponent<WinLoseCondition>().Lose();
    }

    // Main game loop that manages block timing, destruction, and creation
    IEnumerator ManageBlocs()
    {
        bool trigger;
        while (true)
        {
            trigger = false;
            for (int i = 20; i > 0; i--) {
                yield return new WaitForSeconds(1f);
                if (PhotonNetwork.IsMasterClient)
                    photonView.RPC("UpdateTimeLeftRPC", RpcTarget.All, i);
                if (FirstBloc.GetComponent<BlocsManager>().IsFull() && !trigger) {
                    i = 5;
                    trigger = true;
                }
            }
            photonView.RPC("BlockClickRPC", RpcTarget.All);
            if (PhotonNetwork.IsMasterClient) {
                yield return new WaitForSeconds(1f);
            BreakBloc();
            }
            yield return new WaitForSeconds(5f);

            if (PhotonNetwork.IsMasterClient)
            {
                GameObject[] blocs = GameObject.FindGameObjectsWithTag("BlocManager");
                GameObject closestBloc = null;

                if (blocs.Length > 0)
                {
                    closestBloc = blocs[0];
                    foreach (GameObject bloc in blocs)
                    {
                        if (bloc.transform.position.z < closestBloc.transform.position.z)
                        {
                            closestBloc = bloc;
                        }
                    }
                    PhotonNetwork.Destroy(closestBloc);
                }
                i++;

                PhotonNetwork.Instantiate(blocsManagerPrefab.name, new Vector3(0, 0, i * 4), Quaternion.identity);

                GameObject nextClosestBloc = null;
                float minZ = float.MaxValue;
                foreach (GameObject bloc in blocs)
                {
                    if (bloc != closestBloc && bloc.transform.position.z < minZ)
                    {
                        minZ = bloc.transform.position.z;
                        nextClosestBloc = bloc;
                    }
                }
                if (nextClosestBloc != null)
                {
                    nextClosestBloc.GetComponent<BlocsManager>().isFirst = true;
                    photonView.RPC("SetBlocAsFirstRPC", RpcTarget.All, nextClosestBloc.GetComponent<PhotonView>().ViewID);
                }

                photonView.RPC("MoveCameraRPC", RpcTarget.All);
            }
        }
    }

    // RPC function that disables clicking on the first block
    [PunRPC]
    private void BlockClickRPC() {
        FirstBloc.GetComponent<BlocsManager>().isClickable = false;
    }

    // RPC function that sets a block as the first block across all clients
    [PunRPC]
    private void SetBlocAsFirstRPC(int blocViewID)
    {
        FirstBloc = PhotonView.Find(blocViewID).gameObject;
        if (FirstBloc != null)
        {
            FirstBloc.GetComponent<BlocsManager>().isFirst = true;
        }
    }

    // RPC function that triggers camera movement across all clients
    [PunRPC]
    private void MoveCameraRPC()
    {
        StartCoroutine(MoveCamera());
    }

    // Smoothly moves the camera forward
    IEnumerator MoveCamera()
    {
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, 0, 4);
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = endPosition;
    }

    // RPC function that updates the time left display on all clients
    [PunRPC]
    private void UpdateTimeLeftRPC(int timeLeft)
    {
        hudManager.GetComponent<HudManager>().UpdateTimeLeft(timeLeft);
    }

    // Updates the player's status to offline in PlayFab when the game is closed
    private void OnApplicationQuit()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Status", "offline" },
            },
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request, null, null);
    }
}