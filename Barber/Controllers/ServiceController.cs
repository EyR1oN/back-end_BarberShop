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
    public class ServiceController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ServiceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select * from service
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

        [HttpGet("{categoryId}")]
        public JsonResult GetByCategoryId(int categoryId)
        {
            string query = @"
                select * from service where categoryId=@categoryId;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@categoryId", categoryId);
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
        public JsonResult Post(Service service)
        {
            string query = @"
                        insert into service (name, description,picture,price,timeToMake,categoryId) values
                                                    (@name, @description,@picture,@price,@timeToMake,@categoryId);
                        
            ";
          
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@name", service.name);
                    myCommand.Parameters.AddWithValue("@description", service.description);
                    myCommand.Parameters.AddWithValue("@picture", service.picture);
                    myCommand.Parameters.AddWithValue("@price", service.price);
                    myCommand.Parameters.AddWithValue("@timeToMake", service.timeToMake);
                    myCommand.Parameters.AddWithValue("@categoryId", service.categoryId);


                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [Authorize]
        [HttpPut]
        public JsonResult Put(Service services)
        {
            string query = @"
                        update service set 
                        name =@name,
                        description =@description,
                        picture =@picture,
                        price =@price,
                        timeToMake =@timeToMake,
                        categoryId=@categoryId
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
                    myCommand.Parameters.AddWithValue("@name", services.name);
                    myCommand.Parameters.AddWithValue("@description", services.description);
                    myCommand.Parameters.AddWithValue("@picture", services.picture);
                    myCommand.Parameters.AddWithValue("@price", services.price);
                    myCommand.Parameters.AddWithValue("@timeToMake", services.timeToMake);
                    myCommand.Parameters.AddWithValue("@categoryId", services.categoryId);
                    myCommand.Parameters.AddWithValue("@id", services.id);

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
                        delete from service
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
    }
}
