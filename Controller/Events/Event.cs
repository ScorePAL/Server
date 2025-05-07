using Newtonsoft.Json;

namespace ScorePALServer.SSE;

public interface Event
{
    string GetMessage();
}