namespace VamosVamosServer.Model.PlayedModel;

public interface IAssist
{
    int AssistTime { get; set; }
    AssistType AssistType { get; set; }
    Played Played { get; set; }
    Played AssistedPlayed { get; set; }
}