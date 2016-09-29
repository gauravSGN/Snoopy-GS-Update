using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
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

        private bool[] modifierState = new bool[5];
        private KeyCode[][] modifierMapping = new KeyCode[][]
        {
            new[] { KeyCode.LeftShift, KeyCode.RightShift },
            new[] { KeyCode.LeftAlt, KeyCode.RightAlt },
            new[] { KeyCode.LeftControl, KeyCode.RightControl },
            new[] { KeyCode.LeftCommand, KeyCode.RightCommand },
            new[] { KeyCode.LeftAlt, KeyCode.RightAlt },
        };

        private bool inputEnabled = true;

        public void Start()
        {
            GlobalState.EventService.Persistent.AddEventHandler<InputToggleEvent>(OnInputToggle);
        }

        public void OnDestroy()
        {
            GlobalState.EventService.RemoveEventHandler<InputToggleEvent>(OnInputToggle);
        }

        public void Update()
        {
            if (inputEnabled)
            {
                foreach (var mapping in mappings)
                {
                    if (Input.GetKeyUp(mapping.keyCode) && CheckModifiers(mapping))
                    {
                        mapping.action.Invoke();
                    }
                }
            }
        }

        private bool CheckModifiers(ShortcutMapping mapping)
        {
            var result = true;

            modifierState[0] = mapping.shift;
            modifierState[1] = mapping.alt;
            modifierState[2] = mapping.control;
            modifierState[3] = mapping.command;
            modifierState[4] = mapping.option;

            for (int index = 0, count = modifierState.Length; index < count; index++)
            {
                if (modifierState[index])
                {
                    result = result && modifierMapping[index].Aggregate(false, (a, c) => a || Input.GetKey(c));
                }
            }

            return result;
        }

        private void OnInputToggle(InputToggleEvent gameEvent)
        {
            inputEnabled = gameEvent.enabled;
        }
    }
}
