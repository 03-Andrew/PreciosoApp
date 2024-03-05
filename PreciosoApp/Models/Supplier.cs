using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact {  get; set; }

        public List<Supplier> GetAllSupplier()
        {
            Database db = new Database();
            List<Supplier> supp = new List<Supplier>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "Select * from tbl_supplier";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Supplier supplier = new Supplier();
                            supplier.Id = reader.GetInt32(0);
                            supplier.Name = reader.GetString(1);
                            supplier.Contact = reader.GetString(2);
                            supp.Add(supplier);
                        }
                    }
                }
            }
            return supp;
        }



    }
}
