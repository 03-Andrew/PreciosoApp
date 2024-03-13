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
        public double ProdGrossRevenue { get; set; }
        public double ServGrossRevenue { get; set; }
        public double totalRevenue { get; set; }

        Database db = new Database();

        public List<DailyGross> GetDailyGross()
        {
            Func<MySqlDataReader, DailyGross> mapRow = reader => new DailyGross
            {
                Date = reader.GetDateTime(0),
                DateOnly = DateOnly.FromDateTime(reader.GetDateTime(0)),
                ServGrossRevenue = reader.GetDouble(1),
                ProdGrossRevenue = reader.GetDouble(2),
                totalRevenue = reader.GetDouble(3)
            };

            string query = "select CAST(t.transaction_datetime AS DATE) AS transaction_date,\r\nsp.total_revenue as service_promo_sales, p.gross_product_revenue as product_sales, \r\nsp.total_revenue + p.gross_product_revenue as total_sales from tbl_transaction t \r\nleft join daily_gross_product p ON CAST(t.transaction_datetime AS DATE) = p.Date\r\nleft join daily_gross_service_promo sp ON CAST(t.transaction_datetime AS DATE) = sp.transaction_date\r\ngroup by transaction_date;  ";
            
            return db.ExecuteQuery(query, mapRow);
        }
    }

    public class DailyReport
    {
        public int TransactionID { get; set; }
        public DateTime DateTime { get; set; }
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
                Client = reader.GetString("client_name"),
                Therapist = reader.GetString("therapist"),
                MOP = reader.GetString("MOP"),
                Availed = reader.GetString("availed").Replace(", ", "\n").Trim(),
                Type = reader.GetString("type").Replace(", ", "\n").Trim(),
                Price = reader.GetString("cost").Replace(", ", "\n").Trim(),
                Commission = reader.GetString("commission").Replace(", ", "\n").Trim() 
            };

            string query = $"select tr.transaction_id, trx.transaction_datetime, trx.client_name, trx.name as therapist, trx.mode as MOP," +
                $"tr.availed, tr.type, tr.cost, tr.commission from testView tr left join transactions_only trx on tr.transaction_id = trx.transaction_id;";
            return db.ExecuteQuery(query, mapRow);
        }
    }

}
