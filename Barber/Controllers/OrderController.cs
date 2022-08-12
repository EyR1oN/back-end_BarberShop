using Barber.Calculations;
using Barber.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Barber.Controllers
{
    public static class Extensions
    {
        public static void Append<K, V>(this Dictionary<K, V> first, Dictionary<K, V> second)
        {
            List<KeyValuePair<K, V>> pairs = second.ToList();
            pairs.ForEach(pair => first.Add(pair.Key, pair.Value));
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select * from barbershop.order
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                  
                }
            }

            return new JsonResult(table);
        }

        [Authorize]
        [HttpPost("{sumTime}")]
        public JsonResult Post(List<Order> orders, int sumTime)
        {

            
            string query = @"
                        insert into barbershop.order (userId, serviceId,placeId,date,time) values
                                                    (@userId, @serviceId,@placeId,@date,@time);

            ";
            Dictionary<string, List<List<int>>> dictionaryForDefPlace = new Dictionary<string, List<List<int>>>();
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            int start_time = ConvertDateAndTime.ConvertTime((orders[0].time).ToString());
            string date1 = (orders[0].date).ToString();
          
            Dictionary<string, List<List<int>>> dictionaryConv = new Dictionary<string, List<List<int>>>();
           string corectPlaceId = ApiHelper.DefinePlaceId(dictionaryConv, sumTime, start_time, _configuration, ConvertDateAndTime.ConvertDate(date1));
           
            Console.WriteLine(corectPlaceId);
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                for (int i = 0; i < orders.Count(); i++)
                {
                   
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        myCommand.Parameters.AddWithValue("@userId", orders[i].userId);
                        myCommand.Parameters.AddWithValue("@serviceId", orders[i].serviceId);
                        myCommand.Parameters.AddWithValue("@placeId", corectPlaceId);
                        myCommand.Parameters.AddWithValue("@date", orders[i].date);
                        myCommand.Parameters.AddWithValue("@time", orders[i].time);

                        

                        myReader = myCommand.ExecuteReader(); 
                        table.Load(myReader);

                        myReader.Close();
                        
                    }
                    Console.WriteLine(orders[i]);
                }
                mycon.Close();
            }
            Console.WriteLine(start_time+ "  =  "+ sumTime + "  =  " +date1);

            return new JsonResult("Added successfully");
        }

        [Authorize]
        [HttpPut]
        public JsonResult Put(Order order)
        {
            string query = @"
                        update barbershop.order set 
                        userId =@userId,
                        serviceId =@serviceId,
                        placeId =@placeId,
                        date =@date,
                        time =@time 
                        where id=@id;
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@userId", order.userId);
                    myCommand.Parameters.AddWithValue("@serviceId", order.serviceId);
                    myCommand.Parameters.AddWithValue("@placeId", order.placeId);
                    myCommand.Parameters.AddWithValue("@date", order.date);
                    myCommand.Parameters.AddWithValue("@time", order.time);
                    myCommand.Parameters.AddWithValue("@id", order.id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                        delete from barbershop.order 
                        where id=@id;
                        
            ";
           
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }


        [HttpGet("{sumTime}/{date}")]
        public JsonResult CheckTime(int sumTime, string date)
        {

          
            string time1 = " ";

            string hour;
            string minute;
            int sumTimeMinute = 0;
            Dictionary<string, List<List<int>>> dictionary = new Dictionary<string, List<List<int>>>();
            Dictionary<string, List<List<int>>> dictionary2 = new Dictionary<string, List<List<int>>>();
           
            SortedSet<int> resultSet = new SortedSet<int>();
            bool k = true;
           
            Console.WriteLine(sumTime + "  --  " + date);
            dictionary2.Append(ApiHelper.DictionaryFill(dictionary, date, _configuration));
                foreach (var outlist1 in dictionary2.Keys)
                {
                    Console.WriteLine(";;;Key: {0}", outlist1);
                    foreach (var outlist2 in dictionary2[outlist1])
                    {
                        Console.WriteLine("... Value: {0},{1}",
                      outlist2[0], outlist2[1]);
                    }
                }
                foreach (var outlist1 in dictionary2.Keys)
                {
                 
                    for (int i = 0; i < dictionary2[outlist1].Count - 1; i++)
                    {

                        if (dictionary2[outlist1][i][1] != dictionary2[outlist1][i + 1][1])
                        {
                           

                            if (dictionary2[outlist1][i][1] + (int)sumTime <= dictionary2[outlist1][i + 1][0])
                            {

                          
                            ApiHelper.InsertTime(dictionary2[outlist1][i][1], dictionary2[outlist1][i + 1][0], sumTime, resultSet);

                            }
                        }

                    



                }
            
            }

                return new JsonResult(resultSet);
            


        }
      
      

        
    }
}
