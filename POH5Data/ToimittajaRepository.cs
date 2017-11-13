using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using POH5Luokat;

namespace POH5Data
{
    public class ToimittajaRepository : DataAccess, IRepository<Toimittaja>
    {
        public ToimittajaRepository(string yhteys)
            : base(yhteys)
        { }

        public bool Lisaa(Toimittaja o)
        {
            string sql = "INSERT INTO dbo.Suppliers(CompanyName, ContactName, ContactTitle, Address, City, PostalCode, Country) " +
             "VALUES (@CompanyName, @ContactName, @ContactTitle, @Address, @City, @PostalCode, @Country)";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CompanyName", o.Nimi));
                cmd.Parameters.Add(new SqlParameter("@ContactName", o.YhteysHenkilo != null ? o.YhteysHenkilo : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@ContactTitle", o.YhteysTitteli != null ? o.YhteysTitteli : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Address", o.Katuosoite != null ? o.Katuosoite : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@City", o.Kaupunki != null ? o.Kaupunki : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@PostalCode", o.PostiKoodi != null ? o.PostiKoodi : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Country", o.Maa != null ? o.Maa : (object)DBNull.Value));
                cn.Open();
                return (cmd.ExecuteNonQuery() == 1);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }

        public bool Poista(int id)
        {
            string sql = "DELETE FROM dbo.Suppliers " +
                         "WHERE SupplierID = @SupplierID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@SupplierID", id));
                cn.Open();
                return (cmd.ExecuteNonQuery() == 1);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }

        public bool Muuta(Toimittaja o)
        {
            string sql = "UPDATE dbo.Suppliers " +
                         "SET CompanyName = @CompanyName,  ContactName = @ContactName,  ContactTitle = @ContactTitle, Address = @Address, City = @City, " +
                         "PostalCode = @PostalCode, Country = @Country " +
                         "WHERE SupplierID = @SupplierID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CompanyName", o.Nimi));
                cmd.Parameters.Add(new SqlParameter("@ContactName", o.YhteysHenkilo != null ? o.YhteysHenkilo : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@ContactTitle", o.YhteysTitteli != null ? o.YhteysTitteli : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Address", o.Katuosoite != null ? o.Katuosoite : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@City", o.Kaupunki != null ? o.Kaupunki : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@PostalCode", o.PostiKoodi != null ? o.PostiKoodi : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Country", o.Maa != null ? o.Maa : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@SupplierID", o.Id));
                cn.Open();
                return (cmd.ExecuteNonQuery() == 1);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }

        public Toimittaja Hae(int id)
        {
            string sql = "SELECT SupplierID, CompanyName, ContactName, ContactTitle, Address, City, PostalCode, Country " +
                         "FROM dbo.Suppliers " +
                         "WHERE SupplierID = @SupplierID";
            SqlConnection cn = null;
            SqlCommand cmd;
            SqlDataReader reader;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@SupplierID", id));
                cn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                reader.Read();
                return TeeRivistaToimittaja(reader);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }

        public List<Toimittaja> HaeKaikki()
        {
            string sql = "SELECT SupplierID, CompanyName, ContactName, ContactTitle, Address, City, PostalCode, Country " +
                         "FROM dbo.Suppliers " +
                         "ORDER BY SupplierID";
            SqlConnection cn = null;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cn.Open();
                return TeeToimittajaLista(new SqlCommand(sql, cn).ExecuteReader(CommandBehavior.Default));
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                if (cn != null)
                {
                    cn.Close();
                }
            }
        }

        #region Helpers

        private List<Toimittaja> TeeToimittajaLista(IDataReader reader)
        {
            List<Toimittaja> toimittajat = new List<Toimittaja>();
            while (reader.Read())
            {
                toimittajat.Add(TeeRivistaToimittaja(reader));
            }
            return toimittajat;
        }
        private Toimittaja TeeRivistaToimittaja(IDataReader reader)
        {
            ToimittajaProxy paluu = new ToimittajaProxy(
                int.Parse(reader["SupplierID"].ToString()), 
                reader["CompanyName"].ToString()
                );
            paluu.YhteysHenkilo = reader["ContactName"].ToString();
            paluu.YhteysTitteli = reader["ContactTitle"].ToString();
            paluu.Katuosoite = reader["Address"].ToString();
            paluu.Kaupunki = reader["City"].ToString();
            paluu.Maa = reader["Country"].ToString();
            paluu.PostiKoodi = reader["PostalCode"].ToString();

            //Tuote‐olioiden myöhempää populointia varten
            ((ToimittajaProxy)paluu).TuoteRepository = new TuoteRepository(ConnectionString);

            return paluu;
        }
        #endregion 
    }
}

