using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace Diagram
{
    internal class Database
    {
        public enum RoomNames
        {
            graph1,
            graph2,
            graph3,
            graph4,
            graph5,
            graph6,
            graph7,
            graph8,
            graph9,
            graph10,
            graph11,
            graph12,
            graph13,
            graph14,
            graph15,
            graph16,
            graph17,
            graph18,
            graph19,
            graph20,
            graph21,
            graph22,
            graph23,
            graph24,
            graph25,
            graph26,
            graph27,
            graph28,
            graph29,
            graph30,
            graph31,
            graph32,
            graph33,
            graph34,
            graph35,
            graph36,
            graph37,
            graph38,
        };

        private readonly string _databaseName = "diagramrooms"; //заменить название имени бд
        private readonly string _connectString;
        private MySqlConnection _myConnection;
        bool isCreate = false;
        
        public Database()
        {
            _connectString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            if(isCreate == false)
            {
                CreateDataBase();
            }
        }

        private void CreateDataBase()
        {
            using (_myConnection = new MySqlConnection(_connectString))
            {
                _myConnection.Close();
            }

            CheckDataBase(_connectString);

            _myConnection.Close();
        }

        private void CheckDataBase(string connectString)
        {
            if (!DatabaseExists(connectString))
            {
                CreateDatabase(connectString);
                Console.WriteLine("База данных создана успешно");
            }
            else
            {
                Console.WriteLine("База данных уже существует");
            }

            _myConnection.Close();
        }

        private bool DatabaseExists(string connectString)
        {
            using (_myConnection = new MySqlConnection(connectString))
            {
                try
                {
                    _myConnection.Open();

                    string checkDatabaseQuery = $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = DATABASE()";
                    MySqlCommand command = new MySqlCommand(checkDatabaseQuery, _myConnection);

                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при проверке базы данных: {ex.Message}");
                    return false;
                }
            }
        }
        
        public List<Graph> LoadDataDb()
        {
            List<Graph> graphs = new List<Graph>();
            List<DataGraph> dataGraphs = new List<DataGraph>();
            List<DataGraph> copyDataGraphs;
            int maxValueEnum = Enum.GetValues(typeof(RoomNames)).Length;
            Graph graph;
            for (int i = 0; i < maxValueEnum; i++)
            {
                var room = Enum.GetName(typeof(RoomNames), i);
                CheckTable(room);
                using (_myConnection = new MySqlConnection(_connectString))
                {
                    try
                    {
                        _myConnection.Open();
                        string sql =
                            $"SELECT * FROM {_databaseName}.{room} " +
                            $"where idgraph = " +
                            $"(SELECT idgraph FROM {_databaseName}.{room} " +
                            $"order by idgraph desc limit 1);";
                        MySqlCommand command = new MySqlCommand(sql, _myConnection);
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            DataGraph dataGraph = new DataGraph(room, (int)reader[1], (DateTime)reader[2], reader[3].ToString(), (int)reader[4]);
                            dataGraphs.Add(dataGraph);
                        }

                        copyDataGraphs = dataGraphs.ToList();
                        graph = new Graph(copyDataGraphs);
                        graphs.Add(graph);
                        dataGraphs.Clear();

                        reader.Close();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                    finally { _myConnection.Close(); }
                }
            }
            return graphs;
        }

        private void CheckTable(string tableName = "test_table")
        {
            if (!TableExists(tableName))
            {
                CreateTable(tableName);
                Console.WriteLine("Таблица создана успешно.");
            }
            else
            {
                Console.WriteLine("Таблица уже существует.");
            }
        }

        bool TableExists(string tableName)
        {
            using (_myConnection = new MySqlConnection(_connectString))
            {
                _myConnection.Open();

                string checkTableQuery = $"SHOW TABLES LIKE '{tableName}'";
                MySqlCommand command = new MySqlCommand(checkTableQuery, _myConnection);

                using (var reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        private bool CreateDatabase(string connectString)
        {
            using (_myConnection = new MySqlConnection(connectString))
            {
                bool isComplite = false;
                try
                {
                    _myConnection.Open();

                    string createDatabaseQuery = $"CREATE DATABASE {_databaseName}";
                    MySqlCommand command = new MySqlCommand(createDatabaseQuery, _myConnection);
                    command.ExecuteNonQuery();
                    isComplite = true;
                }
                catch (Exception ex)
                {
                    isComplite = false;
                    Console.WriteLine($"Ошибка при создании базы данных: {ex.Message}");
                }
                finally
                {
                    _myConnection.Close();
                }
                return isComplite;
            }
        }

        public List<Graph> GetId(DateTime dateTime, RoomNames roomNames)
        {
            List<Graph> graphs = new List<Graph>();
            List<DataGraph> copyDataGraphs;
            List<string> idGraph = new List<string>();
            List<DataGraph> dataGraphs = new List<DataGraph>();
            Graph graph;

            string endTime = "23:00:00";
            string startTime = "00:00:00";
            string dateEnd = $"{dateTime.Year}-{dateTime.Month}-{dateTime.Day} {endTime}";
            string dateStart = $"{dateTime.Year}-{dateTime.Month}-{dateTime.Day} {startTime}";


            using (_myConnection = new MySqlConnection(_connectString))
            {
                try
                {
                    _myConnection.Open();

                    string sqlCommand = $"SELECT * FROM {_databaseName}.{roomNames} " +
                                              $"WHERE nowTime >= '{dateStart}'AND nowTime <= '{dateEnd}'";

                    MySqlCommand command = new MySqlCommand(sqlCommand, _myConnection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DataGraph dataGraph = new DataGraph($"{roomNames}", (int)reader[1], (DateTime)reader[2], reader[3].ToString(), (int)reader[4]);
                        dataGraphs.Add(dataGraph);
                    }

                    copyDataGraphs = dataGraphs.ToList();
                    graph = new Graph(copyDataGraphs);
                    graphs.Add(graph);
                    dataGraphs.Clear();
                }
                catch
                {
                    MessageBox.Show($"Ошибка при получении начала заготовки");
                }
                finally 
                { 
                    _myConnection.Close();
                }

                return graphs;
            }
        }

        public List<DataGraph> GetDiagram(RoomNames room, int diagramId) 
        {
            List<Graph> graphs = new List<Graph>();
            List<DataGraph> dataGraphs = new List<DataGraph>();
            List<DataGraph> copyDataGraphs;

            using (_myConnection = new MySqlConnection(_connectString))
            {
                try
                {
                    _myConnection.Open();
                    string sql =
                        $"SELECT * FROM {_databaseName}.{room} where idgraph = {diagramId}";
                    MySqlCommand command = new MySqlCommand(sql, _myConnection);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DataGraph dataGraph = new DataGraph(room.ToString(), (int)reader[1], (DateTime)reader[2], reader[3].ToString(), (int)reader[4]);
                        dataGraphs.Add(dataGraph);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                finally { _myConnection.Close(); }
            }

            return dataGraphs;
        }

        private bool CreateTable(string tableName)
        {
            using (_myConnection = new MySqlConnection(_connectString))
            {
                bool isComplite = false;
                try
                {
                    _myConnection.Open();

                    string createTableQuery = $"CREATE TABLE `diagramrooms`.`{tableName}` (" +
                                              "`id` INT(255) AUTO_INCREMENT," +
                                              "`idgraph` INT(5) NOT NULL," +
                                              "`nowTime` DATETIME NOT NULL," +
                                              "`value` FLOAT(10) NOT NULL," +
                                              "`time` INT(10) NOT NULL," +
                                              "PRIMARY KEY (`id`)," +
                                              "UNIQUE INDEX `idgraph2222_UNIQUE` (`id` ASC) VISIBLE);";

                    MySqlCommand command = new MySqlCommand(createTableQuery, _myConnection);
                    command.ExecuteNonQuery();
                    isComplite = true;
                }
                catch
                {
                    isComplite = false;
                    MessageBox.Show($"Ошибка при создании таблицы #{tableName}");
                }
                finally
                {
                    _myConnection.Close();
                }
                return isComplite;
            }
        }
    }
}
