using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Fusion;

namespace FusionUtilsEvents
{
    [CreateAssetMenu]
    public class FusionEvent : ScriptableObject
    {
        public List<Action<PlayerRef, NetworkRunner>> Responses = new List<Action<PlayerRef, NetworkRunner>>();

        public void Raise(PlayerRef player = default, NetworkRunner runner = null)
        {
            for (int i = 0; i < Responses.Count; i++)
            {
                Responses[i].Invoke(player, runner);
            }
        }

        public void RegisterResponse(Action<PlayerRef, NetworkRunner> response)
        {
            Responses.Add(response);
        }
        
        public void RemoveResponse(Action<PlayerRef, NetworkRunner> response)
        {
            if (Responses.Contains(response))
                Responses.Remove(response);
        }
    }
}
