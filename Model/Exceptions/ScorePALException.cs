namespace Model.Exceptions;

/// <summary>
/// Generic exception for the API
/// </summary>
public class ScorePalException : Exception
{
    private int code;

    /// <summary>
    /// Error code
    /// </summary>
    public int Code => this.code;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="message">Exception message</param>
    public ScorePalException(string message, int code) : base(message)
    {
        this.code = code;
    }
}