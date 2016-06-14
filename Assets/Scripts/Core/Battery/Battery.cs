using UnityEngine;

public class Battery : MonoBehaviour
{
    public float totalCapacity;
    public int cells = 1;
    public bool startFull;

    private float currentCapacity;
    private ushort chargeBlockCount;

    //for segment bar drain
    private float[] divisionPoints;

    void Start()
    {
        chargeBlockCount = 0;
        if(startFull)
        {
            currentCapacity = totalCapacity;
        }
        else
        {
            currentCapacity = 0;
        }

        float divisionCapacity = totalCapacity / cells;
        divisionPoints = new float[cells];
        for(int i = 0; i < cells; ++i)
        {
            divisionPoints[i] = divisionCapacity * i;
        }
    }

    public bool HasEnergy()
    {
        return currentCapacity > 0;
    }

    public bool IsEmpty()
    {
        return currentCapacity <= 0;
    }

    public float PercentCharge()
    {
        return currentCapacity / totalCapacity;
    }

    public void Add(float amount)
    {
        if(chargeBlockCount > 0)
        {
            return;
        }
        currentCapacity += amount;
        currentCapacity = Mathf.Min(currentCapacity, totalCapacity);
    }

    public void Sub(float amount)
    {
        currentCapacity -= amount;
        currentCapacity = Mathf.Max(currentCapacity, 0);
    }

    public void SetCapacity(float amount)
    {
        if(amount >= 0 && amount <= totalCapacity)
        {
            currentCapacity = amount;
        }
    }
    // Returns 0 if equal, 1 if current > amount, -1 otherwise
    public int CompareCapacity(float amount)
    {
        int retVal;
        if(currentCapacity == amount)
        {
            retVal = 0;
        }
        else
        {
            retVal = currentCapacity > amount ? 1 : -1;
        }
        return retVal;
    }

    public void ChargeBlockerEnabled()
    {
        chargeBlockCount++;
    }

    public void ChargeBlockerDisabled()
    {
        chargeBlockCount--;
    }

    public float NextTargetEnergyLevel()
    {
        //safety check but should never be allowed to fire if you have no energy
        if(currentCapacity == 0)
        {
            return 0;
        }

        //already handled the 0 case so start at 1
        for(int i = 1; i < divisionPoints.Length; ++i)
        {
            if(currentCapacity <= divisionPoints[i])
            {
                //return the next smallest
                return divisionPoints[i-1];
            }
        }
        //base case return the second highest division
        return divisionPoints[divisionPoints.Length-1];
    }

}
