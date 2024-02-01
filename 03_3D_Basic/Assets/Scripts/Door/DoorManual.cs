using UnityEngine;

public class DoorManual : DoorBase, IInteracable
{
    public void Use()
    {
        Open();
    }
}