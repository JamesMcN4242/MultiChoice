////////////////////////////////////////////////////////////
/////   HostState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class HostState : FlowStateBase
{    protected override bool AquireUIFromScene()
    {
        m_ui = Object.FindObjectOfType<HostUI>();
        return m_ui != null;
    }
}
