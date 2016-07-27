using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITextUpdater : TextUpdater
{
    protected Text text;

    virtual protected void Start()
    {
        text = GetComponent<Text>();
    }

    override protected void UpdateText(Observable target)
    {
        if ((text != null) && (target != null))
        {
            text.text = BuildString();
        }
    }
}
