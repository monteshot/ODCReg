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
        string TableName;
        DataTable Web = new DataTable();
        DataTable Local = new DataTable();

        #region Synhronyza Tables

        public void SynhronyzeTable(string _TableName)
        {
            TableName = _TableName;
             Web = GetTable( conWeb);
             Local = GetTable( conLocal);
            

            //Web = con.get3apTime();
            //Local = conLocal.get3apTime();
            List<DataRow> t0 = Local.AsEnumerable().ToList<DataRow>();
            List<DataRow> t1 = Web.AsEnumerable().ToList<DataRow>();
            Compare(1);

            List<DataRow> t = Local.AsEnumerable().ToList<DataRow>();

            foreach (var a in t)
            {
                DateTime temp = a.Field<MySqlDateTime>("date").GetDateTime();
            }
            int i = 0;//маркер точки останова


        }

        /// <summary>
        /// Функция нахождения розличий между дата тейблами
        /// </summary>
        /// <param name="_mod">
        /// 1-найти розличия между локальной и веб базами(простыми словами нахождение в веб базе того чего не хватает в локальной)
        /// 2-найти розличия между веб и локальной базами(простыми словами нахождение в локальной базе того чего не хватает в веб)
        /// </param>
        private void  Compare(int _mod)
        {
            switch (TableName)
            {
                
                case "ekfgq_ttfsp_dop":
                    if (_mod == 1)
                    {
                        //        Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                        //Any(rl => rl.Field<int>("id") == rw.Field<int>("id") && rl.Field<MySqlDateTime>("date").GetDateTime() == rw.Field<MySqlDateTime>("date").GetDateTime())).CopyToDataTable();
                        Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                        Any(rl => rl.Field<string>("rfio") == rw.Field<string>("rfio") 
                        && rl.Field<MySqlDateTime>("date").GetDateTime() == rw.Field<MySqlDateTime>("date").GetDateTime() 
                        && rl.Field<string>("hours") == rw.Field<string>("hours")
                        && rl.Field<string>("minutes") == rw.Field<string>("minutes") 
                        && rl.Field<string>("specializations_name") == rw.Field<string>("specializations_name")) ).CopyToDataTable();
                    }
                    else
                    {

                    }
                    break;

            }

            

        }

        /// <summary>
        /// Функция получения Таблицы
        /// </summary>
        /// <param name="_WebTablName">Имя желаемой таблицы</param>
        /// <param name="_DBConnection">Параметры подключения(База данных локальная/веб)</param>
        /// <returns></returns>
        private DataTable GetTable(conBD _DBConnection)
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
            cmd.CommandText = "SELECT * FROM "+ TableName+" LIMIT 0,100";
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
