namespace Fake.IdGenerators.Snowflake;

public interface IWorkerProvider
{
    /// <summary>
    /// Working machine id
    /// </summary>
    Task<int> GetWorkerIdAsync();

    /// <summary>
    /// Refresh work id activity status
    /// </summary>
    /// <returns></returns>
    Task RefreshAsync();

    /// <summary>
    /// logout work id active status
    /// </summary>
    /// <returns></returns>
    Task LogOutAsync();
}