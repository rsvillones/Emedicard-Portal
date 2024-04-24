    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Data.Entity;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using Corelib.Models;

namespace Corelib.Classes
{
    public class ExcelTools
    {
        #region -- New Application --

        #region -- Excel Download --

        /// <summary>
        /// Sets the excel template of new application using EPPlus.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>System.Byte[].</returns>
        /// <exception cref="System.Exception">
        /// Invalid WorkBook.
        /// or
        /// Worksheet doesn't exist.
        /// </exception>
        public static byte[] NewApplicationExcelDownload(string accountCode)
        {
            LegacyAccount account;
            using (var db = new LegacyDataContext())
            {
                account = db.LegacyAccounts
                    .Include(t => t.LegacyRoomRates)
                    .Include(t => t.LegacyRoomRates.Select(lr => lr.LegacyPaymode))
                    .Include(t => t.LegacyRoomRates.Select(rr => rr.LegacyPlan))
                    .FirstOrDefault(t => t.Code == accountCode);

                if (!string.IsNullOrEmpty(account.MotherCode))
                {
                    var roomRateIds = account.LegacyRoomRates.Select(t=>t.Id).ToList();

                    var motherAccountLegacyRoomRates = db.LegacyRoomRates
                                .Include(t => t.LegacyPaymode)
                                .Include(t => t.LegacyPlan)
                                .Where(t => t.AccountCode == account.MotherCode && !roomRateIds.Contains(t.Id))
                                .ToList();

                    foreach (var roomRate in motherAccountLegacyRoomRates)
                    {
                        account.LegacyRoomRates.Add(roomRate);
                    }
                }
            }

            var templatePath = HttpContext.Current.Server.MapPath("~/ExcelTemplates/New Application.xlsm");
            var targetPath = String.Format(@"{0}\{1}.xlsm", HttpContext.Current.Server.MapPath("~/Uploads"),
                                           Guid.NewGuid());

            File.Copy(templatePath, targetPath);

            byte[] returnValue;

            #region -- .XLSX File --

            using (var package = new ExcelPackage(new FileInfo(targetPath)))
            {
                var workBook = package.Workbook;
                if (workBook == null) throw new Exception("Invalid WorkBook.");
                if (workBook.Worksheets.Count <= 0) throw new Exception("Worksheet doesn't exist.");
                var worksheet = workBook.Worksheets[1];

                worksheet.Protection.AllowDeleteColumns = false;
                worksheet.Protection.IsProtected = true;
                worksheet.Protection.SetPassword("P@ssw0rd");
                for (var column = 1; column <= 21; column++)
                {
                    worksheet.Column(column).Style.Locked = false;
                }
                worksheet.Column(13).Style.Locked = true;

                worksheet.Cells[1, 1].Value = Guid.NewGuid();

                #region -- Date Validation --

                CellDateValidation(worksheet, "L3:L1048576", DateTime.UtcNow.AddYears(-1000));
                CellDateValidation(worksheet, "O3:O1048576", DateTime.UtcNow.AddYears(-1000));
                CellDateValidation(worksheet, "P3:P1048576", DateTime.UtcNow.AddYears(-1000));

                #endregion

                #region -- Integer Validation --

                for (var currentRow = 3; currentRow <= 20000; currentRow++)
                {
                    worksheet.Cells[currentRow, 13].Formula = string.Format("IF(ISBLANK(L{0}), \"\",  IFERROR(DATEDIF(L{0},TODAY(),\"Y\"), \"\"))", currentRow);
                }

                #endregion

                #region -- Dropdown Validation --

                var availablePlansWS = workBook.Worksheets["AvailablePlansForPrincipal"];
                var row = 1;
                foreach (var legacyRoomRate in account.LegacyRoomRates.Where(t => t.Selected && (t.PaymentFor == 0 || t.PaymentFor == 5 || t.PaymentFor == 8)))
                {
                    availablePlansWS.Cells[row, 1].Value = String.Format("{1} - {2} ({3:Php 0,0.00} Limit)|{0}", legacyRoomRate.Id, legacyRoomRate.LegacyPlan.Description, legacyRoomRate.For, legacyRoomRate.Limit);
                    row++;
                }

                var validation1 = worksheet.DataValidations.AddListValidation("R3:R1048576");
                // set validation properties
                validation1.Formula.ExcelFormula = String.Format("'AvailablePlansForPrincipal'!$A$1:$A${0}", row - 1);
                validation1.ShowErrorMessage = true;
                validation1.AllowBlank = false;
                validation1.ErrorTitle = "An invalid item was selected";
                validation1.Error = "Selected item must be in the list";


                var validation2 = worksheet.DataValidations.AddListValidation("Q3:Q1048576");
                // set validation properties
                validation2.Formula.ExcelFormula = String.Format("'AvailablePlansForPrincipal'!$A$1:$A${0}", row - 1);
                validation2.ShowErrorMessage = true;
                validation2.ErrorTitle = "An invalid item was selected";
                validation2.Error = "Selected item must be in the list";

                availablePlansWS = workBook.Worksheets["AvailablePlansForDependent"];
                row = 1;
                foreach (var legacyRoomRate in account.LegacyRoomRates.Where(t => t.Selected && (t.PaymentFor == 1 || t.PaymentFor == 2 || t.PaymentFor == 5 || t.PaymentFor == 8 || t.PaymentFor == 9)))
                {
                    availablePlansWS.Cells[row, 1].Value = String.Format("{1} - {2} ({3:Php 0,0.00} Limit)|{0}", legacyRoomRate.Id, legacyRoomRate.LegacyPlan.Description, legacyRoomRate.For, legacyRoomRate.Limit);
                    row++;
                }
                var validation3 = worksheet.DataValidations.AddListValidation("T3:T1048576");
                // set validation properties
                validation3.Formula.ExcelFormula = String.Format("'AvailablePlansForDependent'!$A$1:$A${0}", row - 1);
                validation3.ShowErrorMessage = true;
                validation3.ErrorTitle = "An invalid item was selected";
                validation3.Error = "Selected item must be in the list";

                var validation4 = worksheet.DataValidations.AddListValidation("U3:U1048576");
                // set validation properties
                validation4.Formula.ExcelFormula = String.Format("'AvailablePlansForDependent'!$A$1:$A${0}", row - 1);
                validation4.ShowErrorMessage = true;
                validation4.ErrorTitle = "An invalid item was selected";
                validation4.Error = "Selected item must be in the list";

                var secondOptions = new List<string>();
                secondOptions.Add("Male");
                secondOptions.Add("Female");
                CellDropDownValidation(worksheet, "K3:K1048576", secondOptions);

                var nextOption = new List<string>();
                nextOption.Add("Single");
                nextOption.Add("Married");
                nextOption.Add("Widower");
                nextOption.Add("Separated");
                nextOption.Add("Single Parent");
                CellDropDownValidation(worksheet, "N3:N1048576", nextOption);


                var areaWS = workBook.Worksheets["Areas"];
                row = 1;
                var areas = LegacyHelper.GetLegacyArea(accountCode);
                foreach (var area in areas)
                {
                    areaWS.Cells[row, 1].Value = string.Format("{0}|{1}", area.Description, area.AreaCode);
                    row++;
                }

                if (areas != null && areas.ToList().Count() > 0)
                {
                    var areaValidation = worksheet.DataValidations.AddListValidation("C3:C1048576");
                    areaValidation.Formula.ExcelFormula = String.Format("'Areas'!$A$1:$A${0}", row - 1);

                    areaValidation.ShowErrorMessage = true;
                    areaValidation.AllowBlank = true;
                    areaValidation.ErrorTitle = "An invalid item was selected";
                    areaValidation.Error = "Selected item must be in the list";
                }
                else
                {
                    worksheet.Column(3).Style.Locked = true;
                }

                #endregion

                returnValue = package.GetAsByteArray();
            }

            #endregion

            File.Delete(targetPath);
            return returnValue;
        }

