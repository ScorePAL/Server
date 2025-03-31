namespace ScorePALServer.Exceptions.User;

public class UndefinedUserException() : ScorePalException("User is undefined", 404);