using UnityEngine;
using System;
using Model;

namespace LevelEditor.Menu
{
    public interface MenuWidget
    {
        event Action OnWidgetComplete;

        int SortOrder { get; }
        LevelManipulator Manipulator { get; set; }

        bool IsValidFor(BubbleData bubble);
        GameObject CreateWidget(BubbleData bubble);
    }
}
