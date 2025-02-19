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

    // Initialise le HUD en mettant à jour le nombre de joueurs et en démarrant l'animation du texte d'indice
    void Start() {
        UpdatePlayerCountText();
        AnimateHintText();
    }

    // Appelé automatiquement quand un joueur quitte la partie, met à jour le compteur de joueurs
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCountText();
    }

    // Met à jour l'affichage du temps restant
    public void UpdateTimeLeft(int timeLeft) {
        TimeLeft.text = timeLeft.ToString() + "s";
    }

    // Met à jour le texte affichant le nombre de joueurs dans la partie
    private void UpdatePlayerCountText()
    {
        playerCountText.text = PhotonNetwork.PlayerList.Length.ToString();
    }

    // Définit le texte d'indice à afficher
    public void SetHint(string hint) {
        Hint.text = hint;
    }

    // Démarre l'animation du texte d'indice
    public void AnimateHintText()
    {
        StartCoroutine(ScaleHintText());
    }

    // Arrête l'animation du texte d'indice
    public void StopHintAnimation()
    {
        stopAnimation = true;
    }

    // Coroutine qui gère l'animation de mise à l'échelle du texte d'indice
    // Fait un effet de pulsation en agrandissant et rétrécissant le texte en continu
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
