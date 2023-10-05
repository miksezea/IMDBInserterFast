using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace IMDBConsole
{
    public class NormalInserter
    {
        private readonly string ConnString = "server=localhost; database=MyIMDB;" +
            "user id=sa; password=bibliotek; TrustServerCertificate=True";
        
        public void InsertData(List<Title> titles)
        {
            SqlConnection sqlConn = new SqlConnection(ConnString);
            sqlConn.Open();

            foreach (Title title in titles)
            {
                SqlCommand sqlCommand = new SqlCommand(
                    "INSERT INTO [dbo].[Titles]" +
                    "([tconst],[titleType],[primaryTitle],[originalTitle]," +
                    "[isAdult],[startYear],[endYear],[runtimeMinutes])" +
                    "VALUES " +
                    $"('{title.tconst}','{title.titleType}','{title.primaryTitle.Replace("'", "''")}'," +
                    $"'{title.originalTitle.Replace("'", "''")}','{title.isAdult}',{CheckIntForNull(title.startYear)}," +
                    $"{CheckIntForNull(title.endYear)},{CheckIntForNull(title.runtimeMinutes)})", sqlConn);

                try
                {
                    sqlCommand.ExecuteNonQuery();
                } catch (Exception ex)
                {
                    Console.WriteLine(sqlCommand.CommandText);
                }
            }
        }

        public string CheckIntForNull(int? input)
        {
            if (input == null)
            {
                return "NULL";
            } else
            {
                return "" + input;
            }
        }
    }
}
