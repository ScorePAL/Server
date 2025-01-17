using Newtonsoft.Json;

namespace VamosVamosServer.SSE;

public interface Event
{
    string GetMessage();
}