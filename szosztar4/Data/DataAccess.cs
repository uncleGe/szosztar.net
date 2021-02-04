using FirebaseAdmin.Auth;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using szosztar.Models;
using szosztar.Data.Interfaces;

namespace szosztar.Data
{
    public class DataAccess: IDataAccess
    {
        public readonly NpgsqlConnectionStringBuilder builder;
        private readonly IConfiguration config;
        public DataAccess(IConfiguration config)
        {
            this.config = config;
            this.builder = new NpgsqlConnectionStringBuilder
            {
                Host = config.GetValue<string>("DatabaseConnection:Host"),
                Port = config.GetValue<int>("DatabaseConnection:Port"),
                Database = config.GetValue<string>("DatabaseConnection:Database"),
                Username = config.GetValue<string>("DatabaseConnection:Username"),
                Password = config.GetValue<string>("DatabaseConnection:Password")
            };
        }

        public async Task<User> GetUser(string externalId)
        {
            var user = new User();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuerying users...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    var sql = "SELECT user_id, external_id FROM public.users " +
                              $"WHERE external_id = '{externalId}'" +
                              ";";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                user.userId = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0);
                                user.externalId = reader.IsDBNull(1) ? null : reader.GetString(1);
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                return null;
            }
            Console.WriteLine("\nDone.");

            return user;
        }

        public async Task<bool> PostUser(User user)
        {
            if (user == null ||
                user.username == null ||
                String.IsNullOrEmpty(user.externalId))
            {
                return false;
            }

            var results = 0;
            try
            {
                //TODO: check for dupes before posting
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nPosting data...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    var sql =
                        "INSERT INTO public.users (external_id, username) " +
                        $"VALUES('{user.externalId}', '{user.username}')" +
                              ";";

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

        public async Task<IList<string>> GetCategories(string externalId)
        {
            var user = await GetUser(externalId);
            if (user == null)
            {
                return null;
            }

            var results = new List<string>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuerying data...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    var sql = "SELECT category FROM public.categories " +
                              $"WHERE user_id = {user.userId}" +
                              ";";

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

        public async Task<IDictionary<string, int?>> GetAndMapCategories(string externalId)
        {
            var user = await GetUser(externalId);
            if (user == null)
            {
                return null;
            }

            var resultsDict = new Dictionary<string, int?>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuerying data...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    var sql = "SELECT * FROM public.categories " +
                              $"WHERE user_id = {user.userId}" +
                              ";";

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

        public async Task<bool> PostCategory(string externalId, string category)
        {
            var user = await GetUser(externalId);
            if (user == null)
            {
                return false;
            }

            var results = 0;
            try
            {
                //TODO: check for dupes before posting
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nPosting data...");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    var sql =
                        "INSERT INTO public.categories (category, user_id) " +
                        $"VALUES('{category}', {user.userId})" +
                              ";";

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

        public async Task<IList<Word>> GetWords(string externalId)
        {
            var user = await GetUser(externalId);
            if (user == null)
            {
                return null;
            }

            var results = new List<Word>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    var categoryDict = await GetAndMapCategories(externalId);
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

                    var sql = "SELECT * FROM public.words " +
                             $"WHERE user_id = {user.userId}" +
                              ";";

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


        public async Task<bool> PostWord(string externalId, Word word)
        {
            var user = await GetUser(externalId);
            if (user == null)
            {
                return false;
            }

            var results = 0;
            try
            {
                //TODO: check for dupes before posting
                using (NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nGetting Category data...");
                    Console.WriteLine("=========================================\n");
                    var categoryDict = await GetAndMapCategories(externalId);
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
                        "INSERT INTO public.words (english, hungarian, category_id, notes, user_id) " +
                        $"VALUES('{word.english}', '{word.hungarian}', {category}, '{notes}', {user.userId})" +
                              ";";

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
