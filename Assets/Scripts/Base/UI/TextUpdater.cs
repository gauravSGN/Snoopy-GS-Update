using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour
{
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

    public string format;
    public List<string> fields;

    private Text text;
    private Observable target;

    protected virtual void Start()
    {
        text = GetComponent<Text>();
    }

    protected void UpdateText(Observable target)
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
