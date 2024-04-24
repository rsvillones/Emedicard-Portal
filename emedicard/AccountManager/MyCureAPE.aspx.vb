Imports emedicardBLL
Imports System.IO

Public Class MyCureAPE
    Inherits System.Web.UI.Page
    Dim objBll As New eAccountBLL
    Dim objBllEcorp As New eCorporateBLL
    Dim dtReimb As New DataTable
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim sAccountPlan As String
    Public Shared oSanitizer As New emedBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Request.QueryString("t").ToString = "1" Then
                objBllEcorp.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
                objBllEcorp.GetUserInfo()
                objBllEcorp.AccountCode = Request.QueryString("a")
                objBllEcorp.GetAccountPlan()
                sAccountPlan = objBllEcorp.Account_Plan
                objBll.AccountCode = Request.QueryString("a")
            Else
                objBll.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
                objBll.GetAgentInfo()
                objBll.AccountCode = Request.QueryString("a")
                sAccountPlan = "ALL"
            End If

            getMembers()

        Else

        End If

    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnView.Click
        Load_Record()

    End Sub

    Protected Sub ddlPMEType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPMEtype.TextChanged

        'If ddlPMEtype.SelectedValue = "apesum" Or ddlPMEtype.SelectedValue = "pesum" Or ddlPMEtype.SelectedValue = "ecusum" Then
        '    ddlMember.Enabled = False
        'Else
        '    ddlMember.Enabled = True
        'End If

    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
    '    PrintReport()

    'End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        txtMemberCode.Text = Nothing
        getMembers()
        ddlPMEtype.SelectedValue = "apesum"
        dpFrom.Clear()
        dpTo.Clear()
        rpt.Text = Nothing
        btnExportExcel.Visible = False
        btnExportPDF.Visible = False
        btnPrint.Visible = False
        InitiateReportViewer()
    End Sub

    Private Sub Load_Record()
        Try
            rpt.Text = "Loading..." 'clear

            'rpt.Text = txtMemberCode.Text & " " & ddlPMEtype.SelectedValue & " " & dpFrom.SelectedDate & " " & dpTo.SelectedDate

            Dim rptDisplay As String = "No data to display"

            Select Case ddlPMEtype.SelectedValue
                Case "ape", "pe", "ecu"
                    'rptDisplay = txtMemberCode.Text
                    If txtMemberCode.Text IsNot Nothing Then
                        rptDisplay = GetReportFormat1(txtMemberCode.Text, ddlPMEtype.SelectedValue.ToUpper(), dpFrom.SelectedDate, dpTo.SelectedDate)
                    End If

                    'rptDisplay = GetReportFormat1("31513557", ddlPMEtype.SelectedValue.ToUpper())

                    btnPrint.Visible = True
                    btnExportExcel.Visible = False
                    btnExportPDF.Visible = False

                Case "apesum", "pesum", "ecusum"

                    Dim userCode As String = ""
                    Dim sAccountName As String = ""
                    userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                    'Dim acc = New eCorporateBLL(userCode, AccountInformationProperties.AccountType.eCorporate, Request.QueryString("a"))
                    Using acctBLLInfo = New AccountInformationBLL(Request.QueryString("a"), 1)
                        sAccountName = acctBLLInfo.CompanyName
                    End Using

                    'rptDisplay = GetReportFormat2(Request.QueryString("a"), ddlPMEtype.SelectedValue.ToUpper(), txtMemberCode.Text, dpFrom.SelectedDate, dpTo.SelectedDate)
                    rptDisplay = GetReportFormat2_v3(Request.QueryString("a"), ddlPMEtype.SelectedValue.ToUpper(), txtMemberCode.Text,
                                                     dpFrom.SelectedDate, dpTo.SelectedDate, ClinicResultsViewer,
                                                     Server.MapPath("~/AccountManager/ClinicResults.rdlc"),
                                                     sAccountName)

                    btnExportExcel.Visible = True
                    btnExportPDF.Visible = True
                    btnPrint.Visible = False

            End Select

            rpt.Text = rptDisplay

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        End Try
    End Sub

    Private Sub getMembers()

        Dim emed As New AccountInformationBLL(Request.QueryString("a"), AccountInformationProperties.AccountType.eCorporate)

        'Principal & Dependent
        ddlMember.DataSource = (From x In emed.ActiveMembersPrincipal
                                Select New With {
                                   .FullName = x.MEM_LNAME & ", " & x.MEM_FNAME,
                                   .Code = x.PRIN_CODE
                               }).ToList()
        ddlMember.DataTextField = "FullName"
        ddlMember.DataValueField = "Code"
        ddlMember.DataBind()

        ' Additional
        ddlMember.Items.Insert(0, New ListItem("", ""))

    End Sub

    Private Sub getPMEtype()

        ddlPMEtype.Items.Clear()

        ddlPMEtype.Items.Insert(0, New ListItem("APE", "0"))
        ddlPMEtype.Items.Insert(1, New ListItem("PE", "1"))
        ddlPMEtype.Items.Insert(2, New ListItem("ECU", "2"))
        ddlPMEtype.Items.Insert(3, New ListItem("Summary - APE", "3"))
        ddlPMEtype.Items.Insert(4, New ListItem("Summary - PE", "4"))
        ddlPMEtype.Items.Insert(5, New ListItem("Summary - ECU", "5"))

    End Sub

    Private Sub PrintReport()

        Try

            If rpt.Text IsNot Nothing Then

                'Imports iTextSharp.text
                'Imports iTextSharp.text.html.simpleparser
                'Imports iTextSharp.text.pdf

                'Dim stringWrite As TextWriter = New System.IO.StringWriter()
                'stringWrite.Write(rpt.Text)
                'Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)

                Dim sr As New StringReader(rpt.Text)
                Dim pdfDoc As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10.0F, 10.0F, 10.0F, 0.0F)
                Dim htmlparser As New iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc)
                Dim bytes As Byte()
                Using memoryStream As MemoryStream = New MemoryStream
                    Dim writer As iTextSharp.text.pdf.PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, memoryStream)
                    pdfDoc.Open()
                    htmlparser.Parse(sr)
                    pdfDoc.Close()

                    bytes = memoryStream.ToArray()
                    memoryStream.Close()
                End Using

                Response.Clear()
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-disposition", "attachment;filename=HTML.pdf")
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.BinaryWrite(bytes)
                Response.End()

                'Dim pdfdoc As iTextSharp.text.Document = New iTextSharp.text.Document(iTextSharp.text.PageSize.A2, 10.0F, 10.0F, 10.0F, 0.0F)
                'Dim htmlparser As iTextSharp.text.html.simpleparser.HTMLWorker = New iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc)
                'PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
                'pdfDoc.Open()

                'Dim stringWrite As TextWriter = New System.IO.StringWriter()
                'stringWrite.Write(rpt.Text)
                'Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)

                'htmlparser.Parse(htmlWrite)
                'pdfdoc.Close()
                'Response.ContentType = "application/pdf"
                'Response.AddHeader("content-disposition", "attachment;filename=HTMLExport.pdf")
                'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                'Response.Write(pdfDoc)
                'Response.End()

                'Using swAs System.IO.StringWriter = New StringWriter()
                '    Using hwAs HtmlTextWriter =New HtmlTextWriter(sw)
                '        Using srAs StreamReader =New StreamReader(Server.MapPath("~/Customers.htm"))
                '            Dim pdfDocAs Document =New Document(PageSize.A2, 10.0F, 10.0F, 10.0F, 0.0F)
                '            Dim htmlparserAs HTMLWorker =New HTMLWorker(pdfDoc)
                '            PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
                '            pdfDoc.Open()
                '            htmlparser.Parse(SR)
                '            pdfDoc.Close()
                '            Response.ContentType = "application/pdf"
                '            Response.AddHeader("content-disposition", "attachment;filename=HTMLExport.pdf")
                '            Response.Cache.SetCacheability(HttpCacheability.NoCache)
                '            Response.Write(pdfDoc)
                '            Response.End()
                '        End Using
                '    End Using
                'End Using


                'Response.Clear()
                'Response.AddHeader("content-disposition", "attachment; filename=" & DateTime.Now.ToString("MM-dd-yyyy") & ".xls")
                'Response.Charset = ""
                'Response.ContentType = "application/vnd.xls"

                'Dim stringWrite As TextWriter = New System.IO.StringWriter()
                'stringWrite.Write(rpt.Text)

                'Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)

                'Response.Write(stringWrite.ToString())
                'Response.End()
            Else

            End If


            'Dim rptDisplay As String
            'rptDisplay = "<div style='font-size: 7.5pt'><div style='display: flex; flex-wrap: nowrap;'><div style='width: 120px'>{clinic_logo_0}</div><div style='flex-grow: 1;text-align:center'><h2 style='margin:   0 !important'>{clinic_name_0}</h2><span style='font-size: 8pt'>{clinic_address_0}</span><br><span style='font-size: 8pt'>{clinic_phone_0}</span><br><span style='font-size: 8pt'>Website:&nbsp;{clinic_website_0}&nbsp;Email:&nbsp;{clinic_email_0}</span></div><div style='width: 120px; display: flex; flex-direction: column; align-items: flex-end;'><div style='border: 1px solid black; font-size: 8pt; padding: 2px'><span>FSC-FO-018</span><br><span>Rev.00</span><br><span>22 FEB 2018</span></div></div></div><div><br></div><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;width:   15% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Name</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;width:   50% !important'>{patient_name_0}</td><td style='vertical-align: Text-top;width:   10% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Account No.</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;width:   25% !important' colspan='2'>{patient_hmo_accountno_0}</td></tr><tr><td style='vertical-align: Text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Company</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;'>{patient_companies_0}</td><td style='vertical-align: Text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Birth Date</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td colspan='2'>{patient_dob_0}</td></tr><tr><td style='vertical-align: Text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Type of Exam</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;'>EXECUTIVE CHECK-UP<br></td><td style='vertical-align: Text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Civil Status</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td colspan='2'>{patient_marital_status_0}</td></tr><tr><td style='vertical-align: Text-top;width:   15% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Date of Exam</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;width:   50% !important'>{patient_encounter_created_at_0}<br></td><td style='vertical-align: Text-top;width:   10% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Age</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;width:   12% !important'>{patient_age_0}</td><td style='vertical-align: Text-top;width:   13% !important'><b>Sex&nbsp;&nbsp;</b>:&nbsp;{patient_sex_0}</td></tr></tbody></table></div><b><div><br><br></div>HISTORY AND PHYSICAL EXAMINATION</b><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;width:   20% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Chief Complaint</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;width:   80% !important'>{patient_complaint_0}</td></tr><tr><td style='vertical-align: Text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>History of Present Illness</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;'>{patient_hpi_0}</td></tr><tr><td style='vertical-align: Text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Past Medical History</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;'>{patient_pmhx_0}</td></tr></tbody></table></div><div><div style='width:   100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;width:   20% !important'><div><div style='display: flex;flex-direction: row;align-items: center'><b>Allergies</b><b style='font-weight:   600; margin-left: auto'>:</b></div></div></td><td style='vertical-align: Text-top;width:   35% !important'><div>{patient_allergy_name_0}</div></td><td style='vertical-align: Text-top;width:   45% !important'><span style='font-weight: 600;'>Supplements</span><b>:&nbsp;</b>{patient_allergy_supplement_0}</td></tr></tbody></table></div><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;width:   20% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Previous Surgery</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;width:   80% !important'>{patient_surgical_hx_0}</td></tr></tbody></table><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;width:   20% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Menstrual History</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;width:   20% !important'>LMP:&nbsp;{patient_menstrual_lmp_0}</td><td style='vertical-align: Text-top;width:   20% !important'>Cycle:&nbsp;{patient_menstrual_cycle_0}</td><td style='vertical-align: Text-top;width:   20% !important'>Lasting:&nbsp;{patient_menstrual_duration_0}&nbsp;<br></td><td style='vertical-align: Text-top;width:   20% !important'>OB Score:&nbsp;{patient_menstrual_obscore_0}</td></tr></tbody></table><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;width:   20% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Hospitalization</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;width:   80% !important'>{patient_hospitalization_hx_0}</td></tr></tbody></table></div><div><b>Social History:&nbsp;</b></div><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;width:   20% !important'><div style='display: flex;flex-direction: row;align-items: center'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<b>Tobacco</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;width:   35% !important'>{sh_is_smoking_0}&nbsp;{sh_smoking_pack_years_0}&nbsp;<br></td><td style='vertical-align: Text-top;width:   10% !important'><br></td><td style='vertical-align: Text-top;width:   35% !important'><br></td></tr><tr><td style='vertical-align: Text-top;'><div style='display: flex;flex-direction: row;align-items: center'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<b>Alcohol</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;'>{sh_is_drinking_0}&nbsp;{sh_drinking_remarks_0}&nbsp;<br></td><td style='vertical-align: Text-top;'><b>Illicit Drugs:&nbsp;</b></td><td style='vertical-align: Text-top;'>{sh_prohibited_drugs_0}&nbsp;&nbsp;<br></td></tr></tbody></table></div><div style='width:   100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;width:   20% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Family History</b><b style='font-weight:   600; margin-left: auto'>:</b></div></td><td style='vertical-align: Text-top;width:   80% !important'>{patient_fhx_0}</td></tr></tbody></table></div><div style='width: 100% !important'><br><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;width:   12% !important'><span style='font-weight: 600;'>Vitals Signs:</span><br></td><td style='vertical-align: Text-top;width:   12% !important'><b>T:</b>&nbsp;{vital_temperature_0}&nbsp;C</td><td style='vertical-align: Text-top;width:   12% !important'><b>PR:&nbsp;</b>{vital_pulse_rate_0}&nbsp;/min</td><td style='vertical-align: Text-top;width:   12% !important'><b>RR:&nbsp;</b>{vital_resp_rate_0}</td><td style='vertical-align: Text-top;width:   16% !important'><b>BP:&nbsp;</b>{vital_blood_pressure_0}&nbsp;mmHg</td><td style='vertical-align: Text-top;width:   12% !important'><b>BW:&nbsp;</b>{vital_weight_0}&nbsp;kg</td><td style='vertical-align: Text-top;width:   12% !important'><b>Ht:&nbsp;</b>{vital_height_0}&nbsp;cm</td><td style='vertical-align: Text-top;width:   12% !important'><b>BMI:&nbsp;</b>{vital_bmi_0}</td></tr></tbody></table></div><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;width:   12% !important'><span style='font-weight: 600; color: inherit; font-family: inherit;'>Visual Acuity</span><span style='font-weight: 600;'>:</span><br></td><td style='vertical-align: Text-top;width:   22% !important'><b>R:&nbsp;</b>{vital_visual_acuity_right_0}</td><td style='vertical-align: Text-top;width:   22% !important'><b>L:&nbsp;</b>{vital_visual_acuity_left_0}</td><td style='vertical-align: Text-top;width:   22% !important'><b>Visual Remarks:&nbsp;</b>{vital_visual_remarks_0}</td><td style='vertical-align: Text-top;width:   22% !important'><b>Color Vision:&nbsp;</b>{vital_color_vision_0}</td></tr></tbody></table></div><div><br></div><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;text-align: Left(); width: 20% !important;'><span style='font-weight: 600;'>REVIEW OF SYSTEMS:</span></td><td style='vertical-align: Text-top;text-align: Left(); width: 80% !important;'><span style='font-weight: 600;'>[N] - None [P] - Positive</span><br></td></tr></tbody></table><table style='width: 100% !important'><tbody><tr><td style='vertical-align: Text-top;text-align: Left(); width: 20% !important;'><b>Systems</b></td><td style='text-align: Left(); vertical-align: Text-top; width:   10% !important;'><b>Status</b></td><td style='text-align: Left(); vertical-align: Text-top; width:   20% !important;'><b>Remarks</b></td><td style='vertical-align: Text-top;text-align: Left(); width: 20% !important;'><b>Systems</b></td><td style='text-align: Left(); vertical-align: Text-top; width:   10% !important;'><b>Status</b></td><td style='text-align: Left(); vertical-align: Text-top; width:   20% !important;'><b>Remarks</b></td></tr><tr><td style='vertical-align: Text-top;'><b>Eyes</b></td><td style='vertical-align: Text-top;'>{ros_status_eyes_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_eyes_0}&nbsp;<br></td><td style='vertical-align: Text-top;'><b>Musculoskeletal</b></td><td style='vertical-align: Text-top;'>{ros_status_musculoskeletal_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_musculoskeletal_0}&nbsp;<br></td></tr><tr><td style='vertical-align: Text-top;'><b>ENT/Mouth</b></td><td style='vertical-align: Text-top;'>{ros_status_ent_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_ent_0}&nbsp;<br></td><td style='vertical-align: Text-top;'><b>Skin/Breasts</b></td><td style='vertical-align: Text-top;'>{ros_status_breasts_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_breasts_0}&nbsp;<br></td></tr><tr><td style='vertical-align: Text-top;'><b>Cardiovascular</b></td><td style='vertical-align: Text-top;'>{ros_status_cardiovascular_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_cardiovascular_0}&nbsp;<br></td><td style='vertical-align: Text-top;'><b>Neurological</b></td><td style='vertical-align: Text-top;'>{pe_neurologic_status_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_neurologic_0}&nbsp;<br></td></tr><tr><td style='vertical-align: Text-top;'><b>Respiratory</b></td><td style='vertical-align: Text-top;'>{ros_status_respiratory_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_respiratory_0}&nbsp;<br></td><td style='vertical-align: Text-top;'><b>Endocrine</b></td><td style='vertical-align: Text-top;'>{pe_endocrine_status_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_endocrine_0}&nbsp;<br></td></tr><tr><td style='vertical-align: Text-top;'><b>Gastrointestinal</b></td><td style='vertical-align: Text-top;'>{ros_status_gastrointestinal_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_gastrointestinal_0}&nbsp;<br></td><td style='vertical-align: Text-top;'><b>Hematological</b></td><td style='vertical-align: Text-top;'>{pe_hematologic_status_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_hematologic_lymphatic_0}&nbsp;<br></td></tr><tr><td style='vertical-align: Text-top;'><b>Genitourinary</b></td><td style='vertical-align: Text-top;'>{ros_status_genitourinary_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_genitourinary_0}&nbsp;<br></td><td style='vertical-align: Text-top;'><b>Others</b></td><td style='vertical-align: Text-top;'>{pe_general_status_0}&nbsp;<br></td><td style='vertical-align: Text-top;'>{ros_general_0}&nbsp;<br></td></tr></tbody></table></div><div><br></div><div style='width:   100% !important'><table style='color: inherit; font-family: inherit; width: 100% !important;'><tbody><tr><td style='vertical-align: Text-top;width:   20% !important;'><span style='font-weight: 600;'>PHYSICAL EXAM:</span><b></b></td><td style='vertical-align: Text-top;width:   80% !important;'><div><span style='font-weight: 600;'>[N] - Normal&nbsp; [P] - Positive</span></div></td></tr></tbody></table><table style='color: inherit; font-family: inherit; width: 100% !important;'><tbody><tr><td style='vertical-align: Text-top;width: 20% !important;'><br></td><td style='text-align: left; vertical-align: text-top; width: 10% !important;'><b>Status</b></td><td style='text-align: left; vertical-align: text-top; width: 20% !important;'><b>Remarks</b></td><td style='vertical-align: text-top;width: 20% !important;'><br></td><td style='text-align: left; vertical-align: text-top; width: 10% !important;'><b>Status</b></td><td style='text-align: left; vertical-align: text-top; width: 20% !important;'><b>Remarks</b></td></tr><tr><td style='vertical-align: text-top;'><b>General</b></td><td style='vertical-align: text-top;'>{pe_general_status_1}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_general_text_0}&nbsp;<br></td><td style='vertical-align: text-top;'><b>Back</b></td><td style='vertical-align: text-top;'>{pe_back_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_back_text_0}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><b>Skin</b></td><td style='vertical-align: text-top;'>{pe_skin_status_0}<br></td><td style='vertical-align: text-top;'>{pe_skin_text_0}&nbsp;<br></td><td style='vertical-align: text-top;'><b>Heart</b></td><td style='vertical-align: text-top;'>{pe_cardiovascular_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_cardiovascular_text_0}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><b>Head &amp; Neck</b></td><td style='vertical-align: text-top;'>{pe_headneck_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_headneck_text_0}&nbsp;<br></td><td style='vertical-align: text-top;'><b>Abdomen</b></td><td style='vertical-align: text-top;'>{pe_abdomen_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_abdomen_text_0}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><b>Ears, Eyes, Nose</b></td><td style='vertical-align: text-top;'>{pe_earseyesnose_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_earseyesnose_text_0}&nbsp;<br></td><td style='vertical-align: text-top;'><b>Extremities</b></td><td style='vertical-align: text-top;'>{pe_extermities_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_extermities_text_0}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><b>Mouth/Throat</b></td><td style='vertical-align: text-top;'>{pe_throat_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_throat_text_0}&nbsp;<br></td><td style='vertical-align: text-top;'><b>Neurological</b></td><td style='vertical-align: text-top;'>{pe_neurologic_status_1}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_neurologic_text_0}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><b>Chest/Lungs</b></td><td style='vertical-align: text-top;'>{pe_chest_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_chest_text_0}&nbsp;<br></td><td style='vertical-align: text-top;'><b>Rectal</b></td><td style='vertical-align: text-top;'>{pe_rectal_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_rectal_text_0}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><b>Breast</b></td><td style='vertical-align: text-top;'>{pe_breasts_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_breasts_text_0}<br></td><td style='vertical-align: text-top;'><b>Genitalia</b></td><td style='vertical-align: text-top;'>{pe_genitalia_status_0}&nbsp;<br></td><td style='vertical-align: text-top;'>{pe_genitalia_text_0}&nbsp;<br></td></tr></tbody></table></div><div><br></div><div><b>LABORATORY AND ANCILLARY PROCEDURES</b></div><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: text-top;width: 50% !important; padding: 0'><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: text-top;width: 40% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Chest X-ray</b><b style='font-weight: 600; margin-left: auto'>:</b></div></td><td style='vertical-align: text-top;width: 60% !important'>{custom_choices_chest_xray_result_0}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Complete Blood Count</b><b style='font-weight: 600; margin-left: auto'>:</b></div></td><td style='vertical-align: text-top;'>{custom_choices_cbc_result_1}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Urinalysis</b><b style='font-weight: 600; margin-left: auto'>:</b></div></td><td style='vertical-align: text-top;'>{custom_choices_urinalysis_result_2}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Fecalysis</b><b style='font-weight: 600; margin-left: auto'>:</b></div></td><td style='vertical-align: text-top;'>{custom_choices_fecalysis_result_3}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Other Labs</b><b style='font-weight: 600; margin-left: auto'>:</b></div></td><td style='vertical-align: text-top;'>{custom_text_other_labs_result_0}&nbsp;<br></td></tr></tbody></table></div></td><td style='vertical-align: text-top;width: 50% !important; padding: 0'><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: text-top;width: 40% !important'><div style='display: flex;flex-direction: row;align-items: center'><b>Blood Chemistry</b><b style='font-weight: 600; margin-left: auto'>:</b></div></td><td style='vertical-align: text-top;width: 60% !important'>{custom_choices_blood_chemistry_result_4}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Papsmear</b><b style='font-weight: 600; margin-left: auto'>:</b></div></td><td style='vertical-align: text-top;'>{custom_choices_papsmear_result_5}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>ECG</b><b style='font-weight: 600; margin-left: auto'>:</b></div></td><td style='vertical-align: text-top;'>{custom_choices_ecg_result_6}&nbsp;<br></td></tr><tr><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><b>Other Tests</b><b style='font-weight: 600; margin-left: auto'>:</b></div></td><td style='vertical-align: text-top;'>{custom_text_other_ancillary_results_1}<br></td></tr><tr><td style='vertical-align: text-top;'></td><td style='vertical-align: text-top;'></td></tr></tbody></table></div></td></tr></tbody></table></div></div><div><br></div><div style='page-break-before: always !important'><div><br><br></div><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: text-top;width: 15% !important'><div style='display: flex;flex-direction: row;align-items: center'><span style='font-weight: 600;'>Name</span><span style='font-weight: 600; margin-left: auto'>:</span></div></td><td style='vertical-align: text-top;width: 50% !important'>{patient_name_1}</td><td style='vertical-align: text-top;width: 10% !important'><div style='display: flex;flex-direction: row;align-items: center'><span style='font-weight: 600;'>Account No.</span><span style='font-weight: 600; margin-left: auto'>:</span></div></td><td style='vertical-align: text-top;width: 25% !important' colspan='2'>{patient_hmo_accountno_1}</td></tr><tr><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><span style='font-weight: 600;'>Company</span><span style='font-weight: 600; margin-left: auto'>:</span></div></td><td style='vertical-align: text-top;'>{patient_companies_1}</td><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><span style='font-weight: 600;'>Birth Date</span><span style='font-weight: 600; margin-left: auto'>:</span></div></td><td colspan='2'>{patient_dob_1}</td></tr><tr><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><span style='font-weight: 600;'>Type of Exam</span><span style='font-weight: 600; margin-left: auto'>:</span></div></td><td style='vertical-align: text-top;'>EXECUTIVE CHECK-UP<br></td><td style='vertical-align: text-top;'><div style='display: flex;flex-direction: row;align-items: center'><span style='font-weight: 600;'>Civil Status</span><span style='font-weight: 600; margin-left: auto'>:</span></div></td><td colspan='2'>{patient_marital_status_1}</td></tr><tr><td style='vertical-align: text-top;width: 15% !important'><div style='display: flex;flex-direction: row;align-items: center'><span style='font-weight: 600;'>Date of Exam</span><span style='font-weight: 600; margin-left: auto'>:</span></div></td><td style='vertical-align: text-top;width: 50% !important'>{patient_encounter_created_at_1}<br></td><td style='vertical-align: text-top;width: 10% !important'><div style='display: flex;flex-direction: row;align-items: center'><span style='font-weight: 600;'>Age</span><span style='font-weight: 600; margin-left: auto'>:</span></div></td><td style='vertical-align: text-top;width: 12% !important'>{patient_age_1}</td><td style='vertical-align: text-top;width: 13% !important'><b>Sex&nbsp;&nbsp;</b>:&nbsp;{patient_sex_1}</td></tr></tbody></table></div><br><br><br><br><div><b>IMPRESSION</b></div><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: text-top;width: 35px !important'></td><td style='vertical-align: text-top;'>{patient_impression_0}</td></tr></tbody></table></div><div><br></div><div><b>RECOMMENDATION</b></div><div style='width: 100% !important'><table style='width: 100% !important'><tbody><tr><td style='vertical-align: text-top;width: 35px !important'></td><td style='vertical-align: text-top;'>{custom_text_recommendation_2}</td></tr></tbody></table></div></div></div>"
            'rptDisplay = rptDisplay.Replace("{clinic_logo_0}", "<img src='data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAMCAgICAgMCAgIDAwMDBAYEBAQEBAgGBgUGCQgKCgkICQkKDA8MCgsOCwkJDRENDg8QEBEQCgwSExIQEw8QEBD/2wBDAQMDAwQDBAgEBAgQCwkLEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBD/wAARCAj9CP0DASIAAhEBAxEB/8QAHgABAAIBBQEBAAAAAAAAAAAAAAEJCAIDBQYHBAr/xABnEAEAAQIEAwQEBQsMDgcGBQUAAQIDBAUGEQchMQgSQVEJE2FxFCIyR4E3QlJ1hpGhsbKzxRUWFxkjM1dicpTB0RgkNDU2Q1VWZXN0gpKTOFNUlaLS4SVERWOEwidkg6Pw8SYohUb/xAAbAQEAAQUBAAAAAAAAAAAAAAAABgEDBAUHAv/EAEARAQABAwEGAwUGBQMEAgMBAQABAgMEBQYREhMhMRQyUSIzNEFxFRYjUmGRNUJTgbEkcqFDYoLBkvAlorLR4f/aAAwDAQACEQMRAD8AtTAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAVV+nO+ZP7pP0atUVV+nO+ZP7pP0aC1QAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABVX6c75k/uk/Rq1RVX6c75k/uk/RoLVAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFVfpzvmT+6T9GrVFVfpzvmT+6T9GgtUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAVV+nO+ZP7pP0atUVV+nO+ZP7pP0aC1QAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABVX6c75k/uk/Rq1RVX6c75k/uk/RoLVAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAVV+nO+ZP7pP0atUVV+nO+ZP7pP0aC1QAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABVX6c75k/uk/Rq1RVX6c75k/uk/RoLVAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFVfpzvmT+6T9GrVFVfpzvmT+6T9GgtUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAVV+nO+ZP7pP0atUVV+nO+ZP7pP0aC1QAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABVX6c75k/uk/Rq1RVX6c75k/uk/RoLVAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFVfpzvmT+6T9GrVFVfpzvmT+6T9GgtUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAVV+nO+ZP7pP0atUVV+nO+ZP7pP0aC1QAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACqv053zJ/dJ+jVqiqv053zJ/dJ+jQWqAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKq/TnfMn90n6NWqKq/TnfMn90n6NBaoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAqr9Od8yf3Sfo1aoqr9Od8yf3Sfo0FqgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACqv053zJ/dJ+jVqiqv053zJ/dJ+jQWqAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKq/TnfMn90n6NWqKq/TnfMn90n6NBaoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAqr9Od8yf3Sfo1aoqr9Od8yf3Sfo0FqgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACqv053zJ/dJ+jVqiqv053zJ/dJ+jQWqAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKq/TnfMn90n6NWqKq/TnfMn90n6NBaoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAqr9Od8yf3Sfo1aoqr9Od8yf3Sfo0H//Z' height='75' width='75'>")

            ''Dim mem As MemoryStream
            ''mem.Read()

            'Response.Clear()
            'Response.AddHeader("Content-Type", "application/pdf")
            'Response.AddHeader("Cache-Control", "max-age=0")
            'Response.AddHeader("Accept-Ranges", "none")
            'Response.AddHeader("content-disposition", "attachment; filename=MycurePME.pdf")
            ''Response.ContentType = "application/pdf"

            'Dim stringWrite As TextWriter = New System.IO.StringWriter()
            'stringWrite.Write(rptDisplay)

            'Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)

            'Response.Write(htmlWrite)
            'Response.End()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        End Try

    End Sub

    Private Shared Function GetReportFormat1(ByVal memberCode As String, ByVal reportType As String,
                                             Optional ByVal dtFrom As DateTime = Nothing,
                                             Optional ByVal dtTo As DateTime = Nothing) As String

        Dim sRet As String = Nothing

        'Dim encounterId As String = Mailhelper.MyCureAPI.GetMyCureEncounterId(memberCode, reportType)
        Dim encounterId As String = Mailhelper.MyCureAPI.GetMyCureEncounterId(memberCode, reportType, dtFrom, dtTo)
        If Not encounterId Is Nothing Then
            Dim res As Mailhelper.MyCurePMEResultModel = Mailhelper.MyCureAPI.Get_PME(encounterId)
            'Dim template As String = "<div><style>td{vertical-align:top!important}</style><div style='display:flex;flex-wrap:nowrap'><div style='width:120px'>{clinic_logo_0}</div><div style='flex-grow:1;text-align:center'><h2 style='margin:0!important'>{clinic_name_0}</h2><span style='font-size:12px'>{clinic_address_0}</span><br><span style='font-size:12px'>{clinic_phone_0}</span><br><span style='font-size:12px'>Website:{clinic_website_0}Email:{clinic_email_0}</span></div><div style='width:120px;display:flex;flex-direction:column;align-items:flex-end'><div style='border:1px solid #000;font-size:12px;padding:2px'><span>FSC-FO-018</span><br><span>Rev.00</span><br><span>22 FEB 2018</span></div></div></div><div><br></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Name</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'>{patient_name_0}<td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Account No.</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'colspan='2'>{patient_hmo_accountno_0}<tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Company</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'>{patient_companies_0}<td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Birth Date</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='width:auto!important'colspan='2'>{patient_dob_0}<tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Type of Exam</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'>ANNUAL PHYSICAL EXAM<br><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Civil Status</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='width:auto!important'colspan='2'>{patient_marital_status_0}<tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Date of Exam</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'>{patient_encounter_created_at_0}<br><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Age</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:120px!important'>{patient_age_0}<td style='vertical-align:text-top;width:auto!important'><b>Sex</b>:{patient_sex_0}</table></div><b><div><br><br></div>HISTORY AND PHYSICAL EXAMINATION</b><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Chief Complaint</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:80%!important'>{patient_complaint_0}<tr><td style='vertical-align:text-top;display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>History of Present Illness</span><span style='margin-left:auto;font-weight:600'>:</span><br><td style='vertical-align:text-top'>{patient_hpi_0}<tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Past Medical History</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top'>{patient_pmhx_0}</table></div><div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Allergies</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:35%!important'><div>{patient_allergy_name_0}</div><td style='vertical-align:text-top;width:45%!important'><span style='font-weight:600'>Supplements</span><b>:</b>{patient_allergy_supplement_0}</table></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Previous Surgery</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:80%!important'>{patient_surgical_hx_0}</table><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Menstrual History</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:20%!important'>LMP:{patient_menstrual_lmp_0}<td style='vertical-align:text-top;width:20%!important'>Cycle:{patient_menstrual_cycle_0}<td style='vertical-align:text-top;width:20%!important'>Lasting:{patient_menstrual_duration_0}<td style='vertical-align:text-top;width:20%!important'>OB Score:{patient_menstrual_obscore_0}</table><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Hospitalization</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:80%!important'>{patient_hospitalization_hx_0}</table></div><div><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Social History:</span><br></div></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Tobacco</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:35%!important'>{sh_is_smoking_0}{sh_smoking_pack_years_0}<br><td style='vertical-align:text-top;width:10%!important'><br><td style='vertical-align:text-top;width:35%!important'><br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Alcohol</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top'>{sh_is_drinking_0}{sh_drinking_remarks_0}<br><td style='vertical-align:text-top'><b>Illicit Drugs:</b><td style='vertical-align:text-top'>{sh_prohibited_drugs_0}<br></table></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Family History</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:80%!important'>{patient_fhx_0}</table></div><div style='width:100%!important'><br><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:12%!important'><span style='font-weight:600'>Vitals Signs:</span><br><td style='vertical-align:text-top;width:12%!important'><b>T:</b>{vital_temperature_0}C<td style='vertical-align:text-top;width:12%!important'><b>PR:</b>{vital_pulse_rate_0}/min<td style='vertical-align:text-top;width:12%!important'><b>RR:</b>{vital_resp_rate_0}<td style='vertical-align:text-top;width:16%!important'><b>BP:</b>{vital_blood_pressure_0}mmHg<td style='vertical-align:text-top;width:12%!important'><b>BW:</b>{vital_weight_0}kg<td style='vertical-align:text-top;width:12%!important'><b>Ht:</b>{vital_height_0}cm<td style='vertical-align:text-top;width:12%!important'><b>BMI:</b>{vital_bmi_0}</table></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:12%!important'><span style='font-weight:600;color:inherit;font-family:inherit'>Visual Acuity</span><span style='font-weight:600'>:</span><br><td style='vertical-align:text-top;width:22%!important'><b>R:</b>{vital_visual_acuity_right_0}<td style='vertical-align:text-top;width:22%!important'><b>L:</b>{vital_visual_acuity_left_0}<td style='vertical-align:text-top;width:22%!important'><b>Visual Remarks:</b>{vital_visual_remarks_0}<td style='vertical-align:text-top;width:22%!important'><b>Color Vision:</b>{vital_color_vision_0}</table></div><div><br></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;text-align:left;width:20%!important'><span style='font-weight:600'>REVIEW OF SYSTEMS:</span><td style='vertical-align:text-top;text-align:left;width:80%!important'><span style='font-weight:600'>[N] - None [P] - Positive</span><br></table><table style='width:100%!important'><tr><td style='vertical-align:text-top;text-align:left;width:20%!important'><b>Systems</b><td style='vertical-align:text-top;text-align:left;width:10%!important'><b>Status</b><td style='vertical-align:text-top;text-align:left;width:20%!important'><b>Remarks</b><td style='vertical-align:text-top;text-align:left;width:20%!important'><b>Systems</b><td style='vertical-align:text-top;text-align:left;width:10%!important'><b>Status</b><td style='vertical-align:text-top;text-align:left;width:20%!important'><b>Remarks</b><tr><td style='vertical-align:text-top'><b>Eyes</b><td style='vertical-align:text-top'>{ros_status_eyes_0}<br><td style='vertical-align:text-top'>{ros_eyes_0}<br><td style='vertical-align:text-top'><b>Musculoskeletal</b><td style='vertical-align:text-top'>{ros_status_musculoskeletal_0}<br><td style='vertical-align:text-top'>{ros_musculoskeletal_0}<br><tr><td style='vertical-align:text-top'><b>ENT/Mouth</b><td style='vertical-align:text-top'>{ros_status_ent_0}<br><td style='vertical-align:text-top'>{ros_ent_0}<br><td style='vertical-align:text-top'><b>Skin/Breasts</b><td style='vertical-align:text-top'>{ros_status_breasts_0}<br><td style='vertical-align:text-top'>{ros_breasts_0}<br><tr><td style='vertical-align:text-top'><b>Cardiovascular</b><td style='vertical-align:text-top'>{ros_status_cardiovascular_0}<br><td style='vertical-align:text-top'>{ros_cardiovascular_0}<br><td style='vertical-align:text-top'><b>Neurological</b><td style='vertical-align:text-top'>{ros_status_neurologic_0}<br><td style='vertical-align:text-top'>{ros_neurologic_0}<br><tr><td style='vertical-align:text-top'><b>Respiratory</b><td style='vertical-align:text-top'>{ros_status_respiratory_0}<br><td style='vertical-align:text-top'>{ros_respiratory_0}<br><td style='vertical-align:text-top'><b>Endocrine</b><td style='vertical-align:text-top'>{ros_status_endocrine_0}<br><td style='vertical-align:text-top'>{ros_endocrine_0}<br><tr><td style='vertical-align:text-top'><b>Gastrointestinal</b><td style='vertical-align:text-top'>{ros_status_gastrointestinal_0}<br><td style='vertical-align:text-top'>{ros_gastrointestinal_0}<br><td style='vertical-align:text-top'><b>Hematological</b><td style='vertical-align:text-top'>{ros_status_hematologic_lymphatic_0}<br><td style='vertical-align:text-top'>{ros_hematologic_lymphatic_0}<br><tr><td style='vertical-align:text-top'><b>Genitourinary</b><td style='vertical-align:text-top'>{ros_status_genitourinary_0}<br><td style='vertical-align:text-top'>{ros_genitourinary_0}<br><td style='vertical-align:text-top'><b>Others</b><td style='vertical-align:text-top'>{ros_status_general_0}<br><td style='vertical-align:text-top'>{ros_general_0}<br></table></div><div><br></div><div style='width:100%!important'><table style='color:inherit;font-family:inherit;width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><span style='font-weight:600'>PHYSICAL EXAM:</span><b></b><td style='vertical-align:text-top;width:80%!important'><div><span style='font-weight:600'>[N] - Normal [P] - Positive</span></div></table><table style='color:inherit;font-family:inherit;width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><br><td style='text-align:left;vertical-align:text-top;width:10%!important'><b>Status</b><td style='text-align:left;vertical-align:text-top;width:20%!important'><b>Remarks</b><td style='vertical-align:text-top;width:20%!important'><br><td style='text-align:left;vertical-align:text-top;width:10%!important'><b>Status</b><td style='vertical-align:text-top;text-align:center;width:20%!important'><b>Remarks</b><tr><td style='vertical-align:text-top'><b>General</b><td style='vertical-align:text-top'>{pe_general_status_0}<br><td style='vertical-align:text-top'>{pe_general_text_0}<br><td style='vertical-align:text-top'><b>Back</b><td style='vertical-align:text-top'>{pe_back_status_0}<br><td style='vertical-align:text-top'>{pe_back_text_0}<br><tr><td style='vertical-align:text-top'><b>Skin</b><td style='vertical-align:text-top'>{pe_skin_status_0}<br><td style='vertical-align:text-top'>{pe_skin_text_0}<br><td style='vertical-align:text-top'><b>Heart</b><td style='vertical-align:text-top'>{pe_cardiovascular_status_0}<br><td style='vertical-align:text-top'>{pe_cardiovascular_text_0}<br><tr><td style='vertical-align:text-top'><b>Head & Neck</b><td style='vertical-align:text-top'>{pe_headneck_status_0}<br><td style='vertical-align:text-top'>{pe_headneck_text_0}<br><td style='vertical-align:text-top'><b>Abdomen</b><td style='vertical-align:text-top'>{pe_abdomen_status_0}<br><td style='vertical-align:text-top'>{pe_abdomen_text_0}<br><tr><td style='vertical-align:text-top'><b>Ears, Eyes, Nose</b><td style='vertical-align:text-top'>{pe_earseyesnose_status_0}<br><td style='vertical-align:text-top'>{pe_earseyesnose_text_0}<br><td style='vertical-align:text-top'><b>Extremities</b><td style='vertical-align:text-top'>{pe_extermities_status_0}<br><td style='vertical-align:text-top'>{pe_extermities_text_0}<br><tr><td style='vertical-align:text-top'><b>Mouth/Throat</b><td style='vertical-align:text-top'>{pe_throat_status_0}<br><td style='vertical-align:text-top'>{pe_throat_text_0}<br><td style='vertical-align:text-top'><b>Neurological</b><td style='vertical-align:text-top'>{pe_neurologic_status_0}<br><td style='vertical-align:text-top'>{pe_neurologic_text_0}<br><tr><td style='vertical-align:text-top'><b>Chest/Lungs</b><td style='vertical-align:text-top'>{pe_chest_status_0}<br><td style='vertical-align:text-top'>{pe_chest_text_0}<br><td style='vertical-align:text-top'><b>Rectal</b><td style='vertical-align:text-top'>{pe_rectal_status_0}<br><td style='vertical-align:text-top'>{pe_rectal_text_0}<br><tr><td style='vertical-align:text-top'><b>Breast</b><td style='vertical-align:text-top'>{pe_breasts_status_0}<br><td style='vertical-align:text-top'>{pe_breasts_text_0}<br><td style='vertical-align:text-top'><b>Genitalia</b><td style='vertical-align:text-top'>{pe_genitalia_status_0}<br><td style='vertical-align:text-top'>{pe_genitalia_text_0}<br></table></div><div><br></div><div><b>LABORATORY AND ANCILLARY PROCEDURES</b></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:50%!important;padding:0'><div style='width:100%'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:40%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Chest X-ray</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:60%!important'>{custom_choices_chest_xray_result_0}<br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Complete Blood Count</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'>{custom_choices_cbc_result_1}<br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Urinalysis</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'>{custom_choices_urinalysis_result_2}<br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Fecalysis</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'>{custom_choices_fecalysis_result_3}<br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Other Labs</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'>{custom_text_other_labs_result_0}<br></table></div><td style='vertical-align:text-top;width:50%!important;padding:0'><div style='width:100%'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:40%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Blood Chemistry</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:60%!important'>{custom_choices_blood_chemistry_result_4}<br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Papsmear</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'>{custom_choices_papsmear_result_5}<br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>ECG</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'>{custom_choices_ecg_result_6}<br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Other Tests</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'>{custom_text_other_ancillary_results_1}<br><tr><td style='vertical-align:text-top'><td style='vertical-align:text-top'></table></div></table></div></div><div><br></div><div style='page-break-before:always'><div style='width:100%!important;margin-top:30px'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Name</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'>{patient_name_1}<td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Account No.</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'colspan='2'>{patient_hmo_accountno_1}<tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Company</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'>{patient_companies_1}<td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Birth Date</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='width:auto!important'colspan='2'>{patient_dob_1}<tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Type of Exam</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'>ANNUAL PHYSICAL EXAM<br><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Civil Status</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='width:auto!important'colspan='2'>{patient_marital_status_1}<tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Date of Exam</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'>{patient_encounter_created_at_1}<br><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Age</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:120px!important'>{patient_age_1}<td style='vertical-align:text-top;width:auto!important'><b>Sex</b>:{patient_sex_1}</table></div><div><br><br><br><br></div><div><b>IMPRESSION</b></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:35px!important'><td style='vertical-align:text-top'>{patient_impression_0}</table></div><div><br></div><div><b>RECOMMENDATION</b></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:35px!important'><td style='vertical-align:text-top'>{custom_text_recommendation_2}</table></div></div></div>"
            Dim template As String = "<div><style>td vertical-align:top!important </style><div style='display:flex;flex-wrap:nowrap'><div style='width:120px'> clinic_logo_0 </div><div style='flex-grow:1;text-align:center'><h2 style='margin:0!important'> clinic_name_0 </h2><span style='font-size:12px'> clinic_address_0 </span><br><span style='font-size:12px'> clinic_phone_0 </span><br><span style='font-size:12px'>Website: clinic_website_0 Email: clinic_email_0 </span></div><div style='width:120px;display:flex;flex-direction:column;align-items:flex-end'><div style='border:1px solid #000;font-size:12px;padding:2px'><span>FSC-FO-018</span><br><span>Rev.00</span><br><span>22 FEB 2018</span></div></div></div><div><br></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Name</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'> patient_name_0 <td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Account No.</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'colspan='2'> patient_hmo_accountno_0 <tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Company</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'> patient_companies_0 <td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Birth Date</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='width:auto!important'colspan='2'> patient_dob_0 <tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Type of Exam</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'>ANNUAL PHYSICAL EXAM<br><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Civil Status</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='width:auto!important'colspan='2'> patient_marital_status_0 <tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Date of Exam</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'> patient_encounter_created_at_0 <br><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Age</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:120px!important'> patient_age_0 <td style='vertical-align:text-top;width:auto!important'><b>Sex</b>: patient_sex_0 </table></div><b><div><br><br></div>HISTORY AND PHYSICAL EXAMINATION</b><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Chief Complaint</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:80%!important'> patient_complaint_0 <tr><td style='vertical-align:text-top;display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>History of Present Illness</span><span style='margin-left:auto;font-weight:600'>:</span><br><td style='vertical-align:text-top'> patient_hpi_0 <tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Past Medical History</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top'> patient_pmhx_0 </table></div><div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Allergies</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:35%!important'><div> patient_allergy_name_0 </div><td style='vertical-align:text-top;width:45%!important'><span style='font-weight:600'>Supplements</span><b>:</b> patient_allergy_supplement_0 </table></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Previous Surgery</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:80%!important'> patient_surgical_hx_0 </table><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Menstrual History</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:20%!important'>LMP: patient_menstrual_lmp_0 <td style='vertical-align:text-top;width:20%!important'>Cycle: patient_menstrual_cycle_0 <td style='vertical-align:text-top;width:20%!important'>Lasting: patient_menstrual_duration_0 <td style='vertical-align:text-top;width:20%!important'>OB Score: patient_menstrual_obscore_0 </table><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Hospitalization</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:80%!important'> patient_hospitalization_hx_0 </table></div><div><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Social History:</span><br></div></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Tobacco</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:35%!important'> sh_is_smoking_0  sh_smoking_pack_years_0 <br><td style='vertical-align:text-top;width:10%!important'><br><td style='vertical-align:text-top;width:35%!important'><br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Alcohol</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top'> sh_is_drinking_0  sh_drinking_remarks_0 <br><td style='vertical-align:text-top'><b>Illicit Drugs:</b><td style='vertical-align:text-top'> sh_prohibited_drugs_0 <br></table></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Family History</span><span style='margin-left:auto;font-weight:600'>:</span><br></div><td style='vertical-align:text-top;width:80%!important'> patient_fhx_0 </table></div><div style='width:100%!important'><br><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:12%!important'><span style='font-weight:600'>Vitals Signs:</span><br><td style='vertical-align:text-top;width:12%!important'><b>T:</b> vital_temperature_0 C<td style='vertical-align:text-top;width:12%!important'><b>PR:</b> vital_pulse_rate_0 /min<td style='vertical-align:text-top;width:12%!important'><b>RR:</b> vital_resp_rate_0 <td style='vertical-align:text-top;width:16%!important'><b>BP:</b> vital_blood_pressure_0 mmHg<td style='vertical-align:text-top;width:12%!important'><b>BW:</b> vital_weight_0 kg<td style='vertical-align:text-top;width:12%!important'><b>Ht:</b> vital_height_0 cm<td style='vertical-align:text-top;width:12%!important'><b>BMI:</b> vital_bmi_0 </table></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:12%!important'><span style='font-weight:600;color:inherit;font-family:inherit'>Visual Acuity</span><span style='font-weight:600'>:</span><br><td style='vertical-align:text-top;width:22%!important'><b>R:</b> vital_visual_acuity_right_0 <td style='vertical-align:text-top;width:22%!important'><b>L:</b> vital_visual_acuity_left_0 <td style='vertical-align:text-top;width:22%!important'><b>Visual Remarks:</b> vital_visual_remarks_0 <td style='vertical-align:text-top;width:22%!important'><b>Color Vision:</b> vital_color_vision_0 </table></div><div><br></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;text-align:left;width:20%!important'><span style='font-weight:600'>REVIEW OF SYSTEMS:</span><td style='vertical-align:text-top;text-align:left;width:80%!important'><span style='font-weight:600'>[N] - None [P] - Positive</span><br></table><table style='width:100%!important'><tr><td style='vertical-align:text-top;text-align:left;width:20%!important'><b>Systems</b><td style='vertical-align:text-top;text-align:left;width:10%!important'><b>Status</b><td style='vertical-align:text-top;text-align:left;width:20%!important'><b>Remarks</b><td style='vertical-align:text-top;text-align:left;width:20%!important'><b>Systems</b><td style='vertical-align:text-top;text-align:left;width:10%!important'><b>Status</b><td style='vertical-align:text-top;text-align:left;width:20%!important'><b>Remarks</b><tr><td style='vertical-align:text-top'><b>Eyes</b><td style='vertical-align:text-top'> ros_status_eyes_0 <br><td style='vertical-align:text-top'> ros_eyes_0 <br><td style='vertical-align:text-top'><b>Musculoskeletal</b><td style='vertical-align:text-top'> ros_status_musculoskeletal_0 <br><td style='vertical-align:text-top'> ros_musculoskeletal_0 <br><tr><td style='vertical-align:text-top'><b>ENT/Mouth</b><td style='vertical-align:text-top'> ros_status_ent_0 <br><td style='vertical-align:text-top'> ros_ent_0 <br><td style='vertical-align:text-top'><b>Skin/Breasts</b><td style='vertical-align:text-top'> ros_status_breasts_0 <br><td style='vertical-align:text-top'> ros_breasts_0 <br><tr><td style='vertical-align:text-top'><b>Cardiovascular</b><td style='vertical-align:text-top'> ros_status_cardiovascular_0 <br><td style='vertical-align:text-top'> ros_cardiovascular_0 <br><td style='vertical-align:text-top'><b>Neurological</b><td style='vertical-align:text-top'> ros_status_neurologic_0 <br><td style='vertical-align:text-top'> ros_neurologic_0 <br><tr><td style='vertical-align:text-top'><b>Respiratory</b><td style='vertical-align:text-top'> ros_status_respiratory_0 <br><td style='vertical-align:text-top'> ros_respiratory_0 <br><td style='vertical-align:text-top'><b>Endocrine</b><td style='vertical-align:text-top'> ros_status_endocrine_0 <br><td style='vertical-align:text-top'> ros_endocrine_0 <br><tr><td style='vertical-align:text-top'><b>Gastrointestinal</b><td style='vertical-align:text-top'> ros_status_gastrointestinal_0 <br><td style='vertical-align:text-top'> ros_gastrointestinal_0 <br><td style='vertical-align:text-top'><b>Hematological</b><td style='vertical-align:text-top'> ros_status_hematologic_lymphatic_0 <br><td style='vertical-align:text-top'> ros_hematologic_lymphatic_0 <br><tr><td style='vertical-align:text-top'><b>Genitourinary</b><td style='vertical-align:text-top'> ros_status_genitourinary_0 <br><td style='vertical-align:text-top'> ros_genitourinary_0 <br><td style='vertical-align:text-top'><b>Others</b><td style='vertical-align:text-top'> ros_status_general_0 <br><td style='vertical-align:text-top'> ros_general_0 <br></table></div><div><br></div><div style='width:100%!important'><table style='color:inherit;font-family:inherit;width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><span style='font-weight:600'>PHYSICAL EXAM:</span><b></b><td style='vertical-align:text-top;width:80%!important'><div><span style='font-weight:600'>[N] - Normal [P] - Positive</span></div></table><table style='color:inherit;font-family:inherit;width:100%!important'><tr><td style='vertical-align:text-top;width:20%!important'><br><td style='text-align:left;vertical-align:text-top;width:10%!important'><b>Status</b><td style='text-align:left;vertical-align:text-top;width:20%!important'><b>Remarks</b><td style='vertical-align:text-top;width:20%!important'><br><td style='text-align:left;vertical-align:text-top;width:10%!important'><b>Status</b><td style='vertical-align:text-top;text-align:center;width:20%!important'><b>Remarks</b><tr><td style='vertical-align:text-top'><b>General</b><td style='vertical-align:text-top'> pe_general_status_0 <br><td style='vertical-align:text-top'> pe_general_text_0 <br><td style='vertical-align:text-top'><b>Back</b><td style='vertical-align:text-top'> pe_back_status_0 <br><td style='vertical-align:text-top'> pe_back_text_0 <br><tr><td style='vertical-align:text-top'><b>Skin</b><td style='vertical-align:text-top'> pe_skin_status_0 <br><td style='vertical-align:text-top'> pe_skin_text_0 <br><td style='vertical-align:text-top'><b>Heart</b><td style='vertical-align:text-top'> pe_cardiovascular_status_0 <br><td style='vertical-align:text-top'> pe_cardiovascular_text_0 <br><tr><td style='vertical-align:text-top'><b>Head & Neck</b><td style='vertical-align:text-top'> pe_headneck_status_0 <br><td style='vertical-align:text-top'> pe_headneck_text_0 <br><td style='vertical-align:text-top'><b>Abdomen</b><td style='vertical-align:text-top'> pe_abdomen_status_0 <br><td style='vertical-align:text-top'> pe_abdomen_text_0 <br><tr><td style='vertical-align:text-top'><b>Ears, Eyes, Nose</b><td style='vertical-align:text-top'> pe_earseyesnose_status_0 <br><td style='vertical-align:text-top'> pe_earseyesnose_text_0 <br><td style='vertical-align:text-top'><b>Extremities</b><td style='vertical-align:text-top'> pe_extermities_status_0 <br><td style='vertical-align:text-top'> pe_extermities_text_0 <br><tr><td style='vertical-align:text-top'><b>Mouth/Throat</b><td style='vertical-align:text-top'> pe_throat_status_0 <br><td style='vertical-align:text-top'> pe_throat_text_0 <br><td style='vertical-align:text-top'><b>Neurological</b><td style='vertical-align:text-top'> pe_neurologic_status_0 <br><td style='vertical-align:text-top'> pe_neurologic_text_0 <br><tr><td style='vertical-align:text-top'><b>Chest/Lungs</b><td style='vertical-align:text-top'> pe_chest_status_0 <br><td style='vertical-align:text-top'> pe_chest_text_0 <br><td style='vertical-align:text-top'><b>Rectal</b><td style='vertical-align:text-top'> pe_rectal_status_0 <br><td style='vertical-align:text-top'> pe_rectal_text_0 <br><tr><td style='vertical-align:text-top'><b>Breast</b><td style='vertical-align:text-top'> pe_breasts_status_0 <br><td style='vertical-align:text-top'> pe_breasts_text_0 <br><td style='vertical-align:text-top'><b>Genitalia</b><td style='vertical-align:text-top'> pe_genitalia_status_0 <br><td style='vertical-align:text-top'> pe_genitalia_text_0 <br></table></div><div><br></div><div><b>LABORATORY AND ANCILLARY PROCEDURES</b></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:50%!important;padding:0'><div style='width:100%'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:40%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Chest X-ray</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:60%!important'> custom_choices_chest_xray_result_0 <br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Complete Blood Count</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'> custom_choices_cbc_result_1 <br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Urinalysis</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'> custom_choices_urinalysis_result_2 <br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Fecalysis</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'> custom_choices_fecalysis_result_3 <br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Other Labs</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'> custom_text_other_labs_result_0 <br></table></div><td style='vertical-align:text-top;width:50%!important;padding:0'><div style='width:100%'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:40%!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Blood Chemistry</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:60%!important'> custom_choices_blood_chemistry_result_4 <br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Papsmear</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'> custom_choices_papsmear_result_5 <br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>ECG</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'> custom_choices_ecg_result_6 <br><tr><td style='vertical-align:text-top'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Other Tests</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top'> custom_text_other_ancillary_results_1 <br><tr><td style='vertical-align:text-top'><td style='vertical-align:text-top'></table></div></table></div></div><div><br></div><div style='page-break-before:always'><div style='width:100%!important;margin-top:30px'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Name</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'> patient_name_1 <td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Account No.</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'colspan='2'> patient_hmo_accountno_1 <tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Company</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'> patient_companies_1 <td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Birth Date</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='width:auto!important'colspan='2'> patient_dob_1 <tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Type of Exam</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'>ANNUAL PHYSICAL EXAM<br><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Civil Status</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='width:auto!important'colspan='2'> patient_marital_status_1 <tr><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Date of Exam</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:auto!important'> patient_encounter_created_at_1 <br><td style='vertical-align:text-top;width:120px!important'><div style='display:flex;flex-direction:row;align-items:center'><span style='font-weight:600'>Age</span><span style='font-weight:600;margin-left:auto'>:</span></div><td style='vertical-align:text-top;width:120px!important'> patient_age_1 <td style='vertical-align:text-top;width:auto!important'><b>Sex</b>: patient_sex_1 </table></div><div><br><br><br><br></div><div><b>IMPRESSION</b></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:35px!important'><td style='vertical-align:text-top'> patient_impression_0 </table></div><div><br></div><div><b>RECOMMENDATION</b></div><div style='width:100%!important'><table style='width:100%!important'><tr><td style='vertical-align:text-top;width:35px!important'><td style='vertical-align:text-top'> custom_text_recommendation_2 </table></div></div></div>"

            sRet = template

            If Not res Is Nothing Then

                '=========================
                'Personal Details
                '=========================
                If res.populated IsNot Nothing Then
                    If res.populated.patient IsNot Nothing Then
                        Dim memCode As String = ""
                        Dim cardInfo As Mailhelper.MyCurePMEPatientDetailsPatientCardResultModel
                        For Each cardInfo In res.populated.patient.insuranceCards
                            If cardInfo.status = "active" Then
                                memCode = cardInfo.number
                            End If
                        Next

                        Dim accountCode As String = ""
                        Dim accountName As String = ""
                        Dim accInfo As Mailhelper.MyCurePMEPatientDetailsPatientCompanyResultModel
                        For Each accInfo In res.populated.patient.companies
                            accountCode = accInfo.externalId
                            accountName = accInfo.name
                        Next

                        sRet = sRet.Replace("patient_name_0", res.populated.patient.name.firstName & " " & res.populated.patient.name.middleName & " " & res.populated.patient.name.lastName)
                        sRet = sRet.Replace("patient_name_1", res.populated.patient.name.firstName & " " & res.populated.patient.name.middleName & " " & res.populated.patient.name.lastName)
                        sRet = sRet.Replace("patient_hmo_accountno_0", accountCode)
                        sRet = sRet.Replace("patient_hmo_accountno_1", accountCode)
                        sRet = sRet.Replace("patient_companies_0", accountName)
                        sRet = sRet.Replace("patient_companies_1", accountName)
                        sRet = sRet.Replace("patient_dob_0", UnixToDateTime(res.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))
                        sRet = sRet.Replace("patient_dob_1", UnixToDateTime(res.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))
                        sRet = sRet.Replace("patient_marital_status_0", res.populated.patient.maritalStatus)
                        sRet = sRet.Replace("patient_marital_status_1", res.populated.patient.maritalStatus)
                        sRet = sRet.Replace("patient_age_0", GetCurrentAge(UnixToDateTime(res.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))) 'GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString))
                        sRet = sRet.Replace("patient_age_1", GetCurrentAge(UnixToDateTime(res.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))) 'GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString))
                        sRet = sRet.Replace("patient_sex_0", res.populated.patient.sex)
                        sRet = sRet.Replace("patient_sex_1", res.populated.patient.sex)
                    End If
                End If
                '=========================

                If res.values IsNot Nothing Then

                    Dim dList As List(Of Mailhelper.MyCureAPEData1ValuesResultModel) = res.values
                    Dim dItem As Mailhelper.MyCureAPEData1ValuesResultModel
                    For Each dItem In dList

                        If dItem.id = "today_0" Then
                            sRet = sRet.Replace("patient_encounter_created_at_0", dItem.answer)
                        End If

                        If dItem.id = "today_0" Then
                            sRet = sRet.Replace("patient_encounter_created_at_1", dItem.answer)
                        End If

                        ' CHEST XRAY RESULT
                        If dItem.id = "custom_choices_chest_xray_result_0" Then
                            If dItem.answer <> "SEE ATTACHED" Then
                                sRet = sRet.Replace("custom_choices_chest_xray_result_0", dItem.answer)
                            Else
                                Dim att As String = "SEE ATTACHED"
                                Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res.populated.attachments
                                Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                If attList.Count > 0 Then
                                    att = ""
                                    For Each attItem In attList
                                        If attItem.testName = "CXR PA " Or attItem.testName = "CXR PA" Then
                                            For Each urls As String In attItem.attachmentURLs
                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"
                                            Next
                                        End If
                                    Next
                                Else
                                    att = "SEE ATTACHED"
                                End If

                                sRet = sRet.Replace("custom_choices_chest_xray_result_0", att)
                            End If
                        End If

                        ' CBC RESULT
                        If dItem.id = "custom_choices_cbc_result_1" Then
                            If dItem.answer <> "SEE ATTACHED" Then
                                sRet = sRet.Replace("custom_choices_cbc_result_1", dItem.answer)

                            Else
                                Dim att As String = "SEE ATTACHED"
                                Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res.populated.attachments
                                Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                If attList.Count > 0 Then
                                    att = ""
                                    For Each attItem In attList
                                        If attItem.testName = "Complete Blood Count/CBC" Then
                                            For Each urls As String In attItem.attachmentURLs
                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                            Next
                                        End If
                                    Next
                                Else
                                    att = "SEE ATTACHED"
                                End If

                                sRet = sRet.Replace("custom_choices_cbc_result_1", att)

                            End If
                        End If

                        ' URINALYSIS RESULT
                        If dItem.id = "custom_choices_urinalysis_result_2" Then
                            If dItem.answer <> "SEE ATTACHED" Then
                                sRet = sRet.Replace("custom_choices_urinalysis_result_2", dItem.answer)

                            Else
                                Dim att As String = "SEE ATTACHED"
                                Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res.populated.attachments
                                Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                If attList.Count > 0 Then
                                    att = ""
                                    For Each attItem In attList
                                        If attItem.testName = "Urinalysis" Then
                                            For Each urls As String In attItem.attachmentURLs
                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                            Next
                                        End If
                                    Next
                                Else
                                    att = "SEE ATTACHED"
                                End If

                                sRet = sRet.Replace("custom_choices_urinalysis_result_2", att)

                            End If
                        End If

                        ' FECALYSIS RESULT
                        If dItem.id = "custom_choices_fecalysis_result_3" Then
                            If dItem.answer <> "SEE ATTACHED" Then
                                sRet = sRet.Replace("custom_choices_fecalysis_result_3", dItem.answer)

                            Else
                                Dim att As String = "SEE ATTACHED"
                                Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res.populated.attachments
                                Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                If attList.Count > 0 Then
                                    att = ""
                                    For Each attItem In attList
                                        If attItem.testName = "Fecalysis/Stool Exam" Then
                                            For Each urls As String In attItem.attachmentURLs
                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                            Next
                                        End If
                                    Next
                                Else
                                    att = "SEE ATTACHED"
                                End If

                                sRet = sRet.Replace("custom_choices_fecalysis_result_3", att)

                            End If
                        End If

                        ' OTHER LABS RESULT
                        If dItem.id = "custom_text_other_labs_result_0" Then
                            sRet = sRet.Replace("custom_text_other_labs_result_0", dItem.answer)
                        End If

                        ' BLOOD CHEMISTRY RESULT
                        If dItem.id = "custom_choices_blood_chemistry_result_4" Then
                            If dItem.answer.Trim() <> "SEE ATTACHED" Then
                                sRet = sRet.Replace("custom_choices_blood_chemistry_result_4", dItem.answer)

                            Else
                                Dim att As String = "SEE ATTACHED"
                                Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res.populated.attachments
                                Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                If attList.Count > 0 Then
                                    att = ""
                                    For Each attItem In attList
                                        If attItem.testName = "10 Blood Chemistry/10BC" Then
                                            For Each urls As String In attItem.attachmentURLs
                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                            Next
                                        End If
                                    Next
                                Else
                                    att = "SEE ATTACHED"
                                End If

                                sRet = sRet.Replace("custom_choices_blood_chemistry_result_4", att)

                            End If
                        End If

                        ' PAPSMEAR RESULT
                        If dItem.id = "custom_choices_papsmear_result_5" Then
                            If dItem.answer <> "SEE ATTACHED" Then
                                sRet = sRet.Replace("custom_choices_papsmear_result_5", dItem.answer)

                            Else
                                Dim att As String = "SEE ATTACHED"
                                Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res.populated.attachments
                                Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                If attList.Count > 0 Then
                                    att = ""
                                    For Each attItem In attList
                                        If attItem.testName = "Pap Smear/Conventional PS" Then
                                            For Each urls As String In attItem.attachmentURLs
                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                            Next
                                        End If
                                    Next
                                Else
                                    att = "SEE ATTACHED"
                                End If

                                sRet = sRet.Replace("custom_choices_papsmear_result_5", att)
                            End If
                        End If

                        ' ECG RESULT
                        If dItem.id = "custom_choices_ecg_result_6" Then
                            If dItem.answer <> "SEE ATTACHED" Then
                                sRet = sRet.Replace("custom_choices_ecg_result_6", dItem.answer)

                            Else
                                Dim att As String = "SEE ATTACHED"
                                Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res.populated.attachments
                                Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                If attList.Count > 0 Then
                                    att = ""
                                    For Each attItem In attList
                                        If attItem.testName = "12L ECG" Or attItem.testName = "ELECTROCARDIOGRAM (12L ECG)" Then
                                            For Each urls As String In attItem.attachmentURLs
                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                            Next
                                        End If
                                    Next
                                Else
                                    att = "SEE ATTACHED"
                                End If

                                sRet = sRet.Replace("custom_choices_ecg_result_6", att)
                            End If
                        End If

                        ' OTHER ANCILLARY RESULTS
                        If dItem.id = "custom_text_other_ancillary_results_1" Then
                            sRet = sRet.Replace("custom_text_other_ancillary_results_1", dItem.answer)
                        End If

                        'replace
                        sRet = sRet.Replace(dItem.id, dItem.answer)

                    Next

                End If

                'safe
                sRet = sRet.Replace("custom_text_other_labs_result_0", "")
                sRet = sRet.Replace("custom_text_other_ancillary_results_1", "")
                sRet = sRet.Replace("custom_text_recommendation_2", "")

            End If
        Else
            sRet = "No data to display"
        End If

        Return sRet
    End Function

    Private Shared Function GetReportFormat2(ByVal accountCode As String, ByVal reportType As String,
                                             Optional ByVal memberCode As String = Nothing,
                                             Optional ByVal dtFrom As DateTime = Nothing,
                                             Optional ByVal dtTo As DateTime = Nothing) As String

        Dim sRet As String = Nothing

        Dim sb As New StringBuilder
        sb.Append("<div style='width: 100%; position: relative; background-color: #f9f9f9; overflow-x: scroll; max-height: 500px'><table id='tblUtilization'>")
        sb.Append("<tr><th style='min-width: 120px;'>Date of Exam</th><th style='min-width: 120px;'>Member Code</th><th style='min-width: 120px;'>Patient Name</th><th style='min-width: 120px;'>Patient Age</th><th style='min-width: 120px;'>Patient Sex</th><th style='min-width: 120px;'>Vitals - Pulse Rate</th><th style='min-width: 200px;'>Vitals - Respiration Rate</th><th style='min-width: 200px;'>Vitals - Blood Pressure</th><th style='min-width: 150px;'>Vitals - Weight (kg)</th><th style='min-width: 150px;'>Vitals - Height (cm)</th><th style='min-width: 120px;'>Vitals - BMI</th><th style='min-width: 200px;'>CHEST XRAY RESULT</th><th style='min-width: 120px;'>CBC RESULT</th><th style='min-width: 200px;'>URINALYSIS RESULT</th><th style='min-width: 200px;'>FECALYSIS RESULT</th><th style='min-width: 200px;'>OTHER LABS RESULT</th><th style='min-width: 200px;'>BLOOD CHEMISTRY RESULT</th><th style='min-width: 200px;'>PAPSMEAR RESULT</th><th style='min-width: 200px;'>ECG RESULT</th><th style='min-width: 200px;'>OTHER ANCILLARY RESULTS</th><th style='min-width: 200px;'>Impression</th><th style='min-width: 200px;'>RECOMMENDATION</th><th style='min-width: 200px;'>Clinic Name</th></tr>")

        Dim res As Mailhelper.MyCurePMESummaryResultModel = Mailhelper.MyCureAPI.Get_PME_Summary(accountCode, reportType, dtFrom, dtTo)
        'Dim res As Mailhelper.MyCurePMESummaryResultModel = Mailhelper.MyCureAPI.Get_PME_Summary("04102003-000081", reportType)

        Dim patientId As String = ""
        If memberCode <> "" Then
            patientId = Mailhelper.MyCureAPI.GetMyCurePatientId(memberCode, reportType.Replace("SUM", ""))
        End If

        If Not res Is Nothing Then
            If res.total <> "0" Then

                Dim dItem As Mailhelper.MyCurePMESummaryData1ResultModel

                'If memberCode IsNot Nothing Then
                '    Dim temp As List(Of Mailhelper.MyCurePMESummaryData1ResultModel) = res.data
                '    res.data = temp.Where(Function(s) s.patient = patientId)
                'End If

                If res.data.Count > 0 Then
                    Dim iCount As Long = 0
                    For Each dItem In res.data.OrderBy(Function(c) UnixToDateTime(c.createdAt))

                        If memberCode <> "" Then
                            If dItem.patient <> patientId Then
                                'Next/Skip
                                Continue For
                            End If
                        End If

                        Dim res2 As Mailhelper.MyCurePMEResultModel = Mailhelper.MyCureAPI.Get_PME(dItem.id)
                        If res2 IsNot Nothing Then
                            Dim row As String = "<tr><td>date_exam</td><td>mem_code</td><td>patient_name</td><td>patient_age</td><td>patient_sex</td><td>vital_pulse_rate_0</td><td>vital_resp_rate_0</td><td>vital_blood_pressure_0</td><td>vital_weight_0</td><td>vital_height_0</td><td>vital_bmi_0</td><td>custom_choices_chest_xray_result_0</td><td>custom_choices_cbc_result_1</td><td>custom_choices_urinalysis_result_2</td><td>custom_choices_fecalysis_result_3</td><td>custom_text_other_labs_result_0</td><td>custom_choices_blood_chemistry_result_4</td><td>custom_choices_papsmear_result_5</td><td>custom_choices_ecg_result_6</td><td>custom_text_other_ancillary_results_1</td><td>patient_impression_0</td><td>custom_text_recommendation_2</td><td>clinic_name_0</td></tr>"

                            '=========================
                            'Personal Details
                            '=========================
                            If res2.populated IsNot Nothing Then
                                If res2.populated.patient IsNot Nothing Then
                                    Dim memCode As String = ""
                                    Dim cardInfo As Mailhelper.MyCurePMEPatientDetailsPatientCardResultModel
                                    For Each cardInfo In res2.populated.patient.insuranceCards
                                        If cardInfo.status = "active" Then
                                            memCode = cardInfo.number
                                        End If
                                    Next

                                    row = row.Replace("mem_code", memCode)
                                    row = row.Replace("patient_name", res2.populated.patient.name.firstName & " " & res2.populated.patient.name.middleName & " " & res2.populated.patient.name.lastName)
                                    row = row.Replace("patient_age", GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))) 'GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString))
                                    row = row.Replace("patient_sex", res2.populated.patient.sex)
                                End If
                            End If
                            '=========================

                            If res2.values IsNot Nothing Then

                                Dim dList As List(Of Mailhelper.MyCureAPEData1ValuesResultModel) = res2.values
                                Dim dItem2 As Mailhelper.MyCureAPEData1ValuesResultModel
                                For Each dItem2 In dList
                                    'replace
                                    'row = row.Replace(dItem2.id, dItem2.answer)

                                    ' Date of exam
                                    If dItem2.id = "patient_encounter_created_at_0" Or dItem2.id = "patient_encounter_created_at_1" _
                                    Or dItem2.id = "today_0" Or dItem2.id = "today_1" Then
                                        row = row.Replace("date_exam", dItem2.answer)
                                    End If

                                    ' Member Code
                                    ' Patient Name
                                    ' Patient Age
                                    ' Patient Sex

                                    ' Vitals - Pulse Rate
                                    If dItem2.id = "vital_pulse_rate_0" Then
                                        row = row.Replace("vital_pulse_rate_0", dItem2.answer)
                                    End If

                                    ' Vitals - Respiration Rate
                                    If dItem2.id = "vital_resp_rate_0" Then
                                        row = row.Replace("vital_resp_rate_0", dItem2.answer)
                                    End If

                                    ' Vitals - Blood Pressure
                                    If dItem2.id = "vital_blood_pressure_0" Then
                                        row = row.Replace("vital_blood_pressure_0", dItem2.answer)
                                    End If

                                    ' Vitals - Weight (kg)
                                    If dItem2.id = "vital_weight_0" Then
                                        row = row.Replace("vital_weight_0", dItem2.answer)
                                    End If

                                    ' Vitals - Height (cm)
                                    If dItem2.id = "vital_height_0" Then
                                        row = row.Replace("vital_height_0", dItem2.answer)
                                    End If

                                    ' Vitals - BMI
                                    If dItem2.id = "vital_bmi_0" Then
                                        row = row.Replace("vital_bmi_0", dItem2.answer)
                                    End If

                                    ' CHEST XRAY RESULT
                                    If dItem2.id = "custom_choices_chest_xray_result_0" Then
                                        row = row.Replace("custom_choices_chest_xray_result_0", dItem2.answer)
                                    End If

                                    ' CBC RESULT
                                    If dItem2.id = "custom_choices_cbc_result_1" Then
                                        row = row.Replace("custom_choices_cbc_result_1", dItem2.answer)
                                    End If

                                    ' URINALYSIS RESULT
                                    If dItem2.id = "custom_choices_urinalysis_result_2" Then
                                        row = row.Replace("custom_choices_urinalysis_result_2", dItem2.answer)
                                    End If

                                    ' FECALYSIS RESULT
                                    If dItem2.id = "custom_choices_fecalysis_result_3" Then
                                        row = row.Replace("custom_choices_fecalysis_result_3", dItem2.answer)
                                    End If

                                    ' OTHER LABS RESULT
                                    If dItem2.id = "custom_text_other_labs_result_0" Then
                                        row = row.Replace("custom_text_other_labs_result_0", dItem2.answer)
                                    End If

                                    ' BLOOD CHEMISTRY RESULT
                                    If dItem2.id = "custom_choices_blood_chemistry_result_4" Then
                                        row = row.Replace("custom_choices_blood_chemistry_result_4", dItem2.answer)
                                    End If

                                    ' PAPSMEAR RESULT
                                    If dItem2.id = "custom_choices_papsmear_result_5" Then
                                        row = row.Replace("custom_choices_papsmear_result_5", dItem2.answer)
                                    End If

                                    ' ECG RESULT
                                    If dItem2.id = "custom_choices_ecg_result_6" Then
                                        row = row.Replace("custom_choices_ecg_result_6", dItem2.answer)
                                    End If

                                    ' OTHER ANCILLARY RESULTS
                                    If dItem2.id = "custom_text_other_ancillary_results_1" Then
                                        row = row.Replace("custom_text_other_ancillary_results_1", dItem2.answer)
                                    End If

                                    ' Impression
                                    If dItem2.id = "patient_impression_0" Then
                                        row = row.Replace("patient_impression_0", dItem2.answer)
                                    End If

                                    ' RECOMMENDATION
                                    If dItem2.id = "custom_text_recommendation_2" Then
                                        row = row.Replace("custom_text_recommendation_2", dItem2.answer)
                                    End If

                                    ' Clinic Name
                                    If dItem2.id = "clinic_name_0" Then
                                        row = row.Replace("clinic_name_0", dItem2.answer)
                                    End If
                                Next
                            End If

                            'safe
                            row = row.Replace("custom_text_other_labs_result_0", "")
                            row = row.Replace("custom_text_other_ancillary_results_1", "")
                            row = row.Replace("custom_text_recommendation_2", "")

                            sb.Append(row)
                        End If

                        iCount += 1
                    Next

                    If iCount = 0 Then
                        sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
                    End If

                Else
                    sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
                End If

            Else
                sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
            End If
        Else
            sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
        End If

        sb.Append("</table></div>")

        sRet = sb.ToString

        Return sRet
    End Function

    Private Shared Function GetReportFormat2_v2(ByVal accountCode As String, ByVal reportType As String,
                                             Optional ByVal memberCode As String = Nothing,
                                             Optional ByVal dtFrom As DateTime = Nothing,
                                             Optional ByVal dtTo As DateTime = Nothing,
                                             Optional ByRef reportViewer As Microsoft.Reporting.WebForms.ReportViewer = Nothing,
                                             Optional ByVal rptPath As String = Nothing,
                                             Optional ByVal companyName As String = Nothing) As String

        Dim sRet As String = Nothing

        Dim sb As New StringBuilder
        sb.Append("<div style='width: 100%; position: relative; background-color: #f9f9f9; overflow-x: scroll; max-height: 250px'><table id='tblUtilization'>")
        sb.Append("<tr><th style='min-width: 120px;'>Date Created</th><th style='min-width: 120px;'>Date of Exam</th><th style='min-width: 120px;'>Member Code</th><th style='min-width: 120px;'>Patient Name</th><th style='min-width: 120px;'>Patient Age</th><th style='min-width: 120px;'>Patient Sex</th><th style='min-width: 120px;'>Vitals - Pulse Rate</th><th style='min-width: 200px;'>Vitals - Respiration Rate</th><th style='min-width: 200px;'>Vitals - Blood Pressure</th><th style='min-width: 150px;'>Vitals - Weight (kg)</th><th style='min-width: 150px;'>Vitals - Height (cm)</th><th style='min-width: 120px;'>Vitals - BMI</th><th style='min-width: 200px;'>CHEST XRAY RESULT</th><th style='min-width: 120px;'>CBC RESULT</th><th style='min-width: 200px;'>URINALYSIS RESULT</th><th style='min-width: 200px;'>FECALYSIS RESULT</th><th style='min-width: 200px;'>OTHER LABS RESULT</th><th style='min-width: 200px;'>BLOOD CHEMISTRY RESULT</th><th style='min-width: 200px;'>PAPSMEAR RESULT</th><th style='min-width: 200px;'>ECG RESULT</th><th style='min-width: 200px;'>OTHER ANCILLARY RESULTS</th><th style='min-width: 200px;'>Impression</th><th style='min-width: 200px;'>RECOMMENDATION</th><th style='min-width: 200px;'>Clinic Name</th></tr>")

        Dim res As Mailhelper.MyCurePMESummaryResultModel = Mailhelper.MyCureAPI.Get_PME_Summary(accountCode, reportType, dtFrom, dtTo)

        Dim patientId As String = ""
        If memberCode <> "" Then
            patientId = Mailhelper.MyCureAPI.GetMyCurePatientId(memberCode, reportType.Replace("SUM", ""))
        End If

        Dim lDetails As New List(Of Mailhelper.MyCureReportParameter)

        If Not res Is Nothing Then
            If res.total <> "0" Then
                Dim dItem As Mailhelper.MyCurePMESummaryData1ResultModel

                If res.data.Count > 0 Then
                    Dim iCount As Long = 0
                    'sb.Append(res.data.Count.ToString())

                    For Each dItem In res.data.OrderBy(Function(c) UnixToDateTime(c.createdAt))
                        'sb.Append(dItem.id)

                        If memberCode <> "" Then
                            If dItem.patient <> patientId Then
                                'Next/Skip
                                Continue For
                            End If
                        End If

                        Dim details As New Mailhelper.MyCureReportParameter()
                        Dim res2 As Mailhelper.MyCurePMEResultModel = Mailhelper.MyCureAPI.Get_PME(dItem.id)
                        If res2 IsNot Nothing Then

                            Dim row As String = "<tr><td>date_created</td><td>date_exam</td><td>mem_code</td><td>patient_name</td><td>patient_age</td><td>patient_sex</td><td>vital_pulse_rate_0</td><td>vital_resp_rate_0</td><td>vital_blood_pressure_0</td><td>vital_weight_0</td><td>vital_height_0</td><td>vital_bmi_0</td><td>custom_choices_chest_xray_result_0</td><td>custom_choices_cbc_result_1</td><td>custom_choices_urinalysis_result_2</td><td>custom_choices_fecalysis_result_3</td><td>custom_text_other_labs_result_0</td><td>custom_choices_blood_chemistry_result_4</td><td>custom_choices_papsmear_result_5</td><td>custom_choices_ecg_result_6</td><td>custom_text_other_ancillary_results_1</td><td>patient_impression_0</td><td>custom_text_recommendation_2</td><td>clinic_name_0</td></tr>"

                            '=========================
                            'Personal Details
                            '=========================
                            If res2.populated IsNot Nothing Then
                                If res2.populated.patient IsNot Nothing Then
                                    Dim memCode As String = ""
                                    Dim cardInfo As Mailhelper.MyCurePMEPatientDetailsPatientCardResultModel
                                    If res2.populated.patient.insuranceCards IsNot Nothing Then
                                        If res2.populated.patient.insuranceCards.Count > 0 Then
                                            For Each cardInfo In res2.populated.patient.insuranceCards
                                                If cardInfo IsNot Nothing Then
                                                    If cardInfo.status = "active" Then
                                                        memCode = cardInfo.number
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If

                                    If reportType <> "PESUM" Then
                                        ' Only Principal will display
                                        If IsActivePrincipal(memCode, accountCode) = False Then
                                            'Next/Skip
                                            Continue For
                                        End If
                                    End If

                                    row = row.Replace("mem_code", memCode)
                                    row = row.Replace("patient_name", res2.populated.patient.name.firstName & " " & res2.populated.patient.name.middleName & " " & res2.populated.patient.name.lastName)
                                    row = row.Replace("patient_age", GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))) 'GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString))
                                    row = row.Replace("patient_sex", res2.populated.patient.sex)

                                    'Date Created
                                    row = row.Replace("date_created", UnixToDateTime(dItem.createdAt).ToString("MMMM dd, yyyy hh:mm tt"))

                                    'report viewer
                                    details.DateCreated = UnixToDateTime(dItem.createdAt).ToString("MMMM dd, yyyy hh:mm tt")
                                    details.MemberCode = memCode
                                    details.PatientName = res2.populated.patient.name.firstName & " " & res2.populated.patient.name.middleName & " " & res2.populated.patient.name.lastName
                                    details.PatientAge = GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))
                                    details.PatientSex = res2.populated.patient.sex
                                End If
                            End If
                            '=========================

                            If res2.values IsNot Nothing Then

                                Dim dList As List(Of Mailhelper.MyCureAPEData1ValuesResultModel) = res2.values
                                Dim dItem2 As Mailhelper.MyCureAPEData1ValuesResultModel
                                For Each dItem2 In dList
                                    'replace
                                    'row = row.Replace(dItem2.id, dItem2.answer)

                                    ' Date of exam
                                    If dItem2.id = "patient_encounter_created_at_0" Or dItem2.id = "patient_encounter_created_at_1" _
                                    Or dItem2.id = "today_0" Or dItem2.id = "today_1" Then
                                        row = row.Replace("date_exam", dItem2.answer)

                                        'report viewer
                                        If dItem2.answer IsNot Nothing Then
                                            details.DateOfExam = dItem2.answer.Replace("&nbsp;", "")
                                        End If
                                    End If

                                    ' Member Code
                                    ' Patient Name
                                    ' Patient Age
                                    ' Patient Sex

                                    ' Vitals - Pulse Rate
                                    If dItem2.id = "vital_pulse_rate_0" Then
                                        row = row.Replace("vital_pulse_rate_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsPR = dItem2.answer
                                    End If

                                    ' Vitals - Respiration Rate
                                    If dItem2.id = "vital_resp_rate_0" Then
                                        row = row.Replace("vital_resp_rate_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsRR = dItem2.answer
                                    End If

                                    ' Vitals - Blood Pressure
                                    If dItem2.id = "vital_blood_pressure_0" Then
                                        row = row.Replace("vital_blood_pressure_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsBR = dItem2.answer
                                    End If

                                    ' Vitals - Weight (kg)
                                    If dItem2.id = "vital_weight_0" Then
                                        row = row.Replace("vital_weight_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsWeight = dItem2.answer
                                    End If

                                    ' Vitals - Height (cm)
                                    If dItem2.id = "vital_height_0" Then
                                        row = row.Replace("vital_height_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsHeight = dItem2.answer
                                    End If

                                    ' Vitals - BMI
                                    If dItem2.id = "vital_bmi_0" Then
                                        row = row.Replace("vital_bmi_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsBMI = dItem2.answer
                                    End If

                                    ' CHEST XRAY RESULT
                                    If dItem2.id = "custom_choices_chest_xray_result_0" Then
                                        'row = row.Replace("custom_choices_chest_xray_result_0", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_chest_xray_result_0", dItem2.answer)

                                            'report viewer
                                            details.XrayResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "CXR PA " Or attItem.testName = "CXR PA" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                details.XrayResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_chest_xray_result_0", att)

                                        End If
                                    End If

                                    ' CBC RESULT
                                    If dItem2.id = "custom_choices_cbc_result_1" Then
                                        'row = row.Replace("custom_choices_cbc_result_1", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_cbc_result_1", dItem2.answer)

                                            'report viewer
                                            details.CBCResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "Complete Blood Count/CBC" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                details.CBCResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_cbc_result_1", att)

                                        End If
                                    End If

                                    ' URINALYSIS RESULT
                                    If dItem2.id = "custom_choices_urinalysis_result_2" Then
                                        'row = row.Replace("custom_choices_urinalysis_result_2", dItem2.answer)

                                        ''report viewer
                                        'details.UrinalysisResult = dItem2.answer

                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_urinalysis_result_2", dItem2.answer)

                                            'report viewer
                                            details.UrinalysisResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "Urinalysis" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                details.UrinalysisResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_urinalysis_result_2", att)

                                        End If
                                    End If

                                    ' FECALYSIS RESULT
                                    If dItem2.id = "custom_choices_fecalysis_result_3" Then
                                        'row = row.Replace("custom_choices_fecalysis_result_3", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_fecalysis_result_3", dItem2.answer)

                                            'report viewer
                                            details.FecalysisResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "Fecalysis/Stool Exam" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                details.FecalysisResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_fecalysis_result_3", att)

                                        End If
                                    End If

                                    ' OTHER LABS RESULT
                                    If dItem2.id = "custom_text_other_labs_result_0" Then
                                        row = row.Replace("custom_text_other_labs_result_0", dItem2.answer)

                                        'report viewer
                                        details.OtherLabResult = dItem2.answer
                                    End If

                                    ' BLOOD CHEMISTRY RESULT
                                    If dItem2.id = "custom_choices_blood_chemistry_result_4" Then
                                        'row = row.Replace("custom_choices_blood_chemistry_result_4", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_blood_chemistry_result_4", dItem2.answer)

                                            'report viewer
                                            details.BloodChem = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "10 Blood Chemistry/10BC" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                'details.BloodChem = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                                                                details.BloodChem += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_blood_chemistry_result_4", att)

                                        End If
                                    End If

                                    ' PAPSMEAR RESULT
                                    If dItem2.id = "custom_choices_papsmear_result_5" Then
                                        'row = row.Replace("custom_choices_papsmear_result_5", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_papsmear_result_5", dItem2.answer)

                                            'report viewer
                                            details.PapsmearResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "Pap Smear/Conventional PS" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                'details.PapsmearResult = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                                                                details.PapsmearResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_papsmear_result_5", att)
                                        End If
                                    End If

                                    ' ECG RESULT
                                    If dItem2.id = "custom_choices_ecg_result_6" Then
                                        'row = row.Replace("custom_choices_ecg_result_6", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_ecg_result_6", dItem2.answer)

                                            'report viewer
                                            details.ECGResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "12L ECG" Or attItem.testName = "ELECTROCARDIOGRAM (12L ECG)" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                'details.PapsmearResult = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                                                                details.ECGResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_ecg_result_6", att)
                                        End If
                                    End If

                                    ' OTHER ANCILLARY RESULTS
                                    If dItem2.id = "custom_text_other_ancillary_results_1" Then
                                        row = row.Replace("custom_text_other_ancillary_results_1", dItem2.answer)

                                        'report viewer
                                        details.AncillaryResult = dItem2.answer
                                    End If

                                    ' Impression
                                    If dItem2.id = "patient_impression_0" Then
                                        row = row.Replace("patient_impression_0", dItem2.answer)

                                        'report viewer
                                        If dItem2.answer IsNot Nothing Then
                                            details.Impression = dItem2.answer.Replace("<div>", ", ").Replace("</div>", "").Replace("<br>", "")
                                        End If
                                    End If

                                    ' RECOMMENDATION
                                    If dItem2.id = "custom_text_recommendation_2" Then
                                        row = row.Replace("custom_text_recommendation_2", dItem2.answer)

                                        'report viewer
                                        If dItem2.answer IsNot Nothing Then
                                            details.Recommendation = dItem2.answer.Replace("<div>", ", ").Replace("</div>", "").Replace("<br>", "")
                                        End If
                                    End If

                                    ' Clinic Name
                                    If dItem2.id = "clinic_name_0" Then
                                        row = row.Replace("clinic_name_0", dItem2.answer)

                                        'report viewer
                                        details.ClinicName = dItem2.answer
                                    End If
                                Next
                            End If

                            'safe
                            row = row.Replace("custom_text_other_labs_result_0", "")
                            row = row.Replace("custom_text_other_ancillary_results_1", "")
                            row = row.Replace("custom_text_recommendation_2", "")

                            sb.Append(row)

                            'report viewer
                            lDetails.Add(details)

                        End If

                        iCount += 1

                    Next

                    If iCount = 0 Then
                        sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
                    End If

                Else
                    sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
                End If

            Else
                sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
            End If
        Else
            sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
        End If

        sb.Append("</table></div>")

        sRet = sb.ToString

        '=========
        ' Report Viewer
        '=========
        Dim sRptType = ""
        Select Case reportType
            Case "APESUM"
                sRptType = "ANNUAL PHYSICAL EXAM"
            Case "PESUM"
                sRptType = "PRE-EMPLOYMENT"
            Case "ECUSUM"
                sRptType = "EXECUTIVE CHECK-UP"
        End Select

        Dim header As New List(Of Microsoft.Reporting.WebForms.ReportParameter)
        Dim accName As New Microsoft.Reporting.WebForms.ReportParameter("CompanyName", companyName)
        Dim dateRange As New Microsoft.Reporting.WebForms.ReportParameter("DateRange", dtFrom & " - " & dtTo)
        Dim totalData As New Microsoft.Reporting.WebForms.ReportParameter("TotalData", "")
        Dim resultType As New Microsoft.Reporting.WebForms.ReportParameter("ResultType", sRptType)
        header.Add(accName)
        header.Add(dateRange)
        header.Add(totalData)
        header.Add(resultType)

        reportViewer.LocalReport.DataSources.Clear()
        reportViewer.LocalReport.DisplayName = "Medicard Clinic Results"
        reportViewer.LocalReport.ReportPath = rptPath
        reportViewer.LocalReport.SetParameters(header)
        reportViewer.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("MyCureClinicResults", lDetails))
        reportViewer.LocalReport.Refresh()
        '============================

        Return sRet
    End Function

    Private Sub GetReportFormat2_v3(ByVal accountCode As String, ByVal reportType As String,
                                             Optional ByVal memberCode As String = Nothing,
                                             Optional ByVal dtFrom As DateTime = Nothing,
                                             Optional ByVal dtTo As DateTime = Nothing)


        'Dim res As Mailhelper.MyCurePMESummaryResultModel = Mailhelper.MyCureAPI.Get_PME_Summary(accountCode, reportType, dtFrom, dtTo)

        'Dim patientId As String = ""
        'If memberCode <> "" Then
        '    patientId = Mailhelper.MyCureAPI.GetMyCurePatientId(memberCode, reportType.Replace("SUM", ""))
        'End If

        'If Not res Is Nothing Then
        '    If res.total <> "0" Then
        '        Dim dItem As Mailhelper.MyCurePMESummaryData1ResultModel

        '        If res.data.Count > 0 Then
        '            Dim iCount As Long = 0
        '            For Each dItem In res.data

        '                If memberCode <> "" Then
        '                    If dItem.patient <> patientId Then
        '                        'Next/Skip
        '                        Continue For
        '                    End If
        '                End If

        '                Dim res2 As Mailhelper.MyCurePMEResultModel = Mailhelper.MyCureAPI.Get_PME(dItem.id)
        '                If res2 IsNot Nothing Then
        '                    'Dim row As String = "<tr><td>date_exam</td><td>mem_code</td><td>patient_name</td><td>patient_age</td><td>patient_sex</td><td>vital_pulse_rate_0</td><td>vital_resp_rate_0</td><td>vital_blood_pressure_0</td><td>vital_weight_0</td><td>vital_height_0</td><td>vital_bmi_0</td><td>custom_choices_chest_xray_result_0</td><td>custom_choices_cbc_result_1</td><td>custom_choices_urinalysis_result_2</td><td>custom_choices_fecalysis_result_3</td><td>custom_text_other_labs_result_0</td><td>custom_choices_blood_chemistry_result_4</td><td>custom_choices_papsmear_result_5</td><td>custom_choices_ecg_result_6</td><td>custom_text_other_ancillary_results_1</td><td>patient_impression_0</td><td>custom_text_recommendation_2</td><td>clinic_name_0</td></tr>"

        '                    If res2.values IsNot Nothing Then

        '                        Dim dList As List(Of Mailhelper.MyCureAPEData1ValuesResultModel) = res2.values
        '                        Dim dItem2 As Mailhelper.MyCureAPEData1ValuesResultModel
        '                        For Each dItem2 In dList
        '                            'replace

        '                            ' Date of exam
        '                            If dItem2.id = "patient_encounter_created_at_0" Or dItem2.id = "patient_encounter_created_at_1" _
        '                            Or dItem2.id = "today_0" Or dItem2.id = "today_1" Then
        '                                'row = row.Replace("date_exam", dItem2.answer)
        '                            End If

        '                        Next
        '                    End If


        '                    'sb.Append(row)
        '                End If

        '                iCount += 1
        '            Next

        '            If iCount = 0 Then
        '                'sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
        '            End If

        '        Else
        '            'sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
        '        End If

        '    Else
        '        'sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
        '    End If
        'Else
        '    'sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
        'End If

        Dim lDetails As New List(Of Mailhelper.MyCureReportParameter)
        'Dim details As New Mailhelper.MyCureReportParameter()
        'details.DateOfExam = DateTime.Now.ToString("MM/dd/yyyy")
        'lDetails.Add(details)

        Dim header As New List(Of Microsoft.Reporting.WebForms.ReportParameter)
        Dim companyName As New Microsoft.Reporting.WebForms.ReportParameter("CompanyName", accountCode)
        Dim dateRange As New Microsoft.Reporting.WebForms.ReportParameter("DateRange", dtFrom & " - " & dtTo)
        Dim totalData As New Microsoft.Reporting.WebForms.ReportParameter("TotalData", "")
        Dim resultType As New Microsoft.Reporting.WebForms.ReportParameter("ResultType", "")
        header.Add(companyName)
        header.Add(dateRange)
        header.Add(totalData)
        header.Add(resultType)

        'rpt.Text = Server.MapPath("~/AccountManager/ClinicResults.rdlc")

        ClinicResultsViewer.LocalReport.DataSources.Clear()
        ClinicResultsViewer.LocalReport.DisplayName = "Medicard Clinic Results"
        ClinicResultsViewer.LocalReport.ReportPath = Server.MapPath("~/AccountManager/ClinicResults.rdlc")
        ClinicResultsViewer.LocalReport.SetParameters(header)
        ClinicResultsViewer.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("MyCureClinicResults", lDetails))
        ClinicResultsViewer.LocalReport.Refresh()

    End Sub

    Private Shared Function GetReportFormat2_v3_broken(ByVal accountCode As String, ByVal reportType As String,
                                             Optional ByVal memberCode As String = Nothing,
                                             Optional ByVal dtFrom As DateTime = Nothing,
                                             Optional ByVal dtTo As DateTime = Nothing,
                                             Optional ByRef reportViewer As Microsoft.Reporting.WebForms.ReportViewer = Nothing,
                                             Optional ByVal rptPath As String = Nothing,
                                             Optional ByVal companyName As String = Nothing) As String

        Dim sRet As String = Nothing

        Dim sb As New StringBuilder
        sb.Append("<div style='width: 100%; position: relative; background-color: #f9f9f9; overflow-x: scroll; max-height: 250px'><table id='tblUtilization'>")
        sb.Append("<tr><th style='min-width: 120px;'>Date Created</th><th style='min-width: 120px;'>Date of Exam</th><th style='min-width: 120px;'>Member Code</th><th style='min-width: 120px;'>Patient Name</th><th style='min-width: 120px;'>Patient Age</th><th style='min-width: 120px;'>Patient Sex</th><th style='min-width: 120px;'>Vitals - Pulse Rate</th><th style='min-width: 200px;'>Vitals - Respiration Rate</th><th style='min-width: 200px;'>Vitals - Blood Pressure</th><th style='min-width: 150px;'>Vitals - Weight (kg)</th><th style='min-width: 150px;'>Vitals - Height (cm)</th><th style='min-width: 120px;'>Vitals - BMI</th><th style='min-width: 200px;'>CHEST XRAY RESULT</th><th style='min-width: 120px;'>CBC RESULT</th><th style='min-width: 200px;'>URINALYSIS RESULT</th><th style='min-width: 200px;'>FECALYSIS RESULT</th><th style='min-width: 200px;'>OTHER LABS RESULT</th><th style='min-width: 200px;'>BLOOD CHEMISTRY RESULT</th><th style='min-width: 200px;'>PAPSMEAR RESULT</th><th style='min-width: 200px;'>ECG RESULT</th><th style='min-width: 200px;'>OTHER ANCILLARY RESULTS</th><th style='min-width: 200px;'>Impression</th><th style='min-width: 200px;'>RECOMMENDATION</th><th style='min-width: 200px;'>Clinic Name</th></tr>")

        Dim res As Mailhelper.MyCurePMESummaryResultModel = Mailhelper.MyCureAPI.Get_PME_Summary(accountCode, reportType, dtFrom, dtTo)

        Dim patientId As String = ""
        If memberCode <> "" Then
            patientId = Mailhelper.MyCureAPI.GetMyCurePatientId(memberCode, reportType.Replace("SUM", ""))
        End If

        Dim lDetails As New List(Of Mailhelper.MyCureReportParameter)
        Dim lAll As New List(Of Mailhelper.MyCurePMESummaryData1ResultModel)

        If Not res Is Nothing Then
            If res.total <> "0" Then
                Dim dItem As Mailhelper.MyCurePMESummaryData1ResultModel

                Dim iTotal As Integer = Convert.ToInt32(res.total)
                If iTotal > 100 Then
                    lAll = res.data.ToList()
                    Dim icurrCount As Integer = 100
                    Do Until icurrCount > iTotal
                        Dim resNext As Mailhelper.MyCurePMESummaryResultModel = Mailhelper.MyCureAPI.Get_PME_Summary_continuation(accountCode, reportType, icurrCount, dtFrom, dtTo)
                        If Not resNext Is Nothing Then
                            If resNext.total <> "0" Then
                                If resNext.data.Count > 0 Then
                                    'combine to result 1
                                    'res.data.Concat(resNext.data)

                                    For Each dItem In resNext.data
                                        lAll.Add(dItem)
                                    Next

                                    icurrCount += resNext.data.Count
                                Else
                                    icurrCount += 0
                                End If
                            Else
                                icurrCount += 0
                            End If
                        End If
                    Loop
                End If


                If res.data.Count > 0 Then
                    Dim iCount As Long = 0
                    sb.Append(res.data.Count.ToString())
                    sb.Append(lAll.Count.ToString())

                    'Dim iTotal As Integer = Convert.ToInt32(res.total)
                    'If iTotal > 100 Then
                    '    Dim icurrCount As Integer = 0
                    '    Do Until icurrCount > iTotal
                    '        If icurrCount > 0 Then
                    '            res = Mailhelper.MyCureAPI.Get_PME_Summary_continuation(accountCode, reportType, icurrCount, dtFrom, dtTo)
                    '        End If
                    '        If Not res Is Nothing Then
                    '            If res.total <> "0" Then
                    '                If res.data.Count > 0 Then

                    '                    ' data starts here...
                    '                    For Each dItem In res.data.OrderBy(Function(c) UnixToDateTime(c.createdAt))
                    '                        sb.Append(dItem.id)

                    '                        'If memberCode <> "" Then
                    '                        '    If dItem.patient <> patientId Then
                    '                        '        'Next/Skip
                    '                        '        Continue For
                    '                        '    End If
                    '                        'End If

                    '                        'Dim details As New Mailhelper.MyCureReportParameter()
                    '                        'Dim res2 As Mailhelper.MyCurePMEResultModel = Mailhelper.MyCureAPI.Get_PME(dItem.id)
                    '                        'If res2 IsNot Nothing Then

                    '                        '    Dim row As String = "<tr><td>date_created</td><td>date_exam</td><td>mem_code</td><td>patient_name</td><td>patient_age</td><td>patient_sex</td><td>vital_pulse_rate_0</td><td>vital_resp_rate_0</td><td>vital_blood_pressure_0</td><td>vital_weight_0</td><td>vital_height_0</td><td>vital_bmi_0</td><td>custom_choices_chest_xray_result_0</td><td>custom_choices_cbc_result_1</td><td>custom_choices_urinalysis_result_2</td><td>custom_choices_fecalysis_result_3</td><td>custom_text_other_labs_result_0</td><td>custom_choices_blood_chemistry_result_4</td><td>custom_choices_papsmear_result_5</td><td>custom_choices_ecg_result_6</td><td>custom_text_other_ancillary_results_1</td><td>patient_impression_0</td><td>custom_text_recommendation_2</td><td>clinic_name_0</td></tr>"

                    '                        '    '=========================
                    '                        '    'Personal Details
                    '                        '    '=========================
                    '                        '    If res2.populated IsNot Nothing Then
                    '                        '        If res2.populated.patient IsNot Nothing Then
                    '                        '            Dim memCode As String = ""
                    '                        '            Dim cardInfo As Mailhelper.MyCurePMEPatientDetailsPatientCardResultModel
                    '                        '            If res2.populated.patient.insuranceCards IsNot Nothing Then
                    '                        '                If res2.populated.patient.insuranceCards.Count > 0 Then
                    '                        '                    For Each cardInfo In res2.populated.patient.insuranceCards
                    '                        '                        If cardInfo IsNot Nothing Then
                    '                        '                            If cardInfo.status = "active" Then
                    '                        '                                memCode = cardInfo.number
                    '                        '                            End If
                    '                        '                        End If
                    '                        '                    Next
                    '                        '                End If
                    '                        '            End If

                    '                        '            If reportType <> "PESUM" Then
                    '                        '                ' Only Principal will display
                    '                        '                If IsActivePrincipal(memCode, accountCode) = False Then
                    '                        '                    'Next/Skip
                    '                        '                    Continue For
                    '                        '                End If
                    '                        '            End If

                    '                        '            row = row.Replace("mem_code", memCode)
                    '                        '            row = row.Replace("patient_name", res2.populated.patient.name.firstName & " " & res2.populated.patient.name.middleName & " " & res2.populated.patient.name.lastName)
                    '                        '            row = row.Replace("patient_age", GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))) 'GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString))
                    '                        '            row = row.Replace("patient_sex", res2.populated.patient.sex)

                    '                        '            'Date Created
                    '                        '            row = row.Replace("date_created", UnixToDateTime(dItem.createdAt).ToString("MMMM dd, yyyy hh:mm tt"))

                    '                        '            'report viewer
                    '                        '            details.DateCreated = UnixToDateTime(dItem.createdAt).ToString("MMMM dd, yyyy hh:mm tt")
                    '                        '            details.MemberCode = memCode
                    '                        '            details.PatientName = res2.populated.patient.name.firstName & " " & res2.populated.patient.name.middleName & " " & res2.populated.patient.name.lastName
                    '                        '            details.PatientAge = GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))
                    '                        '            details.PatientSex = res2.populated.patient.sex
                    '                        '        End If
                    '                        '    End If
                    '                        '    '=========================

                    '                        '    If res2.values IsNot Nothing Then

                    '                        '        Dim dList As List(Of Mailhelper.MyCureAPEData1ValuesResultModel) = res2.values
                    '                        '        Dim dItem2 As Mailhelper.MyCureAPEData1ValuesResultModel
                    '                        '        For Each dItem2 In dList
                    '                        '            'replace
                    '                        '            'row = row.Replace(dItem2.id, dItem2.answer)

                    '                        '            ' Date of exam
                    '                        '            If dItem2.id = "patient_encounter_created_at_0" Or dItem2.id = "patient_encounter_created_at_1" _
                    '                        '            Or dItem2.id = "today_0" Or dItem2.id = "today_1" Then
                    '                        '                row = row.Replace("date_exam", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                If dItem2.answer IsNot Nothing Then
                    '                        '                    details.DateOfExam = dItem2.answer.Replace("&nbsp;", "")
                    '                        '                End If
                    '                        '            End If

                    '                        '            ' Member Code
                    '                        '            ' Patient Name
                    '                        '            ' Patient Age
                    '                        '            ' Patient Sex

                    '                        '            ' Vitals - Pulse Rate
                    '                        '            If dItem2.id = "vital_pulse_rate_0" Then
                    '                        '                row = row.Replace("vital_pulse_rate_0", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                details.VitalsPR = dItem2.answer
                    '                        '            End If

                    '                        '            ' Vitals - Respiration Rate
                    '                        '            If dItem2.id = "vital_resp_rate_0" Then
                    '                        '                row = row.Replace("vital_resp_rate_0", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                details.VitalsRR = dItem2.answer
                    '                        '            End If

                    '                        '            ' Vitals - Blood Pressure
                    '                        '            If dItem2.id = "vital_blood_pressure_0" Then
                    '                        '                row = row.Replace("vital_blood_pressure_0", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                details.VitalsBR = dItem2.answer
                    '                        '            End If

                    '                        '            ' Vitals - Weight (kg)
                    '                        '            If dItem2.id = "vital_weight_0" Then
                    '                        '                row = row.Replace("vital_weight_0", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                details.VitalsWeight = dItem2.answer
                    '                        '            End If

                    '                        '            ' Vitals - Height (cm)
                    '                        '            If dItem2.id = "vital_height_0" Then
                    '                        '                row = row.Replace("vital_height_0", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                details.VitalsHeight = dItem2.answer
                    '                        '            End If

                    '                        '            ' Vitals - BMI
                    '                        '            If dItem2.id = "vital_bmi_0" Then
                    '                        '                row = row.Replace("vital_bmi_0", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                details.VitalsBMI = dItem2.answer
                    '                        '            End If

                    '                        '            ' CHEST XRAY RESULT
                    '                        '            If dItem2.id = "custom_choices_chest_xray_result_0" Then
                    '                        '                'row = row.Replace("custom_choices_chest_xray_result_0", dItem2.answer)
                    '                        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '                        '                    row = row.Replace("custom_choices_chest_xray_result_0", dItem2.answer)

                    '                        '                    'report viewer
                    '                        '                    details.XrayResult = dItem2.answer
                    '                        '                Else
                    '                        '                    Dim att As String = "SEE ATTACHED"
                    '                        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '                        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '                        '                    If attList.Count > 0 Then
                    '                        '                        att = ""
                    '                        '                        For Each attItem In attList
                    '                        '                            If attItem.testName = "CXR PA " Or attItem.testName = "CXR PA" Then
                    '                        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '                        '                                    For Each urls As String In attItem.attachmentURLs
                    '                        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '                        '                                        'report viewer
                    '                        '                                        details.XrayResult += " SEE ATTACHED: " & urls & ", "
                    '                        '                                    Next
                    '                        '                                End If

                    '                        '                            End If
                    '                        '                        Next
                    '                        '                    Else
                    '                        '                        att = "SEE ATTACHED"
                    '                        '                    End If

                    '                        '                    row = row.Replace("custom_choices_chest_xray_result_0", att)

                    '                        '                End If
                    '                        '            End If

                    '                        '            ' CBC RESULT
                    '                        '            If dItem2.id = "custom_choices_cbc_result_1" Then
                    '                        '                'row = row.Replace("custom_choices_cbc_result_1", dItem2.answer)
                    '                        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '                        '                    row = row.Replace("custom_choices_cbc_result_1", dItem2.answer)

                    '                        '                    'report viewer
                    '                        '                    details.CBCResult = dItem2.answer
                    '                        '                Else
                    '                        '                    Dim att As String = "SEE ATTACHED"
                    '                        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '                        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '                        '                    If attList.Count > 0 Then
                    '                        '                        att = ""
                    '                        '                        For Each attItem In attList
                    '                        '                            If attItem.testName = "Complete Blood Count/CBC" Then
                    '                        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '                        '                                    For Each urls As String In attItem.attachmentURLs
                    '                        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '                        '                                        'report viewer
                    '                        '                                        details.CBCResult += " SEE ATTACHED: " & urls & ", "
                    '                        '                                    Next
                    '                        '                                End If

                    '                        '                            End If
                    '                        '                        Next
                    '                        '                    Else
                    '                        '                        att = "SEE ATTACHED"
                    '                        '                    End If

                    '                        '                    row = row.Replace("custom_choices_cbc_result_1", att)

                    '                        '                End If
                    '                        '            End If

                    '                        '            ' URINALYSIS RESULT
                    '                        '            If dItem2.id = "custom_choices_urinalysis_result_2" Then
                    '                        '                'row = row.Replace("custom_choices_urinalysis_result_2", dItem2.answer)

                    '                        '                ''report viewer
                    '                        '                'details.UrinalysisResult = dItem2.answer

                    '                        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '                        '                    row = row.Replace("custom_choices_urinalysis_result_2", dItem2.answer)

                    '                        '                    'report viewer
                    '                        '                    details.UrinalysisResult = dItem2.answer
                    '                        '                Else
                    '                        '                    Dim att As String = "SEE ATTACHED"
                    '                        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '                        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '                        '                    If attList.Count > 0 Then
                    '                        '                        att = ""
                    '                        '                        For Each attItem In attList
                    '                        '                            If attItem.testName = "Urinalysis" Then
                    '                        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '                        '                                    For Each urls As String In attItem.attachmentURLs
                    '                        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '                        '                                        'report viewer
                    '                        '                                        details.UrinalysisResult += " SEE ATTACHED: " & urls & ", "
                    '                        '                                    Next
                    '                        '                                End If

                    '                        '                            End If
                    '                        '                        Next
                    '                        '                    Else
                    '                        '                        att = "SEE ATTACHED"
                    '                        '                    End If

                    '                        '                    row = row.Replace("custom_choices_urinalysis_result_2", att)

                    '                        '                End If
                    '                        '            End If

                    '                        '            ' FECALYSIS RESULT
                    '                        '            If dItem2.id = "custom_choices_fecalysis_result_3" Then
                    '                        '                'row = row.Replace("custom_choices_fecalysis_result_3", dItem2.answer)
                    '                        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '                        '                    row = row.Replace("custom_choices_fecalysis_result_3", dItem2.answer)

                    '                        '                    'report viewer
                    '                        '                    details.FecalysisResult = dItem2.answer
                    '                        '                Else
                    '                        '                    Dim att As String = "SEE ATTACHED"
                    '                        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '                        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '                        '                    If attList.Count > 0 Then
                    '                        '                        att = ""
                    '                        '                        For Each attItem In attList
                    '                        '                            If attItem.testName = "Fecalysis/Stool Exam" Then
                    '                        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '                        '                                    For Each urls As String In attItem.attachmentURLs
                    '                        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '                        '                                        'report viewer
                    '                        '                                        details.FecalysisResult += " SEE ATTACHED: " & urls & ", "
                    '                        '                                    Next
                    '                        '                                End If

                    '                        '                            End If
                    '                        '                        Next
                    '                        '                    Else
                    '                        '                        att = "SEE ATTACHED"
                    '                        '                    End If

                    '                        '                    row = row.Replace("custom_choices_fecalysis_result_3", att)

                    '                        '                End If
                    '                        '            End If

                    '                        '            ' OTHER LABS RESULT
                    '                        '            If dItem2.id = "custom_text_other_labs_result_0" Then
                    '                        '                row = row.Replace("custom_text_other_labs_result_0", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                details.OtherLabResult = dItem2.answer
                    '                        '            End If

                    '                        '            ' BLOOD CHEMISTRY RESULT
                    '                        '            If dItem2.id = "custom_choices_blood_chemistry_result_4" Then
                    '                        '                'row = row.Replace("custom_choices_blood_chemistry_result_4", dItem2.answer)
                    '                        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '                        '                    row = row.Replace("custom_choices_blood_chemistry_result_4", dItem2.answer)

                    '                        '                    'report viewer
                    '                        '                    details.BloodChem = dItem2.answer
                    '                        '                Else
                    '                        '                    Dim att As String = "SEE ATTACHED"
                    '                        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '                        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '                        '                    If attList.Count > 0 Then
                    '                        '                        att = ""
                    '                        '                        For Each attItem In attList
                    '                        '                            If attItem.testName = "10 Blood Chemistry/10BC" Then
                    '                        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '                        '                                    For Each urls As String In attItem.attachmentURLs
                    '                        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '                        '                                        'report viewer
                    '                        '                                        'details.BloodChem = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                    '                        '                                        details.BloodChem += " SEE ATTACHED: " & urls & ", "
                    '                        '                                    Next
                    '                        '                                End If

                    '                        '                            End If
                    '                        '                        Next
                    '                        '                    Else
                    '                        '                        att = "SEE ATTACHED"
                    '                        '                    End If

                    '                        '                    row = row.Replace("custom_choices_blood_chemistry_result_4", att)

                    '                        '                End If
                    '                        '            End If

                    '                        '            ' PAPSMEAR RESULT
                    '                        '            If dItem2.id = "custom_choices_papsmear_result_5" Then
                    '                        '                'row = row.Replace("custom_choices_papsmear_result_5", dItem2.answer)
                    '                        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '                        '                    row = row.Replace("custom_choices_papsmear_result_5", dItem2.answer)

                    '                        '                    'report viewer
                    '                        '                    details.PapsmearResult = dItem2.answer
                    '                        '                Else
                    '                        '                    Dim att As String = "SEE ATTACHED"
                    '                        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '                        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '                        '                    If attList.Count > 0 Then
                    '                        '                        att = ""
                    '                        '                        For Each attItem In attList
                    '                        '                            If attItem.testName = "Pap Smear/Conventional PS" Then
                    '                        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '                        '                                    For Each urls As String In attItem.attachmentURLs
                    '                        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '                        '                                        'report viewer
                    '                        '                                        'details.PapsmearResult = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                    '                        '                                        details.PapsmearResult += " SEE ATTACHED: " & urls & ", "
                    '                        '                                    Next
                    '                        '                                End If

                    '                        '                            End If
                    '                        '                        Next
                    '                        '                    Else
                    '                        '                        att = "SEE ATTACHED"
                    '                        '                    End If

                    '                        '                    row = row.Replace("custom_choices_papsmear_result_5", att)
                    '                        '                End If
                    '                        '            End If

                    '                        '            ' ECG RESULT
                    '                        '            If dItem2.id = "custom_choices_ecg_result_6" Then
                    '                        '                'row = row.Replace("custom_choices_ecg_result_6", dItem2.answer)
                    '                        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '                        '                    row = row.Replace("custom_choices_ecg_result_6", dItem2.answer)

                    '                        '                    'report viewer
                    '                        '                    details.ECGResult = dItem2.answer
                    '                        '                Else
                    '                        '                    Dim att As String = "SEE ATTACHED"
                    '                        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '                        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '                        '                    If attList.Count > 0 Then
                    '                        '                        att = ""
                    '                        '                        For Each attItem In attList
                    '                        '                            If attItem.testName = "12L ECG" Or attItem.testName = "ELECTROCARDIOGRAM (12L ECG)" Then
                    '                        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '                        '                                    For Each urls As String In attItem.attachmentURLs
                    '                        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '                        '                                        'report viewer
                    '                        '                                        'details.PapsmearResult = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                    '                        '                                        details.ECGResult += " SEE ATTACHED: " & urls & ", "
                    '                        '                                    Next
                    '                        '                                End If

                    '                        '                            End If
                    '                        '                        Next
                    '                        '                    Else
                    '                        '                        att = "SEE ATTACHED"
                    '                        '                    End If

                    '                        '                    row = row.Replace("custom_choices_ecg_result_6", att)
                    '                        '                End If
                    '                        '            End If

                    '                        '            ' OTHER ANCILLARY RESULTS
                    '                        '            If dItem2.id = "custom_text_other_ancillary_results_1" Then
                    '                        '                row = row.Replace("custom_text_other_ancillary_results_1", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                details.AncillaryResult = dItem2.answer
                    '                        '            End If

                    '                        '            ' Impression
                    '                        '            If dItem2.id = "patient_impression_0" Then
                    '                        '                row = row.Replace("patient_impression_0", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                If dItem2.answer IsNot Nothing Then
                    '                        '                    details.Impression = dItem2.answer.Replace("<div>", ", ").Replace("</div>", "").Replace("<br>", "")
                    '                        '                End If
                    '                        '            End If

                    '                        '            ' RECOMMENDATION
                    '                        '            If dItem2.id = "custom_text_recommendation_2" Then
                    '                        '                row = row.Replace("custom_text_recommendation_2", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                If dItem2.answer IsNot Nothing Then
                    '                        '                    details.Recommendation = dItem2.answer.Replace("<div>", ", ").Replace("</div>", "").Replace("<br>", "")
                    '                        '                End If
                    '                        '            End If

                    '                        '            ' Clinic Name
                    '                        '            If dItem2.id = "clinic_name_0" Then
                    '                        '                row = row.Replace("clinic_name_0", dItem2.answer)

                    '                        '                'report viewer
                    '                        '                details.ClinicName = dItem2.answer
                    '                        '            End If
                    '                        '        Next
                    '                        '    End If

                    '                        '    'safe
                    '                        '    row = row.Replace("custom_text_other_labs_result_0", "")
                    '                        '    row = row.Replace("custom_text_other_ancillary_results_1", "")
                    '                        '    row = row.Replace("custom_text_recommendation_2", "")

                    '                        '    sb.Append(row)

                    '                        '    'report viewer
                    '                        '    lDetails.Add(details)

                    '                        'End If

                    '                        iCount += 1
                    '                    Next
                    '                    ' data ends here...

                    '                    'counter
                    '                    icurrCount += res.data.Count
                    '                Else
                    '                    'couner
                    '                    icurrCount += 0
                    '                End If
                    '            Else
                    '                'counter
                    '                icurrCount += 0
                    '            End If
                    '        End If
                    '    Loop
                    'Else
                    '    ' data starts here...
                    '    For Each dItem In res.data.OrderBy(Function(c) UnixToDateTime(c.createdAt))
                    '        sb.Append(dItem.id)

                    '        'If memberCode <> "" Then
                    '        '    If dItem.patient <> patientId Then
                    '        '        'Next/Skip
                    '        '        Continue For
                    '        '    End If
                    '        'End If

                    '        'Dim details As New Mailhelper.MyCureReportParameter()
                    '        'Dim res2 As Mailhelper.MyCurePMEResultModel = Mailhelper.MyCureAPI.Get_PME(dItem.id)
                    '        'If res2 IsNot Nothing Then

                    '        '    Dim row As String = "<tr><td>date_created</td><td>date_exam</td><td>mem_code</td><td>patient_name</td><td>patient_age</td><td>patient_sex</td><td>vital_pulse_rate_0</td><td>vital_resp_rate_0</td><td>vital_blood_pressure_0</td><td>vital_weight_0</td><td>vital_height_0</td><td>vital_bmi_0</td><td>custom_choices_chest_xray_result_0</td><td>custom_choices_cbc_result_1</td><td>custom_choices_urinalysis_result_2</td><td>custom_choices_fecalysis_result_3</td><td>custom_text_other_labs_result_0</td><td>custom_choices_blood_chemistry_result_4</td><td>custom_choices_papsmear_result_5</td><td>custom_choices_ecg_result_6</td><td>custom_text_other_ancillary_results_1</td><td>patient_impression_0</td><td>custom_text_recommendation_2</td><td>clinic_name_0</td></tr>"

                    '        '    '=========================
                    '        '    'Personal Details
                    '        '    '=========================
                    '        '    If res2.populated IsNot Nothing Then
                    '        '        If res2.populated.patient IsNot Nothing Then
                    '        '            Dim memCode As String = ""
                    '        '            Dim cardInfo As Mailhelper.MyCurePMEPatientDetailsPatientCardResultModel
                    '        '            If res2.populated.patient.insuranceCards IsNot Nothing Then
                    '        '                If res2.populated.patient.insuranceCards.Count > 0 Then
                    '        '                    For Each cardInfo In res2.populated.patient.insuranceCards
                    '        '                        If cardInfo IsNot Nothing Then
                    '        '                            If cardInfo.status = "active" Then
                    '        '                                memCode = cardInfo.number
                    '        '                            End If
                    '        '                        End If
                    '        '                    Next
                    '        '                End If
                    '        '            End If

                    '        '            If reportType <> "PESUM" Then
                    '        '                ' Only Principal will display
                    '        '                If IsActivePrincipal(memCode, accountCode) = False Then
                    '        '                    'Next/Skip
                    '        '                    Continue For
                    '        '                End If
                    '        '            End If

                    '        '            row = row.Replace("mem_code", memCode)
                    '        '            row = row.Replace("patient_name", res2.populated.patient.name.firstName & " " & res2.populated.patient.name.middleName & " " & res2.populated.patient.name.lastName)
                    '        '            row = row.Replace("patient_age", GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))) 'GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString))
                    '        '            row = row.Replace("patient_sex", res2.populated.patient.sex)

                    '        '            'Date Created
                    '        '            row = row.Replace("date_created", UnixToDateTime(dItem.createdAt).ToString("MMMM dd, yyyy hh:mm tt"))

                    '        '            'report viewer
                    '        '            details.DateCreated = UnixToDateTime(dItem.createdAt).ToString("MMMM dd, yyyy hh:mm tt")
                    '        '            details.MemberCode = memCode
                    '        '            details.PatientName = res2.populated.patient.name.firstName & " " & res2.populated.patient.name.middleName & " " & res2.populated.patient.name.lastName
                    '        '            details.PatientAge = GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))
                    '        '            details.PatientSex = res2.populated.patient.sex
                    '        '        End If
                    '        '    End If
                    '        '    '=========================

                    '        '    If res2.values IsNot Nothing Then

                    '        '        Dim dList As List(Of Mailhelper.MyCureAPEData1ValuesResultModel) = res2.values
                    '        '        Dim dItem2 As Mailhelper.MyCureAPEData1ValuesResultModel
                    '        '        For Each dItem2 In dList
                    '        '            'replace
                    '        '            'row = row.Replace(dItem2.id, dItem2.answer)

                    '        '            ' Date of exam
                    '        '            If dItem2.id = "patient_encounter_created_at_0" Or dItem2.id = "patient_encounter_created_at_1" _
                    '        '            Or dItem2.id = "today_0" Or dItem2.id = "today_1" Then
                    '        '                row = row.Replace("date_exam", dItem2.answer)

                    '        '                'report viewer
                    '        '                If dItem2.answer IsNot Nothing Then
                    '        '                    details.DateOfExam = dItem2.answer.Replace("&nbsp;", "")
                    '        '                End If
                    '        '            End If

                    '        '            ' Member Code
                    '        '            ' Patient Name
                    '        '            ' Patient Age
                    '        '            ' Patient Sex

                    '        '            ' Vitals - Pulse Rate
                    '        '            If dItem2.id = "vital_pulse_rate_0" Then
                    '        '                row = row.Replace("vital_pulse_rate_0", dItem2.answer)

                    '        '                'report viewer
                    '        '                details.VitalsPR = dItem2.answer
                    '        '            End If

                    '        '            ' Vitals - Respiration Rate
                    '        '            If dItem2.id = "vital_resp_rate_0" Then
                    '        '                row = row.Replace("vital_resp_rate_0", dItem2.answer)

                    '        '                'report viewer
                    '        '                details.VitalsRR = dItem2.answer
                    '        '            End If

                    '        '            ' Vitals - Blood Pressure
                    '        '            If dItem2.id = "vital_blood_pressure_0" Then
                    '        '                row = row.Replace("vital_blood_pressure_0", dItem2.answer)

                    '        '                'report viewer
                    '        '                details.VitalsBR = dItem2.answer
                    '        '            End If

                    '        '            ' Vitals - Weight (kg)
                    '        '            If dItem2.id = "vital_weight_0" Then
                    '        '                row = row.Replace("vital_weight_0", dItem2.answer)

                    '        '                'report viewer
                    '        '                details.VitalsWeight = dItem2.answer
                    '        '            End If

                    '        '            ' Vitals - Height (cm)
                    '        '            If dItem2.id = "vital_height_0" Then
                    '        '                row = row.Replace("vital_height_0", dItem2.answer)

                    '        '                'report viewer
                    '        '                details.VitalsHeight = dItem2.answer
                    '        '            End If

                    '        '            ' Vitals - BMI
                    '        '            If dItem2.id = "vital_bmi_0" Then
                    '        '                row = row.Replace("vital_bmi_0", dItem2.answer)

                    '        '                'report viewer
                    '        '                details.VitalsBMI = dItem2.answer
                    '        '            End If

                    '        '            ' CHEST XRAY RESULT
                    '        '            If dItem2.id = "custom_choices_chest_xray_result_0" Then
                    '        '                'row = row.Replace("custom_choices_chest_xray_result_0", dItem2.answer)
                    '        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '        '                    row = row.Replace("custom_choices_chest_xray_result_0", dItem2.answer)

                    '        '                    'report viewer
                    '        '                    details.XrayResult = dItem2.answer
                    '        '                Else
                    '        '                    Dim att As String = "SEE ATTACHED"
                    '        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '        '                    If attList.Count > 0 Then
                    '        '                        att = ""
                    '        '                        For Each attItem In attList
                    '        '                            If attItem.testName = "CXR PA " Or attItem.testName = "CXR PA" Then
                    '        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '        '                                    For Each urls As String In attItem.attachmentURLs
                    '        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '        '                                        'report viewer
                    '        '                                        details.XrayResult += " SEE ATTACHED: " & urls & ", "
                    '        '                                    Next
                    '        '                                End If

                    '        '                            End If
                    '        '                        Next
                    '        '                    Else
                    '        '                        att = "SEE ATTACHED"
                    '        '                    End If

                    '        '                    row = row.Replace("custom_choices_chest_xray_result_0", att)

                    '        '                End If
                    '        '            End If

                    '        '            ' CBC RESULT
                    '        '            If dItem2.id = "custom_choices_cbc_result_1" Then
                    '        '                'row = row.Replace("custom_choices_cbc_result_1", dItem2.answer)
                    '        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '        '                    row = row.Replace("custom_choices_cbc_result_1", dItem2.answer)

                    '        '                    'report viewer
                    '        '                    details.CBCResult = dItem2.answer
                    '        '                Else
                    '        '                    Dim att As String = "SEE ATTACHED"
                    '        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '        '                    If attList.Count > 0 Then
                    '        '                        att = ""
                    '        '                        For Each attItem In attList
                    '        '                            If attItem.testName = "Complete Blood Count/CBC" Then
                    '        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '        '                                    For Each urls As String In attItem.attachmentURLs
                    '        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '        '                                        'report viewer
                    '        '                                        details.CBCResult += " SEE ATTACHED: " & urls & ", "
                    '        '                                    Next
                    '        '                                End If

                    '        '                            End If
                    '        '                        Next
                    '        '                    Else
                    '        '                        att = "SEE ATTACHED"
                    '        '                    End If

                    '        '                    row = row.Replace("custom_choices_cbc_result_1", att)

                    '        '                End If
                    '        '            End If

                    '        '            ' URINALYSIS RESULT
                    '        '            If dItem2.id = "custom_choices_urinalysis_result_2" Then
                    '        '                'row = row.Replace("custom_choices_urinalysis_result_2", dItem2.answer)

                    '        '                ''report viewer
                    '        '                'details.UrinalysisResult = dItem2.answer

                    '        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '        '                    row = row.Replace("custom_choices_urinalysis_result_2", dItem2.answer)

                    '        '                    'report viewer
                    '        '                    details.UrinalysisResult = dItem2.answer
                    '        '                Else
                    '        '                    Dim att As String = "SEE ATTACHED"
                    '        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '        '                    If attList.Count > 0 Then
                    '        '                        att = ""
                    '        '                        For Each attItem In attList
                    '        '                            If attItem.testName = "Urinalysis" Then
                    '        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '        '                                    For Each urls As String In attItem.attachmentURLs
                    '        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '        '                                        'report viewer
                    '        '                                        details.UrinalysisResult += " SEE ATTACHED: " & urls & ", "
                    '        '                                    Next
                    '        '                                End If

                    '        '                            End If
                    '        '                        Next
                    '        '                    Else
                    '        '                        att = "SEE ATTACHED"
                    '        '                    End If

                    '        '                    row = row.Replace("custom_choices_urinalysis_result_2", att)

                    '        '                End If
                    '        '            End If

                    '        '            ' FECALYSIS RESULT
                    '        '            If dItem2.id = "custom_choices_fecalysis_result_3" Then
                    '        '                'row = row.Replace("custom_choices_fecalysis_result_3", dItem2.answer)
                    '        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '        '                    row = row.Replace("custom_choices_fecalysis_result_3", dItem2.answer)

                    '        '                    'report viewer
                    '        '                    details.FecalysisResult = dItem2.answer
                    '        '                Else
                    '        '                    Dim att As String = "SEE ATTACHED"
                    '        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '        '                    If attList.Count > 0 Then
                    '        '                        att = ""
                    '        '                        For Each attItem In attList
                    '        '                            If attItem.testName = "Fecalysis/Stool Exam" Then
                    '        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '        '                                    For Each urls As String In attItem.attachmentURLs
                    '        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '        '                                        'report viewer
                    '        '                                        details.FecalysisResult += " SEE ATTACHED: " & urls & ", "
                    '        '                                    Next
                    '        '                                End If

                    '        '                            End If
                    '        '                        Next
                    '        '                    Else
                    '        '                        att = "SEE ATTACHED"
                    '        '                    End If

                    '        '                    row = row.Replace("custom_choices_fecalysis_result_3", att)

                    '        '                End If
                    '        '            End If

                    '        '            ' OTHER LABS RESULT
                    '        '            If dItem2.id = "custom_text_other_labs_result_0" Then
                    '        '                row = row.Replace("custom_text_other_labs_result_0", dItem2.answer)

                    '        '                'report viewer
                    '        '                details.OtherLabResult = dItem2.answer
                    '        '            End If

                    '        '            ' BLOOD CHEMISTRY RESULT
                    '        '            If dItem2.id = "custom_choices_blood_chemistry_result_4" Then
                    '        '                'row = row.Replace("custom_choices_blood_chemistry_result_4", dItem2.answer)
                    '        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '        '                    row = row.Replace("custom_choices_blood_chemistry_result_4", dItem2.answer)

                    '        '                    'report viewer
                    '        '                    details.BloodChem = dItem2.answer
                    '        '                Else
                    '        '                    Dim att As String = "SEE ATTACHED"
                    '        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '        '                    If attList.Count > 0 Then
                    '        '                        att = ""
                    '        '                        For Each attItem In attList
                    '        '                            If attItem.testName = "10 Blood Chemistry/10BC" Then
                    '        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '        '                                    For Each urls As String In attItem.attachmentURLs
                    '        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '        '                                        'report viewer
                    '        '                                        'details.BloodChem = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                    '        '                                        details.BloodChem += " SEE ATTACHED: " & urls & ", "
                    '        '                                    Next
                    '        '                                End If

                    '        '                            End If
                    '        '                        Next
                    '        '                    Else
                    '        '                        att = "SEE ATTACHED"
                    '        '                    End If

                    '        '                    row = row.Replace("custom_choices_blood_chemistry_result_4", att)

                    '        '                End If
                    '        '            End If

                    '        '            ' PAPSMEAR RESULT
                    '        '            If dItem2.id = "custom_choices_papsmear_result_5" Then
                    '        '                'row = row.Replace("custom_choices_papsmear_result_5", dItem2.answer)
                    '        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '        '                    row = row.Replace("custom_choices_papsmear_result_5", dItem2.answer)

                    '        '                    'report viewer
                    '        '                    details.PapsmearResult = dItem2.answer
                    '        '                Else
                    '        '                    Dim att As String = "SEE ATTACHED"
                    '        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '        '                    If attList.Count > 0 Then
                    '        '                        att = ""
                    '        '                        For Each attItem In attList
                    '        '                            If attItem.testName = "Pap Smear/Conventional PS" Then
                    '        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '        '                                    For Each urls As String In attItem.attachmentURLs
                    '        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '        '                                        'report viewer
                    '        '                                        'details.PapsmearResult = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                    '        '                                        details.PapsmearResult += " SEE ATTACHED: " & urls & ", "
                    '        '                                    Next
                    '        '                                End If

                    '        '                            End If
                    '        '                        Next
                    '        '                    Else
                    '        '                        att = "SEE ATTACHED"
                    '        '                    End If

                    '        '                    row = row.Replace("custom_choices_papsmear_result_5", att)
                    '        '                End If
                    '        '            End If

                    '        '            ' ECG RESULT
                    '        '            If dItem2.id = "custom_choices_ecg_result_6" Then
                    '        '                'row = row.Replace("custom_choices_ecg_result_6", dItem2.answer)
                    '        '                If dItem2.answer <> "SEE ATTACHED" Then
                    '        '                    row = row.Replace("custom_choices_ecg_result_6", dItem2.answer)

                    '        '                    'report viewer
                    '        '                    details.ECGResult = dItem2.answer
                    '        '                Else
                    '        '                    Dim att As String = "SEE ATTACHED"
                    '        '                    Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                    '        '                    Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                    '        '                    If attList.Count > 0 Then
                    '        '                        att = ""
                    '        '                        For Each attItem In attList
                    '        '                            If attItem.testName = "12L ECG" Or attItem.testName = "ELECTROCARDIOGRAM (12L ECG)" Then
                    '        '                                If HasProperty(attItem, "attachmentURLs") Then
                    '        '                                    For Each urls As String In attItem.attachmentURLs
                    '        '                                        att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                    '        '                                        'report viewer
                    '        '                                        'details.PapsmearResult = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                    '        '                                        details.ECGResult += " SEE ATTACHED: " & urls & ", "
                    '        '                                    Next
                    '        '                                End If

                    '        '                            End If
                    '        '                        Next
                    '        '                    Else
                    '        '                        att = "SEE ATTACHED"
                    '        '                    End If

                    '        '                    row = row.Replace("custom_choices_ecg_result_6", att)
                    '        '                End If
                    '        '            End If

                    '        '            ' OTHER ANCILLARY RESULTS
                    '        '            If dItem2.id = "custom_text_other_ancillary_results_1" Then
                    '        '                row = row.Replace("custom_text_other_ancillary_results_1", dItem2.answer)

                    '        '                'report viewer
                    '        '                details.AncillaryResult = dItem2.answer
                    '        '            End If

                    '        '            ' Impression
                    '        '            If dItem2.id = "patient_impression_0" Then
                    '        '                row = row.Replace("patient_impression_0", dItem2.answer)

                    '        '                'report viewer
                    '        '                If dItem2.answer IsNot Nothing Then
                    '        '                    details.Impression = dItem2.answer.Replace("<div>", ", ").Replace("</div>", "").Replace("<br>", "")
                    '        '                End If
                    '        '            End If

                    '        '            ' RECOMMENDATION
                    '        '            If dItem2.id = "custom_text_recommendation_2" Then
                    '        '                row = row.Replace("custom_text_recommendation_2", dItem2.answer)

                    '        '                'report viewer
                    '        '                If dItem2.answer IsNot Nothing Then
                    '        '                    details.Recommendation = dItem2.answer.Replace("<div>", ", ").Replace("</div>", "").Replace("<br>", "")
                    '        '                End If
                    '        '            End If

                    '        '            ' Clinic Name
                    '        '            If dItem2.id = "clinic_name_0" Then
                    '        '                row = row.Replace("clinic_name_0", dItem2.answer)

                    '        '                'report viewer
                    '        '                details.ClinicName = dItem2.answer
                    '        '            End If
                    '        '        Next
                    '        '    End If

                    '        '    'safe
                    '        '    row = row.Replace("custom_text_other_labs_result_0", "")
                    '        '    row = row.Replace("custom_text_other_ancillary_results_1", "")
                    '        '    row = row.Replace("custom_text_recommendation_2", "")

                    '        '    sb.Append(row)

                    '        '    'report viewer
                    '        '    lDetails.Add(details)

                    '        'End If

                    '        iCount += 1
                    '    Next

                    'End If

                    If iCount = 0 Then
                        sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
                    End If

                Else
                    sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
                End If

            Else
                sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
            End If
        Else
            sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
        End If

        sb.Append("</table></div>")

        sRet = sb.ToString

        '=========
        ' Report Viewer
        '=========
        Dim sRptType = ""
        Select Case reportType
            Case "APESUM"
                sRptType = "ANNUAL PHYSICAL EXAM"
            Case "PESUM"
                sRptType = "PRE-EMPLOYMENT"
            Case "ECUSUM"
                sRptType = "EXECUTIVE CHECK-UP"
        End Select

        Dim header As New List(Of Microsoft.Reporting.WebForms.ReportParameter)
        Dim accName As New Microsoft.Reporting.WebForms.ReportParameter("CompanyName", companyName)
        Dim dateRange As New Microsoft.Reporting.WebForms.ReportParameter("DateRange", dtFrom & " - " & dtTo)
        Dim totalData As New Microsoft.Reporting.WebForms.ReportParameter("TotalData", "")
        Dim resultType As New Microsoft.Reporting.WebForms.ReportParameter("ResultType", sRptType)
        header.Add(accName)
        header.Add(dateRange)
        header.Add(totalData)
        header.Add(resultType)

        reportViewer.LocalReport.DataSources.Clear()
        reportViewer.LocalReport.DisplayName = "Medicard Clinic Results"
        reportViewer.LocalReport.ReportPath = rptPath
        reportViewer.LocalReport.SetParameters(header)
        reportViewer.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("MyCureClinicResults", lDetails))
        reportViewer.LocalReport.Refresh()
        '============================

        Return sRet
    End Function

    Private Shared Function GetReportFormat2_v3(ByVal accountCode As String, ByVal reportType As String,
                                             Optional ByVal memberCode As String = Nothing,
                                             Optional ByVal dtFrom As DateTime = Nothing,
                                             Optional ByVal dtTo As DateTime = Nothing,
                                             Optional ByRef reportViewer As Microsoft.Reporting.WebForms.ReportViewer = Nothing,
                                             Optional ByVal rptPath As String = Nothing,
                                             Optional ByVal companyName As String = Nothing) As String

        Dim sRet As String = Nothing

        Dim sb As New StringBuilder
        sb.Append("<div style='width: 100%; position: relative; background-color: #f9f9f9; overflow-x: scroll; max-height: 250px'><table id='tblUtilization'>")
        sb.Append("<tr><th style='min-width: 120px;'>Date Created</th><th style='min-width: 120px;'>Date of Exam</th><th style='min-width: 120px;'>Member Code</th><th style='min-width: 120px;'>Patient Name</th><th style='min-width: 120px;'>Patient Age</th><th style='min-width: 120px;'>Patient Sex</th><th style='min-width: 120px;'>Vitals - Pulse Rate</th><th style='min-width: 200px;'>Vitals - Respiration Rate</th><th style='min-width: 200px;'>Vitals - Blood Pressure</th><th style='min-width: 150px;'>Vitals - Weight (kg)</th><th style='min-width: 150px;'>Vitals - Height (cm)</th><th style='min-width: 120px;'>Vitals - BMI</th><th style='min-width: 200px;'>CHEST XRAY RESULT</th><th style='min-width: 120px;'>CBC RESULT</th><th style='min-width: 200px;'>URINALYSIS RESULT</th><th style='min-width: 200px;'>FECALYSIS RESULT</th><th style='min-width: 200px;'>OTHER LABS RESULT</th><th style='min-width: 200px;'>BLOOD CHEMISTRY RESULT</th><th style='min-width: 200px;'>PAPSMEAR RESULT</th><th style='min-width: 200px;'>ECG RESULT</th><th style='min-width: 200px;'>OTHER ANCILLARY RESULTS</th><th style='min-width: 200px;'>Impression</th><th style='min-width: 200px;'>RECOMMENDATION</th><th style='min-width: 200px;'>Clinic Name</th></tr>")

        Dim res As Mailhelper.MyCurePMESummaryResultModel = Mailhelper.MyCureAPI.Get_PME_Summary(accountCode, reportType, dtFrom, dtTo)

        Dim patientId As String = ""
        If memberCode <> "" Then
            patientId = Mailhelper.MyCureAPI.GetMyCurePatientId(memberCode, reportType.Replace("SUM", ""))
        End If

        Dim lDetails As New List(Of Mailhelper.MyCureReportParameter)

        If Not res Is Nothing Then
            If res.total <> "0" Then
                Dim dItem As Mailhelper.MyCurePMESummaryData1ResultModel

                If res.data.Count > 0 Then
                    Dim iCount As Long = 0
                    'sb.Append(res.data.Count.ToString())

                    For Each dItem In res.data.OrderBy(Function(c) UnixToDateTime(c.createdAt))
                        'sb.Append(dItem.id)

                        If memberCode <> "" Then
                            If dItem.patient <> patientId Then
                                'Next/Skip
                                Continue For
                            End If
                        End If

                        Dim details As New Mailhelper.MyCureReportParameter()
                        Dim res2 As Mailhelper.MyCurePMEResultModel = Mailhelper.MyCureAPI.Get_PME(dItem.id)
                        If res2 IsNot Nothing Then

                            Dim row As String = "<tr><td>date_created</td><td>date_exam</td><td>mem_code</td><td>patient_name</td><td>patient_age</td><td>patient_sex</td><td>vital_pulse_rate_0</td><td>vital_resp_rate_0</td><td>vital_blood_pressure_0</td><td>vital_weight_0</td><td>vital_height_0</td><td>vital_bmi_0</td><td>custom_choices_chest_xray_result_0</td><td>custom_choices_cbc_result_1</td><td>custom_choices_urinalysis_result_2</td><td>custom_choices_fecalysis_result_3</td><td>custom_text_other_labs_result_0</td><td>custom_choices_blood_chemistry_result_4</td><td>custom_choices_papsmear_result_5</td><td>custom_choices_ecg_result_6</td><td>custom_text_other_ancillary_results_1</td><td>patient_impression_0</td><td>custom_text_recommendation_2</td><td>clinic_name_0</td></tr>"

                            '=========================
                            'Personal Details
                            '=========================
                            If res2.populated IsNot Nothing Then
                                If res2.populated.patient IsNot Nothing Then
                                    Dim memCode As String = ""
                                    Dim cardInfo As Mailhelper.MyCurePMEPatientDetailsPatientCardResultModel
                                    If res2.populated.patient.insuranceCards IsNot Nothing Then
                                        If res2.populated.patient.insuranceCards.Count > 0 Then
                                            For Each cardInfo In res2.populated.patient.insuranceCards
                                                If cardInfo IsNot Nothing Then
                                                    If cardInfo.status = "active" Then
                                                        memCode = cardInfo.number
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If

                                    If reportType <> "PESUM" Then
                                        ' Only Principal will display
                                        If IsActivePrincipal(memCode, accountCode) = False Then
                                            'Next/Skip
                                            Continue For
                                        End If
                                    End If

                                    row = row.Replace("mem_code", memCode)
                                    row = row.Replace("patient_name", res2.populated.patient.name.firstName & " " & res2.populated.patient.name.middleName & " " & res2.populated.patient.name.lastName)
                                    row = row.Replace("patient_age", GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))) 'GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString))
                                    row = row.Replace("patient_sex", res2.populated.patient.sex)

                                    'Date Created
                                    row = row.Replace("date_created", UnixToDateTime(dItem.createdAt).ToString("MMMM dd, yyyy hh:mm tt"))

                                    'report viewer
                                    details.DateCreated = UnixToDateTime(dItem.createdAt).ToString("MMMM dd, yyyy hh:mm tt")
                                    details.MemberCode = memCode
                                    details.PatientName = res2.populated.patient.name.firstName & " " & res2.populated.patient.name.middleName & " " & res2.populated.patient.name.lastName
                                    details.PatientAge = GetCurrentAge(UnixToDateTime(res2.populated.patient.dateOfBirth.ToString).ToString("MM/dd/yyyy"))
                                    details.PatientSex = res2.populated.patient.sex
                                End If
                            End If
                            '=========================

                            If res2.values IsNot Nothing Then

                                Dim dList As List(Of Mailhelper.MyCureAPEData1ValuesResultModel) = res2.values
                                Dim dItem2 As Mailhelper.MyCureAPEData1ValuesResultModel
                                For Each dItem2 In dList
                                    'replace
                                    'row = row.Replace(dItem2.id, dItem2.answer)

                                    ' Date of exam
                                    If dItem2.id = "patient_encounter_created_at_0" Or dItem2.id = "patient_encounter_created_at_1" _
                                    Or dItem2.id = "today_0" Or dItem2.id = "today_1" Then
                                        row = row.Replace("date_exam", dItem2.answer)

                                        'report viewer
                                        If dItem2.answer IsNot Nothing Then
                                            details.DateOfExam = dItem2.answer.Replace("&nbsp;", "")
                                        End If
                                    End If

                                    ' Member Code
                                    ' Patient Name
                                    ' Patient Age
                                    ' Patient Sex

                                    ' Vitals - Pulse Rate
                                    If dItem2.id = "vital_pulse_rate_0" Then
                                        row = row.Replace("vital_pulse_rate_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsPR = dItem2.answer
                                    End If

                                    ' Vitals - Respiration Rate
                                    If dItem2.id = "vital_resp_rate_0" Then
                                        row = row.Replace("vital_resp_rate_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsRR = dItem2.answer
                                    End If

                                    ' Vitals - Blood Pressure
                                    If dItem2.id = "vital_blood_pressure_0" Then
                                        row = row.Replace("vital_blood_pressure_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsBR = dItem2.answer
                                    End If

                                    ' Vitals - Weight (kg)
                                    If dItem2.id = "vital_weight_0" Then
                                        row = row.Replace("vital_weight_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsWeight = dItem2.answer
                                    End If

                                    ' Vitals - Height (cm)
                                    If dItem2.id = "vital_height_0" Then
                                        row = row.Replace("vital_height_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsHeight = dItem2.answer
                                    End If

                                    ' Vitals - BMI
                                    If dItem2.id = "vital_bmi_0" Then
                                        row = row.Replace("vital_bmi_0", dItem2.answer)

                                        'report viewer
                                        details.VitalsBMI = dItem2.answer
                                    End If

                                    ' CHEST XRAY RESULT
                                    If dItem2.id = "custom_choices_chest_xray_result_0" Then
                                        'row = row.Replace("custom_choices_chest_xray_result_0", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_chest_xray_result_0", dItem2.answer)

                                            'report viewer
                                            details.XrayResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "CXR PA " Or attItem.testName = "CXR PA" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                details.XrayResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_chest_xray_result_0", att)

                                        End If
                                    End If

                                    ' CBC RESULT
                                    If dItem2.id = "custom_choices_cbc_result_1" Then
                                        'row = row.Replace("custom_choices_cbc_result_1", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_cbc_result_1", dItem2.answer)

                                            'report viewer
                                            details.CBCResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "Complete Blood Count/CBC" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                details.CBCResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_cbc_result_1", att)

                                        End If
                                    End If

                                    ' URINALYSIS RESULT
                                    If dItem2.id = "custom_choices_urinalysis_result_2" Then
                                        'row = row.Replace("custom_choices_urinalysis_result_2", dItem2.answer)

                                        ''report viewer
                                        'details.UrinalysisResult = dItem2.answer

                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_urinalysis_result_2", dItem2.answer)

                                            'report viewer
                                            details.UrinalysisResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "Urinalysis" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                details.UrinalysisResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_urinalysis_result_2", att)

                                        End If
                                    End If

                                    ' FECALYSIS RESULT
                                    If dItem2.id = "custom_choices_fecalysis_result_3" Then
                                        'row = row.Replace("custom_choices_fecalysis_result_3", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_fecalysis_result_3", dItem2.answer)

                                            'report viewer
                                            details.FecalysisResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "Fecalysis/Stool Exam" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                details.FecalysisResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_fecalysis_result_3", att)

                                        End If
                                    End If

                                    ' OTHER LABS RESULT
                                    If dItem2.id = "custom_text_other_labs_result_0" Then
                                        row = row.Replace("custom_text_other_labs_result_0", dItem2.answer)

                                        'report viewer
                                        details.OtherLabResult = dItem2.answer
                                    End If

                                    ' BLOOD CHEMISTRY RESULT
                                    If dItem2.id = "custom_choices_blood_chemistry_result_4" Then
                                        'row = row.Replace("custom_choices_blood_chemistry_result_4", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_blood_chemistry_result_4", dItem2.answer)

                                            'report viewer
                                            details.BloodChem = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "10 Blood Chemistry/10BC" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                'details.BloodChem = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                                                                details.BloodChem += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_blood_chemistry_result_4", att)

                                        End If
                                    End If

                                    ' PAPSMEAR RESULT
                                    If dItem2.id = "custom_choices_papsmear_result_5" Then
                                        'row = row.Replace("custom_choices_papsmear_result_5", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_papsmear_result_5", dItem2.answer)

                                            'report viewer
                                            details.PapsmearResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "Pap Smear/Conventional PS" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                'details.PapsmearResult = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                                                                details.PapsmearResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_papsmear_result_5", att)
                                        End If
                                    End If

                                    ' ECG RESULT
                                    If dItem2.id = "custom_choices_ecg_result_6" Then
                                        'row = row.Replace("custom_choices_ecg_result_6", dItem2.answer)
                                        If dItem2.answer <> "SEE ATTACHED" Then
                                            row = row.Replace("custom_choices_ecg_result_6", dItem2.answer)

                                            'report viewer
                                            details.ECGResult = dItem2.answer
                                        Else
                                            Dim att As String = "SEE ATTACHED"
                                            Dim attList As List(Of Mailhelper.MyCurePMEAttachmentResultModel) = res2.populated.attachments
                                            Dim attItem As Mailhelper.MyCurePMEAttachmentResultModel
                                            If attList.Count > 0 Then
                                                att = ""
                                                For Each attItem In attList
                                                    If attItem.testName = "12L ECG" Or attItem.testName = "ELECTROCARDIOGRAM (12L ECG)" Then
                                                        If HasProperty(attItem, "attachmentURLs") Then
                                                            For Each urls As String In attItem.attachmentURLs
                                                                att += "<a href='" & urls & "' target='_blank'>SEE ATTACHED</a><br/>"

                                                                'report viewer
                                                                'details.PapsmearResult = "=HYPERLINK(""" & urls & """, ""SEE ATTACHED"")"
                                                                details.ECGResult += " SEE ATTACHED: " & urls & ", "
                                                            Next
                                                        End If

                                                    End If
                                                Next
                                            Else
                                                att = "SEE ATTACHED"
                                            End If

                                            row = row.Replace("custom_choices_ecg_result_6", att)
                                        End If
                                    End If

                                    ' OTHER ANCILLARY RESULTS
                                    If dItem2.id = "custom_text_other_ancillary_results_1" Then
                                        row = row.Replace("custom_text_other_ancillary_results_1", dItem2.answer)

                                        'report viewer
                                        details.AncillaryResult = dItem2.answer
                                    End If

                                    ' Impression
                                    If dItem2.id = "patient_impression_0" Then
                                        row = row.Replace("patient_impression_0", dItem2.answer)

                                        'report viewer
                                        If dItem2.answer IsNot Nothing Then
                                            details.Impression = dItem2.answer.Replace("<div>", ", ").Replace("</div>", "").Replace("<br>", "")
                                        End If
                                    End If

                                    ' RECOMMENDATION
                                    If dItem2.id = "custom_text_recommendation_2" Then
                                        row = row.Replace("custom_text_recommendation_2", dItem2.answer)

                                        'report viewer
                                        If dItem2.answer IsNot Nothing Then
                                            details.Recommendation = dItem2.answer.Replace("<div>", ", ").Replace("</div>", "").Replace("<br>", "")
                                        End If
                                    End If

                                    ' Clinic Name
                                    If dItem2.id = "clinic_name_0" Then
                                        row = row.Replace("clinic_name_0", dItem2.answer)

                                        'report viewer
                                        details.ClinicName = dItem2.answer
                                    End If
                                Next
                            End If

                            'safe
                            row = row.Replace("custom_text_other_labs_result_0", "")
                            row = row.Replace("custom_text_other_ancillary_results_1", "")
                            row = row.Replace("custom_text_recommendation_2", "")

                            sb.Append(row)

                            'report viewer
                            lDetails.Add(details)

                        End If

                        iCount += 1

                    Next

                    If iCount = 0 Then
                        sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
                    End If

                Else
                    sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
                End If

            Else
                sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
            End If
        Else
            sb.Append("<tr><td>No data to display</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
        End If

        sb.Append("</table></div>")

        sRet = sb.ToString

        '=========
        ' Report Viewer
        '=========
        Dim sRptType = ""
        Select Case reportType
            Case "APESUM"
                sRptType = "ANNUAL PHYSICAL EXAM"
            Case "PESUM"
                sRptType = "PRE-EMPLOYMENT"
            Case "ECUSUM"
                sRptType = "EXECUTIVE CHECK-UP"
        End Select

        Dim header As New List(Of Microsoft.Reporting.WebForms.ReportParameter)
        Dim accName As New Microsoft.Reporting.WebForms.ReportParameter("CompanyName", companyName)
        Dim dateRange As New Microsoft.Reporting.WebForms.ReportParameter("DateRange", dtFrom & " - " & dtTo)
        Dim totalData As New Microsoft.Reporting.WebForms.ReportParameter("TotalData", "")
        Dim resultType As New Microsoft.Reporting.WebForms.ReportParameter("ResultType", sRptType)
        header.Add(accName)
        header.Add(dateRange)
        header.Add(totalData)
        header.Add(resultType)

        reportViewer.LocalReport.DataSources.Clear()
        reportViewer.LocalReport.DisplayName = "Medicard Clinic Results"
        reportViewer.LocalReport.ReportPath = rptPath
        reportViewer.LocalReport.SetParameters(header)
        reportViewer.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("MyCureClinicResults", lDetails))
        reportViewer.LocalReport.Refresh()
        '============================

        Return sRet
    End Function


    Private Sub InitiateReportViewer()

        Dim lDetails As New List(Of Mailhelper.MyCureReportParameter)

        ClinicResultsViewer.LocalReport.DataSources.Clear()
        ClinicResultsViewer.LocalReport.DisplayName = "Medicard Clinic Results"
        ClinicResultsViewer.LocalReport.ReportPath = Server.MapPath("~/AccountManager/ClinicResults.rdlc")
        ClinicResultsViewer.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("MyCureClinicResults", lDetails))
        ClinicResultsViewer.LocalReport.Refresh()

    End Sub

    Public Shared Function UnixToDateTime(ByVal strUnixTime As String) As DateTime

        Dim nTimestamp As Double = strUnixTime
        Dim nDateTime As System.DateTime = New System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
        nDateTime = nDateTime.AddMilliseconds(nTimestamp).ToLocalTime()

        Return nDateTime

    End Function

    Public Shared Function GetCurrentAge(ByVal dob As Date) As Integer
        Dim age As Integer
        age = Today.Year - dob.Year
        If (dob > Today.AddYears(-age)) Then age -= 1
        Return age
    End Function

    Public Shared Function IsActivePrincipal(ByVal memberCode As String, ByVal accountCode As String) As Boolean
        Dim bRet As Boolean = False
        Dim emed As New AccountInformationBLL(accountCode, AccountInformationProperties.AccountType.eCorporate)

        Dim obj As Object = (From x In emed.ActiveMembersPrincipal
                             Where x.PRIN_CODE = memberCode).FirstOrDefault()

        If obj IsNot Nothing Then
            bRet = True
        End If

        Return bRet
    End Function

    Public Shared Function HasProperty(ByVal obj As Object, ByVal propertyName As String) As Boolean
        If obj.GetType.GetProperty(propertyName) IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

End Class