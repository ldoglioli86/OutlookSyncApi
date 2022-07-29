namespace OutlookSyncApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IGraphConfigurationRepository graphConfigurationRepository)
        {
            GraphConfigurations = graphConfigurationRepository;
        }

        public IGraphConfigurationRepository GraphConfigurations { get; }
    }
}
