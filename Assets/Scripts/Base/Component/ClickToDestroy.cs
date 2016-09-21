using UnityEngine;
using UnityEngine.UI;

sealed public class ClickToDestroy : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    public void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (target != null)
        {
            Destroy(target);
            target = null;
        }
    }
}
