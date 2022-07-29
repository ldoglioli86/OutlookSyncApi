using System.Collections.Generic;
using OutlookSyncApi.Models;

namespace OutlookSyncApi.Repositories
{
    public interface IGraphConfigurationRepository
    {
        IReadOnlyList<GraphConfiguration> GetAll();
    }
}
