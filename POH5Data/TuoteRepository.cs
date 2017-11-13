using System;
using System.Collections.Generic;
using POH5Luokat;
using System.Data.SqlClient;
using System.Data;

namespace POH5Data
{
    public class TuoteRepository : DataAccess, IRepository<Tuote>
    {
        public TuoteRepository(string yhteys)
            : base(yhteys)
        {
        }

        public bool Lisaa(Tuote o)
        {
            string sql = "INSERT INTO dbo.Products(ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, ReorderLevel, Discontinued) " +
             "VALUES (@ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @ReorderLevel, @Discontinued)";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@ProductName", o.Nimi));
                cmd.Parameters.Add(new SqlParameter("@SupplierID", o.ToimittajaId.HasValue ? o.ToimittajaId.Value.ToString() : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@CategoryID", o.RyhmaId.HasValue ? o.RyhmaId.Value.ToString() : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@QuantityPerUnit", o.YksikkoKuvaus != null ? o.YksikkoKuvaus : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@UnitPrice", o.YksikkoHinta.HasValue ? o.YksikkoHinta.ToString().Replace(',', '.').Replace(" ", "") : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@UnitsInStock", o.VarastoSaldo.HasValue ? o.VarastoSaldo.Value.ToString() : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@ReorderLevel", o.HalytysRaja.HasValue ? o.HalytysRaja.ToString() : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Discontinued", o.EiKaytossa));
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
            string sql = "DELETE FROM dbo.Products " +
                         "WHERE ProductID = @ProductName";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@ProductID", id));
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

        public bool Muuta(Tuote o)
        {
            string sql = "UPDATE dbo.Products " +
                         "SET ProductName = @ProductName, SupplierID = @SupplierID, CategoryID = @CategoryID, QuantityPerUnit =  @QuantityPerUnit, " +
                         "UnitPrice = @UnitPrice, UnitsInStock = @UnitsInStock, ReorderLevel = @ReorderLevel, Discontinued = @Discontinued " +
                         "WHERE ProductID = @ProductID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@ProductName", o.Nimi));
                cmd.Parameters.Add(new SqlParameter("@SupplierID", o.ToimittajaId.HasValue ? o.ToimittajaId.Value.ToString() : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@CategoryID", o.RyhmaId.HasValue ? o.RyhmaId.Value.ToString() : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@QuantityPerUnit", o.YksikkoKuvaus != null ? o.YksikkoKuvaus : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@UnitPrice", o.YksikkoHinta.HasValue ? o.YksikkoHinta.ToString().Replace(',', '.').Replace(" ", "") : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@UnitsInStock", o.VarastoSaldo.HasValue ? o.VarastoSaldo.Value.ToString() : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@ReorderLevel", o.HalytysRaja.HasValue ? o.HalytysRaja.ToString() : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Discontinued", o.EiKaytossa));
                cmd.Parameters.Add(new SqlParameter("@ProductID", o.Id));
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

        public Tuote Hae(int id)
        {
            string sql = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, ReorderLevel, Discontinued " +
                         "FROM dbo.Products " +
                         "WHERE ProductID = @ProductID";
            SqlConnection cn = null;
            SqlCommand cmd;
            SqlDataReader reader;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@ProductID", id));
                cn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                reader.Read();
                return TeeRivistaTuote(reader);
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

        public List<Tuote> HaeRyhmanKaikki(int id)
        {
            string sql = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, ReorderLevel, Discontinued " +
                         "FROM dbo.Products " +
                         "WHERE CategoryID = @CategoryID " +
                         "ORDER BY ProductID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CategoryID", id));
                cn.Open();
                return TeeTuoteLista(cmd.ExecuteReader(CommandBehavior.Default));
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

        public List<Tuote> HaeToimittajanKaikki(int id)
        {
            string sql = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, ReorderLevel, Discontinued " +
                         "FROM dbo.Products " +
                         "WHERE SupplierID = @SupplierID " +
                         "ORDER BY ProductID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@SupplierID", id));
                cn.Open();
                return TeeTuoteLista(cmd.ExecuteReader(CommandBehavior.Default));
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

        public List<Tuote> HaeKaikki()
        {
            string sql = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, ReorderLevel, Discontinued " +
                         "FROM dbo.Products " +
                         "ORDER BY ProductID";
            SqlConnection cn = null;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cn.Open();
                return TeeTuoteLista(new SqlCommand(sql, cn).ExecuteReader(CommandBehavior.Default));
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

        #region Helpers
        private List<Tuote> TeeTuoteLista(IDataReader reader)
        {
            List<Tuote> tuotteet = new List<Tuote>();
            while (reader.Read())
            {
                tuotteet.Add(TeeRivistaTuote(reader));
            }
            return tuotteet;
        }

        private Tuote TeeRivistaTuote(IDataReader reader)
        {
            TuoteProxy paluu = new TuoteProxy(
                int.Parse(reader["ProductID"].ToString()), 
                reader["ProductName"].ToString()
                );

            if (!(reader["SupplierID"] is DBNull))
            {
                paluu.ToimittajaId = int.Parse(reader["SupplierID"].ToString());
            }
            else
            {
                paluu.ToimittajaId = null;
            }

            if (!(reader["CategoryID"] is DBNull))
            {
                paluu.RyhmaId = int.Parse(reader["CategoryID"].ToString());
            }
            else
            {
                paluu.RyhmaId = null;
            }

            if (!(reader["UnitsInStock"] is DBNull))
            {
                paluu.VarastoSaldo = int.Parse(reader["UnitsInStock"].ToString());
            }
            else
            {
                paluu.VarastoSaldo = null;
            }

            if (!(reader["ReorderLevel"] is DBNull))
            {
                paluu.HalytysRaja = int.Parse(reader["ReorderLevel"].ToString());
            }
            else
            {
                paluu.HalytysRaja = null;
            }

            if (!(reader["UnitPrice"] is DBNull))
            {
                paluu.YksikkoHinta = double.Parse(reader["UnitPrice"].ToString().Replace('.', ','));
            }
            else
            {
                paluu.YksikkoHinta = null;
            }

            paluu.YksikkoKuvaus = reader["QuantityPerUnit"].ToString();
            paluu.EiKaytossa = bool.Parse(reader["Discontinued"].ToString());

            //Toimittaja ja TuoteRyhma‐olioiden myöhempää populointia varten
            ((TuoteProxy)paluu).ToimittajaRepository = new ToimittajaRepository(ConnectionString);
            ((TuoteProxy)paluu).TuoteRyhmaRepository = new TuoteRyhmaRepository(ConnectionString);

            return paluu;
        }
        #endregion
    }
}

