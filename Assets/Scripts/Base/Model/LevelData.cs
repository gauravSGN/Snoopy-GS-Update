using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Level")]
public class LevelData
{
    public class BubbleData
    {
        [XmlAttribute("grid_x")]
        public int x;

        [XmlAttribute("grid_y")]
        public int y;

        [XmlAttribute("typeID")]
        public int typeID;
    }

    [XmlAttribute("remainingBubble")]
    public int remainingBubble;

    [XmlElement("Bubble")]
    public List<BubbleData> bubbles;
}
