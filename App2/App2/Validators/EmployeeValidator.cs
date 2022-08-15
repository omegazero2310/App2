using App2.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(emp => emp.Id).NotEmpty().WithMessage("Employee ID cannot be empty");
            RuleFor(emp => emp.Name).NotEmpty().WithMessage("Employee Name cannot be empty");
        }
    }
}
