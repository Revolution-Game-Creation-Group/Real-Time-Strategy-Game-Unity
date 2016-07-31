﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSkeleton : UnitHero
{
    protected override void Start()
    {
        base.Start();
    }

    public override void PerformCommand(Command command)
    {
        if (isDead)
            return;

        base.PerformCommand(command);
    }
}
