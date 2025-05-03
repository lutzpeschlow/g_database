using System.CodeDom;
using System.Data;
using System.Data.SQLite;



namespace ExampleSqlite
{



class Processing
// class for work on database, write new infomation into database
//
// - manage new common data
// - write score into according tables
{

    // variable from main to be defined in this class via constructor
    private string _dbLoc;
    private string _path;



    // constructor for class
    public Processing(string dbLoc, string path)
    {
        _dbLoc = dbLoc;
        _path = path;
    }


    // get files matching to criteria
    private List<string> GetMatchingFiles(string directoryPath)
        {
            Console.WriteLine("try to read files in directory ...");
            List<string> matchingFiles = new List<string>();
            try
            {
                string[] files = Directory.GetFiles(directoryPath, "text_to_db_*.txt");
                matchingFiles.AddRange(files);
                Console.WriteLine("in try ..." + files.Length);
                foreach (string str in files)
                {
                    Console.WriteLine(str);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error reading directory: {ex.Message}");
            }
        return matchingFiles;
        }


    // parent function calling txt to database for each file found in directory
    public void FilesToDatabase()
    {
        List<string> matchingFiles = GetMatchingFiles(_path);

        foreach (string filePath in matchingFiles)
        {
            Console.WriteLine(filePath);
            TextToDatabase(filePath);
        }
    }


    public void TextToDatabase(string filePath)
    {
        // (0)
        // variables
        long MaxPrimId = 0;
        // string filePath = @"C:\tmp\text_to_db.txt";
        // string filePath = @"/tmp/text_to_db.txt";
        string tableName = "";
        Dictionary<string, string> columnValues = new Dictionary<string, string>();
        // (1)
        // read text file and assign values to dictionary: columnValues
        foreach (string line in File.ReadLines(filePath))
        {
            if (line.StartsWith("table:"))
            {
                tableName = line.Split(':')[1].Trim();
            }
            else if (line.Contains(":"))
            {
                var parts = line.Split(':');
                string columnName = parts[0].Trim();
                string columnValue = parts[1].Trim();
                columnValues[columnName] = columnValue;
            }
        }
        // output of content of columnValues
        Console.WriteLine($" Table:{tableName}");
        foreach (KeyValuePair<string, string> kvp in columnValues)
        {
            Console.WriteLine($"    Key:{kvp.Key},    Value:{kvp.Value}");
            string myKey = kvp.Key;
        }
        // (2)
        // open sqlite database
        string zeitString = DateTime.Now.ToString("HH:mm:ss");
        // string atmyKey = "@" + myKey;
        using (var conn = new SQLiteConnection($"Data Source={_dbLoc};Version=3;"))    
        {
            // open database connection
            conn.Open();
            // find out primary key in current table
            // key 5 is set to 1 for primary key, key 1 is name, key 2 is datatype ...
            using (SQLiteCommand cmd = new SQLiteCommand($"PRAGMA table_info({tableName});", conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(5) == 1)
                        {
                            string search_name = reader.GetString(1);
                            Console.WriteLine(search_name);
                        }
                    }
                }
            }
            // find out number of entries in table and define max id
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT MAX(club_id) FROM GolfClubs", conn))
            {
                var res_object = cmd.ExecuteScalar();
                MaxPrimId = (res_object is null || res_object == DBNull.Value) ? 0 : Convert.ToInt64(res_object);
                Console.WriteLine(MaxPrimId);
            }
            // loop over all entries in dictionary and write into new table entry
            long last_row_id = -1;
            foreach (KeyValuePair<string, string> kvp in columnValues)
            {
                Console.WriteLine($"    {kvp.Key}-{kvp.Value}");
                string myKey = kvp.Key;
                // first pass: INSERT + ID abrufen
                if (last_row_id < 0)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand($"INSERT INTO {tableName} ({kvp.Key}) VALUES (@value); " +
                                                                  "SELECT last_insert_rowid();",conn))
                    {
                        cmd.Parameters.AddWithValue("@value", kvp.Value);
                        last_row_id = Convert.ToInt64(cmd.ExecuteScalar()); // Direkte Zuweisung
                        Console.WriteLine($"Neue ID: {last_row_id}");
                    }
                }
                // follow-up runs: UPDATE same row
                else
                {
                    using (SQLiteCommand updateCmd = new SQLiteCommand($"UPDATE {tableName} SET {kvp.Key} = @value " + 
                                                                       "WHERE rowid = @id", conn))
                    {
                        updateCmd.Parameters.AddWithValue("@value", kvp.Value);
                        updateCmd.Parameters.AddWithValue("@id", last_row_id);
                        updateCmd.ExecuteNonQuery();
                        Console.WriteLine($"Updated {kvp.Key} in row {last_row_id}");
                    }
                }
            }
        }
    }















            //long last_row_id = -1;
            //foreach (KeyValuePair<string, string> kvp in columnValues)
            //{
            //    Console.WriteLine($"    {kvp.Key}-{kvp.Value}");
            //    string myKey = kvp.Key;
