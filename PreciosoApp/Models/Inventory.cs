﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    public class Inventory
    {
        public int invID { get; set; }
        public string prodName { get; set; }
        public float prodCost { get; set; }
        public float prodComm { get; set; }
        public string prodType { get; set; }
        public int prodLevel { get; set; }
        public int prodStock { get; set; }

        public Inventory() { }

        public List<Inventory> GetInventory()
        {
            Database db = new Database();
            List<Inventory> inventory = new List<Inventory>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "SELECT p.product_id, p.product_name, p.product_cost, p.commission, t.type, p.critical_level, " +
                    "( SELECT SUM(quantity) FROM tbl_stockin_product t1 WHERE t1.product_id = p.product_id ) - " +
                    "( SELECT SUM(quantity) FROM tbl_products_sold t2 WHERE t2.product_id = p.product_id ) AS stock_level " +
                    "FROM tbl_product p JOIN tbl_product_type t ON p.product_type = t.type_id; ";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Inventory inv = new Inventory();
                            inv.invID = reader.GetInt32("product_id");
                            inv.prodName = reader.GetString("product_name");
                            inv.prodCost = reader.GetFloat("product_cost");
                            inv.prodComm = reader.GetFloat("commission");
                            inv.prodType = reader.GetString("type");
                            inv.prodLevel = reader.GetInt32("critical_level");
                            inv.prodStock = reader.GetInt32("stock_level");
                            inventory.Add(inv);
                        }
                    }
                }

            }
            return inventory;
        }

        public List<string> GetProductName()
        {
            Database db = new Database();
            List<string> inventory = new List<string>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "SELECT product_name FROM tbl_product;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            String productName = reader.GetString("product_name");
                            inventory.Add(productName);
                        }
                    }
                }

            }
            Console.WriteLine(inventory);
            return inventory;
        }
        public static List<Inventory> SearchInventory(List<Inventory> allInv, string searchText)
        {
            return allInv.FindAll(inv => inv.prodName.ToLower().Contains(searchText.ToLower()));
        }
    }
}
