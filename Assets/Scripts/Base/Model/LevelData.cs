using System.Collections.Generic;
using System.Xml.Serialization;
using Goal;

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

        [XmlAttribute("normalBubbleSubType")]
        public int contentType;

        [XmlIgnore]
        public Bubble model;
    }

    [XmlAttribute("remainingBubble")]
    public int remainingBubble;

    [XmlElement("Bubble")]
    public List<BubbleData> bubbles;

    [XmlIgnore]
    public List<LevelGoal> goals;

    [XmlAttribute("bombFill")]
    public float bombFill;

    [XmlAttribute("horzFill")]
    public float horzFill;

    [XmlAttribute("snakeFill")]
    public float snakeFill;

    [XmlAttribute("fireFill")]
    public float fireFill;
}
