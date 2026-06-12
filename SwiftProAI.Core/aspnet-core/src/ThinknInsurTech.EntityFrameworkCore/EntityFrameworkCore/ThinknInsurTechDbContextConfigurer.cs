using System.Data.Common;
using System.IO;
using Abp.Runtime.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Web;

namespace ThinknInsurTech.EntityFrameworkCore
{
    public static class ThinknInsurTechDbContextConfigurer
    {
        // uncomment code below for tracking sql query
        //public static readonly LoggerFactory loggerFactory = new LoggerFactory(new[]
        //{
        //    new Log4NetProvider("log4net.config")
        //});

        public static void Configure(DbContextOptionsBuilder<ThinknInsurTechDbContext> builder, string connectionString)
        {
            //builder.UseLoggerFactory(loggerFactory);
            //builder.UseNpgsql(connectionString);
            builder.UseSqlServer(connectionString);

        }

        public static void Configure(DbContextOptionsBuilder<ThinknInsurTechDbContext> builder, DbConnection connection)
        {
            //builder.UseLoggerFactory(loggerFactory);
            //builder.UseNpgsql(connection);
            builder.UseSqlServer(connection);
        }


    }
}