using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyComponent : MonoBehaviour
{
    public bool isEnergyZero;

    public int Energy;

    public int initialEnergy;

    public Action<int> OnEnergyChanged;

    public Action OnEnergyZero;

    public Action OnDecreaseEnergy;

    [SerializeField] float lastEnergyTime;
    [SerializeField] float energyLoopTime;

    private void Awake()
    {
        Energy = initialEnergy;
    }

    public void DecreseEnergy(int damage)
    {
        if (isEnergyZero)
            return;
        if (EnergyTimeFinished())
        {
            Energy -= damage;

            if (Energy <= 0)
            {
                Energy = 0;
                isEnergyZero = true;
                OnEnergyZero?.Invoke();
                Debug.LogError("Energy is Zero: " + this.gameObject.name);
            }
            else
            {
                OnDecreaseEnergy?.Invoke();
                isEnergyZero = false;
            }

            OnEnergyChanged?.Invoke(Energy);

            lastEnergyTime = Time.time;
        }
    }

    public void IncreaseEnergy(int addValue)
    {
        if (EnergyTimeFinished())
        {
            if (Energy + addValue > initialEnergy)
            {
                Energy = initialEnergy;

                lastEnergyTime = Time.time;
            }
            else
            {

                Debug.Log("2");
                Energy += addValue;

                lastEnergyTime = Time.time;
            }
            if (isEnergyZero)
            {
                isEnergyZero = false;
            }            
        }
    }

    public bool EnergyTimeFinished()
    {
        return Time.time - lastEnergyTime >= energyLoopTime;
    }
}
