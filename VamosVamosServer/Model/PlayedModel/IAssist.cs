namespace VamosVamosServer.Model.PlayedModel;

public interface IAssist
{
    long AssistTime { get; set; }
    AssistType AssistType { get; set; }
    Played Played { get; set; }
    Played AssistedPlayed { get; set; }
}