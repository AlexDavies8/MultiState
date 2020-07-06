using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiState
{
    public class EnumerateState : IState
    {
        public bool Completed { get; private set; }

        protected Func<IEnumerator> _enumeratorGetter;
        private IEnumerator _enumerator;

        public EnumerateState(Func<IEnumerator> enumeratorGetter)
        {
            _enumeratorGetter = enumeratorGetter;
        }

        public void OnEnter()
        {
            _enumerator = _enumeratorGetter.Invoke();
            Completed = false;
        }

        public void Tick()
        {
            if (!_enumerator.MoveNext()) Completed = true;
        }

        public void OnExit() { }
    }
}
