namespace MSSQLServerAuditor.Model
{
    public enum NodeUpdatingSource
    {
        ForcedFromServer,
        FromServerIfNotSavedLocally,
        LocallyOnly,
    }
}