        #endregion

        #endregion

        #region -- Function --

        /// <summary>
        /// Validation of cell in an excel sheet when entering datetime.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="column">The column.</param>
        /// <param name="date">The date.</param>
        public static void CellDateValidation(ExcelWorksheet worksheet, string column, DateTime date)
        {
            // Add a date time validation
            var validation = worksheet.DataValidations.AddDateTimeValidation(column);
            // set validation properties
            validation.ShowErrorMessage = true;
            validation.ErrorTitle = "An invalid date was entered";
            validation.Error = "Input value is invalid.";
            validation.Prompt = "Enter date here";
            validation.Formula.Value = date;
            validation.Operator = ExcelDataValidationOperator.greaterThan;
        }

        /// <summary>
        /// Validation of cell in an excel sheet when entering integer.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="column">The column.</param>
        /// <param name="minimumValue">The minimum value.</param>
        public static void CellIntegerValidation(ExcelWorksheet worksheet, string column, int minimumValue)
        {
            // Add a integer validation
            var validation = worksheet.DataValidations.AddIntegerValidation(column);
            // set validation properties
            validation.ShowErrorMessage = true;
            validation.ErrorTitle = "Please enter a valid number.";
            validation.Error = string.Format("The number must be greater than or equal to {0}.", minimumValue);
            validation.Prompt = "Enter number here";
            validation.Formula.Value = minimumValue;
            validation.Operator = ExcelDataValidationOperator.greaterThanOrEqual;
        }

        /// <summary>
        /// Validation of cell in an excel sheet when selecting an item in select list.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="column">The column.</param>
        /// <param name="items">The items.</param>
        public static void CellDropDownValidation(ExcelWorksheet worksheet, string column, List<string> items)
        {
            // Add a list validation
            var validation = worksheet.DataValidations.AddListValidation(column);
            // set validation properties
            foreach (var item in items)
            {
                validation.Formula.Values.Add(item);
            }
            validation.ShowErrorMessage = true;
            validation.ErrorTitle = "An invalid item was selected";
            validation.Error = "Selected item must be in the list";
        }

        /// <summary>
        /// Validation of cell in an excel sheet when entering text.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="column">The column.</param>
        /// <param name="limit">The limit.</param>
        public static void CellTextLengthValidation(ExcelWorksheet worksheet, string column, int limit)
        {
            // Add a text length validation
            var validation = worksheet.DataValidations.AddTextLengthValidation(column);
            // set validation properties
            validation.ShowErrorMessage = true;
            validation.ErrorTitle = "An invalid text length was entered";
            validation.Error = string.Format("The text length must be less than or equal to {0}.", limit);
            validation.Formula.Value = limit;
            validation.Operator = ExcelDataValidationOperator.lessThanOrEqual;
        }

        #endregion
    }
}
