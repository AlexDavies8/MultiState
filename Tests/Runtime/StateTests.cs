using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MultiState;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class StateTests
    {
        const int RunCount = 5;

        [UnityTest]
        public IEnumerator TestSleepState()
        {
            for (int i = 0; i < RunCount; i++)
            {
                yield return TestSleepState(UnityEngine.Random.Range(0.2f, 0.5f));
            }
        }

        IEnumerator TestSleepState(float duration)
        {
            bool running = true;

            float previousTimestamp = Time.time;

            float sleepTime = UnityEngine.Random.Range(0.2f, 0.5f);

            var assertState = new CommandState(() => Assert.LessOrEqual(Mathf.Abs(Time.time - previousTimestamp - sleepTime), 0.05f));
            var sleepState = new EnumerateState(StateEnumerators.WaitForSeconds(sleepTime));
            var endState = new CommandState(() => running = false);

            StateMachine stateMachine = new StateMachine();

            stateMachine.AddTransition(sleepState, assertState, () => sleepState.Completed);
            stateMachine.AddTransition(assertState, endState, () => true);

            stateMachine.SetState(sleepState);

            yield return TestStateMachine(stateMachine, () => running);
        }

        [UnityTest]
        public IEnumerator TestCommandState()
        {
            for (int i = 0; i < RunCount; i++)
            {
                yield return TestCommandStateInternal();
            }
        }

        IEnumerator TestCommandStateInternal()
        {
            int stateIndex = 0;

            List<CommandState> states = new List<CommandState>();
            
            for (int i = 0; i < 5; i++)
            {
                int _i = i;
                states.Add(new CommandState(() =>
                {
                    Assert.IsTrue(_i == stateIndex, $"State {_i} was run with index {stateIndex}");
                    stateIndex++;
                }));
            }

            StateMachine stateMachine = new StateMachine();

            for (int i = 0; i < states.Count - 1; i++)
            {
                int _i = i;
                stateMachine.AddTransition(states[i], states[i + 1], () => true);
            }

            CommandState startState = new CommandState(null);
            stateMachine.AddTransition(startState, states[0], () => true);

            bool running = true;
            CommandState endState = new CommandState(() => running = false);
            stateMachine.AddTransition(states[states.Count-1], endState, () => true);

            stateMachine.SetState(startState);

            yield return TestStateMachine(stateMachine, () => running);
        }

        IEnumerator TestStateMachine(StateMachine stateMachine, Func<bool> predicate)
        {
            while (predicate.Invoke())
            {
                stateMachine.Tick();

                yield return null;
            }
        }
    }
}
