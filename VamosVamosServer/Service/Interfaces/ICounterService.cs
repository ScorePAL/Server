namespace VamosVamosServer.Service.Interfaces;

public interface ICounterService
{
    Task CountdownDelay(CancellationToken cancellationToken);
    int StartValue { get; }
}