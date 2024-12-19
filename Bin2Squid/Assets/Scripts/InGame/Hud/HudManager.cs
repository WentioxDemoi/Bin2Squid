using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class HudManager : MonoBehaviourPunCallbacks
{
    public Text playerCountText;
    public Text Hint;
    public Text TimeLeft;

    private bool stopAnimation = false;

    void Start() {
        UpdatePlayerCountText();
        AnimateHintText();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCountText();
    }
    public void UpdateTimeLeft(int timeLeft) {
        TimeLeft.text = timeLeft.ToString() + "s";
    }

    private void UpdatePlayerCountText()
    {
        playerCountText.text = PhotonNetwork.PlayerList.Length.ToString();
    }

    public void SetHint(string hint) {
        Hint.text = hint;
    }

    public void AnimateHintText()
    {
        StartCoroutine(ScaleHintText());
    }

    public void StopHintAnimation()
    {
        stopAnimation = true;
    }

    private IEnumerator ScaleHintText()
    {
        float duration = 3.0f;
        Vector3 originalScale = Hint.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        while (true)
        {
            if (stopAnimation) {
                stopAnimation = false;
                yield break;
            }

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                if (stopAnimation) {
                stopAnimation = false;
                yield break;
            }
                Hint.transform.localScale = Vector3.Lerp(originalScale, targetScale, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                if (stopAnimation) {
                stopAnimation = false;
                yield break;
            }
                Hint.transform.localScale = Vector3.Lerp(targetScale, originalScale, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
