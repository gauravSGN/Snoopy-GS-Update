using UnityEngine;
using UnityEngine.UI;

public class StarActivator : MonoBehaviour
{
    [SerializeField]
    private Sprite activeStar;

    public void ActivateStar()
    {
        var star = transform.parent.parent.GetComponent<Image>();

        star.sprite = activeStar;
        star.SetNativeSize();
    }
}
