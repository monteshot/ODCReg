using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrachMedcentr
{
    class SynhronyzeClass
    {
        conBD conWeb = new conBD();
        conBD conLocal = new conBD("shostka.mysql.ukraine.com.ua", "shostka_medcen", "shostka_medcen", "n5t7jzqv");

        #region Synhronyza Tables

        public void SynhronyzeTable(string _TableName)
        {
            DataTable Web = GetTable("ekfgq_ttfsp_dop", conWeb);
            DataTable Local = GetTable("ekfgq_ttfsp_dop", conLocal);

            //Web = con.get3apTime();
            //Local = conLocal.get3apTime();

            Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
            Any(rl => rl.Field<int>("id") == rw.Field<int>("id") && rl.Field<MySqlDateTime>("date").GetDateTime() == rw.Field<MySqlDateTime>("date").GetDateTime())).CopyToDataTable();

            List<DataRow> t = Local.AsEnumerable().ToList<DataRow>();

            foreach (var a in t)
            {
                DateTime temp = a.Field<MySqlDateTime>("date").GetDateTime();
            }
            int i = 0;//маркер точки останова


        }

        /// <summary>
        /// Функция получения Таблицы
        /// </summary>
        /// <param name="_WebTablName">Имя желаемой таблицы</param>
        /// <param name="_DBConnection">Параметры подключения(База данных локальная/веб)</param>
        /// <returns></returns>
        private DataTable GetTable( string _WebTablName , conBD _DBConnection)
        {
            
            MySqlConnectionStringBuilder mysqlCSB;
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = _DBConnection.server;
            mysqlCSB.Database = _DBConnection.database;
            mysqlCSB.UserID = _DBConnection.UserID;
            mysqlCSB.Password = _DBConnection.Password;
            // mysqlCSB.ConvertZeroDateTime = true;
            mysqlCSB.AllowZeroDateTime = true;

            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = mysqlCSB.ConnectionString;
            MySqlCommand cmd = new MySqlCommand();
            con.Open();
            cmd.CommandText = "SELECT * FROM "+ _WebTablName;
            //cmd.Parameters.AddWithValue("@TableName", _WebTablName);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();

            MySqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            con.Close();
            return dt;
        }

        #endregion
    }
}
