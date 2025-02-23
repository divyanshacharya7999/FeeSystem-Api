using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace FeeSystem.EntityFrameworkCore
{
    public static class FeeSystemDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<FeeSystemDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        public static void Configure(DbContextOptionsBuilder<FeeSystemDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection, ServerVersion.AutoDetect(connection.ConnectionString));
        }
    }
}
