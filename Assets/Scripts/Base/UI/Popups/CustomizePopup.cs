using UnityEngine;
using System.Collections.Generic;

public interface CustomizePopup
{
    void CustomizeText(Dictionary<string, string> data);

    void CustomizeImages(Dictionary<string, Sprite> data);
}
