using UnityEngine;
using System.Collections.Generic;

abstract public class TextUpdater : MonoBehaviour
{
    public string format;
    public List<string> fields;

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

    abstract protected void UpdateText(Observable target);

    protected string BuildString()
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
        return string.Format(format, args.ToArray());
    }
}
