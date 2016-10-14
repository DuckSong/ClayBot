using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ClayBot.StateMachine
{
    partial class MainWorker
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
                { State.Unknown, currentTransition },
                { State.Initial, new Transition(
                    State.Initial,
                    5,
                    () =>
                    {
                        int processCount = 0;

                        foreach (string processName in Static.PROCESS_NAMES)
                        {
                            processCount += Process.GetProcessesByName(processName).Count();
                        }

                        return processCount == 0;
                    },
                    () =>
                    {
                        new Process()
                        {
                            StartInfo = new ProcessStartInfo()
                            {
                                FileName = mainForm.Config.LolLauncherPath
                            }
                        }.Start();
                    },
                    new State[]
                    {
                        State.Patcher,
                        State.Patching,
                        State.PatcherAgreement
                    }) },
                { State.Patching, new Transition(
                    State.Patching,
                    5,
                    () =>
                    {
                        PatcherSize patcherSize;
                        if (!CheckPatcher(out patcherSize)) return false;

                        ActivateTargetWindow(Static.PATCHER_SIZES[patcherSize]);

                        return
                            CheckImageOnTargetWindow(
                                patcherSize == PatcherSize.Small ? Images.Patcher.Small.PatcherImages.PatcherIndicator : Images.Patcher.Large.PatcherImages.PatcherIndicator,
                                Static.PATCHER_RECTANGLES[PatcherRectangle.PatcherIndicator][patcherSize]) &&
                            CheckImageOnTargetWindow(
                                patcherSize == PatcherSize.Small ? Images.Patcher.Small.PatcherImages.Online : Images.Patcher.Large.PatcherImages.Online,
                                Static.PATCHER_RECTANGLES[PatcherRectangle.Online][patcherSize]) &&
                            !CheckImageOnTargetWindow(
                                patcherSize == PatcherSize.Small ? Images.Patcher.Small.PatcherImages.Launch : Images.Patcher.Large.PatcherImages.Launch,
                                Static.PATCHER_RECTANGLES[PatcherRectangle.Launch][patcherSize]);
                    },
                    () =>
                    {
                    },
                    new State[]
                    {
                        State.Patching,
                        State.Patcher
                    }) },
                { State.Patcher, new Transition(
                    State.Patcher,
                    5,
                    () =>
                    {
                        PatcherSize patcherSize;
                        if (!CheckPatcher(out patcherSize)) return false;

                        ActivateTargetWindow(Static.PATCHER_SIZES[patcherSize]);

                        return
                            CheckImageOnTargetWindow(
                                patcherSize == PatcherSize.Small ? Images.Patcher.Small.PatcherImages.PatcherIndicator : Images.Patcher.Large.PatcherImages.PatcherIndicator,
                                Static.PATCHER_RECTANGLES[PatcherRectangle.PatcherIndicator][patcherSize]) &&
                            CheckImageOnTargetWindow(
                                patcherSize == PatcherSize.Small ? Images.Patcher.Small.PatcherImages.Online : Images.Patcher.Large.PatcherImages.Online,
                                Static.PATCHER_RECTANGLES[PatcherRectangle.Online][patcherSize]) &&
                            CheckImageOnTargetWindow(
                                patcherSize == PatcherSize.Small ? Images.Patcher.Small.PatcherImages.Launch : Images.Patcher.Large.PatcherImages.Launch,
                                Static.PATCHER_RECTANGLES[PatcherRectangle.Launch][patcherSize]);
                    },
                    () =>
                    {
                        PatcherSize patcherSize;
                        CheckPatcher(out patcherSize);
                        ClickTargetWindowRectangle(Static.PATCHER_RECTANGLES[PatcherRectangle.Launch][patcherSize]);
                    },
                    new State[]
                    {
                        State.PatcherAgreement,
                        //State.Login
                    }) },
                { State.PatcherAgreement, new Transition(
                    State.PatcherAgreement,
                    5,
                    () =>
                    {
                        PatcherSize patcherSize;
                        if (!CheckPatcher(out patcherSize)) return false;

                        ActivateTargetWindow(Static.PATCHER_SIZES[patcherSize]);

                        return CheckImageOnTargetWindow(
                            patcherSize == PatcherSize.Small ? Images.Patcher.Small.PatcherImages.Accept : Images.Patcher.Large.PatcherImages.Accept,
                            Static.PATCHER_RECTANGLES[PatcherRectangle.Accept][patcherSize]);
                    },
                    () =>
                    {
                        PatcherSize patcherSize;
                        CheckPatcher(out patcherSize);
                        ClickTargetWindowRectangle(Static.PATCHER_RECTANGLES[PatcherRectangle.Accept][patcherSize]);
                    },
                    new State[]
                    {
                        State.PatcherAgreement,
                        //State.Login
                    }) }
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
