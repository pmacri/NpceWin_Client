﻿using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data
{
    public interface IAnagraficaLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetAnagraficaLookupAsync();

    }
}