using UnityEngine;
using FSM;
using System.Collections.Generic;

public class ActivateObjects : StateComponent
{
    [SerializeField]
    private List<GameObject> objects;

    public override void OnEnter()
    {
        for(int i = 0; i < objects.Count; ++i)
        {
            objects[i].SetActive(true);
        }
    }

    public override void OnExit()
    {
        for(int i = 0; i < objects.Count; ++i)
        {
            objects[i].SetActive(false);
        }
    }
}
