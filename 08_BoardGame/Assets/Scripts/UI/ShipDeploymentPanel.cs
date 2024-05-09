using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeploymentPanel : MonoBehaviour
{
    DeploymentToggle[] toggles;

    private void Awake()
    {
        toggles = GetComponentsInChildren<DeploymentToggle>();
        foreach (DeploymentToggle toggle in toggles)
        {
            toggle.onSelect += UnSelectOthers;
        }
    }

    private void UnSelectOthers(DeploymentToggle self)
    {
        foreach(DeploymentToggle toggle in toggles)
        {
            if(toggle != self)
            {
                toggle.SetNotSelect();
            }
        }
    }
}
