using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBConsole
{
    public class PreparedInserter : IInserter
    {
        public void InsertData(SqlConnection sqlConn, List<Title> titles)
        {
            SqlCommand sqlCmd = new SqlCommand("" +
                        "INSERT INTO [dbo].[Titles]" +
                        "([tconst],[titleType],[primaryTitle],[originalTitle]" +
                        ",[isAdult],[startYear],[endYear],[runTimeMinutes])" +
                        "VALUES " +
                        $"(@tconst," +
                        $"@titleType," +
                        $"@primaryTitle," +
                        $"@originalTitle," +
                        $"@isAdult," +
                        $"@startYear," +
                        $"@endYear," +
                        $"@runtimeMinutes)"
                        , sqlConn);
            SqlParameter tconstParameter = new SqlParameter("@tconst",
                System.Data.SqlDbType.VarChar, 10);
            sqlCmd.Parameters.Add(tconstParameter);

            SqlParameter titleTypeParameter = new SqlParameter("@titleType",
                System.Data.SqlDbType.VarChar, 50);
            sqlCmd.Parameters.Add(titleTypeParameter);

            SqlParameter primaryTitleParameter = new SqlParameter("@primaryTitle",
                System.Data.SqlDbType.VarChar, 8000);
            sqlCmd.Parameters.Add(primaryTitleParameter);

            SqlParameter originalTitleParameter = new SqlParameter("@originalTitle",
                System.Data.SqlDbType.VarChar, 8000);
            sqlCmd.Parameters.Add(originalTitleParameter);

            SqlParameter isAdultParameter = new SqlParameter("@isAdult",
                System.Data.SqlDbType.Bit);
            sqlCmd.Parameters.Add(isAdultParameter);

            SqlParameter startYearParameter = new SqlParameter("@startYear",
                System.Data.SqlDbType.Int);
            sqlCmd.Parameters.Add(startYearParameter);

            SqlParameter endYearParameter = new SqlParameter("@endYear",
                System.Data.SqlDbType.Int);
            sqlCmd.Parameters.Add(endYearParameter);

            SqlParameter runtimeMinutesParameter = new SqlParameter("@runtimeMinutes",
                System.Data.SqlDbType.Int);
            sqlCmd.Parameters.Add(runtimeMinutesParameter);

            sqlCmd.Prepare();

            int counter = 0;

            foreach (Title title in titles)
            {
                FillParameter(tconstParameter, title.tconst);
                FillParameter(titleTypeParameter, title.titleType);
                FillParameter(primaryTitleParameter, title.primaryTitle);
                FillParameter(originalTitleParameter, title.originalTitle);
                FillParameter(isAdultParameter, title.isAdult);
                FillParameter(startYearParameter, title.startYear);
                FillParameter(endYearParameter, title.endYear);
                FillParameter(runtimeMinutesParameter, title.runtimeMinutes);
                try
                {
                    sqlCmd.ExecuteNonQuery();
                    counter++;
                    Console.WriteLine(counter + "...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(sqlCmd.CommandText);
                    Console.ReadKey();
                }

            }
        }

        public void FillParameter(SqlParameter parameter, object? value)
        {
            if (value != null)
            {
                parameter.Value = value;
            }
            else
            {
                parameter.Value = DBNull.Value;
            }
        }
    }
}
