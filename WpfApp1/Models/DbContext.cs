using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace WpfApp1.Models
{
    public class DbContext
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;
        private string _cstring;

        public DbContext(string connectionString)
        {
            _cstring = connectionString;
            _connection = new NpgsqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        
        public List<Dictionary<string, object>> ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            OpenConnection();
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            using (NpgsqlCommand command = new NpgsqlCommand(query, _connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader[i];
                        }

                        results.Add(row);
                    }
                }
            }

            return results;
        }


        public void Execute(string query, Dictionary<string, object> parameters = null)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(query, _connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                command.ExecuteNonQuery();
            }
        }

       /* Этот метод  возвращает результат выполнения SQL-запроса в виде объекта,
        который затем преобразуется в указанный тип T с помощью Convert.ChangeType.Таким образом,
        метод ExecuteScalar позволяет получить скалярное значение из
        базы данных и привести его к нужному типу данных для дальнейшей обработки.*/
        public T ExecuteScalar<T>(string query, Dictionary<string, object> parameters = null)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(query, _connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                object result = command.ExecuteScalar();
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        public void BeginTransaction()
        {
            OpenConnection();
            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
            CloseConnection();
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
            CloseConnection();
        }

    }
}
