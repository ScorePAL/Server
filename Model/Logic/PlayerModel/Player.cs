using ScorePALServer.Model.PlayedModel;

namespace ScorePALServerModel.Logic.PlayerModel;

public class Player
{

    public long Id { get; set; }

    public String FirstName { get; set; }

    public String LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public Position[] Positions { get; set; }

    public List<Played> Played { get; set; }
}