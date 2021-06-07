using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetection
{
    void Detect(out Volt_Tile closetTile, WhatIsTarget whatIsTarget);
}
