using UnityEngine;
using UnityEngine.UI;
using LevelEditor.Manipulator;
using Model;

namespace LevelEditor.Menu
{
    sealed public class DeleteBubbleWidget : MenuWidgetBase, MenuWidget
    {
        public int SortOrder { get { return 1; } }

        private GameObject prefab;
        private BubbleData data;
        private DeleteBubbleAction deleter = new DeleteBubbleAction();

        public DeleteBubbleWidget()
        {
            prefab = Resources.Load("LevelEditor/Menu/DeleteBubbleButton", typeof(GameObject)) as GameObject;
        }

        public bool IsValidFor(BubbleData bubble)
        {
            return bubble != null;
        }

        public GameObject CreateWidget(BubbleData bubble)
        {
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
