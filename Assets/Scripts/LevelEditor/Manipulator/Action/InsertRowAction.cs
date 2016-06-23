using System.Linq;
using Model;
using UnityEngine;

namespace LevelEditor.Manipulator
{
    [ManipulatorAction(ManipulatorActionType.InsertRow)]
    public class InsertRowAction : ManipulatorAction
    {
        public Sprite ButtonSprite
        {
            get { return Resources.Load("Textures/UI/InsertRowButton", typeof(Sprite)) as Sprite; }
        }

        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            ShiftRows(manipulator, y & ~1, 2);
        }

        public void PerformAlternate(LevelManipulator manipulator, int x, int y)
        {
            int baseRow = y & ~1;

            RemoveRows(manipulator, baseRow, baseRow + 1);
            ShiftRows(manipulator, baseRow + 2, -2);
        }

        private void RemoveRows(LevelManipulator manipulator, params int[] rows)
        {
            var rowList = rows.ToList();
            var models = manipulator.Models;
            var views = manipulator.Views;
            var deleteList = models.Where(p => rowList.Contains(p.Value.Y)).Select(p => p.Value.Key).ToArray();

            foreach (var key in deleteList)
            {
                GameObject.Destroy(views[key]);

                models.Remove(key);
                views.Remove(key);
            }
        }

        private void ShiftRows(LevelManipulator manipulator, int start, int delta)
        {
            var models = manipulator.Models;
            var views = manipulator.Views;
            var moveList = models.Where(p => p.Value.Y >= start).Select(p => p.Value).ToArray();
            var viewList = moveList.Select(m => views[m.Key]).ToArray();

            // Remove all of the models we're moving before we try to shift anything around so we don't have collisions.
            foreach (var model in moveList)
            {
                models.Remove(model.Key);
                views.Remove(model.Key);
            }

            for (var index = 0; index < moveList.Length; index++)
            {
                var model = moveList[index];
                var view = viewList[index];

                var newModel = new LevelData.BubbleData(model.X, model.Y + delta, model.Type, model.ContentType);
                view.transform.localPosition = PlaceBubbleAction.GetBubbleLocation(newModel.X, newModel.Y);

                models[newModel.Key] = newModel;
                views[newModel.Key] = view;
            }
        }
    }
}
