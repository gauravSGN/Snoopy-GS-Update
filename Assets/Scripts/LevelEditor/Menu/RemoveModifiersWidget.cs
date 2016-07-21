using UnityEngine;
using UnityEngine.UI;
using LevelEditor.Manipulator;
using Model;

namespace LevelEditor.Menu
{
    sealed public class RemoveModifiersWidget : MenuWidgetBase, MenuWidget
    {
        public int SortOrder { get { return 2; } }

        private GameObject prefab;
        private BubbleData data;
        private PlaceBubbleAction placer = new PlaceBubbleAction();

        public RemoveModifiersWidget()
        {
            prefab = Resources.Load("LevelEditor/Menu/RemoveModifiersButton", typeof(GameObject)) as GameObject;
        }

        public bool IsValidFor(BubbleData bubble)
        {
            return (bubble != null) && (bubble.modifiers != null) && (bubble.modifiers.Length > 0);
        }

        public GameObject CreateWidget(BubbleData bubble)
        {
            data = bubble;

            var instance = GameObject.Instantiate(prefab);

            instance.GetComponent<Button>().onClick.AddListener(RemoveAction);

            return instance;
        }

        private void RemoveAction()
        {
            data.modifiers = null;

            PerformNonvolatileAction(() => {
                Manipulator.SetBubbleType(data.Type);
                placer.Perform(Manipulator, data.X, data.Y);
            });

            Complete();
        }
    }
}
