﻿using ACNHWorldMVC.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace ACNHWorldMVC.Repositories
{
    public interface IFossilRepository
    {
        List<Fossil> GetAllFossils();
        Fossil GetFossilById(int id);
    }

}