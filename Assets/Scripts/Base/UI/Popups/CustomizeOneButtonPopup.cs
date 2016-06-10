using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomizeOneButtonPopup : MonoBehaviour, CustomizePopup
{
    [SerializeField]
    private Text body;
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private Image mainImage;

    public void CustomizeText(Dictionary<string, string> data)
    {
        body.text = data["body"];
        buttonText.text = data["button_label"];
    }

    public void CustomizeImages(Dictionary<string, Sprite> data)
    {
        mainImage.sprite = data["main_image"];
    }
}
