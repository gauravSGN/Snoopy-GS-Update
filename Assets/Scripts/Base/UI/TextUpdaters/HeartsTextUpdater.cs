using State;
using Service;
using UnityEngine;

public class HeartsTextUpdater : TextUpdater
{
    [SerializeField]
    private string textWhenFull;

    override protected void Start()
    {
        base.Start();

        var user = GlobalState.Instance.Services.Get<UserStateService>();

        if (user != null)
        {
            Target = user.purchasables.hearts;
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
