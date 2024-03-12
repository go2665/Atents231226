using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Transform weaponParent;
    Transform shiledParent;

    private void Awake()
    {
        Transform child = transform.GetChild(2);    // root
        child = child.GetChild(0);                  // pelvis
        child = child.GetChild(0);                  // spine1
        child = child.GetChild(0);                  // spine2

        Transform spine3 = child.GetChild(0);       // spine3
        weaponParent = spine3.GetChild(2);          // clavicle_r
        weaponParent = weaponParent.GetChild(1);    // upperarm_r
        weaponParent = weaponParent.GetChild(0);    // lowerarm_r
        weaponParent = weaponParent.GetChild(0);    // hand_r
        weaponParent = weaponParent.GetChild(2);    // weapon_r

        shiledParent = spine3.GetChild(1);          // clavicle_l
        shiledParent = shiledParent.GetChild(1);    // upperarm_l
        shiledParent = shiledParent.GetChild(0);    // lowerarm_l
        shiledParent = shiledParent.GetChild(0);    // hand_l
        shiledParent = shiledParent.GetChild(2);    // weapon_l
    }

    public void ShowWeaponAndShield(bool isShow = true)
    {
        weaponParent.gameObject.SetActive(isShow);
        shiledParent.gameObject.SetActive(isShow);
    }
}
