using UnityEngine;

public class InstantCharger : MonoBehaviour
{
    [SerializeField]
    private Battery battery;

    [SerializeField]
    private float chargePercent;

    void OnEnable()
    {
        float amount = battery.totalCapacity * chargePercent;
        battery.SetCapacity(amount);
    }
}
