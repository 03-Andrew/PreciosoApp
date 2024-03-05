using DynamicData;
using MySql.Data.MySqlClient;
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


        public void AddStockInProduct(int prod_id, int quantity, double price)
        {
            Database db = new Database();
            using (MySqlConnection con = db.GetCon())
            {
                con.Open();
                string query = "INSERT INTO tbl_stockin_product (inventory_id, product_id, quantity, purchase_price) VALUES ((SELECT MAX(inventory_id) FROM tbl_stockin), @prod_id, @quan, @price);";  

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@prod_id", prod_id);
                    cmd.Parameters.AddWithValue("@quan", quantity);
                    cmd.Parameters.AddWithValue("@price", price);

                    cmd.ExecuteNonQuery();
                }

            }

        }

        public void StockIn(int Supplier_id, DateTime date, int Therapist_id)
        {
            Database db = new Database();
            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "INSERT INTO tbl_stockin (supplier_id, date, therapist_id) values (@supplier, @date, @therapist)"; 
                
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@supplier", Supplier_id);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@therapist", Therapist_id);

                    cmd.ExecuteNonQuery();
                }
            }
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