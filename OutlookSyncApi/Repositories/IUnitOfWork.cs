namespace OutlookSyncApi.Repositories
{
    public interface IUnitOfWork
    {
        IGraphConfigurationRepository GraphConfigurations { get; }
    }
}
