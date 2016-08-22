using UnityEngine;
using System.Collections;
using PowerUps;
using System.Collections.Generic;

public class LevelIntroScroll : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed;

    [SerializeField]
    private Transform scrollBound;

    [SerializeField]
    private GameObject launcherGroup;

    [SerializeField]
    private List<GameObject> disableOnScroll;

    public void ScrollTo(float yPos)
    {
        var targetY = Mathf.Min(yPos, scrollBound.position.y);
        GameObjectUtil.DisableObjects(disableOnScroll);
        StartCoroutine(DoScroll(targetY));
    }

    private IEnumerator DoScroll(float targetY)
    {
        PowerUpController powerUpController = GetComponentInChildren<PowerUpController>();
        powerUpController.HidePowerUps();
        var finalLauncherPosition = launcherGroup.transform.localPosition + new Vector3(0f, targetY, 0f);
        while (transform.position.y > targetY)
        {
            transform.position = transform.position + new Vector3(0, -(scrollSpeed * Time.deltaTime), 0);
            launcherGroup.transform.position = finalLauncherPosition;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        launcherGroup.transform.position = finalLauncherPosition;

        GameObjectUtil.EnableObjects(disableOnScroll);
        powerUpController.ShowPowerUps();

        GlobalState.EventService.Dispatch(new IntroScrollCompleteEvent());
    }
}
