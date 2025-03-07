﻿using MySql.Data.MySqlClient;
using Mysqlx.Crud;
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

            Func<MySqlDataReader, Supplier> mapRow = reader => new Supplier
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Contact = reader.GetString(2),
            };
            string query = "Select * from tbl_supplier";
            return db.ExecuteQuery(query, mapRow);
        }

        public void addNewSuppler(string suppName, string suppNo)
        {
            Database db = new Database();
            using (MySqlConnection con = db.GetCon())
            {
                con.Open();
                string query = "INSERT INTO `tbl_supplier` (`supplier_name`, `supplier_contactnum`) " +
                               "VALUES (@suppName, @suppNo);";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@suppName", suppName);
                    cmd.Parameters.AddWithValue("@suppNo", suppNo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void updateSupplier(string suppName, string suppNo, int suppID)
        {
            Database db = new Database();
            using (MySqlConnection con = db.GetCon())
            {
                con.Open();
                string query = "UPDATE `tbl_supplier` " +
                               "SET `supplier_name` = @suppName, `supplier_contactnum` = @suppNo " +
                               "WHERE `tbl_supplier`.`supplier_id` = @suppID";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@suppName", suppName);
                    cmd.Parameters.AddWithValue("@suppNo", suppNo);
                    cmd.Parameters.AddWithValue("@suppID", suppID);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
