using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomizeOneButtonPopup : MonoBehaviour, CustomizePopup
{
    [SerializeField]
    private Text bodyText;
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Image mainImage;

    public void CustomizeText(Dictionary<string, string> data)
    {
        bodyText.text = data["body"];
        buttonText.text = data["button_label"];
        titleText.text = data["title"];
    }

    public void CustomizeImages(Dictionary<string, Sprite> data)
    {
        mainImage.sprite = data["main_image"];
    }
}
