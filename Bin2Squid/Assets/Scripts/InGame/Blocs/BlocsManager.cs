using Photon.Pun;
using UnityEngine;

public class BlocsManager : MonoBehaviour
{
    public BlocItem BlocLeftItem_;
    public BlocItem BlocRightItem_;

    public bool isFirst = false;

    public bool isSelected = false;
    private bool hasInitialized = false;

    private void Start()
    {
        Color leftColor = GetRandomColor();
        Color rightColor;
        do
        {
            rightColor = GetRandomColor();
        } while (rightColor == leftColor);

        BlocLeftItem_.SetColor(leftColor);
        BlocRightItem_.SetColor(rightColor);
    }

    private void Update()
    {
        if (isFirst && !hasInitialized)
        {
            BlocLeftItem_.isClickable = true;
            BlocRightItem_.isClickable = true;
            BlocLeftItem_.CapacityText.gameObject.SetActive(true);
            BlocRightItem_.CapacityText.gameObject.SetActive(true);
            BlocLeftItem_.StartCapacityText();
            BlocRightItem_.StartCapacityText();
            hasInitialized = true;
        }

        if (BlocLeftItem_.selected || BlocRightItem_.selected)
            isSelected = true;
        else
            isSelected = false;

        if (BlocLeftItem_.selected)
            BlocRightItem_.selected = false;
        else if (BlocRightItem_.selected)
            BlocLeftItem_.selected = false;
    }

    public bool IsFull() {
        if (BlocLeftItem_.IsFull() + BlocRightItem_.IsFull() == PhotonNetwork.PlayerList.Length) {
            return true;
        }
        return false;
    }

    private Color GetRandomColor()
    {
        int randomIndex = Random.Range(0, 3);
        switch (randomIndex)
        {
            case 0:
                return Color.red;
            case 1:
                return Color.green;
            case 2:
                return Color.blue;
            default:
                return Color.white;
        }
    }

    

}