namespace Fake.UnitOfWork;

public enum UnitOfWorkStatus
{
    Active,

    Saving,
    Saved,
    SaveFailed,
    
    Completing,
    Completed,
    CompletedFailed,
    
    RollBacking,
    RollBacked,
    RollBackFailed,

    Disposing,
    Disposed,
    DisposeFailed
}