using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class TextMeshUpdater : TextUpdater
{
    protected TextMesh text;

    virtual protected void Start()
    {
        text = GetComponent<TextMesh>();
    }

    override protected void UpdateText(Observable target)
    {
        if ((text != null) && (target != null))
        {
            text.text = BuildString();
        }
    }
}
