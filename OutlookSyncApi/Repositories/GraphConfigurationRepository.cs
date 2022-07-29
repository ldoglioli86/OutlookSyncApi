using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using OutlookSyncApi.Models;

namespace OutlookSyncApi.Repositories
{
    public class GraphConfigurationRepository : IGraphConfigurationRepository
    {
        private readonly IConfiguration configuration;
        public GraphConfigurationRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IReadOnlyList<GraphConfiguration> GetAll()
        {
            var sql = "SELECT * FROM GraphConfiguration";
            using (var connection = new SqlConnection(configuration.GetConnectionString("SqlConnection")))
            {
                connection.Open();
                var result = connection.Query<GraphConfiguration>(sql);
                return result.ToList();
            }
        }
    }
}
