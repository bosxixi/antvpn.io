﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SharedProject;

namespace Accounting.RouterReporter
{
    public class Repo : IRepo
    {
        private readonly string _connectionString;
        private readonly string _connectionStringServer;
        private readonly string _connectionStringDc;
        public Repo(string connectionString, string connectionStringDc, string connectionStringServer)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            if (String.IsNullOrEmpty(connectionStringDc))
            {
                throw new ArgumentNullException(nameof(connectionStringDc));
            }
            if (String.IsNullOrEmpty(connectionStringServer))
            {
                throw new ArgumentNullException(nameof(connectionStringServer));
            }
            this._connectionStringDc = connectionStringDc;
            this._connectionString = connectionString;
            this._connectionStringServer = connectionStringServer;
        }
        public void InsertDatas(List<RemoteAccessConnection> racs)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                connection.Execute(@"insert [dbo].[current](AuthMethod, Bandwidth, ClientExternalAddress, ClientIPv4Address, ConnectionDuration, ConnectionStartTime, ConnectionType, MachineName, TimeStamp, TotalBytesIn, TotalBytesOut, TransitionTechnology, TunnelType, UserActivityState, UserName) values (@AuthMethod, @Bandwidth, @ClientExternalAddress, @ClientIPv4Address, @ConnectionDuration, @ConnectionStartTime, @ConnectionType, @MachineName, @TimeStamp, @TotalBytesIn, @TotalBytesOut, @TransitionTechnology, @TunnelType, @UserActivityState, @UserName)", racs);
            }
        }

        public string[] GetDisconnectUserNames()
        {
            using (var connection = new SqlConnection(this._connectionStringDc))
            {
                return connection.Query<string>(@"select [LoginName] from logins where [AllowDialIn] = 0 or [Enabled] = 0").ToArray();
            }
        }

        public IEnumerable<Login> GetLiveUsers()
        {
            using (var connection = new SqlConnection(this._connectionStringDc))
            {
                return connection.Query<Login>(@"select [LoginName],[Password],[Port] from logins where [AllowDialIn] = 1 and [Enabled] = 1").ToList();
            }
        }

        public void InsertOrUpdateTimetamp(string machineName, DateTime timestamp)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var b = connection.Query<int>("select count(*) from [dbo].[currentmeta] where MachineName = @machineName ", new { machineName });
                if (b.FirstOrDefault() == 0)
                {
                    connection.Execute("insert [dbo].[currentmeta](MachineName, TimeStamp) values(@machineName, @timestamp)", new { machineName, timestamp });
                }
                else
                {
                    connection.Execute("update [dbo].[currentmeta] set TimeStamp = @timestamp where MachineName = @machineName", new { machineName, timestamp });
                }
            }
        }

        public void InsertSSEventRaws(IEnumerable<SSEventraw> sseventraw)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                connection.Execute(@"insert [dbo].[sseventraw](MachineName, TimeStamp, TotalBytesInOut, UserName) values (@MachineName, @TimeStamp, @TotalBytesInOut, @UserName)", sseventraw);
            }
        }

        public void InsertServerHealthReport(HealthReport healthReport)
        {
            if (healthReport == null)
            {
                throw new ArgumentNullException(nameof(healthReport));
            }
            var healthReportJson = Newtonsoft.Json.JsonConvert.SerializeObject(healthReport);
            using (var connection = new SqlConnection(this._connectionStringServer))
            {
                connection.Execute(@"insert [dbo].[healthreports](HealthReportJson) values (@healthReportJson)", new { healthReportJson });
            }
        }
    }
}
