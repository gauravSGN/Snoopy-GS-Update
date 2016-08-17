using State;
using UnityEngine;

public class HeartsTextUpdater : UITextUpdater
{
    [SerializeField]
    private string textWhenFull;

    override protected void Start()
    {
        base.Start();

        if (GlobalState.User != null)
        {
            Target = GlobalState.User.purchasables.hearts;
        }
    }

    override protected void UpdateText(Observable target)
    {
        if (target != null)
        {
            var hearts = (Hearts)target;

            if (hearts.quantity == GlobalState.Instance.Config.purchasables.maxHearts)
            {
                text.text = textWhenFull;
            }
            else
            {
                base.UpdateText(target);
            }
        }
    }
}
