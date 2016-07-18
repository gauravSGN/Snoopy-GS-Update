using UnityEngine;
using UnityEngine.UI;

public class OneButtonPopup : Popup
{
    [SerializeField]
    private Text bodyText;

    [SerializeField]
    private Text buttonText;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Image mainImage;

    public void Setup(OneButtonPopupConfig config)
    {
        bodyText.text = config.BodyText;
        buttonText.text = config.ButtonText;
        titleText.text = config.TitleText;
        mainImage.sprite = config.MainImage;
    }
}
