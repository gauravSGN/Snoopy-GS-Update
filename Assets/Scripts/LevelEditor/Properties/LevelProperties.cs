using UnityEngine;
using Model;

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

        public LevelProperties()
        {
            StarValues = new[] { 100, 500, 1000 };
            PowerUpFills = new float[6];
        }

        public void FromLevelData(LevelData data)
        {
            StarValues = data.StarValues;
            PowerUpFills = data.PowerUpFills;
        }

        public void ToLevelData(MutableLevelData data)
        {
            data.StarValues = StarValues;
            data.PowerUpFills = PowerUpFills;
        }
    }
}
