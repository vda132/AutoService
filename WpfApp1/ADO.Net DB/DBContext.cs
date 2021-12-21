using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ADO.Net_DB
{
    class DBContext
    {
        private readonly string connectionString;
        public DBContext()
        {
            var config = Configurations.Configuration.GetConfiguration();
            connectionString = config.Config.GetConnectionString("AutoServiceConnection");
        }

        public List<KeyValuePair<string, int>> GetWorkerStatistic()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "select t2.NameWorker, (select count(*) from AutoService t1 where t2.IDWorker=t1.IDWorker) as 'Количество обслуживаний' from Worker t2 Where t2.IDPosition = (select t3.IDPosition from Position t3 where t3.NamePosition = 'Мастер') order by 'Количество обслуживаний' desc";
                var adapter = new SqlDataAdapter(query, connection);
                DataSet ds = new DataSet();
                try
                {
                    adapter.Fill(ds, "Table");
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
                var dt = ds.Tables["Table"];
                var clients = new List<KeyValuePair<string, int>>();
                foreach (DataRow row in dt.Rows)
                {
                    clients.Add(new KeyValuePair<string, int>(row["NameWorker"].ToString(), (int)row["Количество обслуживаний"]));
                }
                return clients;
            }
        }
        public List<KeyValuePair<string, int>> GetWorkerStatistic(DateTime dateBegin, DateTime dateEnd)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = $"select t2.NameWorker, (select count(*) from AutoService t1 where t2.IDWorker=t1.IDWorker and t1.DateAutoService between '{dateBegin}' and '{dateEnd}') as 'Количество обслуживаний' from Worker t2 Where t2.IDPosition = (select t3.IDPosition from Position t3 where t3.NamePosition = 'Мастер') order by 'Количество обслуживаний' desc";
                var adapter = new SqlDataAdapter(query, connection);
                DataSet ds = new DataSet();
                try
                {
                    adapter.Fill(ds, "Table");
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
                var dt = ds.Tables["Table"];
                var clients = new List<KeyValuePair<string, int>>();
                foreach (DataRow row in dt.Rows)
                {
                    clients.Add(new KeyValuePair<string, int>(row["NameWorker"].ToString(), (int)row["Количество обслуживаний"]));
                }
                return clients;
            }
        }
        public List<KeyValuePair<string, decimal>> GetBrandStatistic()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "select distinct NameCarBrand, sum(t4.Price) over(partition by NameCarBrand) as 'Сумма обслуживания' from CarBrand t1 inner join Model t2 on t1.IDCarBrand=t2.IDCarBrand inner join Car t3 on t3.IDModel=t2.IDModel inner join AutoService t4 on t3.StateNumber=t4.StateNumber";
                var adapter = new SqlDataAdapter(query, connection);
                DataSet ds = new DataSet();
                try
                {
                    adapter.Fill(ds, "Table");
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
                var dt = ds.Tables["Table"];
                var statistic = new List<KeyValuePair<string, decimal>>();
                foreach (DataRow row in dt.Rows)
                {
                    statistic.Add(new KeyValuePair<string, decimal>(row["NameCarBrand"].ToString(), (decimal)row["Сумма обслуживания"]));
                }
                return statistic;
            }
        }
    }
}
