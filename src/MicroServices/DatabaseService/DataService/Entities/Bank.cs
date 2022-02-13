using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataService.Entities
{
    public class Bank
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Company Company { get; set; }
        public int CompanyID { get; set; }
        public string Country { get; set; }
        public string BankKey { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string BankAccount { get; set; }
        public string AccountHolderName { get; set; }
        public string IBAN { get; set; }
        public string Currency { get; set; }
    }


    public class BankDTO
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string BankKey { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string BankAccount { get; set; }
        public string AccountHolderName { get; set; }
        public string IBAN { get; set; }
        public string Currency { get; set; }
    }

    public class BankPostDTO
    {
        public string Country { get; set; }
        public string BankKey { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string BankAccount { get; set; }
        public string AccountHolderName { get; set; }
        public string IBAN { get; set; }
        public string Currency { get; set; }
    }

    public class BankPutDTO
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public string Country { get; set; }
        public string BankKey { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string BankAccount { get; set; }
        public string AccountHolderName { get; set; }
        public string IBAN { get; set; }
        public string Currency { get; set; }
    }

    public class BankVM
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public int CompanyID { get; set; }
        public string BankKey { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string BankAccount { get; set; }
        public string AccountHolderName { get; set; }
        public string IBAN { get; set; }
        public string Currency { get; set; }
    }
}
