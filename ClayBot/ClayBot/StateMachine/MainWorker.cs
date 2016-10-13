using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ClayBot.StateMachine
{
    class MainWorker
    {
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
