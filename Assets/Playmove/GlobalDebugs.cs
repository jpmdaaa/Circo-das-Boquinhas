using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDebugs : StaticInstance<GlobalDebugs>
{
    private bool debugTimer = false;
    private bool autoMetrics = false;
    private bool debugMetrics = false;
    private bool debugWebRequests = false;
    
    public bool DebugTimer
    {
        get => debugTimer;
        //set => debugTimer = value;
    }
    
    public bool AutoMetrics
    {
        get => autoMetrics;
        //set => autoMetrics = value;
    }
    
    public bool DebugMetrics
    {
        get => debugMetrics;
        //set => debugMetrics = value;
    }
    
    public bool DebugWebRequests
    {
        get => debugWebRequests;
        //set => debugWebRequests = value;
    }
}
