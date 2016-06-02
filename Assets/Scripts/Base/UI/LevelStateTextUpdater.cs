using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class LevelStateTextUpdater : MonoBehaviour
{
    public Level level;
    public string format;
    public string fieldName;

    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        level.LevelState.AddListener(UpdateState);
    }

    private void UpdateState(LevelState levelState)
    {
        var type = levelState.GetType();
        var field = type.GetField(fieldName);

        if (field != null)
        {
            var value = field.GetValue(levelState).ToString();

            text.text = string.Format(format, value);
        }
    }
}
