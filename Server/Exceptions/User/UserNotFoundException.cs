namespace ScorePALServer.Exceptions.User;

public class UserNotFoundException(string email) : ScorePalException($"User not found ({email})", 404);