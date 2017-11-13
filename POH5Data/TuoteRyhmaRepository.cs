using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using POH5Luokat;

namespace POH5Data
{
    public class TuoteRyhmaRepository : DataAccess, IRepository<TuoteRyhma>
    {
        public TuoteRyhmaRepository(string yhteys)
            : base(yhteys)
        { }

        public bool Lisaa(TuoteRyhma o)
        {
            string sql = "INSERT INTO dbo.Categories(CategoryName, Description) " +
                         "VALUES (@CategoryName, @Description)";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CategoryName", o.Nimi));
                cmd.Parameters.Add(new SqlParameter("@Description", o.Kuvaus != null ? o.Kuvaus : (object)DBNull.Value));
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
            string sql = "DELETE FROM dbo.Categories " +
                         "WHERE CategoryID = @CategoryID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CategoryID", id));
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

        public bool Muuta(TuoteRyhma o)
        {
            string sql = "UPDATE dbo.Categories " +
                         "SET CategoryName = @CategoryName,  Description = @Description " +
                         "WHERE CategoryID = @CategoryID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CategoryName", o.Nimi));
                cmd.Parameters.Add(new SqlParameter("@Description", o.Kuvaus != null ? o.Kuvaus : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@CategoryID", o.Id));
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

        public TuoteRyhma Hae(int id)
        {
            string sql = "SELECT CategoryID, CategoryName, Description " +
                         "FROM dbo.Categories " +
                         "WHERE CategoryID = @CategoryID";
            SqlConnection cn = null;
            SqlCommand cmd;
            SqlDataReader reader;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CategoryID", id));
                cn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                reader.Read();
                return TeeRivistaTuoteRyhma(reader);
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

        public List<TuoteRyhma> HaeKaikki()
        {
            string sql = "SELECT CategoryID, CategoryName, Description " +
                         "FROM dbo.Categories " +
                         "ORDER BY CategoryID";
            SqlConnection cn = null;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cn.Open();
                return TeeTuoteRyhmaLista(new SqlCommand(sql, cn).ExecuteReader(CommandBehavior.Default));
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

        private List<TuoteRyhma> TeeTuoteRyhmaLista(IDataReader reader)
        {
            List<TuoteRyhma> ryhmat = new List<TuoteRyhma>();
            while (reader.Read())
            {
                ryhmat.Add(TeeRivistaTuoteRyhma(reader));
            }
            return ryhmat;
        }
        private TuoteRyhma TeeRivistaTuoteRyhma(IDataReader reader)
        {
            TuoteRyhma paluu = new TuoteRyhma(
                int.Parse(reader["CategoryID"].ToString()), 
                reader["CategoryName"].ToString()
                );

            paluu.Kuvaus = reader["Description"].ToString();

            return paluu;
        }
        #endregion 
    }
}

