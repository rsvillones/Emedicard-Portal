﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public interface IMemberProcess
    {
        decimal PrinAppNum { get; set; }
        decimal? DepAppNum { get; set; }
        string PrincipalCode { get; set; }
        string DependentCode { get; set; }
        int? StatCode { get; set; }

        DateTime? UpdatedDate { get; set; }
        DateTime? AlteredDate { get; set; }
        DateTime? AmendedDate { get; set; }
        string PrincipalOrDependent { get; }

        string LastName { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        DateTime? DateOfBirth { get; set; }

    }
}