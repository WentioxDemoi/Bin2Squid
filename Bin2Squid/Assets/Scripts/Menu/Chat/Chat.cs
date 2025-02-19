using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    // Start is called before the first frame update

    public Text Message;
    public Text Name;

    // Initialise le chat en le plaçant au premier plan dans la hiérarchie des éléments UI
    void Start()
    {
        GetComponent<RectTransform>().SetAsFirstSibling();
    }

    // Fonction appelée à chaque frame pour mettre à jour le chat
    // Actuellement vide car aucune mise à jour n'est nécessaire
    void Update()
    {
        
    }
}
