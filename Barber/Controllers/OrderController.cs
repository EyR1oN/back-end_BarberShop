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
                    //  table.Clear();
                }
            }

            return new JsonResult(table);
        }

        [Authorize]
        [HttpPost]
        public JsonResult Post(List<Order> orders)
        {

            
            string query = @"
                        insert into barbershop.order (userId, serviceId,placeId,date,time) values
                                                    (@userId, @serviceId,@placeId,@date,@time);

            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                for (int i = 0; i < orders.Count(); i++)
                {
                   
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        myCommand.Parameters.AddWithValue("@userId", orders[i].userId);
                        myCommand.Parameters.AddWithValue("@serviceId", orders[i].serviceId);
                        myCommand.Parameters.AddWithValue("@placeId", orders[i].placeId);
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

            string query = @"
             SELECT * FROM barbershop.order INNER JOIN service 
                                on service.id=barbershop.order.serviceId WHERE date=@date;
            ";
            string query2 = @" SELECT * FROM place;";
            string time1 = " ";
           
            string hour;
            string minute;
            int sumTimeMinute = 0;
            Dictionary<string, List<List<int>>> dictionary = new Dictionary<string, List<List<int>>>();
            // List<List<int>> resultList = new List<List<int>>();
            // List<List<int>> fullresultList = new List<List<int>>();
            SortedSet<int> resultSet = new SortedSet<int>();
            bool k = true; 
            DataTable table = new DataTable();
            DataTable table2 = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                   // Console.WriteLine("sumTime  " + sumTime);
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
                   // Console.WriteLine( (table2.Rows[0]["id"]).ToString());
                }
                for(int i=0;i< table2.Rows.Count; i++)
                {
                    dictionary[table2.Rows[i]["id"].ToString()] = new List<List<int>> ();
                    List<int> start = new List<int>();
                    start.Add(0);
                    start.Add(540);
                    List<int> end = new List<int>();
                    end.Add(1080);
                    end.Add(0);
                    dictionary[table2.Rows[i]["id"].ToString()].Add(start);
                    dictionary[table2.Rows[i]["id"].ToString()].Add(end);
                }

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    List<int> addToList = new List<int>();
                    addToList.Add(ConvertTime(table.Rows[i]["time"].ToString()));
                   // Console.WriteLine("time to make:"+ table.Rows[i]["timeToMake"].ToString());
                    addToList.Add(ConvertTime(table.Rows[i]["time"].ToString())+ ConvertTime(table.Rows[i]["timeToMake"].ToString()));
                    dictionary[table.Rows[i]["placeId"].ToString()].Add(addToList);

                }   
                dictionary = dictionary
                    .OrderBy(d => d.Key)
                       .ToDictionary(
                          d => d.Key,
                      d => (List<List<int>>)d.Value.OrderBy(v => v[0]).ToList());
               
                //foreach (var outlist1 in dictionary.Keys)
                //{
                //    Console.WriteLine("Key: {0}", outlist1);
                //    foreach (var outlist2 in dictionary[outlist1])
                //    {
                //        Console.WriteLine(" Value: {0},{1}",
                //      outlist2[0], outlist2[1]);
                //    }
                //}
                foreach (var outlist1 in dictionary.Keys)
                {
                  //  Console.WriteLine("Key: {0}", outlist1);
                 //   Console.WriteLine("fgg " + dictionary[outlist1].Count);
                   
                    for (int i = 0; i < dictionary[outlist1].Count-1; i++)
                    {

                        if (dictionary[outlist1][i][1] != dictionary[outlist1][i + 1][1])
                        {
                           // Console.WriteLine(" Value: {0}-{1}",
                           //         dictionary[outlist1][i][1], dictionary[outlist1][i + 1][0]);


                            if (dictionary[outlist1][i][1] + (int)sumTime<= dictionary[outlist1][i + 1][0])
                            {

                                //List<int> shortList = new List<int>();
                                //shortList.Add(dictionary[outlist1][i][1]);
                                //shortList.Add(dictionary[outlist1][i + 1][0]);
                                //resultList.Add(shortList);
                              
                                InsertTime(dictionary[outlist1][i][1], dictionary[outlist1][i + 1][0], sumTime, resultSet);

                            }
                        }
                            
                    }
                    
                    

                }
             
               // Console.WriteLine("list  " + String.Join(",", resultSet));
                //for (int i = 0; i < resultList.Count ; i++)
                //{
                //    Console.WriteLine(" Rseult List" + resultList[i][0] + "---" + resultList[i][1]);
                   

                //}
                //int smin=0, emax=0;
                //List<int> short2List = new List<int>();
                //for (int i = 0; i < resultList.Count-1; i++)
                //{
                //     smin = resultList[i][0];
                //     emax = resultList[i][1];
                //    if (smin >= resultList[i + 1][0] && emax <= resultList[i + 1][1])
                //    {
                //        smin = resultList[i + 1][0];
                //        emax = resultList[i + 1][1];
                //        Console.WriteLine("u"+ smin + "   "+ emax );


                //    }
                //    else
                //    {
                //        Console.WriteLine("Fuuuuuu");
                //        short2List.Add(resultList[i][0]);
                //        short2List.Add(resultList[i][1]);
                //    }
                   
                  

                //}
                //Console.WriteLine("qq " + smin + "   " + emax);
                //short2List.Add(smin);
                //short2List.Add(emax);
                //fullresultList.Add(short2List);
                //for (int i = 0; i < fullresultList.Count; i++)
                //{
                //    Console.WriteLine("Fq");
                //    Console.WriteLine("Full Result List" + fullresultList[i][0] + "---" + fullresultList[i][1]);
                //}
            }

                return new JsonResult(resultSet);
            


        }

        public static int ConvertTime(string time)
        {
            string hour;
            string minute;
            string sumTimeMinute;
            hour = time.Split(":")[0];
            minute = time.Split(":")[1];
            sumTimeMinute = ((Int32.Parse(hour) * 60) + Int32.Parse(minute)).ToString();
            return (Int32.Parse(sumTimeMinute));
        }
     
        public static SortedSet<int> InsertTime(int begin, int end, int sum, SortedSet<int> resultSet)
        {
            double h = Math.Ceiling((double)(begin / 30));
            begin = (int)h*30;
            for (int i= begin; i+sum<=end; i = i + 30)
            {
               
                resultSet.Add(i);
            }
            return resultSet;
        }
    }
}
