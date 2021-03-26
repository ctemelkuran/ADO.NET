using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDemo
{ // Normalde kurumsal mimaride IProductDal 'dan çekiyor olururuz
    public class ProductDal //Data Access Layer veya Object
    {
        //integrated security=true veri tabanına direkt local erişim sağlar, bilgisayara login olmak yeterli
        // uzaktaki veritabanı için false yapılır kullanıcı girilir 
        //class içinde ancak methodların dışında ise başına _ konur
        SqlConnection _connection = new SqlConnection(@"server=(localdb)\mssqllocaldb; initial catalog = ETradeCSharpEngin; integrated security=true");
        public List<Product> GetAll()
        {
            ConnectionControl();
            //sql komutu yazılır
            SqlCommand sqlCommand = new SqlCommand("Select * from Products", _connection);
            //komut execute edilmeli
            SqlDataReader reader = sqlCommand.ExecuteReader();

            List<Product> products = new List<Product>();

            while (reader.Read()) // dataları okuyabildiğin sürece çalıştır
            {
                Product product = new Product
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    StockAmount = Convert.ToInt32(reader["StockAmount"]),
                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"])
                };
                products.Add(product);
            }


            reader.Close();
            _connection.Close();
            return products;

        }

        private void ConnectionControl()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public DataTable GetAll2()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            //sql komutu yazılır
            SqlCommand sqlCommand = new SqlCommand("Select * from Products", _connection);
            //komut execute edilmeli
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable(); // DataTable nesnesi kullanılmaz istenmez
            dataTable.Load(reader);
            reader.Close();
            _connection.Close();
            return dataTable;

        }

        public void Add(Product product)
        {
            ConnectionControl();
            SqlCommand sqlCommand = new SqlCommand(
                "Insert into Products values(@name, @unitPrice, @stockAmount)", _connection);
            sqlCommand.Parameters.AddWithValue("@name", product.Name);
            sqlCommand.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
            sqlCommand.Parameters.AddWithValue("@stockAmount", product.StockAmount);
            sqlCommand.ExecuteNonQuery();

            _connection.Close();
        }

        public void Update(Product product)
        {
            ConnectionControl();
            SqlCommand sqlCommand = new SqlCommand(
                "Update Products set Name=@name, UnitPrice=@unitPrice, StockAmount=@stockAmount where Id=@id", _connection);
            sqlCommand.Parameters.AddWithValue("@name", product.Name);
            sqlCommand.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
            sqlCommand.Parameters.AddWithValue("@stockAmount", product.StockAmount);
            sqlCommand.Parameters.AddWithValue("@id", product.Id);
            sqlCommand.ExecuteNonQuery();

            _connection.Close();
        }

        public void Delete(int id)
        {
            ConnectionControl();
            SqlCommand sqlCommand = new SqlCommand(
                "Delete from Products where Id=@id", _connection);

            sqlCommand.Parameters.AddWithValue("@id", id);
            sqlCommand.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
