using Assignment.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Models
{
    public class ValidateEmailExists : ValidationAttribute
    {
        private CustomerDAL CustomerContext = new CustomerDAL();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);
            // Casting the validation context to the "Staff" model class
            Customer customer = (Customer)validationContext.ObjectInstance;
            // Get the Staff Id from the staff instance
            int customerid = customer.CustomerID;
            if (CustomerContext.IsEmailExist(email, customerid))
                // validation failed
                return new ValidationResult
                ("Email address already exists!");
            else
                // validation passed
                return ValidationResult.Success;
        }
    }
}
