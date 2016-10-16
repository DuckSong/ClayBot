using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

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

        public Transition(State currentState, Func<bool> check, Action doWork, params State[] expectedStates)
        {
            this.currentState = currentState;
            this.check = check;
            this.doWork = doWork;
            this.expectedStates = expectedStates;
        }

        public Transition Work(MainForm mainForm, Dictionary<State, Transition> transitions)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(mainForm.Config.LolLocale);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(mainForm.Config.LolLocale);

            if (!check()) return transitions[State.Unknown];

            int numRetry = 0;

            while (numRetry < Static.RETRY)
            {
                mainForm.SetStatus(string.Format(Strings.Strings.Status, currentState, numRetry, mainForm.Config.LolLocale), currentState == State.Unknown);

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

                if (!check()) return transitions[State.Unknown];

                numRetry++;
            }

            if (currentState == State.Unknown)
            {
                foreach (string processName in Static.PROCESS_NAMES)
                {
                    foreach (Process p in Process.GetProcessesByName(processName))
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                }
            }

            return transitions[State.Unknown];
        }
    }
}
