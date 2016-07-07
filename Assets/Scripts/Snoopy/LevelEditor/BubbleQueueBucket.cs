using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Snoopy.Model;
using System.Linq;

namespace Snoopy.LevelEditor
{
    public class BubbleQueueBucket : MonoBehaviour
    {
        [SerializeField]
        private Text label;

        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private RectTransform elementContainer;

        [SerializeField]
        private Toggle mandatoryToggle;

        [SerializeField]
        private GameObject elementPrefab;

        private BubbleQueueDefinition.Bucket bucket;
        private List<BubbleQueueElement> elements = new List<BubbleQueueElement>();

        public void Start()
        {
            inputField.onEndEdit.AddListener(OnEndEdit);
            mandatoryToggle.onValueChanged.AddListener(OnMandatoryValueChanged);
        }

        public void Initialize(BubbleFactory factory, BubbleQueueDefinition.Bucket bucket)
        {
            this.bucket = bucket;
            int index = 0;

            foreach (var def in factory.Bubbles.Where(b => b.category == BubbleCategory.Basic))
            {
                var sprite = def.Prefab.GetComponentInChildren<SpriteRenderer>();

                if (sprite != null)
                {
                    var instance = Instantiate(elementPrefab);
                    var element = instance.GetComponent<BubbleQueueElement>();

                    instance.transform.SetParent(elementContainer, false);
                    element.Sprite = sprite.sprite;
                    element.Initialize(bucket, index);

                    elements.Add(element);
                    index++;
                }
            }
        }

        private void OnEndEdit(string text)
        {
            bucket.length = int.Parse(text);
        }

        private void OnMandatoryValueChanged(bool value)
        {
            bucket.mandatory = value;
        }
    }
}
