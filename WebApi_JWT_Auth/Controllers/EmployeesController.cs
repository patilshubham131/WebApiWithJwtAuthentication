using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

using System.Collections;
using WebApi_JWT_Auth.Models;

namespace WebApi_JWT_Auth.Controllers
{

   

    public class EmployeesController : ApiController
    {
        private static string Token;

        [Route("api/employees/UserLogin")]
        [HttpGet]
        public ResponseVM UserLogin(string sUserName, string sPassword)
        {
            //if (true)
            if(sUserName == "admin" && sPassword == "admin")
            {
                ResponseVM response = new ResponseVM();
                response.Status = "Valid";
                response.Message = TokenManager.GenerateToken(sUserName);
                return response;
            }
            else
            {
                return new ResponseVM { Status = "Invalid", Message = "Username Or Password is incorrect.." };
            }

           
        }


        [Route("api/employees/GetEmployees")]
        [HttpGet]
        public Result GetEmployees()
        {

            string AuthenticationToken = Request.Headers.Authorization.ToString();

            if (TokenManager.ValidateToken(AuthenticationToken))
            {
                return new Result() { status = "success", response = new List<string>() { "shubham", "Ankit", "mayank", "salang", "shoaib" } };
            }
            else
            {
                return new Result() { status="failed", response = null };
            }
            //using (EmployeeDBEntities entity = new EmployeeDBEntities())
            //{
            //    return entity.employees.ToList();
            //}

            
        }

    }
}
