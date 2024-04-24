using Corelib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public interface IActionMemo
    {
        string ProcessId { get; set; }
        string ControlNumber { get; set; }
        decimal PrinAppNum { get; set; }
        decimal? DepAppNum { get; set; }
        string PrincipalCode { get; set; }
        string DependentCode { get; set; }
        string AccountCode { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        DateTime? DateOfBirth { get; set; }
        string StatusRemarks { get; set; }
        string OtherRemarks { get; set; }
        DateTime? AmmendmentDate { get; set; }
        DateTime? ResignedDate { get; set; }
        int Posted { get; set; }
        string ActionMemoRemarks { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        DateTime? ActionMemoDate { get; set; }
        int? StatCode { get; set; }
        DateTime? DataCardPrintedDate { get; set; }

        string FullName { get; }
        ActionMemoType Type { get; }
        string PrincipalOrDependent { get; }
    }
}
