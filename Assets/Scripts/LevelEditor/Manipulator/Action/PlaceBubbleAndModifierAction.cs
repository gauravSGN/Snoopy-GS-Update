namespace LevelEditor.Manipulator
{
    [ManipulatorAction(ManipulatorActionType.PlaceBubbleAndModifier)]
    public class PlaceBubbleAndModifierAction : ManipulatorAction
    {
        private readonly PlaceBubbleAction bubblePlacer = new PlaceBubbleAction();
        private readonly PlaceModifierAction modifierPlacer = new PlaceModifierAction();

        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            manipulator.PushState();

            bubblePlacer.Perform(manipulator, x, y);
            modifierPlacer.Perform(manipulator, x, y);

            manipulator.PopState();
        }

        public void PerformAlternate(LevelManipulator manipulator, int x, int y)
        {
            Perform(manipulator, x, y);
        }
    }
}
