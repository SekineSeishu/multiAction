using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomConnectionHandler
{
    public class ConnectionContainer
    {
        public NetworkRunner Runner;
        public ConnectionCallbacks Callback;
        public ConnectionData ActiveConnection;
        public App App;
        public bool IsValid => Runner != default;
        public bool IsRunning => IsValid && Runner.IsRunning;
    }
}
