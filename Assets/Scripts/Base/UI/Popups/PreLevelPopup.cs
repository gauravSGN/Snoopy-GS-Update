using Service;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Popup
{
    public class PreLevelPopup : Popup
    {
        private bool transitionOnClose;

        public void Transition()
        {
            transitionOnClose = true;
            Close();
        }

        override protected void OnCloseTweenComplete(AbstractGoTween tween)
        {
            base.OnCloseTweenComplete(tween);

            if (transitionOnClose)
            {
                SceneManager.LoadScene(StringConstants.Scenes.LEVEL);
            }
            else
            {
                GlobalState.Instance.Services.Get<SceneService>().Reset();
            }
        }
    }
}