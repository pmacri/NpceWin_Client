﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NPCE_WinClient.Model;

namespace NPCE_WinClient.UI.Data
{
    public interface INpceDataService
    {
       Task<List<Service>> GetAllAsync();
        Task<Service> GetServiceByIdAsync(long id);
        Task<Mittente> GetMittenteByIdAsync(long id);
    }
}