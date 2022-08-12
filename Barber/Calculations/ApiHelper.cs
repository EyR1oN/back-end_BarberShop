using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Barber.Calculations
{
    public static class Extensions
    {
        public static void Append<K, V>(this Dictionary<K, V> first, Dictionary<K, V> second)
        {
            List<KeyValuePair<K, V>> pairs = second.ToList();
            pairs.ForEach(pair => first.Add(pair.Key, pair.Value));
        }
    }
    public class ApiHelper
    {
        public static SortedSet<int> InsertTime(int begin, int end, int sum, SortedSet<int> resultSet)
        {
            double h = Math.Ceiling((double)(begin / 30));
            begin = (int)h * 30;
            for (int i = begin; i + sum <= end; i = i + 30)
            {

                resultSet.Add(i);
            }
            return resultSet;
        }

        public static Dictionary<string, List<List<int>>> DictionaryFill(Dictionary<string, List<List<int>>> dictionary, string date, IConfiguration _configuration)
        {

            string query = @"
             SELECT * FROM barbershop.order INNER JOIN service 
                                on service.id=barbershop.order.serviceId WHERE date=@date;
            ";
            string query2 = @" SELECT * FROM place;";

            DataTable table = new DataTable();
            DataTable table2 = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                  
                    myCommand.Parameters.AddWithValue("@date", date);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                }
                using (MySqlCommand myCommand = new MySqlCommand(query2, mycon))
                {

                    myReader = myCommand.ExecuteReader();
                    table2.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                  
                }
                for (int i = 0; i < table2.Rows.Count; i++)
                {
                    dictionary[table2.Rows[i]["id"].ToString()] = new List<List<int>>();
                    List<int> start = new List<int>();
                    start.Add(0);
                    start.Add(540);
                    List<int> end = new List<int>();
                    end.Add(1080);
                    end.Add(0);
                    dictionary[table2.Rows[i]["id"].ToString()].Add(start);
                    dictionary[table2.Rows[i]["id"].ToString()].Add(end);
                }
                Console.WriteLine("in prog " + table.Rows.Count);
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    List<int> addToList = new List<int>();
                    addToList.Add(ConvertDateAndTime.ConvertTime(table.Rows[i]["time"].ToString()));
                   
                    addToList.Add(ConvertDateAndTime.ConvertTime(table.Rows[i]["time"].ToString()) + ConvertDateAndTime.ConvertTime(table.Rows[i]["timeToMake"].ToString()));
                    dictionary[table.Rows[i]["placeId"].ToString()].Add(addToList);

                }
                dictionary = dictionary
                    .OrderBy(d => d.Key)
                       .ToDictionary(
                          d => d.Key,
                      d => (List<List<int>>)d.Value.OrderBy(v => v[0]).ToList());

                return dictionary;
            }

        }
        public static string DefinePlaceId(Dictionary<string, List<List<int>>> dictionary, int sumTime, int start_time, IConfiguration _configuration, string date)
        {
            Dictionary<string, List<List<int>>> dictionaryConv = new Dictionary<string, List<List<int>>>();
            dictionaryConv.Append(DictionaryFill(dictionary, date, _configuration));
          
            foreach (var outlist1 in dictionaryConv.Keys)
            {
               
                for (int i = 0; i < dictionaryConv[outlist1].Count - 1; i++)
                {

                    if (dictionaryConv[outlist1][i][1] != dictionaryConv[outlist1][i + 1][0])
                    {
                        

                        if (start_time >= dictionaryConv[outlist1][i][1] && start_time + sumTime <= dictionaryConv[outlist1][i + 1][0])
                        {
                            return outlist1;


                        }
                    }
                }
            }
            return "error";
        }
    }
}
