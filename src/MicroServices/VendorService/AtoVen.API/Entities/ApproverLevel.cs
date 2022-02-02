﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtoVen.API.Entities
{
    public class ApproverLevel
    {
        public int Id { get; set; }

        public int Level { get; set; }

        public bool IsEnabled{ get; set; }
    }
}
