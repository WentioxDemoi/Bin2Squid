using System.Collections;
using Photon.Pun;
using UnityEngine;

public class InGameManager : MonoBehaviourPun
{
    public GameObject blocsManagerPrefab;
    public Camera mainCamera;

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

    IEnumerator ManageBlocs()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
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
        GameObject bloc = PhotonView.Find(blocViewID).gameObject;
        if (bloc != null)
        {
            bloc.GetComponent<BlocsManager>().isFirst = true;
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
}