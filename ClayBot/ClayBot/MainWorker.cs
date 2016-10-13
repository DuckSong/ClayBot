using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ClayBot
{
    class MainWorker
    {
        enum State
        {
            Unknown,
            Initial,
            Patcher,
            Patching,
            PatcherAgreement,
            Login,
            InvalidLogin,
            LoggingIn,
            Reconnect,
            Main,
            SelectGameMode,
            SelectedAram,
            JoinQueueFailed,
            LeaverBuster,
            InQueue,
            AcceptQueue,
            ChampionSelect,
            InGame,
            Result
        }

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

        private MainForm mainForm;
        private Dictionary<State, Transition> transitions;
        private bool mainLoop;
        private Transition currentTransition;
        
        public MainWorker(MainForm mainForm)
        {
            mainLoop = true;
            this.mainForm = mainForm;

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(mainForm.Config.LolLocale);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(mainForm.Config.LolLocale);

            currentTransition = new Transition(
                State.Unknown,
                5,
                () => { return true; },
                () => { },
                (Enum.GetValues(typeof(State)) as State[]).Where(x => x != State.Unknown).ToArray());

            transitions = new Dictionary<State, Transition>()
            {
                { State.Unknown, currentTransition }
            };
        }

        public void Quit()
        {
            mainLoop = false;
        }

        public void Work()
        {
            while (mainLoop)
            {
                currentTransition = currentTransition.Work(mainForm, transitions);
            }
        }
    }
}
