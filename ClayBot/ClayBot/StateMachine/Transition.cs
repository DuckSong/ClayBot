using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClayBot.StateMachine
{
    class Transition
    {
        public Func<bool> Check
        {
            get { return check; }
        }

        private State currentState;
        private State[] expectedStates;
        private Func<bool> check;
        private Action doWork;
        private int retry;

        public Transition(State currentState, int retry, Func<bool> check, Action doWork, params State[] expectedStates)
        {
            this.currentState = currentState;
            this.retry = retry;
            this.check = check;
            this.doWork = doWork;
            this.expectedStates = expectedStates;
        }

        public Transition Work(MainForm mainForm, Dictionary<State, Transition> transitions)
        {
            if (!check()) return transitions[State.Unknown];

            int numRetry = 0;

            while (numRetry < retry)
            {
                mainForm.SetStatus(string.Format(Strings.Strings.Status, currentState, numRetry), currentState == State.Unknown);

                doWork();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (stopwatch.ElapsedMilliseconds < Static.TIMEOUT_CLIENT)
                {
                    foreach (State expectedState in expectedStates)
                    {
                        if (transitions[expectedState].Check()) return transitions[expectedState];
                    }
                }

                numRetry++;
            }

            return transitions[State.Unknown];
        }
    }
}
