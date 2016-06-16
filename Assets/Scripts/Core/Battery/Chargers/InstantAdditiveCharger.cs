using UnityEngine;

public class InstantAdditiveCharger : MonoBehaviour
{
    [SerializeField]
    private Battery battery;

    [SerializeField]
    private float chargePercent;

    void OnEnable()
    {
        float amount = battery.totalCapacity * chargePercent;
        battery.Add(amount);
    }
}
