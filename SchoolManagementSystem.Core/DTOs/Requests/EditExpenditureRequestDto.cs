﻿using SchoolManagementSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class EditExpenditureRequestDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
