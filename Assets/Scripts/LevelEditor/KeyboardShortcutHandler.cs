using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace LevelEditor
{
    public class KeyboardShortcutHandler : MonoBehaviour
    {
        [Serializable]
        public sealed class ShortcutMapping
        {
            public KeyCode keyCode;
            public bool shift;
            public bool alt;
            public bool control;
            public bool command;
            public bool option;
            public UnityEvent action;
        };

        [SerializeField]
        private List<ShortcutMapping> mappings;

        public void Update()
        {
            foreach (var mapping in mappings)
            {
                if (Input.GetKeyUp(mapping.keyCode) && CheckModifiers(mapping))
                {
                    mapping.action.Invoke();
                }
            }
        }

        private bool CheckModifiers(ShortcutMapping mapping)
        {
            var result = true;

            result = result && (!mapping.shift || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
            result = result && (!mapping.alt || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));
            result = result && (!mapping.control || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
            result = result && (!mapping.command || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand));
            result = result && (!mapping.option || Input.GetKey(KeyCode.LeftApple) || Input.GetKey(KeyCode.RightApple));

            return result;
        }
    }
}
