using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Reflection;
#endif

[CreateAssetMenu(fileName = "NewEvent", menuName = "Scriptable/Event")]
public class ScriptableEvent : ScriptableObject
{
    public event Action OnEvent;

    public void LaunchEvent()
    {
        OnEvent?.Invoke();
    }

#if UNITY_EDITOR
    private bool TryGetMethodsInfo(out MethodInfo[] infos)
    {
        if (OnEvent?.GetInvocationList().Length > 0)
        {
            List<MethodInfo> listInfos = new List<MethodInfo>();

            foreach (var delegateInvoke in OnEvent.GetInvocationList())
            {
                listInfos.Add(delegateInvoke.Method);
            }

            infos = listInfos.ToArray();

            return true;
        }
        else
        {
            infos = new MethodInfo[0];

            return false;
        }
    }

    public bool TryGetEventMethodsNames(out string[] names)
    {
        List<string> strings = new List<string>();
        MethodInfo[] infos;

        bool getting = TryGetMethodsInfo(out infos);

        if (getting)
        {
            foreach (MethodInfo info in infos)
            {
                strings.Add(info.Name);
            }

        }

        names = strings.ToArray();

        return getting;
    }
#endif

}