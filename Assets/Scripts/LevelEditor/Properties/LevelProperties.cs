using Model;
using System.Linq;
using System.Collections.Generic;

namespace LevelEditor.Properties
{
    sealed public class LevelProperties : Observable, PropertyProxy
    {
        [PropertyDisplayAttribute(
            new[] { "Blue PowerUp Fills", "Yellow PowerUp Fills", "Red PowerUp Fills",
                           "Green PowerUp Fills", "Pink PowerUp Fills", "Purple PowerUp Fills" },
            new[] { "3484d8", "ea9115", "d22d2b", "11952a", "e746a1", "891fc4" })]
        public float[] PowerUpFills { get; set; }

        [PropertyDisplayAttribute(
            new[] { "Star 1 Score", "Star 2 Score", "Star 3 Score" })]
        public int[] StarValues { get; set; }

        public float StarMultiplier
        {
            get { return starMultiplier; }
            set
            {
                var newValue = (value > 0.0f) ? value : starMultiplier;

                for (int index = 0, count = StarValues.Length; index < count; index++)
                {
                    StarValues[index] = (int)(StarValues[index] / starMultiplier * newValue);
                }

                starMultiplier = newValue;
                NotifyListeners();
            }
        }

        public bool EnableExtendedAimline { get; set; }

        private float starMultiplier = 1.0f;

        public LevelProperties()
        {
            StarValues = new[] { 100, 500, 1000 };
            PowerUpFills = new float[6];
        }

        public void FromLevelData(LevelData data)
        {
            StarValues = data.StarValues;
            starMultiplier = data.StarMultiplier;
            PowerUpFills = data.PowerUpFills;

            EnableExtendedAimline = (data.Modifiers != null) &&
                                    data.Modifiers.Any(m => m.Type == LevelModifierType.ExtendedAimline);
        }

        public void ToLevelData(MutableLevelData data)
        {
            data.StarValues = StarValues;
            data.StarMultiplier = starMultiplier;
            data.PowerUpFills = PowerUpFills;

            // TODO: We should probably create a real system for managing modifiers once we have more than this one.
            var modifiers = new List<LevelModifierData>();

            if (EnableExtendedAimline)
            {
                modifiers.Add(new LevelModifierData(LevelModifierType.ExtendedAimline, null));
            }

            data.Modifiers = modifiers.ToArray();
        }
    }
}
