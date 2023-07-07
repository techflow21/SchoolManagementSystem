using SchoolManagementSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Core.DTOs.Responses
{
    public class EditExpenditureResponseDto
    {        
        public ExpenseType Type { get; set; }
        public string Description { get; set; }        
        public DateTime? UpdatedDate { get; set; }
    }
}
