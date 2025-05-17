using ScorePALServer.Model.PlayedModel;

namespace ScorePALServerModel.Logic.PlayedModel;

public class Penalty
{
    public PenaltyObtainingMethod ObtainingMethod { get; set; }

    public PenaltyResult Result { get; set; }

    public long PenaltyTime { get; set; }

    public Played Played { get; set; }
}