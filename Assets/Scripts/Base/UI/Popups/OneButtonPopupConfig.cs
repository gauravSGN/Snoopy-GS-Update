using UnityEngine;

public class OneButtonPopupConfig : PopupConfig
{
    [SerializeField]
    private string bodyText;

    [SerializeField]
    private string buttonText;

    [SerializeField]
    private string titleText;

    [SerializeField]
    private Sprite mainImage;

    public string BodyText { get { return bodyText; } }

    public string ButtonText { get { return buttonText; } }

    public string TitleText { get { return titleText; } }

    public Sprite MainImage { get { return mainImage; } }

}
