using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiState
{
    public class CommandState : IState
    {
        private Action _command;

        public CommandState(Action command)
        {
            _command = command;
        }

        public void OnEnter() { }

        public void Tick()
        {
            _command?.Invoke();
        }

        public void OnExit() { }
    }
}
