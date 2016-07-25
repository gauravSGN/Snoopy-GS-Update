using UnityEngine;
using UnityEngine.UI;
using LevelEditor.Manipulator;
using Model;
using Service;

namespace LevelEditor.Menu
{
    sealed public class DeleteBubbleWidget : MenuWidgetBase, MenuWidget
    {
        private const string PREFAB_PATH = "LevelEditor/Menu/DeleteBubbleButton";

        public int SortOrder { get { return 1; } }

        private GameObject prefab;
        private BubbleData data;
        private DeleteBubbleAction deleter = new DeleteBubbleAction();

        public bool IsValidFor(BubbleData bubble)
        {
            return bubble != null;
        }

        public GameObject CreateWidget(BubbleData bubble)
        {
            prefab = prefab ?? GlobalState.Instance.Services.Get<AssetService>().LoadAsset<GameObject>(PREFAB_PATH);
            data = bubble;

            var instance = GameObject.Instantiate(prefab);

            instance.GetComponent<Button>().onClick.AddListener(DeleteAction);

            return instance;
        }

        private void DeleteAction()
        {
            deleter.Perform(Manipulator, data.X, data.Y);

            Complete();
        }
    }
}
