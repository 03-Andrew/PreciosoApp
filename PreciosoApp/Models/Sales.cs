using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    public class Sales
    {
        
    }

    public class DailyGross
    {
        public DateTime Date { get; set; }
        public DateOnly DateOnly { get; set; }
        public double ProdSales { get; set; }
        public double ServPromoSales { get; set; }
        public double TotalSales { get; set; }

        Database db = new Database();

        public List<DailyGross> GetDailyGross()
        {
            Func<MySqlDataReader, DailyGross> mapRow = reader => new DailyGross
            {
                Date = reader.GetDateTime(0),
                DateOnly = DateOnly.FromDateTime(reader.GetDateTime(0)),
                ProdSales = reader.GetDouble(1),
                ServPromoSales = reader.GetDouble(2),
                TotalSales = reader.GetDouble(3)
            };

            string query = "select * from daily_sales";            
            return db.ExecuteQuery(query, mapRow);
        }
    }

    public class DailyReport
    {
        public int TransactionID { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan Time { get; set; }
        public string? Client { get; set; }
        public string? Therapist { get; set; }
        public string? MOP { get; set; }
        public string? Availed { get; set; }
        public string? Type { get; set; }
        public string? Price { get; set; }
        public string? Commission { get; set; }
        Database db = new Database();

        public List<DailyReport> GetDailyReports()
        {
            Func<MySqlDataReader, DailyReport> mapRow = reader => new DailyReport
            {
               TransactionID = reader.GetInt16("transaction_id"),
               DateTime = reader.GetDateTime("transaction_datetime"),
               Time = reader.GetDateTime("transaction_datetime").TimeOfDay,      
               Client = reader.GetString("client_name"),
               Therapist = reader.GetString("therapist"),
               MOP = reader.GetString("MOP"),
               Availed = reader.GetString("availed").Replace(", ", "\n").Trim(),
               Type = reader.GetString("type").Replace(", ", "\n").Trim(),
               Price = reader.GetString("cost").Replace(", ", "\n").Trim(),
               Commission = reader.GetString("commission").Replace(", ", "\n").Trim() 
            };
            string query = $"select tr.transaction_id, trx.transaction_datetime, trx.client_name, trx.name as therapist, trx.mode as MOP," +
                $"tr.availed, tr.type, tr.cost, tr.commission from testView tr " +
                $"left join transactions_only trx on tr.transaction_id = trx.transaction_id;";
            return db.ExecuteQuery(query, mapRow);
        }
    }

    public class Commissions
    {
        public string? TherapistName { get; set; }
        public double Commission { get; set; }
        public DateOnly Date { get; set; }
        Database db = new Database();

        public List<Commissions> GetCommissions()
        {
            Func<MySqlDataReader, Commissions> mapRow = reader => new Commissions
            {
                TherapistName = reader.GetString("therapist"),
                Commission = reader.GetDouble("total_commission"),
                Date = DateOnly.FromDateTime(reader.GetDateTime("date"))
            };

            string query = "select date(transaction_datetime) as date, therapist,sum(total_commission) as total_commission " +
                "from Transactions_all group by therapist, DATE(transaction_datetime) order by DATE(transaction_datetime) asc";
            return db.ExecuteQuery(query, mapRow); 

        }
    }

}
