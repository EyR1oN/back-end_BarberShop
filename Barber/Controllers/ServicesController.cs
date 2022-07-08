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
    public class ServicesController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ServicesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select * from services
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
        public JsonResult Post(Services services)
        {
            string query = @"
                        insert into services (name, description,picture,price,timeToMake,categoryId) values
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
                    myCommand.Parameters.AddWithValue("@name", services.name);
                    myCommand.Parameters.AddWithValue("@description", services.description);
                    myCommand.Parameters.AddWithValue("@picture", services.picture);
                    myCommand.Parameters.AddWithValue("@price", services.price);
                    myCommand.Parameters.AddWithValue("@timeToMake", services.timeToMake);
                    myCommand.Parameters.AddWithValue("@categoryId", services.categoryId);


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
        public JsonResult Put(Services services)
        {
            string query = @"
                        update services set 
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
                        delete from services 
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
