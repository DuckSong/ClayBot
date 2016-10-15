namespace ClayBot.StateMachine
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
        LeaverBusterWarning,
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
}