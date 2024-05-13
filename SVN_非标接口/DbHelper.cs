using System.Data.OleDb;
using System.Data.SqlClient;

namespace HealthExaminationSystem.ThirdParty.DataTransmission
{
    /// <summary>
    /// 数据库帮助器
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// 测试连接字符串是否有效
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static bool TestConnectionString(string connStr)
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = connStr;
                try
                {
                    conn.Open();
                    conn.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool TestOleDbConnectionString(string connStr)
        {
            using (var conn = new OleDbConnection())
            {
                conn.ConnectionString = connStr;
                try
                {
                    conn.Open();
                    conn.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 测试给定的服务器、数据库名称、用户名、密码是否有效
        /// </summary>
        /// <param name="serverName">服务器名称</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static bool TestConnectionString(string serverName, string dbName, string user, string pwd)
        {
            var connSb = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                UserID = user,
                Password = pwd,
                InitialCatalog = dbName
            };
            var r = TestConnectionString(connSb.ConnectionString);
            return r;
        }

        /// <summary>
        /// 测试给定的服务器、数据库名称、用户名、密码是否有效
        /// </summary>
        /// <param name="serverName">服务器名称</param>
        /// <param name="dbName">数据库名称</param>
        /// <returns></returns>
        public static bool TestConnectionString(string serverName, string dbName)
        {
            var connSb = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            var r = TestConnectionString(connSb.ConnectionString);
            return r;
        }

        public static bool TestOleDbConnectionString(string fileName, string user, string pwd, string dataPwd)
        {
            var connectionString = GetOleDbConnectionString(fileName, user, pwd, dataPwd);
            return TestOleDbConnectionString(connectionString);
        }

        public static string GetSqlConnectionString(string serverName, string dbName, string user, string pwd)
        {
            var connSb = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                UserID = user,
                Password = pwd,
                InitialCatalog = dbName
            };
            return connSb.ConnectionString;
        }

        public static string GetSqlConnectionString(string serverName, string dbName)
        {
            var connSb = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                InitialCatalog = dbName
            };
            return connSb.ConnectionString;
        }

        public static string GetOleDbConnectionString(string fileName, string user, string pwd)
        {
            var connSb = new OleDbConnectionStringBuilder
            {
                DataSource = fileName,
                Provider = "Microsoft.Jet.Oledb.4.0",
                ["User ID"] = user,
                ["Password"] = pwd
            };

            return connSb.ConnectionString;
        }

        public static string GetOleDbConnectionString(string fileName)
        {
            var connSb = new OleDbConnectionStringBuilder
            {
                DataSource = fileName,
                Provider = "Microsoft.Jet.Oledb.4.0"
            };
            return connSb.ConnectionString;
        }

        public static string GetOleDbConnectionString(string fileName, string user, string pwd, string dataPwd)
        {
            var connSb = new OleDbConnectionStringBuilder
            {
                DataSource = fileName,
                Provider = "Microsoft.Jet.Oledb.4.0",
                ["User ID"] = user,
                ["Password"] = pwd
            };
            connSb.Add("Jet OLEDB:Database Password", dataPwd);

            return connSb.ConnectionString;
        }
    }
}