namespace ScorePALServerModel.Exceptions.Club;

public class ClubNotFoundException(long clubId) : ScorePalException($"Club not found: {clubId}", 404);