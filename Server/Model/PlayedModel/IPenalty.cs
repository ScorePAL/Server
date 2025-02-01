namespace ScorePALServer.Model.PlayedModel;

public interface IPenalty
{
    long PenaltyTime { get; set; }
    PenaltyResult Result { get; set; }
    PenaltyObtainingMethod ObtainingMethod { get; set; }
    Played Played { get; set; }
}