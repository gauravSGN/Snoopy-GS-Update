using System;

namespace LevelEditor.Menu
{
    abstract public class MenuWidgetBase
    {
        public event Action OnWidgetComplete;

        public LevelManipulator Manipulator { get; set; }

        protected void Complete()
        {
            if (OnWidgetComplete != null)
            {
                OnWidgetComplete();
            }
        }
    }
}
