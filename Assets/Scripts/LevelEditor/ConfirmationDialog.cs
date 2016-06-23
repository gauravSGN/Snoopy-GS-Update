using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationDialog : MonoBehaviour
{
    [SerializeField]
    private Text title;

    [SerializeField]
    private Text body;

    [SerializeField]
    private Text confirmText;

    [SerializeField]
    private Text cancelText;

    public string Title
    {
        get { return title.text; }
        set { title.text = value; }
    }

    public string Body
    {
        get { return body.text; }
        set { body.text = value; }
    }

    public string ConfirmText
    {
        get { return confirmText.text; }
        set { confirmText.text = value; }
    }

    public string CancelText
    {
        get { return cancelText.text; }
        set { cancelText.text = value; }
    }

    public Action OnConfirm { get; set; }
    public Action OnCancel { get; set; }

    public void Confirm()
    {
        DispatchResult(OnConfirm);
    }

    public void Cancel()
    {
        DispatchResult(OnCancel);
    }

    private void DispatchResult(Action action)
    {
        Destroy(gameObject);

        if (action != null)
        {
            action.Invoke();
        }
    }
}
