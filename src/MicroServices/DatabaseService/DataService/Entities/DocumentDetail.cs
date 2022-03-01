using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DataService.Entities
{
    public class DocumentDetail
    {


        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string GivenDocumentName { get; set; }
        [Required]
        public string ActualFileName { get; set; }

        [Required]
        public string UniqueFileName { get; set; }
        

        public DateTime? DateOfIssue { get; set; }

        public DateTime? DateOfExpiry{ get; set; }

        public string CertificateNumber { get; set; }

        


    }


    public class DocumentPostDTO
    {

        public string GivenDocumentName { get; set; }

        public DateTime? DateOfIssue { get; set; }

        public DateTime? DateOfExpiry { get; set; }

        public string CertificateNumber { get; set; }

        public IFormFile FormFileDocument { get; set; }

    }

    public class DocumentDetailsDTO
    {

        //public int Id { get; set; }
        public int CompanyID { get; set; }

        public string ActualFileName { get; set; }

        public DateTime? DateOfIssue { get; set; }

        public DateTime? DateOfExpiry { get; set; }

        public IFormFile FormFileDocument { get; set; }

    }




}
