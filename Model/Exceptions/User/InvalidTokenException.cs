namespace ScorePALServerModel.Exceptions.User;

public class InvalidTokenException(string token) : ScorePalException($"Invalid Token ({token})", 401);