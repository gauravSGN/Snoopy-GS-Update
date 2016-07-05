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
            new string[] { "Blue PowerUp Fills", "Red PowerUp Fills", "Green PowerUp Fills", "Yellow PowerUp Fills", "Pink PowerUp Fills", "Purple PowerUp Fills" },
            new string[] { "95ecfd", "f2a5a3", "a1e699", "fbd575", "ff8080", "8000ff" })]
        public float[] PowerUpFills { get; set; }

        public LevelProperties()
        {
            StarValues = new int[3];
            PowerUpFills = new float[6];
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
