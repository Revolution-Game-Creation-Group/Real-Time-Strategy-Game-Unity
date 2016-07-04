﻿using UnityEngine;
using System.Collections.Generic;

public class Data : MonoBehaviour
{
    public GameObject UnitsContainer;

    private Unit[] units;
    private List<Unit> unitList;

    private float lastUpdate;
    private readonly float updateInterval = 0.1f;

    void Start() { Instance = this; UpdateUnits(); }
    private static Data Instance;
    public static Data GetInstance()
    {
        return Instance;
    }

    public void UpdateUnits()
    {
        units = UnitsContainer.GetComponentsInChildren<Unit>();
        unitList = new List<Unit>(units);
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
        units = unitList.ToArray();
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
        units = unitList.ToArray();
    }

    public Unit[] GetAllUnits()
    {
        // Should never ever return null units inside!!!!!
        return units; 
    }
}