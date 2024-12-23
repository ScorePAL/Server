namespace VamosVamosServer.Model.MatchHistoryModel;

public enum MatchEvent
{
    CreateCanPlay,
    UpdateCanPLay,
    DeleteCanPlay,
    AddGoal,
    RemoveGoal,
    AddAssist,
    RemoveAssist,
    AddYellowCard,
    RemoveYellowCard,
    AddRedCard,
    RemoveRedCard,
    AddBlockedShots,
    RemoveBlockedShot,
    AddOnTargetShot,
    RemoveOnTargetShot,
    AddOffTargetShot,
    RemoveOffTargetShot,
    SwitchPlayer,
    AddPenalty,
    RemovePenalty,
    AddInjury,
    RemoveInjury,
}