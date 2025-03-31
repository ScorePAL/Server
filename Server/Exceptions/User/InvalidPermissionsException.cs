namespace ScorePALServer.Exceptions.User;

public class InvalidPermissionsException(string perm) : ScorePalException($"Invalid Permissions {perm}", 403);