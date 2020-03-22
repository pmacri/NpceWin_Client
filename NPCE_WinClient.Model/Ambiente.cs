﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class Ambiente
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }
        public string customerid { get; set; }
        public string costcenter { get; set; }
        public string billingcenter { get; set; }
        public string idsender { get; set; }

        [Required]
        public string sendersystem { get; set; }
        
        [Required]
        public string smuser { get; set; }
        public string contracttype { get; set; }
        public string usertype { get; set; }
        public string codicefiscale { get; set; }
        public string partitaiva { get; set; }        
        public string Username { get; set; }
        public string Password { get; set; }
        public string LolUri { get; set; }
        public string RolUri { get; set; }
        public string ColUri { get; set; }
        public string MolUri { get; set; }
        public string Contratto { get; set; }
    }
}

