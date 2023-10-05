using IMDBConsole;
using System.Data.SqlClient;

string ConnString = "server=localhost; database=MyIMDB;" +
            "user id=sa; password=bibliotek; TrustServerCertificate=True";

List<Title> titles = new List<Title>();

foreach (string line in
    System.IO.File.ReadLines
    (@"C:\Users\mikke\OneDrive\Desktop\4Semester\IMDBTSV\title.basics.tsv\data.tsv")
    .Skip(1).Take(50000))
{
    string[] values = line.Split("\t");
    if (values.Length == 9)
    {
        titles.Add(new Title(
            values[0], values[1], values[2], values[3],
            ConvertToBool(values[4]), ConvertToInt(values[5]),
            ConvertToInt(values[6]), ConvertToInt(values[7])
            ));
    }
}

Console.WriteLine(titles.Count);

Console.WriteLine("Hva' vil du?");
Console.WriteLine("1: Delete all");
Console.WriteLine("2: Normal insert");
Console.WriteLine("3: Prepared insert");
Console.WriteLine("4: Bulked insert");

string input = Console.ReadLine();

DateTime before = DateTime.Now;

SqlConnection sqlConn = new SqlConnection(ConnString);
sqlConn.Open();

IInserter? myInserter = null;

switch (input)
{
    case "1":
        Console.WriteLine("Deleting all...");
        SqlCommand cmd = new SqlCommand("DELETE FROM Titles", sqlConn);
        cmd.ExecuteNonQuery();
        break;
    case "2":
        Console.WriteLine("Inserting data...");
        myInserter = new NormalInserter();
        break;
    case "3":
        Console.WriteLine("Inserting data...");
        myInserter = new PreparedInserter();
        break;
    case "4":
        Console.WriteLine("Inserting data...");
        myInserter = new BulkInserter();
        break;
}

if (myInserter != null)
{
    myInserter.InsertData(sqlConn, titles);
}

sqlConn.Close();

DateTime after = DateTime.Now;

Console.WriteLine("Tid: " + (after - before));


bool ConvertToBool(string input)
{
    if (input == "0")
    {
        return false;
    }
    else if (input == "1")
    {
        return true;
    }
    throw new ArgumentException(
        "Kolonne er ikke 0 eller 1, men " + input);
}

int? ConvertToInt(string input)
{
    if (input.ToLower() == @"\n")
    {
        return null;
    }
    else
    {
        return int.Parse(input);
    }

}