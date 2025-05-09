namespace ScorePALServerModel.Exceptions.Team;

public class TeamNotFoundException(long teamId) : ScorePalException($"Team with ID {teamId} not found", 404);