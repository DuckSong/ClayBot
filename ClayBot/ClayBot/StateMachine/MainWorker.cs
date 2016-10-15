using ClayBot.Images.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ClayBot.StateMachine
{
    partial class MainWorker
    {
        private MainForm mainForm;
        private Dictionary<State, Transition> transitions;
        private Transition currentTransition;
        
        public MainWorker(MainForm mainForm)
        {
            this.mainForm = mainForm;

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(mainForm.Config.LolLocale);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(mainForm.Config.LolLocale);

            currentTransition = new Transition(
                State.Unknown,
                () => { return true; },
                () => { },
                (Enum.GetValues(typeof(State)) as State[]).Where(x => x != State.Unknown).ToArray());

            transitions = new Dictionary<State, Transition>()
            {
                #region Unknown State
                { State.Unknown, currentTransition },
                #endregion
                
                #region Initial State
                { State.Initial, new Transition(
                    State.Initial,
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
                #endregion

                #region Patching State
                { State.Patching, new Transition(
                    State.Patching,
                    () =>
                    {
                        PatcherSize patcherSize;
                        if (!CheckPatcher(out patcherSize)) return false;

                        ActivateTargetWindow(Static.PATCHER_SIZES[patcherSize]);

                        return
                            ValidatePatcher(new PatcherValidation[]
                            {
                                new PatcherValidation(
                                    true,
                                    PatcherRectangle.PatcherIndicator,
                                    patcherSize,
                                    false),
                                new PatcherValidation(
                                    true,
                                    PatcherRectangle.Online,
                                    patcherSize,
                                    false),
                                new PatcherValidation(
                                    false,
                                    PatcherRectangle.Launch,
                                    patcherSize,
                                    false)
                            });
                    },
                    () =>
                    {
                    },
                    new State[]
                    {
                        State.Patching,
                        State.Patcher
                    }) },
                #endregion

                #region Patcher State
                { State.Patcher, new Transition(
                    State.Patcher,
                    () =>
                    {
                        PatcherSize patcherSize;
                        if (!CheckPatcher(out patcherSize)) return false;

                        ActivateTargetWindow(Static.PATCHER_SIZES[patcherSize]);

                        return ValidatePatcher(new PatcherValidation[]
                        {
                            new PatcherValidation(
                                true,
                                PatcherRectangle.PatcherIndicator,
                                patcherSize,
                                false),
                            new PatcherValidation(
                                true,
                                PatcherRectangle.Online,
                                patcherSize,
                                false),
                            new PatcherValidation(
                                true,
                                PatcherRectangle.Launch,
                                patcherSize,
                                false)
                        });
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
                        State.Login
                    }) },
                #endregion

                #region Patcher Agreement State
                { State.PatcherAgreement, new Transition(
                    State.PatcherAgreement,
                    () =>
                    {
                        PatcherSize patcherSize;
                        if (!CheckPatcher(out patcherSize)) return false;

                        ActivateTargetWindow(Static.PATCHER_SIZES[patcherSize]);

                        return ValidatePatcher(new PatcherValidation[]
                        {
                            new PatcherValidation(
                                true,
                                PatcherRectangle.Accept,
                                patcherSize,
                                false)
                        });
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
                        State.Login
                    }) },
                #endregion

                #region Login State
                { State.Login, new Transition(
                    State.Login,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.LoginIndicator,
                                false),
                            new ClientValidation(
                                true,
                                ClientRectangle.LoginIndicator2,
                                false),
                            new ClientValidation(
                                true,
                                ClientRectangle.LoginIndicatorThreshold,
                                true),
                            new ClientValidation(
                                false,
                                ClientRectangle.InvalidLoginOk,
                                false),
                            new ClientValidation(
                                false,
                                ClientRectangle.LoggingInIndicatorThreshold,
                                true)
                        });
                    },
                    () =>
                    {
                        ClickTarget(Static.CLIENT_CLICK_POINTS[ClientClickPoint.LoginUsername]);
                        EmptyTextBox();
                        SendText(mainForm.Config.LolId);
                        ClickTarget(Static.CLIENT_CLICK_POINTS[ClientClickPoint.LoginPassword]);
                        EmptyTextBox();
                        SendText(mainForm.Config.LolPassword);
                        ClickTargetWindowRectangle(Static.CLIENT_RECTANGLES[ClientRectangle.Login]);
                    },
                    new State[]
                    {
                        State.InvalidLogin,
                        State.LoggingIn,
                        State.Main,
                        State.Reconnect,
                        State.LeaverBusterWarning
                    }) },
                #endregion

                #region Invalid Login State
                { State.InvalidLogin, new Transition(
                    State.InvalidLogin,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.LoginIndicator,
                                false),
                            new ClientValidation(
                                true,
                                ClientRectangle.LoginIndicator2,
                                false),
                            new ClientValidation(
                                true,
                                ClientRectangle.LoginIndicatorThreshold,
                                true),
                            new ClientValidation(
                                true,
                                ClientRectangle.InvalidLoginOk,
                                false),
                            new ClientValidation(
                                false,
                                ClientRectangle.LoggingInIndicatorThreshold,
                                true)
                        });
                    },
                    () =>
                    {
                        ClickTargetWindowRectangle(Static.CLIENT_RECTANGLES[ClientRectangle.InvalidLoginOk]);
                        mainForm.Pause();
                    },
                    new State[]
                    {
                        State.Login
                    }) },
                #endregion

                #region Logging In State
                { State.LoggingIn, new Transition(
                    State.LoggingIn,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.LoginIndicator,
                                false),
                            new ClientValidation(
                                true,
                                ClientRectangle.LoginIndicator2,
                                false),
                            new ClientValidation(
                                true,
                                ClientRectangle.LoginIndicatorThreshold,
                                true),
                            new ClientValidation(
                                false,
                                ClientRectangle.InvalidLoginOk,
                                false),
                            new ClientValidation(
                                true,
                                ClientRectangle.LoggingInIndicatorThreshold,
                                true)
                        });
                    },
                    () =>
                    {
                    },
                    new State[]
                    {
                        State.InvalidLogin,
                        State.Main,
                        State.Reconnect,
                        State.LeaverBusterWarning
                    }) },
                #endregion

                #region Reconnect State
                { State.Reconnect, new Transition(
                    State.Reconnect,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.Reconnect,
                                false)
                        });
                    },
                    () =>
                    {
                        ClickTargetWindowRectangle(Static.CLIENT_RECTANGLES[ClientRectangle.Reconnect]);
                    },
                    new State[]
                    {
                        State.InGame
                    }) },
                #endregion

                #region Main State
                { State.Main, new Transition(
                    State.Main,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.Play,
                                false)
                        });
                    },
                    () =>
                    {
                        ClickTargetWindowRectangle(Static.CLIENT_RECTANGLES[ClientRectangle.Play]);
                    },
                    new State[]
                    {
                        State.SelectGameMode,
                        State.SelectedAram,
                        State.LeaverBusterWarning
                    }) },
                #endregion

                #region Leaver Buster Warning State
                { State.LeaverBusterWarning, new Transition(
                    State.LeaverBusterWarning,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.LeaverBusterWarning,
                                false)
                        });
                    },
                    () =>
                    {
                        ClickTarget(Static.CLIENT_CLICK_POINTS[ClientClickPoint.LeaverBusterWarning]);
                        EmptyTextBox();
                        SendText(Strings.Strings.LeaverBusterAgreement);
                        ClickTarget(Static.CLIENT_CLICK_POINTS[ClientClickPoint.LeaverBusterOk]);
                    },
                    new State[]
                    {
                        State.SelectGameMode,
                        State.SelectedAram
                    }) },
                #endregion

                #region Select Game Mode State
                { State.SelectGameMode, new Transition(
                    State.SelectGameMode,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.InactivePlay,
                                false),
                            new ClientValidation(
                                false,
                                ClientRectangle.AramIndicator,
                                false)
                        });
                    },
                    () =>
                    {
                        ClickTarget(Static.CLIENT_CLICK_POINTS[ClientClickPoint.SelectPvp]);
                        ClickTarget(Static.CLIENT_CLICK_POINTS[ClientClickPoint.SelectAram]);
                        ClickTarget(Static.CLIENT_CLICK_POINTS[ClientClickPoint.SelectHowlingAbyss]);
                        ClickTarget(Static.CLIENT_CLICK_POINTS[ClientClickPoint.SelectNormal]);
                    },
                    new State[]
                    {
                        State.SelectedAram,
                        State.LeaverBusterWarning
                    }) },
                #endregion

                #region Selected Aram State
                { State.SelectedAram, new Transition(
                    State.SelectedAram,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.InactivePlay,
                                false),
                            new ClientValidation(
                                true,
                                ClientRectangle.AramIndicator,
                                false),
                            new ClientValidation(
                                true,
                                ClientRectangle.Solo,
                                false)
                        });
                    },
                    () =>
                    {
                        ClickTargetWindowRectangle(Static.CLIENT_RECTANGLES[ClientRectangle.Solo]);
                    },
                    new State[]
                    {
                        State.InQueue,
                        State.JoinQueueFailed,
                        State.LeaverBuster,
                        State.LeaverBusterWarning
                    }) },
                #endregion

                #region Join Queue Failed State
                { State.JoinQueueFailed, new Transition(
                    State.JoinQueueFailed,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.JoinQueueFailedIndicator,
                                false)
                        });
                    },
                    () =>
                    {
                    },
                    new State[]
                    {
                        State.JoinQueueFailed,
                        State.InQueue,
                        State.Main
                    }) },
                #endregion

                #region Leaver Buster State
                { State.LeaverBuster, new Transition(
                    State.LeaverBuster,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.LeaverBusterIndicator,
                                false)
                        });
                    },
                    () =>
                    {
                    },
                    new State[]
                    {
                        State.LeaverBuster,
                        State.InQueue,
                        State.Main
                    }) },
                #endregion

                #region In Queue State
                { State.InQueue, new Transition(
                    State.InQueue,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.InQueueIndicator,
                                false)
                        });
                    },
                    () =>
                    {
                    },
                    new State[]
                    {
                        State.InQueue,
                        State.AcceptQueue
                    }) },
                #endregion

                #region Accept Queue State
                { State.AcceptQueue, new Transition(
                    State.AcceptQueue,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.Accept,
                                false)
                        });
                    },
                    () =>
                    {
                        ClickTargetWindowRectangle(Static.CLIENT_RECTANGLES[ClientRectangle.Accept]);
                    },
                    new State[]
                    {
                        State.InQueue,
                        State.ChampionSelect
                    }) },
                #endregion

                #region Champion Select Queue
                { State.ChampionSelect, new Transition(
                    State.ChampionSelect,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.TeamChat,
                                false)
                        });
                    },
                    () =>
                    {
                    },
                    new State[]
                    {
                        State.ChampionSelect,
                        State.InGame,
                        State.InQueue
                    }) },
                #endregion

                #region In Game Queue
                { State.InGame, new Transition(
                    State.InGame,
                    () =>
                    {
                        if (!FindWindow(Static.GAME_CLASS_NAME, Strings.Strings.GameText)) return false;

                        ActivateTargetWindow();

                        return true;
                    },
                    () =>
                    {
                        ClickImageOnTargetWindow(GameImages.Continue);
                    },
                    new State[]
                    {
                        State.InGame,
                        State.Result
                    }) },
                #endregion

                #region Result State
                { State.Result, new Transition(
                    State.Result,
                    () =>
                    {
                        if (!FindWindow(Static.CLIENT_CLASS_NAME, Strings.Strings.ClientText)) return false;

                        ActivateTargetWindow(Static.CLIENT_SIZE);

                        return ValidateClient(new ClientValidation[]
                        {
                            new ClientValidation(
                                true,
                                ClientRectangle.Home,
                                false)
                        });
                    },
                    () =>
                    {
                        ClickTargetWindowRectangle(Static.CLIENT_RECTANGLES[ClientRectangle.Home]);
                    },
                    new State[]
                    {
                        State.Main
                    }) }
                #endregion
            };
        }
        
        public void Work()
        {
            while (true)
            {
                currentTransition = currentTransition.Work(mainForm, transitions);
            }
        }
    }
}
