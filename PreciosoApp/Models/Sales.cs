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
                ProdGrossRevenue = reader.GetDouble(1),
                ServGrossRevenue = reader.GetDouble(2),
                totalRevenue = reader.GetDouble(3)
            };

            string query = "select * from daily_revenue;";
            
            return db.ExecuteQuery(query, mapRow);
        }


    }
}