//
            //    // first write data case
            //    if (last_row_id < 0)
            //    {
            //        using (SQLiteCommand cmd = new SQLiteCommand($"INSERT INTO {tableName} (address) VALUES (@address);", conn))
            //        {
            //            cmd.Parameters.AddWithValue("@address", "new adress string");
            //            cmd.ExecuteNonQuery();
            //        }
            //        // 2. Neue ID abfragen
            //        using (var idCmd = new SQLiteCommand("SELECT last_insert_rowid()", conn))
            //        {
            //            last_row_id = Convert.ToInt64(idCmd.ExecuteScalar());
            //            Console.WriteLine($"Neue ID: {last_row_id}");
            //        }
            //    }
            //    else
            //    {
//
            //    }
//
            //}
            
            
            
        

            // using (SQLiteCommand cmd = new SQLiteCommand("SELECT last_insert_rowid()", conn))
            // {
            //     var res_object = cmd.ExecuteScalar();
            //    MaxPrimId = Convert.ToInt64(res_object);
            //     Console.WriteLine(MaxPrimId);
            // }

            // // add working value
            // string newAddress = $"new_adress_{zeitString}";
            // // insert value into GolfClubs
            // string sql = "INSERT INTO GolfClubs (address) VALUES (@address);";
            // Console.WriteLine(sql);
            // using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            // {
            //    cmd.Parameters.AddWithValue("@address", newAddress);
            //    cmd.ExecuteNonQuery();
            // }
            // Console.WriteLine($"added: {newAddress}");
            // // add non working value
            // sql = $"INSERT INTO {tableName} ({myKey}) VALUES (@{myKey});";
            // Console.WriteLine(sql);
            // using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            // {
            //    cmd.Parameters.AddWithValue(atmyKey, myValue);
            //    cmd.ExecuteNonQuery();
            // }

        
        //     string columns = string.Join(", ", columnValues.Keys);
        //     string values = string.Join(", ", columnValues.Values);
        //     string sql = $"INSERT INTO {tableName} ({columns}) VALUES ({string.Join(", ", columnValues.Keys)})";
        //     Console.WriteLine(columns);
        //     Console.WriteLine(values);
        //     Console.WriteLine(sql);
        //     using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        //     {
        //         // Parameter hinzufügen, um SQL-Injection zu vermeiden
        //         foreach (var kvp in columnValues)
        //         {
        //             cmd.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
        //         }
        //         cmd.ExecuteNonQuery();
        //     }
        
        // Console.WriteLine("data written ...");
    






    public void WriteToDatabase(Score score)
    {
        Console.WriteLine("Write to Database");
        
        using (var conn = new SQLiteConnection($"Data Source={_dbLoc};Version=3;"))
        {
            conn.Open();

            // input data into table
            // TABLE: TOURNAMENT
            string createT = @"CREATE TABLE IF NOT EXISTS Tournament (Name TEXT,Datum TEXT)";
            using (SQLiteCommand createTableCmd = new SQLiteCommand(createT, conn))
            {
                createTableCmd.ExecuteNonQuery();
            }

            
            
            string insertT = "INSERT INTO Tournament (Name, Datum) VALUES (@Name, @Datum)";
            using (SQLiteCommand insertCmd = new SQLiteCommand(insertT, conn))
            {
                insertCmd.Parameters.AddWithValue("@Name", score.TName);
                insertCmd.Parameters.AddWithValue("@Datum", score.TDate);
                insertCmd.ExecuteNonQuery();
            }

            // TABLE: PLAYER
            string createP = @"CREATE TABLE IF NOT EXISTS Player (Name TEXT,Datum TEXT)";
            using (SQLiteCommand createTableCmd = new SQLiteCommand(createP, conn))
            {
                createTableCmd.ExecuteNonQuery();
            }
            string insertP = "INSERT INTO Player (Name) VALUES (@Name)";
            using (SQLiteCommand insertCmd = new SQLiteCommand(insertP, conn))
            {
                insertCmd.Parameters.AddWithValue("@Name", score.TPlayer);
                insertCmd.ExecuteNonQuery();
            }            
                    
        }
    }



    public void ReadFromDatabase()
    {
        Console.WriteLine("Read from Database ...");
        
        using (var conn = new SQLiteConnection($"Data Source={_dbLoc};Version=3;"))
        {
            conn.Open();
            string query = "SELECT Name, Datum FROM Tournament";

            using (SQLiteCommand command = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);  // Spalte 'Name'
                        string datum = reader.GetString(1); // Spalte 'Datum'

                        Console.WriteLine($"Name: {name}, Datum: {datum}");
                    }
                }
            }

        }
    }

    

    public void DebugPrintout()
    // DebugPrintout
    // printout of information for debugging issues
    {
        Console.WriteLine("debug printout of database access");
    }
}

}



// store:


            // get all column entris from table
            // string sql = "PRAGMA table_info(GolfClubs);";
            // using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            // {
            //     using (SQLiteDataReader reader = cmd.ExecuteReader())
            //     {
            //         Console.WriteLine("Column Names: ");
            //         while (reader.Read())
            //         {
            //             Console.WriteLine(reader["name"]);
            //         }
            //             
            //         Console.WriteLine();
            //     }
            // }
            // find max primary ID in table GolfClubs
            // sql = "SELECT ISNULL(MAX(Id), 0) FROM GolfClubs";
            // using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            // {
            //     object result = cmd.ExecuteScalar();
            //     maxId = Convert.ToInt32(result);
            // }
            // int newId = maxId + 1;