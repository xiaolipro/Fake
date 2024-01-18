namespace Fake.IdGenerators.Snowflake;

public class DefaultWorkerProvider : IWorkerProvider
{
    private const string WorkerIdKey = "Fake_WorkerId";
    private readonly int _workerId = GetEnvironmentVariable(WorkerIdKey) ?? 0;

    public Task<int> GetWorkerIdAsync()
    {
        return Task.FromResult(_workerId);
    }

    public Task RefreshAsync()
    {
        return Task.CompletedTask;
    }

    public Task LogOutAsync()
    {
        return Task.CompletedTask;
    }

    private static int? GetEnvironmentVariable(string variable)
    {
        var environmentVariable = Environment.GetEnvironmentVariable(variable);
        if (string.IsNullOrEmpty(environmentVariable))
            return null;

        return int.Parse(environmentVariable);
    }
}