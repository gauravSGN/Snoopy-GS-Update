using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace LevelEditor
{
    public class RandomBubbleWeight : MonoBehaviour
    {
        [SerializeField]
        private Text text;

        private InputField inputField;

        void Start()
        {
            inputField = GetComponent<InputField>();
        }
    }
}
