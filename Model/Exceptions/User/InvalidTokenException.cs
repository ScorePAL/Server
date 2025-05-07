namespace Model.Exceptions.User;

public class InvalidTokenException(string token) : ScorePalException($"Invalid Token ({token})", 401);