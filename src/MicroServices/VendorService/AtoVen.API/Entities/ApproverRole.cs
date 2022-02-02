using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AtoVen.API.Entities
{
    public class ApproverRole
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public bool IsEnabled { get; set; }
    }
}
