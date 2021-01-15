using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using szosztar.Models;
using szosztar.Data.Interfaces;
using Npgsql;

namespace szosztar.Data
{
    public class DataAccess: IDataAccess
    {
        public readonly NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
        {
            Host = "",
            Port = 0,
            Database = "",
            Username = "",
            Password = "",
        };

        public async Task<IList<Word>> GetWords()
        {
            var results = new List<Word>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuerying data...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    var sql = "SELECT * FROM public.words";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var newWord = new Word();

                                newWord.id = reader.GetInt32(0);
                                newWord.english = reader.GetString(1);
                                newWord.hungarian = reader.GetString(2);
                                newWord.category = reader.IsDBNull(3) ? (Category?)null : (Category)reader.GetInt32(3);
                                newWord.notes = reader.IsDBNull(4) ? null : reader.GetString(4);

                                results.Add(newWord);
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }
            Console.WriteLine("\nDone.");

            return results;
        }


        public async Task<bool> PostWord(Word word)
        {
            var results = 0;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nPosting data...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    // If the category is in enum, cast it to int and post it
                    // TODO: this enum sucks cuz the user should be able to create categories
                    var category = new object();
                    if (word.category != null && Enum.IsDefined(typeof(Category), word.category))
                    {
                        category = (int?)word.category;
                    }
                    else
                    {
                        category = "null";
                    }

                    var notes = word.notes ?? "";

                    var sql =
                        "INSERT INTO public.words (english, hungarian, category_id, notes) " +
                        $"VALUES('{word.english}', '{word.hungarian}', {category}, '{word.notes}');";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        results = await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }
            Console.WriteLine("\nDone");

            return results > 0;
        }
    }
}
