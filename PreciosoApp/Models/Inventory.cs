﻿using DynamicData;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
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
        public int prodCritLevel { get; set; }
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
                    "( SELECT COALESCE(SUM(quantity), 0) FROM tbl_stockin_product t1 WHERE t1.product_id = p.product_id ) - " +
                    "( SELECT COALESCE(SUM(quantity), 0) FROM tbl_products_sold t2 WHERE t2.product_id = p.product_id ) " +
                    "AS stock_level FROM tbl_product p JOIN tbl_product_type t ON p.product_type = t.type_id;";
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
                            inv.prodCritLevel = reader.GetInt32("critical_level");
                            inv.prodStock = reader.GetInt32("stock_level");
                            inventory.Add(inv);
                        }
                    }
                }

            }
            return inventory;
        }


        public void AddStockInProduct(int invID, int prod_id, int quantity, double price)
        {
            Database db = new Database();
            using (MySqlConnection con = db.GetCon())
            {
                con.Open();
                string query = "INSERT INTO tbl_stockin_product (inventory_id, product_id, quantity, purchase_price) " +
                               "VALUES (@invID, @prod_id, @quan, @price);";  

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@invID", invID);
                    cmd.Parameters.AddWithValue("@prod_id", prod_id);
                    cmd.Parameters.AddWithValue("@quan", quantity);
                    cmd.Parameters.AddWithValue("@price", price);

                    cmd.ExecuteNonQuery();
                }

            }

        }

        public void AddNewProduct(string prodName, float prodCost, int typeID)
        {
            Database db = new Database();
            using (MySqlConnection con = db.GetCon())
            {
                con.Open();
                string query = "INSERT INTO `tbl_product` (`product_name`, `product_cost`, `product_type`, `critical_level`) " +
                               "VALUES (@prodName, @prodCost, @typeID, '5');";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@prodName", prodName);
                    cmd.Parameters.AddWithValue("@prodCost", prodCost);
                    cmd.Parameters.AddWithValue("@typeID", typeID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProduct(string prodName, float prodCost, int typeID, int productID)
        {
            Database db = new Database();
            using (MySqlConnection con = db.GetCon())
            {
                con.Open();
                string query = "UPDATE `tbl_product` " +
                               "SET `product_name` = @prodName, `product_cost` = @prodCost, `product_type` = @typeID " +
                               "WHERE `tbl_product`.`product_id` = @prodID;";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@prodName", prodName);
                    cmd.Parameters.AddWithValue("@prodCost", prodCost);
                    cmd.Parameters.AddWithValue("@typeID", typeID);
                    cmd.Parameters.AddWithValue("@prodID", productID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int StockIn(int Supplier_id, DateTime date, int Therapist_id)
        {
            int invID = -1;
            Database db = new Database();
            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "INSERT INTO tbl_stockin (supplier_id, date, therapist_id) values (@supplier, @date, @therapist);" +
                               "SELECT LAST_INSERT_ID();"; 
                
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@supplier", Supplier_id);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@therapist", Therapist_id);

                    invID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return invID;
        }

        public List<string> GetProductName()
        {
            Database db = new Database();
            List<string> inventory = new List<string>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "SELECT product_name, product_cost FROM tbl_product;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productName = reader.GetString("product_name");
                            float prodCost = reader.GetFloat("product_cost");
                            inventory.Add(productName);
                        }
                    }
                }

            }
            Console.WriteLine(inventory);
            return inventory;
        }

        public List<int> GetProductID(string productName)
        {
            Database db = new Database();
            List<int> inventory = new List<int>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = $"SELECT product_id FROM tbl_product WHERE product_name = '{productName}';";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productID = reader.GetInt32("product_id");
                            inventory.Add(productID);
                        }
                    }
                }
            }
            return inventory;
        }


        public static List<Inventory> SearchInventory(List<Inventory> allInv, string searchText)
        {
            return allInv.FindAll(inv => inv.prodName.ToLower().Contains(searchText.ToLower()));
        }
    }
}