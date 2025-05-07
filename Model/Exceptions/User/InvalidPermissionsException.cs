namespace ScorePALServerModel.Exceptions.User;

public class InvalidPermissionsException(string perm) : ScorePalException($"Invalid Permissions {perm}", 403);