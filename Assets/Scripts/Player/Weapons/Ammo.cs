using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {

    [SerializeField] int currentAmmo = 50;

    public int GetCurrentAmount() { return currentAmmo; }

    public void ReduceCurrentAmmo() { currentAmmo--; }

}
