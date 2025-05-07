namespace Model.Exceptions.User;

public class InvalidPermissionsException(string perm) : ScorePalException($"Invalid Permissions {perm}", 403);