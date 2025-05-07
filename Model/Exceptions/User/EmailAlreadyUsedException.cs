namespace ScorePALServerModel.Exceptions.User;

public class EmailAlreadyUsedException() : ScorePalException("Email is already used", 409);