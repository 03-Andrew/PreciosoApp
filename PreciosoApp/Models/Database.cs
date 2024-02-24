using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    internal class Database
    {
        public MySqlConnection getCon()
        {
            string mysqlCon = "server=127.0.0.1 user=root; database=db_preciosospa2";
            MySqlConnection conn = new MySqlConnection(mysqlCon);
            return conn;
        }
    }
}
