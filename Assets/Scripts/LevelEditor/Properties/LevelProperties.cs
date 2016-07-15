using UnityEngine;
using Model;

namespace LevelEditor.Properties
{
    sealed public class LevelProperties : Observable, PropertyProxy
    {
        public int ShotCount { get; set; }

        [PropertyDisplayAttribute(
            new string[] { "Star 1 Score", "Star 2 Score", "Star 3 Score" })]
        public int[] StarValues { get; set; }

        [PropertyDisplayAttribute(
            new string[] { "Blue PowerUp Fills", "Yellow PowerUp Fills", "Red PowerUp Fills", "Green PowerUp Fills", "Pink PowerUp Fills", "Purple PowerUp Fills" },
            new string[] { "3484d8", "ea9115", "d22d2b", "11952a", "e746a1", "891fc4" })]
        public int[] PowerUpFills { get; set; }

        public LevelProperties()
        {
            ShotCount = 20;
            StarValues = new int[] { 100, 500, 1000 };
            PowerUpFills = new int[6];
        }

        public void FromLevelData(LevelData data)
        {
            ShotCount = data.ShotCount;
            StarValues = data.StarValues;
            PowerUpFills = data.PowerUpFills;
        }

        public void ToLevelData(MutableLevelData data)
        {
            data.ShotCount = ShotCount;
            data.StarValues = StarValues;
            data.PowerUpFills = PowerUpFills;
        }
    }
}
