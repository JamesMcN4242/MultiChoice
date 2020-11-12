////////////////////////////////////////////////////////////
/////   AppDirector.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class AppDirector : LocalDirector
{
    [RuntimeInitializeOnLoadMethod]
    private static void CreateAppDirector()
    {
        GameObject _ = new GameObject("AppDirector", typeof(AppDirector));
    }

    public void Start()
    {
        m_stateController.PushState(new MenuState());
    }
}
