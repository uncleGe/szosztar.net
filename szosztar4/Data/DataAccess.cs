using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using szosztar.Models;
using szosztar.Data.Interfaces;
using Npgsql;
using Microsoft.Extensions.Configuration;

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

        private readonly IConfiguration config;
        public DataAccess(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<IList<string>> GetCategories()
        {
            var results = new List<string>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuerying data...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    var sql = "SELECT category FROM public.categories";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var newWord = new Word();

                                results.Add(reader.GetString(0));
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

        public async Task<IDictionary<string, int?>> GetAndMapCategories()
        {
            var resultsDict = new Dictionary<string, int?>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuerying data...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    var sql = "SELECT * FROM public.categories";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var newWord = new Word();

                                resultsDict.Add(reader.GetString(1), reader.GetInt32(0));
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

            return resultsDict;
        }

        public async Task<bool> PostCategory(string category)
        {
            var results = 0;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nPosting data...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    var sql =
                        "INSERT INTO public.categories (category)" +
                        $"VALUES('{category}'); ";

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

        public async Task<IList<Word>> GetWords()
        {
            var results = new List<Word>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nGetting Category data...");
                    Console.WriteLine("=========================================\n");
                    var categoryDict = await GetAndMapCategories();
                    if (categoryDict == null)
                    {
                        Console.WriteLine("\n Null Category data...");
                        Console.WriteLine("=========================================\n");
                        return null;
                    }

                    var reversedCategoryDict = categoryDict.ToDictionary(x => x.Value, x => x.Key);

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
                                newWord.category = !reader.IsDBNull(3) && reversedCategoryDict.TryGetValue(reader.GetInt32(3), out var category)
                                    ? category
                                    : null;
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
                    Console.WriteLine("\nGetting Category data...");
                    Console.WriteLine("=========================================\n");
                    var categoryDict = await GetAndMapCategories();
                    if (categoryDict == null)
                    {
                        Console.WriteLine("\n Null Category data...");
                        Console.WriteLine("=========================================\n");
                        return false;
                    }

                    Console.WriteLine("\nPosting data...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    string category;
                    if (categoryDict.TryGetValue(word.category, out int? value) && value != null)
                    {
                        category = value.ToString();
                    }
                    else
                    {
                        category = "null";
                    }

                    string notes = word.notes ?? "null";

                    var sql =
                        "INSERT INTO public.words (english, hungarian, category_id, notes) " +
                        $"VALUES('{word.english}', '{word.hungarian}', {category}, '{notes}');";

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
