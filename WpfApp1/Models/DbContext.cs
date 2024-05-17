using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace WpfApp1.Models
{
    //Класс который реализует
    public class DbContext
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;
        private string _cstring = "Server=localhost;Port=5433;Database=test;User Id=postgres;Password=12345;";

        public DbContext()
        {
            _connection = new NpgsqlConnection(_cstring);
        }

        //Метод открывающий соединение
        public void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        //Метод закрывающий соединение
        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        //Метод предназначен для выполнения SQL-запросов к базе данных и получения результатов
        //в виде списка словарей где, каждый словарь представляет одну строку результата.
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

        //Метод Execute используется для выполнения SQL-запросов, которые не возвращают набор данных,
        //например, запросы на вставку, обновление или удаление данных.
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
        базы данных и привести его к нужному типу данных для дальнейшей обработки, например
        генерация id.*/
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
        //Метод предназначен для начала какой-либо транзакии.
        public void BeginTransaction()
        {
            OpenConnection();
            _transaction = _connection.BeginTransaction();
        }
        //Этот метод используется для фиксации транзакции в базе данных.
        //После успешного выполнения всех операций в рамках транзакции,
        //вызов этого метода сохраняет все изменения в базе данных.
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
        //Данный метод используется для отката транзакции в случае возникновения ошибки или отмены операций.
        //Он отменяет все изменения, сделанные в рамках транзакции.
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
