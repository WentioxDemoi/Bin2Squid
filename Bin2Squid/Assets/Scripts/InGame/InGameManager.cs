using System.Collections;
using Photon.Pun;
using UnityEngine;

public class InGameManager : MonoBehaviourPun
{
    public GameObject blocsManagerPrefab;
    public Camera mainCamera;
    public GameObject hudManager;
    private GameObject FirstBloc;

    int i = 0;

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

    private void Update() {
        if (FirstBloc.GetComponent<BlocsManager>().isSelected) {
            hudManager.GetComponent<HudManager>().StopHintAnimation();
            hudManager.GetComponent<HudManager>().SetHint("Nice Choice !");
        }
        else {
            hudManager.GetComponent<HudManager>().AnimateHintText();
            hudManager.GetComponent<HudManager>().SetHint("Click on a tile to select it !");
        }
    }

    private void BreakBloc() {
        bool randomBool = Random.value > 0.5f;
        if (randomBool) {
            photonView.RPC("BreakBlocRPC", RpcTarget.All, 0);
        } else {
            photonView.RPC("BreakBlocRPC", RpcTarget.All, 1);
        }
    }

    [PunRPC]
    public void BreakBlocRPC(int side) {
        if (side == 0) {
            FirstBloc.GetComponent<BlocsManager>().BlocLeftItem_.SetColor(Color.black);
        } else {
            FirstBloc.GetComponent<BlocsManager>().BlocRightItem_.SetColor(Color.black);
        }
    }

    IEnumerator ManageBlocs()
    {
        bool trigger = false;
        while (true)
        {
            for (int i = 20; i > 0; i--) {
                yield return new WaitForSeconds(1f);
                if (PhotonNetwork.IsMasterClient)
                    photonView.RPC("UpdateTimeLeftRPC", RpcTarget.All, i);
                if (FirstBloc.GetComponent<BlocsManager>().IsFull() && !trigger) {
                    i = 5;
                    trigger = true;
                }
            }
            if (PhotonNetwork.IsMasterClient) {
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

    [PunRPC]
    private void SetBlocAsFirstRPC(int blocViewID)
    {
        FirstBloc = PhotonView.Find(blocViewID).gameObject;
        if (FirstBloc != null)
        {
            FirstBloc.GetComponent<BlocsManager>().isFirst = true;
        }
    }

    [PunRPC]
    private void MoveCameraRPC()
    {
        StartCoroutine(MoveCamera());
    }

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

    [PunRPC]
    private void UpdateTimeLeftRPC(int timeLeft)
    {
        hudManager.GetComponent<HudManager>().UpdateTimeLeft(timeLeft);
    }
}