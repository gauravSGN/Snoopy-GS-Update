using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
#if (DEBUG)
    [SerializeField]
    private float updateDelay;

    private float wallTime;
    private float delayTimer;
    private int frames;

    private string text;

    private GUIStyle style;
#endif

    public void Start()
    {
#if !(DEBUG)
        Destroy(gameObject);
#else
        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = Screen.height * 2 / 100;
        style.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
#endif
    }

#if (DEBUG)
    public void Update()
    {
        var deltaTime = Time.realtimeSinceStartup - wallTime;
        wallTime = Time.realtimeSinceStartup;
        frames++;
        delayTimer += deltaTime;

        if (delayTimer >= updateDelay)
        {
            var msec = (delayTimer * 1000.0f) / frames;
            var fps = frames / delayTimer;

            text = string.Format("{0:0.0} ms ({1:0.0} fps)", msec, fps);

            delayTimer = 0;
            frames = 0;
        }
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 2 / 100), text, style);
    }
#endif
}