namespace VamosVamosServer.Model.PlayedModel;

public interface IPenalty
{
    int PenaltyTime { get; set; }
    PenaltyResult Result { get; set; }
    PenaltyObtainingMethod ObtainingMethod { get; set; }
    Played Played { get; set; }
}