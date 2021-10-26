using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ACNHWorldMVC.Models;

namespace ACNHWorldMVC.Repositories
{
    public class BaseRepository
    {
        private string _connectionString;
        public BaseRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        protected SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }
    }
}
