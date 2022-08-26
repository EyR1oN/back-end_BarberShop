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
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
       
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select * from user
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

        [HttpPost]
        public JsonResult Post(User user)
        {
            string query = @"
                        insert into user (username, password, email, name, surname, statusId) values
                                                    (@username, @password, @email, @name, @surname, @statusId);
                        
            ";

            string hashed_password = Password.HashPassword(user.password); 

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@username", user.username);
                    myCommand.Parameters.AddWithValue("@password", hashed_password);
                    myCommand.Parameters.AddWithValue("@email", user.email);
                    myCommand.Parameters.AddWithValue("@name", user.name);
                    myCommand.Parameters.AddWithValue("@surname", user.surname);
                    myCommand.Parameters.AddWithValue("@statusId", user.statusId);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);
        }

        [Authorize]
        [HttpPut]
        public JsonResult Put(User user)
        {
            string query = @"
                        update user set 
                        username =@username,
                        password =@password,
                        email =@email,
                        name =@name,
                        surname =@surname
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
                    myCommand.Parameters.AddWithValue("@username", user.username);
                    myCommand.Parameters.AddWithValue("@password", user.password);
                    myCommand.Parameters.AddWithValue("@email", user.email);
                    myCommand.Parameters.AddWithValue("@name", user.name);
                    myCommand.Parameters.AddWithValue("@surname", user.surname);
                    myCommand.Parameters.AddWithValue("@statusId", user.statusId);
                    myCommand.Parameters.AddWithValue("@id", user.id);

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
                        delete from user 
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

        [HttpGet("{username}/{password}")]
        public JsonResult Login(string username, string password)
        {

            //var user1 = _context.user.Where(users => users.username.Equals(username) && users.password.Equals(password)).ToList();


            //if (user1.Count() == 0)
            //{
            //    return new JsonResult("Error");
            //}
            //else
            //{
            //    return new JsonResult("Success");
            //}
            string query = @"
               SELECT * FROM user WHERE username=@username;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@username", username);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                    //  table.Clear();
                }
            }
            if (table.Rows.Count == 0)
            {
                return new JsonResult("Error");
            }

            if (Password.VerifyPassword(table.Rows[0]["password"].ToString(), password))
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Error");
            }

           
        }

    }

}

