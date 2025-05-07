namespace Model.Exceptions.Match;

public class MatchNotFoundException(long matchId) : ScorePalException($"Match not found ({matchId})", 404);