using Photon.Pun;
using UnityEngine;

public class BlocsManager : MonoBehaviour
{
    public BlocItem BlocLeftItem_;
    public BlocItem BlocRightItem_;

    public bool isFirst = false;
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
        if (isFirst)
        {
            BlocLeftItem_.isClickable = true;
            BlocRightItem_.isClickable = true;
            BlocLeftItem_.CapacityText.gameObject.SetActive(true);
            BlocRightItem_.CapacityText.gameObject.SetActive(true);
        }
        if (BlocLeftItem_.selected)
            BlocRightItem_.selected = false;
        else if (BlocRightItem_.selected)
            BlocLeftItem_.selected = false;
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