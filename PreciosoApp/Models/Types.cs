using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    public class TherapistTypes
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public List<TherapistTypes> GetTherapistTypes()
        {
            List<TherapistTypes> types = new List<TherapistTypes>();
            Database db = new Database();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from tbl_therapist_type";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TherapistTypes type = new TherapistTypes();
                            type.Id = reader.GetInt32("type_id");
                            type.TypeName = reader.GetString("type");

                            types.Add(type);
                        }
                    }
                }
                return types;
            }
        }
    }

    public class Gender
    {
        public int Id { get; set; }
        public string GenderType { get; set; }

        private static readonly Dictionary<string, int> Genders = new Dictionary<string, int>();
        Database db = new Database();
        public List<Gender> GetGenders()
        {
            List<Gender> types = new List<Gender>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from tbl_gender";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gender type = new Gender();
                            type.Id = reader.GetInt32("gender_id");
                            type.GenderType = reader.GetString("gender");

                            types.Add(type);
                        }
                    }
                }
                return types;
            }
        }

    }
    public class TherapistStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        Database db = new Database();

        public List<TherapistStatus> GetTStatus()
        {
            List<TherapistStatus> types = new List<TherapistStatus>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from tbl_therapist_status";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TherapistStatus type = new TherapistStatus();
                            type.Id = reader.GetInt32("status_id");
                            type.StatusName = reader.GetString("status");

                            types.Add(type);
                        }
                    }
                }
                return types;
            }
        }
    }

    public class ModeOfPayment
    {
        public int Id { get; set; }
        public string payMode { get; set; }
        Database db = new Database();


        public List<ModeOfPayment> GetMOP()
        {
            List<ModeOfPayment> mops = new List<ModeOfPayment>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from tbl_therapist_status";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ModeOfPayment mopay = new ModeOfPayment();
                            mopay.Id = reader.GetInt32("status_id");
                            mopay.payMode = reader.GetString("status");

                            mops.Add(mopay);
                        }
                    }
                }
                return mops;
            }
        }

        public List<int> GetMOPID(string name)
        {
            List<int> mops = new List<int>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = $"select * from tbl_modeofpayment WHERE mode = '{name}'";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ModeOfPayment mopay = new ModeOfPayment();
                            int Id = reader.GetInt32("mode_id");

                            mops.Add(Id);
                        }
                    }
                }
                return mops;
            }
        }

        public List<string> GetMOPName()
        {
            List<string> mops = new List<string>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from tbl_modeofpayment ";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ModeOfPayment mopay = new ModeOfPayment();
                            string payMode = reader.GetString("mode");

                            mops.Add(payMode);
                        }
                    }
                }
                return mops;
            }
        }
    }

    public class CommissionRate
    {
        public int id { get; set; }
        public float rateValue { get; set; }

        Database db = new Database();
        public List<CommissionRate> GetComissionRate()
        {
            List<CommissionRate> types = new List<CommissionRate>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from tbl_commission_rate";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CommissionRate type = new CommissionRate();
                            type.id = reader.GetInt32(0);
                            type.rateValue = reader.GetFloat(1);

                            types.Add(type);
                        }
                    }
                }
                return types;
            }
        }
    }

    public class ServiceType
    {
        public int id { get; set; }
        public string serviceType { get; set; }

        Database db = new Database();
        public List<ServiceType> GetServiceType()
        {
            List<ServiceType> types = new List<ServiceType>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from tbl_service_type";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServiceType type = new ServiceType();
                            type.id = reader.GetInt32(0);
                            type.serviceType = reader.GetString(1);

                            types.Add(type);
                        }
                    }
                }
                return types;
            }
        }
    }

    public class ProductType
    {
        public int id { get; set; }
        public string productType { get; set; }

        Database db = new Database();
        public List<ProductType> GetProductType()
        {
            List<ProductType> types = new List<ProductType>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from  tbl_product_type";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductType type = new ProductType();
                            type.id = reader.GetInt32(0);
                            type.productType = reader.GetString(1);

                            types.Add(type);
                        }
                    }
                }
                return types;
            }
        }
    }
}
