using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TextMesh))]
public class TextMeshUpdater : MonoBehaviour
{
    public string format;
    public List<string> fields;

    protected TextMesh text;
    protected Observable target;

    public Observable Target
    {
        get { return target; }
        set
        {
            if (target != null)
            {
                target.RemoveListener(UpdateText);
            }

            target = value;

            if (target != null)
            {
                target.AddListener(UpdateText);
                UpdateText(target);
            }
        }
    }

    virtual protected void OnDestroy()
    {
        if (target != null)
        {
            target.RemoveListener(UpdateText);
        }
    }

    virtual protected void Start()
    {
        text = GetComponent<TextMesh>();
        UpdateText(Target);
    }

    virtual protected void UpdateText(Observable target)
    {
        if ((text != null) && (target != null))
        {
            var type = Target.GetType();
            var args = new List<string>();

            foreach (var fieldName in fields)
            {
                var field = type.GetField(fieldName);

                if (field != null)
                {
                    args.Add(field.GetValue(Target).ToString());
                }
                else
                {
                    var property = type.GetProperty(fieldName);

                    if (property != null)
                    {
                        args.Add(property.GetValue(Target, new object[] { }).ToString());
                    }
                    else
                    {
                        args.Add(null);
                    }
                }
            }

            text.text = string.Format(format, args.ToArray());
        }
    }
}
