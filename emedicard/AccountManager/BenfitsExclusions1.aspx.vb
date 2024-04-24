Imports Telerik.Web.UI
Imports emedicardBLL
Imports EncryptDecrypt
Public Class BenfitsExclusions1
    Inherits System.Web.UI.Page
    Dim eAcctBLL As New eAccountBLL
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Public sPackageCode As String
    Public dtHCSPrn As New DataTable
    Public dtHCSDep As New DataTable
    Public dtHCSExt As New DataTable
    Public dtPHCSPrn As New DataTable
    Public dtPHCSDep As New DataTable
    Public dtPHCSExt As New DataTable
    Public dtDCSP As New DataTable
    Public dtDCSQ As New DataTable
    Public dtDCSE As New DataTable
    Public dtEXC As New DataTable
    Public dtPECOthersP As New DataTable
    Public dtPECOthersD As New DataTable
    Public dtPECOthersE As New DataTable
    Public dtMATOthers As New DataTable
    Public dtEXECothers As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetPackageCode()

            ShowPageView(Request.QueryString("tab"))
            If sPackageCode <> "PKG0000005" Then
                Select Case Request.QueryString("tab")
                    Case 0
                        LoadStandard("HCS")
                    Case 1
                        LoadStandard("PHCS")
                    Case 2
                        LoadStandard("ECS")
                    Case 3
                        LoadStandard("MFA")
                    Case 4
                        LoadStandard("DCS")
                    Case 5
                        '' LoadStandard("ECS")
                        '' ShowPageView(6)
                    Case 10
                        LoadStandard("DRD")
                    Case 11
                        LoadStandard("ME")
                End Select


                RadTabStrip1.Tabs(6).Visible = False
                RadMultiPage1.PageViews(7).Visible = False

                RadTabStrip1.Tabs(7).Visible = False
                RadMultiPage1.PageViews(8).Visible = False

                RadTabStrip1.Tabs(8).Visible = False
                RadMultiPage1.PageViews(9).Visible = False

                RadTabStrip1.Tabs(9).Visible = False
                RadMultiPage1.PageViews(5).Visible = False

            Else
                Load_HCS()
                Select Case Request.QueryString("tab")
                    Case 0
                        Load_HCS()
                    Case 1
                        Load_PreventiveHCS()
                    Case 2
                        Load_ECS("PRN")
                        Load_ECS("DEP")
                        Load_ECS("EXT")

                    Case 3
                        Load_MFA("PRN")
                        Load_MFA("DEP")
                        Load_MFA("EXT")
                    Case 4
                        Load_DCS()
                    Case 5
                        Load_Exclusion()
                    Case 6
                        Load_PEC("PRN")
                        Load_PEC("DEP")
                        Load_PEC("EXT")
                    Case 7
                        Load_POS()
                    Case 8
                        Load_MAT()
                    Case 9
                        Executive_Checkup()
                End Select

                RadTabStrip1.Tabs(10).Visible = False
                RadMultiPage1.PageViews(10).Visible = False

                RadTabStrip1.Tabs(10).Visible = False
                RadMultiPage1.PageViews(10).Visible = False

            End If


        End If
    End Sub

    Private Sub ShowPageView(index As Integer)
        RadMultiPage1.PageViews(0).Visible = False
        RadMultiPage1.PageViews(1).Visible = False
        RadMultiPage1.PageViews(2).Visible = False
        RadMultiPage1.PageViews(3).Visible = False
        RadMultiPage1.PageViews(4).Visible = False
        RadMultiPage1.PageViews(5).Visible = False
        RadMultiPage1.PageViews(6).Visible = False
        RadMultiPage1.PageViews(7).Visible = False
        RadMultiPage1.PageViews(8).Visible = False
        RadMultiPage1.PageViews(9).Visible = False
        RadMultiPage1.PageViews(10).Visible = False
        '' RadMultiPage1.PageViews(12).Visible = False

        RadMultiPage1.PageViews(index).Visible = True
        RadMultiPage1.SelectedIndex = index
    End Sub

    Private Sub GetPackageCode()

        eAcctBLL.AccountCode = Request.QueryString("a")
        eAcctBLL.GetMotherCode()
        sPackageCode = eAcctBLL.GetPackageCode
    End Sub

    Private Sub Load_HCS()


        With eAcctBLL

            .Member_Type = "PRN"
            dtHCSPrn = .GetHCS
            Load_HCS_PRN(dtHCSPrn, lblLapSurgP, lblChemoP, lblMRIP, lblHumBloodP, lblComplexDiagP, lblWorkIllP, _
                         lblCongIllP, lblOpenSurgP, lblCataractP, lblPTP, lblAntiRabP, lblLTGlaucuomaP)

            dtgHCSOtherPRN.DataSource = .GetOhtersHCS
            dtgHCSOtherPRN.DataBind()

            dtgPHCSOtherPRN.DataSource = .GetOhtersPHCS
            dtgPHCSOtherPRN.DataBind()

            dtgECSOtherPRN.DataSource = .GetOhtersECS
            dtgECSOtherPRN.DataBind()

            dtgMFAOtherPRN.DataSource = .GetOhtersMFA
            dtgMFAOtherPRN.DataBind()

            dtgDCSOtherPRN.DataSource = .GetOhtersDCS
            dtgDCSOtherPRN.DataBind()

            .Member_Type = "DEP"
            dtHCSDep = .GetHCS
            Load_HCS_PRN(dtHCSDep, lblLapSurgQ, lblChemoQ, lblMRIQ, lblHumBloodQ, lblComplexDiagQ, lblWorkIllQ, _
                         lblCongIllQ, lblOpenSurgQ, lblCataractQ, lblPTQ, lblAntiRabQ, lblLTGlaucuomaQ)

            dtgHCSOtherDEP.DataSource = .GetOhtersHCS
            dtgHCSOtherDEP.DataBind()

            dtgPHCSOtherDEP.DataSource = .GetOhtersPHCS
            dtgPHCSOtherDEP.DataBind()

            dtgECSOtherDEP.DataSource = .GetOhtersECS
            dtgECSOtherDEP.DataBind()

            dtgMFAOtherDEP.DataSource = .GetOhtersMFA
            dtgMFAOtherDEP.DataBind()

            dtgDCSOtherDEP.DataSource = .GetOhtersDCS
            dtgDCSOtherDEP.DataBind()

            .Member_Type = "EXT"
            dtHCSExt = .GetHCS
            Load_HCS_PRN(dtHCSExt, lblLapSurg, lblChemo, lblMRI, lblHumBlood, lblComplexDiag, lblWorkIll, _
                         lblCongIll, lblOpenSurg, lblCataract, lblPT, lblAntiRab, lblLTGlaucuoma)

            dtgHCSOtherEXT.DataSource = .GetOhtersHCS
            dtgHCSOtherEXT.DataBind()

            dtgPHCSOtherEXT.DataSource = .GetOhtersPHCS
            dtgPHCSOtherEXT.DataBind()

            dtgECSOtherEXT.DataSource = .GetOhtersECS
            dtgECSOtherEXT.DataBind()

            dtgMFAOtherEXT.DataSource = .GetOhtersMFA
            dtgMFAOtherEXT.DataBind()

            dtgDCSOtherEXT.DataSource = .GetOhtersDCS
            dtgDCSOtherEXT.DataBind()

        End With
    End Sub

    Public Sub Load_HCS_PRN(ByVal dt As DataTable, ByRef lLSL As Label, ByRef lChemo As Label, ByRef lCtScan As Label, _
                            ByRef lhumBld As Label, ByRef lCompDiag As Label, ByRef lWorkIll As Label, ByRef lCongIll As Label, _
                            ByRef lHearthSurg As Label, ByRef lCararact As Label, ByRef lTherapy As Label, ByRef lRabies As Label, _
                            ByRef lGlaucomo As Label)

        Dim sLSL As String
        Dim sChemoRad As String = String.Empty
        Dim sCtScan As String = String.Empty
        Dim sHumBld As String = String.Empty
        Dim sCompDiag As String = String.Empty
        Dim sWorkIll As String = String.Empty
        Dim sOrgTrans As String = String.Empty
        Dim sCongiIll As String = String.Empty
        Dim sHearthSurg As String = String.Empty
        Dim sCataract As String = String.Empty
        Dim sSpeech As String = String.Empty
        Dim sAntiRabies As String = String.Empty
        Dim sGlaucoma As String = String.Empty

        If Not IsDBNull(dt(0)("HCS_L_SURGERY_LITHO")) Then
            If Trim(dt(0)("HCS_L_SURGERY_LITHO")) <> "" Then
                sLSL = dt(0)("HCS_L_SURGERY_LITHO")
            Else
                sLSL = "STANDARD"
            End If
        Else
            sLSL = "STANDARD"
        End If
        lLSL.Text = sLSL

        If Not IsDBNull(dt(0)("HCS_CHEMO_RAD_DIA")) Then
            If Trim(dt(0)("HCS_CHEMO_RAD_DIA")) <> "" Then
                sChemoRad = dt(0)("HCS_CHEMO_RAD_DIA") & " session w/in DDL"
            Else
                sChemoRad = "STANDARD"
            End If
        Else
            sChemoRad = "STANDARD"
        End If
        lChemo.Text = sChemoRad

        If Not IsDBNull(dt(0)("HCS_CTSCAN_MRI_UTS")) Then
            If Trim(dt(0)("HCS_CTSCAN_MRI_UTS")) <> "" Then
                sCtScan = dt(0)("HCS_CTSCAN_MRI_UTS")
            Else
                sCtScan = "STANDARD"
            End If
        Else
            sCtScan = "STANDARD"
        End If
        lCtScan.Text = sCtScan

        If Not IsDBNull(dt(0)("HCS_HUMAN_BLD_PROD")) Then
            If Trim(dt(0)("HCS_HUMAN_BLD_PROD")) <> "" Then
                sHumBld = dt(0)("HCS_HUMAN_BLD_PROD")
            Else
                sHumBld = "NONE"
            End If
        Else
            sHumBld = "NONE"
        End If
        lhumBld.Text = sHumBld

        If Not IsDBNull(dt(0)("HCS_COMPLEX_DIAG")) Then
            If Trim(dt(0)("HCS_COMPLEX_DIAG")) <> "" Then
                sCompDiag = dt(0)("HCS_COMPLEX_DIAG")
            Else
                sCompDiag = "STANDARD"
            End If
        Else
            sCompDiag = "STANDARD"
        End If
        lCompDiag.Text = sCompDiag

        If Not IsDBNull(dt(0)("HCS_WRK_REL_ILL_ACCIDENT")) Then
            If Trim(dt(0)("HCS_WRK_REL_ILL_ACCIDENT")) <> "" Then
                sWorkIll = dt(0)("HCS_WRK_REL_ILL_ACCIDENT")
            Else
                sWorkIll = "STANDARD"
            End If
        Else
            sWorkIll = "STANDARD"
        End If
        lWorkIll.Text = sWorkIll


        If Not IsDBNull(dt(0)("HCS_ORGAN_TRANS")) Then
            If Trim(dt(0)("HCS_ORGAN_TRANS")) <> "" Then
                sOrgTrans = Trim(dt(0)("HCS_ORGAN_TRANS"))
            Else
                sOrgTrans = "NONE"
            End If
        Else
            sOrgTrans = "NONE"
        End If
        lblOrgTrans.Text = sOrgTrans


        If Not IsDBNull(dt(0)("HCS_CONGENI_ILL")) Then
            If Trim(dt(0)("HCS_CONGENI_ILL")) <> "" Then
                sCongiIll = Trim(dt(0)("HCS_CONGENI_ILL"))
            Else
                sCongiIll = "NONE"
            End If
        Else
            sCongiIll = "NONE"
        End If
        lCongIll.Text = sCongiIll

        If Not IsDBNull(dt(0)("HCS_OPEN_HEART_SUR")) Then
            If Trim(dt(0)("HCS_OPEN_HEART_SUR")) <> "" Then
                sHearthSurg = dt(0)("HCS_OPEN_HEART_SUR")
            Else
                sHearthSurg = "NONE"
            End If
        Else
            sHearthSurg = "NONE"
        End If
        lHearthSurg.Text = sHearthSurg

        If Not IsDBNull(dt(0)("HCS_CATARACT_EXT")) Then
            If Trim(dt(0)("HCS_CATARACT_EXT")) <> "" Then
                sCataract = dt(0)("HCS_CATARACT_EXT")
            Else
                sCataract = "STANDARD"
            End If
        Else
            sCataract = "STANDARD"
        End If
        lCararact.Text = sCataract

        If Not IsDBNull(dt(0)("OPCS_PT_SPEECH_SESSION")) Then
            If Trim(dt(0)("OPCS_PT_SPEECH_SESSION")) <> "" Then
                sSpeech = dt(0)("OPCS_PT_SPEECH_SESSION")
            Else
                sSpeech = "STANDARD"
            End If
        Else
            sSpeech = "STANDARD"
        End If
        lTherapy.Text = sSpeech

        If Not IsDBNull(dt(0)("OPCS_FDOSE_ANTI_RABIES_TETANUS")) Then
            If Trim(dt(0)("OPCS_FDOSE_ANTI_RABIES_TETANUS")) <> "" Then
                sAntiRabies = dt(0)("OPCS_FDOSE_ANTI_RABIES_TETANUS")
            Else
                sAntiRabies = "NONE"
            End If
        Else
            sAntiRabies = "NONE"
        End If
        lRabies.Text = sAntiRabies

        If Not IsDBNull(dt(0)("OPCS_LASER_TREAT_GLAUCOMA")) Then
            If Trim(dt(0)("OPCS_LASER_TREAT_GLAUCOMA")) <> "" Then
                sGlaucoma = dt(0)("OPCS_LASER_TREAT_GLAUCOMA")
            Else
                sGlaucoma = "STANDARD"
            End If
        Else
            sGlaucoma = "STANDARD"
        End If
        lGlaucomo.Text = sGlaucoma

    End Sub

    Private Sub Load_PreventiveHCS()
        With eAcctBLL

            .Member_Type = "PRN"
            dtPHCSPrn = .GetPreventiveHCS

            Load_PreventiveHCS(dtPHCSPrn, imgMedHOP, imgCompPremP, imgNearHospP, lblECGP, lblPapsmearP, imgEyeRefP)

            .Member_Type = "DEP"
            dtPHCSDep = .GetPreventiveHCS

            Load_PreventiveHCS(dtPHCSDep, imgMedHOQ, imgCompPremQ, imgNearHospQ, lblECGQ, lblPapsmearQ, imgEyeRefQ)

            .Member_Type = "EXT"
            dtPHCSExt = .GetPreventiveHCS

            Load_PreventiveHCS(dtPHCSExt, imgMedHOE, imgCompPremE, imgNearHospE, lblECGE, lblPapsmearE, imgEyeRefE)
        End With
    End Sub
    Private Sub Load_PreventiveHCS(ByVal dt As DataTable, ByRef imgHO As Image, ByVal imgCoPrem As Image, ByRef imgHospClinic As Image, ByVal lECG As Label, _
                                   ByRef lpap As Label, ByRef imgEye As Image)

        Dim sECGFor As String = String.Empty
        Dim sPapsFor As String = String.Empty

        If Not IsDBNull(dt(0)("PHCS_MEDI_HO")) Then
            If dt(0)("PHCS_MEDI_HO") = True Then
                imgHO.ImageUrl = "~/img/tick.ico"
            Else
                imgHO.ImageUrl = "~/img/cross.ico"
            End If
        Else
            imgHO.ImageUrl = "~/img/cross.ico"
        End If

        If Not IsDBNull(dt(0)("PHCS_CO_PREM")) Then
            If dt(0)("PHCS_CO_PREM") = True Then
                imgCoPrem.ImageUrl = "~/img/tick.ico"
            Else
                imgCoPrem.ImageUrl = "~/img/cross.ico"
            End If
        Else
            imgCoPrem.ImageUrl = "~/img/cross.ico"
        End If

        If Not IsDBNull(dt(0)("PHCS_NEAREST_HOSP")) Then
            If dt(0)("PHCS_NEAREST_HOSP") = True Then
                imgHospClinic.ImageUrl = "~/img/tick.ico"
            Else
                imgHospClinic.ImageUrl = "~/img/cross.ico"
            End If
        Else
            imgHospClinic.ImageUrl = "~/img/cross.ico"
        End If


        If Not IsDBNull(dt(0)("PHCS_ECG")) Then
            If Trim(dt(0)("PHCS_ECG")) <> "" Then
                sECGFor = dt(0)("PHCS_ECG") & " years old or If prescribed."
            Else
                sECGFor = "NONE"
            End If
        Else
            sECGFor = "NONE"
        End If
        lECG.Text = sECGFor

        If Not IsDBNull(dt(0)("PHCS_PAP_SMEAR")) Then
            If Trim(dt(0)("PHCS_PAP_SMEAR")) <> "" Then
                sPapsFor = dt(0)("PHCS_PAP_SMEAR") & " years old or If prescribed."
            Else
                sPapsFor = "NONE"
            End If
        Else
            sPapsFor = "NONE"
        End If
        lpap.Text = sPapsFor

        If Not IsDBNull(dt(0)("PHCS_EYE_REFRACT")) Then
            If dt(0)("PHCS_EYE_REFRACT") = True Then
                imgEye.ImageUrl = "~/img/tick.ico"
            Else
                imgEye.ImageUrl = "~/img/cross.ico"
            End If
        Else
            imgEye.ImageUrl = "~/img/cross.ico"
        End If

    End Sub
    Private Sub Load_ECS(ByVal sMemType As String)

        Dim dtECS As New DataTable
        Dim row As Long = 1
        Dim bAllowed As Boolean = False

        Try

            With eAcctBLL

                .Member_Type = sMemType
                dtECS = .GetECS
            End With

            Dim sHTML As String = String.Empty
            Dim sbHTMLBuilder As New StringBuilder
            Dim bNobenefits As Boolean = False

            sbHTMLBuilder.Append("<table class=""ECSdtECSls"" >")

            bNobenefits = dtECS(0)("ECS_NO_BENEFIT")

            sbHTMLBuilder.Append("<tr><td>Leading for confinement only</td><td colspan='2'><img class='icons' src=""")
            If dtECS(0)("ECS_LEAD_TO_CONFINEMENT") Then
                sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
            Else
                sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
            End If
            sbHTMLBuilder.Append("""/></td></tr>")

            sbHTMLBuilder.Append("<tr><td class='col1'></td><td class='thcol2' >Amount in Percent(%)</td><td class='thcol3'>Dreaded Disease Limit</td></tr>")

            sbHTMLBuilder.Append(ShowData("In Non Accredited Hospitals  ", dtECS(0)("ECS_NON_ACCR_HOSP_IN_PERC"), dtECS(0)("ECS_NON_ACCR_HOSP_IN_DDL"), "STANDARD", "STANDARD"))

            sbHTMLBuilder.Append(ShowData("In areas w/o Accredited Hospitals  ", dtECS(0)("ECS_WOUT_ACCR_HOSP_IN_PERC"), dtECS(0)("ECS_WOUT_ACCR_HOSP_IN_DDL"), "STANDARD", "STANDARD"))

            sbHTMLBuilder.Append(ShowData("In Foreign Countries  ", dtECS(0)("ECS_IN_FORIEGN_COUNTRY_IN_PERC"), dtECS(0)("ECS_IN_FORIEGN_COUNTRY_IN_DDL"), "NONE", "NONE"))

            sbHTMLBuilder.Append("</table>")

            Select Case sMemType
                Case "PRN"
                    If bNobenefits = True Then
                        tblContentP.InnerHtml = "<p style='color: Red; font: bold 14px;'>NO BENEFITS</p>"
                    Else
                        tblContentP.InnerHtml = sbHTMLBuilder.ToString & "<br />"
                    End If
                Case "DEP"
                    If bNobenefits = True Then
                        tblContentQ.InnerHtml = "<p style='color: Red; font: bold 14px;'>NO BENEFITS</p>"
                    Else
                        tblContentQ.InnerHtml = sbHTMLBuilder.ToString & "<br />"
                    End If
                Case "EXT"
                    If bNobenefits = True Then
                        tblContentE.InnerHtml = "<p style='color: Red; font: bold 14px;'>NO BENEFITS</p>"
                    Else
                        tblContentE.InnerHtml = sbHTMLBuilder.ToString & "<br />"
                    End If
            End Select

        Catch ex As Exception
            Response.Write(ex.InnerException.ToString)
        Finally

        End Try
    End Sub

    Public Sub Load_MFA(ByVal sMemType As String)

        Dim dtMFA As New DataTable
        Dim row As Long = 1
        Dim bAllowed As Boolean = False

        Try
            With eAcctBLL

                .Member_Type = sMemType
                dtMFA = .GetMFA
            End With

            Dim sHTML As String = String.Empty
            Dim sbHTMLBuilder As New StringBuilder
            Dim bNobenefits As Boolean = False

            sbHTMLBuilder.Append("<table class='MFADtls'>")

            bNobenefits = dtMFA(0)("MFA_NO_BENEFIT")
            sbHTMLBuilder.Append("<tr><td class='col1'></td><td class='thcol2' >Option 1</td><td class='thcol3'>Option 2</td></tr>")
            sbHTMLBuilder.Append("<tr><td class='col1'>Natural Death</td><td class='ecscol2'>")
            sbHTMLBuilder.Append(dtMFA(0)("MFA_NTRL_DEATH_OPT1"))
            sbHTMLBuilder.Append("</td><td class='col3'>")
            sbHTMLBuilder.Append(dtMFA(0)("MFA_NTRL_DEATH_OPT2"))
            sbHTMLBuilder.Append("</td></tr>")

            sbHTMLBuilder.Append("<tr><td class='col1'>Accident Death</td><td class='ecscol2'>")
            sbHTMLBuilder.Append(dtMFA(0)("MFA_ACCI_DEATH_OPT1"))
            sbHTMLBuilder.Append("</td><td class='col3'>")
            sbHTMLBuilder.Append(dtMFA(0)("MFA_ACCI_DEATH_OPT2"))
            sbHTMLBuilder.Append("</td></tr>")

            sbHTMLBuilder.Append("<tr><td class='col1'>Lost of two sights or two limbs</td><td class='ecscol2'>")
            sbHTMLBuilder.Append(dtMFA(0)("MFA_LOSS_SIGHT_LIMBS_OPT1"))
            sbHTMLBuilder.Append("</td><td class='col3'>")
            sbHTMLBuilder.Append(dtMFA(0)("MFA_LOSS_SIGHT_LIMBS_OPT2"))
            sbHTMLBuilder.Append("</td></tr>")

            sbHTMLBuilder.Append("<tr><td class='col1'>Loss of an eye, one hand or one foot</td><td class='ecscol2'>")
            sbHTMLBuilder.Append(dtMFA(0)("MFA_LOSS_EYE_LIMB_OPT1"))
            sbHTMLBuilder.Append("</td><td class='col3'>")
            sbHTMLBuilder.Append(dtMFA(0)("MFA_LOSS_EYE_LIMB_OPT2"))
            sbHTMLBuilder.Append("</td></tr>")


            sbHTMLBuilder.Append("</table>")

            Select Case sMemType
                Case "PRN"
                    If bNobenefits = True Then
                        tblMFAP.InnerHtml = "<p style='color: Red; font: bold 14px;'>NO BENEFITS</p>"
                    Else
                        tblMFAP.InnerHtml = sbHTMLBuilder.ToString & "<br />"
                    End If
                Case "DEP"
                    If bNobenefits = True Then
                        tblMFAQ.InnerHtml = "<p style='color: Red; font: bold 14px;'>NO BENEFITS</p>"
                    Else
                        tblMFAQ.InnerHtml = sbHTMLBuilder.ToString & "<br />"
                    End If
                Case "EXT"
                    If bNobenefits = True Then
                        tblMFAE.InnerHtml = "<p style='color: Red; font: bold 14px;'>NO BENEFITS</p>"
                    Else
                        tblMFAE.InnerHtml = sbHTMLBuilder.ToString & "<br />"
                    End If
            End Select
        Catch ex As Exception
            Response.Write(ex.InnerException.ToString)
        Finally

        End Try
    End Sub

    Private Sub Load_DCS()
        With eAcctBLL

            .Member_Type = "PRN"
            dtDCSP = .GetDCS
            Load_DCS(dtDCSP, lblFillingP, lblProphylaxisP, lblLigtCureP)

            .Member_Type = "DEP"
            dtDCSQ = .GetDCS
            Load_DCS(dtDCSQ, lblFillingQ, lblProphylaxisQ, lblLigtCureQ)

            .Member_Type = "EXT"
            dtDCSE = .GetDCS
            Load_DCS(dtDCSE, lblFillingE, lblProphylaxisE, lblLigtCureE)
        End With
    End Sub

    Private Sub Load_DCS(ByVal dt As DataTable, ByRef lfill As Label, ByRef lprop As Label, ByRef llight As Label)

        If Not IsDate(dt(0)("DCS_PERMA_AMALGAM")) Then
            If Trim(dt(0)("DCS_PERMA_AMALGAM")) <> "" Then
                lfill.Text = dt(0)("DCS_PERMA_AMALGAM")
            Else
                lfill.Text = "NONE"
                lfill.ForeColor = Drawing.Color.Red
            End If
        Else
            lfill.Text = "NONE"
            lfill.ForeColor = Drawing.Color.Red
        End If


        If Not IsDate(dt(0)("DCS_PROP")) Then
            If Trim(dt(0)("DCS_PROP")) <> "" Then
                lprop.Text = dt(0)("DCS_PROP")
            Else
                lprop.Text = "NONE"
                lfill.ForeColor = Drawing.Color.Red
            End If
        Else
            lprop.Text = "NONE"
            lfill.ForeColor = Drawing.Color.Red
        End If

        If Not IsDate(dt(0)("DCS_LIGHT_CURE")) Then
            If Trim(dt(0)("DCS_LIGHT_CURE")) <> "" Then
                llight.Text = dt(0)("DCS_LIGHT_CURE")
            Else
                llight.Text = "NONE"
                lfill.ForeColor = Drawing.Color.Red
            End If
        Else
            llight.Text = "NONE"
            lfill.ForeColor = Drawing.Color.Red
        End If

    End Sub

    Private Sub Load_PEC(ByVal sType As String)
        Dim dtPEC As New DataTable
        Dim row As Long = 1
        Dim bAllowed As Boolean = False

        Try
            Dim sHTML As String = String.Empty
            Dim sbHTMLBuilder As New StringBuilder
            Dim sExtEnrl As String = String.Empty
            Dim sExtContestability As Boolean = False
            Dim sExtMe As Boolean = False
            Dim sNewEnrl As String = String.Empty
            Dim sNewContestability As Boolean = False
            Dim sNewMe As Boolean = False


            With eAcctBLL

                .Member_Type = sType
                dtPEC = .GetPEC
            End With

            If dtPEC.Rows.Count > 0 Then
                For Each dr As DataRow In dtPEC.Rows
                    sExtEnrl = IIf(Not IsDBNull(dr("PEC_EXISTING_ENROLL")), dr("PEC_EXISTING_ENROLL"), "")
                    sExtContestability = dr("PEC_EXISTING_CONTEST")
                    sExtMe = dr("PEC_EXISTING_MB")
                    sNewEnrl = IIf(Not IsDBNull(dr("PEC_NEW_ENROLL")), dr("PEC_NEW_ENROLL"), "")
                    sNewContestability = dr("PEC_NEW_CONTEST")
                    sNewMe = dr("PEC_NEW_MB")
                Next
            End If

            sbHTMLBuilder.Append("<br /><table class=""table table-bordered table-striped tblCons"" >")
            sbHTMLBuilder.Append("<tr><th class='PECthcol1' colspan='2'>Pre-Existing Conditions Coverage</th><th class='PECthcol3'>Contestability</th><th class='PECthcol4'>ME</th></tr>")
            sbHTMLBuilder.Append("<tr><td class='PECcol1'>Existing Enrolees</td><td class='PECcol2'>")
            sbHTMLBuilder.Append(sExtEnrl)
            sbHTMLBuilder.Append("</td><td class='PECcol3'>")
            sbHTMLBuilder.Append("<img class='icons' src=""")
            If sExtContestability = True Then
                sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
            Else
                sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
            End If
            sbHTMLBuilder.Append("""/>")
            sbHTMLBuilder.Append("</td><td class='PECcol4'>")
            sbHTMLBuilder.Append("<img class='icons' src=""")
            If sExtMe = True Then
                sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
            Else
                sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
            End If
            sbHTMLBuilder.Append("""/>")
            sbHTMLBuilder.Append("</td></tr>")

            sbHTMLBuilder.Append("<tr><td class='PECcol1'>New Enrolees</td><td class='PECcol2'>")
            sbHTMLBuilder.Append(sNewEnrl)
            sbHTMLBuilder.Append("</td><td class='PECcol3'>")
            sbHTMLBuilder.Append("<img class='icons' src=""")
            If sNewContestability = True Then
                sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
            Else
                sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
            End If
            sbHTMLBuilder.Append("""/>")
            sbHTMLBuilder.Append("</td><td class='PECcol4'>")
            sbHTMLBuilder.Append("<img class='icons' src=""")
            If sNewMe = True Then
                sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
            Else
                sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
            End If
            sbHTMLBuilder.Append("""/>")
            sbHTMLBuilder.Append("</td></tr>")

            sbHTMLBuilder.Append("</table>")

            Select Case UCase(sType)
                Case "PRN"

                    tblPECP.InnerHtml = sbHTMLBuilder.ToString
                    dtPECOthersP = eAcctBLL.GetOthersPEC
                    dtgOthersPECP.DataSource = dtPECOthersP
                    dtgOthersPECP.DataBind()

                Case "DEP"

                    tblPECD.InnerHtml = sbHTMLBuilder.ToString
                    dtPECOthersD = eAcctBLL.GetOthersPEC
                    dtgOthersPECD.DataSource = dtPECOthersD
                    dtgOthersPECD.DataBind()

                Case "EXT"

                    tblPECE.InnerHtml = sbHTMLBuilder.ToString
                    dtPECOthersE = eAcctBLL.GetOthersPEC
                    dtgOthersPECE.DataSource = dtPECOthersE
                    dtgOthersPECE.DataBind()

            End Select



        Catch ex As Exception
            Response.Write(ex.InnerException.ToString)
        Finally

        End Try
    End Sub

    Private Sub Load_POS()
        Dim dtPOS As New DataTable
        Dim row As Long = 1
        Dim bAllowed As Boolean = False

        Try
            Dim sHTML As String = String.Empty
            Dim sbHTMLBuilder As New StringBuilder


            sbHTMLBuilder.Append("<table class='table table-bordered table-striped tblCons'>")

            With eAcctBLL

                dtPOS = .GetPOS

            End With

            If dtPOS.Rows.Count > 0 Then
                For Each dr As DataRow In dtPOS.Rows

                    sbHTMLBuilder.Append("<tr><td class='col1'>Optional w/ corresponding load premium </td><td class='col2'>")
                    sbHTMLBuilder.Append("<img class='icons' src=""")
                    If dr("POS_OPT") = True Then
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
                    Else
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                    End If
                    sbHTMLBuilder.Append("""/>")
                    sbHTMLBuilder.Append("</td><td class='col3' colspan='2'>")
                    If eAcctBLL.AccountCode = "07172001-000988" Then
                        sbHTMLBuilder.Append("See other info")
                    End If
                    sbHTMLBuilder.Append("</td></tr>")
                    sbHTMLBuilder.Append("<tr><td class='col1'>For <td class='col2' colspan='3'>")
                    sbHTMLBuilder.Append(dr("POS_FOR"))
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Out Patient </td><td class='col2'>")
                    sbHTMLBuilder.Append("<img class='icons' src=""")
                    If Not IsDBNull(dr("POS_CONSULT")) Then
                        If Len(Trim(dr("POS_CONSULT"))) > 0 Then
                            sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
                        Else
                            sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                        End If
                    Else
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                    End If
                    sbHTMLBuilder.Append("""/>")
                    sbHTMLBuilder.Append("</td><td class='col3'>Consultation Fee</td><td class='col4'>")
                    sbHTMLBuilder.Append(dr("POS_CONSULT"))
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>In Patient </td><td class='col2'>")
                    sbHTMLBuilder.Append("<img class='icons' src=""")
                    If Not IsDBNull(dr("POS_PROF_FEE")) Then
                        If Len(Trim(dr("POS_PROF_FEE"))) > 0 Then
                            sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
                        Else
                            sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                        End If
                    Else
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                    End If
                    sbHTMLBuilder.Append("""/>")
                    sbHTMLBuilder.Append("</td><td class='col3'>Professional Fee </td><td class='col4'>")
                    sbHTMLBuilder.Append(dr("POS_PROF_FEE"))
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("</td></tr>")
                    sbHTMLBuilder.Append("<tr><td class='col1'>Approved Lab Exams <td class='col2' colspan='3'>")
                    sbHTMLBuilder.Append(dr("POS_LAB_EXAM"))
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1hdr' colspan='4' style='background: #368bd7 !important; color: white !important; '>Approved Hospital Bills</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Deductible <td class='col2' colspan='3'>")
                    sbHTMLBuilder.Append(dr("POS_HOSP_BILL_DEDUCT"))
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Co - Payment <td class='col2' colspan='3'>")
                    sbHTMLBuilder.Append(dr("POS_HOSP_BILL_CO_PAYMENT"))
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Ambulance service throughout the country on a reimbursement basis up to <td class='col2' colspan='3'>")
                    sbHTMLBuilder.Append(dr("ADD_AMBU_PRIC"))
                    If Trim(dr("ADD_AMBU_PER")) <> "" Then
                        sbHTMLBuilder.Append(" per " & dr("ADD_AMBU_PER"))
                    End If
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Pre - Employment <td class='col2' colspan='3'>")
                    sbHTMLBuilder.Append(dr("PRE_EMP"))
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>At MPI H.O Clinic at <td class='col2' colspan='3'>")
                    If Len(Trim(dr("PRE_EMP_PRICE"))) > 0 Then
                        sbHTMLBuilder.Append(dr("PRE_EMP_PRICE") & " per applicant to company If hired and will serve as his APE.")
                    End If
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Cash Basis </td><td class='col2' colspan='3'>")
                    sbHTMLBuilder.Append("<img class='icons' src=""")
                    If dr("PRE_CASHBASIS") = True Then
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
                    Else
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                    End If
                    sbHTMLBuilder.Append("""/>")
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>For Billing </td><td class='col2' colspan='3'>")
                    sbHTMLBuilder.Append("<img class='icons' src=""")
                    If dr("PRE_FORBILLING") = True Then
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
                    Else
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                    End If
                    sbHTMLBuilder.Append("""/>")
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Experience Refund Benefit <td class='col2' colspan='3'>")
                    If Len(Trim(dr("PRE_EMP_EXP_REFUND"))) > 0 And Len(Trim(dr("PRE_EMP_EXP_PERCENT"))) > 0 Then
                        sbHTMLBuilder.Append(Trim(dr("PRE_EMP_EXP_REFUND")) & " " & Trim(dr("PRE_EMP_EXP_PERCENT")))
                    End If
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Fast - relief medicines for the whole company worth of <td class='col2' colspan='3'>")
                    If Len(Trim(dr("PRE_EMP_FAST_PER"))) > 0 And Len(Trim(dr("PRE_EMP_FAST_PRICE"))) > 0 Then
                        sbHTMLBuilder.Append(Trim(dr("PRE_EMP_FAST_PER")) & " " & Trim(dr("PRE_EMP_FAST_PRICE")))
                    End If
                    sbHTMLBuilder.Append("</td></tr>")
                Next
            End If

            sbHTMLBuilder.Append("</table>")


            posContent.InnerHtml = sbHTMLBuilder.ToString
        Catch ex As Exception
            Response.Write(ex.InnerException.ToString)
        Finally

        End Try
    End Sub

    Private Sub Load_MAT()
        Dim dtMAT As New DataTable
        Dim row As Long = 1
        Dim bAllowed As Boolean = False

        Try
            Dim sHTML As String = String.Empty
            Dim sbHTMLBuilder As New StringBuilder


            sbHTMLBuilder.Append("<table class='table table-bordered table-striped tblCons'>")

            With eAcctBLL

                dtMAT = .GetPOS

            End With

            If dtMAT.Rows.Count > 0 Then
                For Each dr As DataRow In dtMAT.Rows

                    sbHTMLBuilder.Append("<tr><td class='col1'>Maternity Assistance Benefits For </td><td class='col2'>")
                    sbHTMLBuilder.Append(dr("MAB_FOR"))
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Inclusive in Premium </td><td class='col2' >")
                    sbHTMLBuilder.Append("<img class='icons' src=""")
                    If dr("PRE_FORBILLING") = True Then
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
                    Else
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                    End If
                    sbHTMLBuilder.Append("""/>")
                    sbHTMLBuilder.Append("</td></tr>")

                    If Not IsDBNull(dr("MAB_OPTIONAL")) Then
                        If Len(Trim(dr("MAB_OPTIONAL"))) > 0 Then
                            sbHTMLBuilder.Append("<tr><td class='col1'>Maternity Assistance Benefits For </td><td class='col2'>")
                            If Len(Trim(dr("MAB_OPTIONAL"))) > 0 And Len(Trim(dr("MAB_OPT_PAYABLE"))) > 0 Then
                                sbHTMLBuilder.Append(dr("MAB_OPTIONAL") & " / HEAD PAYABLE BY " & dr("MAB_OPT_PAYABLE"))
                            End If
                            sbHTMLBuilder.Append("</td></tr>")
                        End If
                    End If

                    sbHTMLBuilder.Append("<tr><td class='col1' rowspan='3'>Pre/Post Natal excluding lab exams</td><td class='col2'>")
                    If Not IsDBNull(dr("OPCS_PREPOST_NATAL_PRN")) Then
                        If Trim(dr("OPCS_PREPOST_NATAL_PRN")) <> "" Then
                            sbHTMLBuilder.Append("<font color='#4682b4'><strong>Principal: </strong></font>" & dr("OPCS_PREPOST_NATAL_PRN"))
                        Else
                            sbHTMLBuilder.Append("<font color='#4682b4'><strong>Principal: </strong></font><font color='red'>NONE</font>")
                        End If

                    Else
                        sbHTMLBuilder.Append("<font color='#4682b4'><strong>Principal: </strong></font><font color='red'>NONE</font>")
                    End If
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col2'>")
                    If Not IsDBNull(dr("OPCS_PREPOST_NATAL_DEP")) Then
                        If Trim(dr("OPCS_PREPOST_NATAL_DEP")) <> "" Then
                            sbHTMLBuilder.Append("<font color='#4682b4' ><strong>Dependent: </strong></font>" & dr("OPCS_PREPOST_NATAL_DEP"))
                        Else
                            sbHTMLBuilder.Append("<font color='#4682b4'><strong>Dependent: </strong></font><font color='red'>NONE</font>")
                        End If

                    Else
                        sbHTMLBuilder.Append("<font color='#4682b4'><strong>Dependent: </strong></font><font color='red'>NONE</font>")
                    End If
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col2'>")
                    If Not IsDBNull(dr("OPCS_PREPOST_NATAL_EXT")) Then
                        If Trim(dr("OPCS_PREPOST_NATAL_EXT")) <> "" Then
                            sbHTMLBuilder.Append("<font color='#4682b4'><strong>Extended: </strong></font>" & dr("OPCS_PREPOST_NATAL_EXT"))
                        Else
                            sbHTMLBuilder.Append("<font color='#4682b4'><strong>Extended: </strong></font><font color='red'>NONE</font>")
                        End If

                    Else
                        sbHTMLBuilder.Append("<font color='#4682b4'><strong>Extended: </strong></font><font color='red'>NONE</font>")
                    End If
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Normal Delivery </td><td class='col2'>")
                    If Not IsDBNull(dr("MAB_NORM_DELIVERY")) Then
                        If Trim(dr("MAB_NORM_DELIVERY")) <> "" Then
                            sbHTMLBuilder.Append(dr("MAB_NORM_DELIVERY"))
                        Else
                            sbHTMLBuilder.Append("</font><font color='red'>NONE</font>")
                        End If
                    Else
                        sbHTMLBuilder.Append("</font><font color='red'>NONE</font>")
                    End If
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Caesarean </td><td class='col2'>")
                    If Not IsDBNull(dr("MAB_CAEAREAN")) Then
                        If Trim(dr("MAB_CAEAREAN")) <> "" Then
                            sbHTMLBuilder.Append(dr("MAB_CAEAREAN"))
                        Else
                            sbHTMLBuilder.Append("</font><font color='red'>NONE</font>")
                        End If
                    Else
                        sbHTMLBuilder.Append("</font><font color='red'>NONE</font>")
                    End If
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Miscarrieage/Abortion </td><td class='col2'>")
                    If Not IsDBNull(dr("MAB_ABORT")) Then
                        If Trim(dr("MAB_ABORT")) <> "" Then
                            sbHTMLBuilder.Append(dr("MAB_ABORT"))
                        Else
                            sbHTMLBuilder.Append("</font><font color='red'>NONE</font>")
                        End If
                    Else
                        sbHTMLBuilder.Append("</font><font color='red'>NONE</font>")
                    End If
                    sbHTMLBuilder.Append("</td></tr>")

                    sbHTMLBuilder.Append("<tr><td class='col1'>Home Delivery </td><td class='col2'>")
                    If Not IsDBNull(dr("MAB_HOME_DELIVERY")) Then
                        If Trim(dr("MAB_HOME_DELIVERY")) <> "" Then
                            sbHTMLBuilder.Append(dr("MAB_HOME_DELIVERY"))
                        Else
                            sbHTMLBuilder.Append("</font><font color='red'>NONE</font>")
                        End If
                    Else
                        sbHTMLBuilder.Append("</font><font color='red'>NONE</font>")
                    End If
                    sbHTMLBuilder.Append("</td></tr>")

                Next
            End If

            sbHTMLBuilder.Append("</table>")

            sbHTMLBuilder.Append("<table class='table table-bordered table-striped tblCons'>")
            With eAcctBLL

                dtMATOthers = .GetMATOthers

            End With

            If dtMATOthers.Rows.Count > 0 Then
                For Each dr As DataRow In dtMATOthers.Rows
                    sbHTMLBuilder.Append("<tr><td>")
                    If Not IsDBNull(dr("C_ID")) Then
                        sbHTMLBuilder.Append(dr("C_ID"))
                    End If
                    sbHTMLBuilder.Append("</td></tr>")
                Next
            End If

            sbHTMLBuilder.Append("</table>")

            matContent.InnerHtml = sbHTMLBuilder.ToString
        Catch ex As Exception
            Response.Write(ex.InnerException.ToString)
        Finally

        End Try
    End Sub

    Private Sub Executive_Checkup()
        Dim dtEXEC As New DataTable
        Dim row As Long = 1
        Dim bAllowed As Boolean = False

        Try
            Dim sHTML As String = String.Empty
            Dim sbHTMLBuilder As New StringBuilder


            sbHTMLBuilder.Append("<table class='table table-bordered table-striped tblCons'>")
            With eAcctBLL

                dtEXEC = .GetPOS

            End With

            If dtEXEC.Rows.Count > 0 Then
                For Each dr As DataRow In dtEXEC.Rows

                    sbHTMLBuilder.Append("<tr><td class='col1'>Out Patient </td><td class='col2'>")
                    sbHTMLBuilder.Append("<img class='icons' src=""")
                    If Not IsDBNull(dr("ECU_OUT")) Then
                        If dr("ECU_OUT") = True Then
                            sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
                        Else
                            sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                        End If
                    Else
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                    End If
                    sbHTMLBuilder.Append("""/>")
                    sbHTMLBuilder.Append("</td></tr>")
                    sbHTMLBuilder.Append("<tr><td class='col1'>In Patient </td><td class='col2'>")
                    sbHTMLBuilder.Append("<img class='icons' src=""")
                    If Not IsDBNull(dr("ECU_IN")) Then
                        If dr("ECU_IN") = True Then
                            sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
                        Else
                            sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                        End If
                    Else
                        sbHTMLBuilder.Append("images/cross.ico")
                    End If
                    sbHTMLBuilder.Append("""/>")
                    sbHTMLBuilder.Append("</td></tr>")
                    sbHTMLBuilder.Append("<tr><td class='col1'>Inclusive in Premium </td><td class='col2'>")
                    sbHTMLBuilder.Append("<img class='icons' src=""")
                    If Not IsDBNull(dr("ECU_INCL")) Then
                        If dr("ECU_INCL") = True Then
                            sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/tick.ico")
                        Else
                            sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                        End If
                    Else
                        sbHTMLBuilder.Append("https://webportal.medicardphils.com/emedicard/img/cross.ico")
                    End If
                    sbHTMLBuilder.Append("""/>")
                    sbHTMLBuilder.Append("</td></tr>")
                    sbHTMLBuilder.Append("<tr><td class='col1'>For </td><td class='col2'>")
                    If Not IsDBNull(dr("ECU_FOR")) Then
                        sbHTMLBuilder.Append(dr("ECU_FOR"))
                    End If
                    sbHTMLBuilder.Append("</td></tr>")
                    sbHTMLBuilder.Append("<tr><td class='col1'>At </td><td class='col2'>")
                    If Not IsDBNull(dr("ECU_AT")) Then
                        sbHTMLBuilder.Append(dr("ECU_AT"))
                    End If
                    sbHTMLBuilder.Append("</td></tr>")
                    sbHTMLBuilder.Append("<tr><td class='col1'>Free for service basis at </td><td class='col2'>")
                    If Not IsDBNull(dr("ECU_BASIS")) Then
                        sbHTMLBuilder.Append(dr("ECU_BASIS"))
                    End If
                    sbHTMLBuilder.Append("</td></tr>")
                Next
            End If

            sbHTMLBuilder.Append("</table>")

            dtEXECothers = eAcctBLL.GetECUOthers()

            If dtEXECothers.Rows.Count > 0 Then



                sbHTMLBuilder.Append("<legend><h4>OTHERS</h4></legend><table class='table table-bordered table-striped tblCons'>")
                'sbHTMLBuilder.Append("<tr><th class='titlehdr'>Others</th></tr>")

                For Each dr As DataRow In dtEXECothers.Rows
                    sbHTMLBuilder.Append("<tr><td>")
                    If Not IsDBNull(dr("C_ID")) Then
                        sbHTMLBuilder.Append(dr("C_ID"))
                    End If
                    sbHTMLBuilder.Append("</td></tr>")
                Next
                sbHTMLBuilder.Append("</table>")
            End If

            execContent.InnerHtml = sbHTMLBuilder.ToString
        Catch ex As Exception
            Response.Write(ex.InnerException.ToString)
        End Try
    End Sub

    Private Sub Load_Exclusion()

        Dim row As Long = 1
        Dim bAllowed As Boolean = False

        Try

            With eAcctBLL
                dtEXC = .GetExclusion
            End With

            Dim sHTML As String = String.Empty
            Dim sbHTMLBuilder As New StringBuilder
            Dim iRowNum As Integer = 0
            Dim sAccountName As String = String.Empty

            sbHTMLBuilder.Append("<table class='exclusion'>")

            iRowNum = 1
            For Each dr As DataRow In dtEXC.Rows
                sbHTMLBuilder.Append("<tr><td class='col1' style='vertical-align: top; color: black;'>")
                sbHTMLBuilder.Append(iRowNum & ".")
                sbHTMLBuilder.Append("</td><td class='col2' style='padding-left: 5px;'>")
                sbHTMLBuilder.Append(dr("Exclusion_Desc"))
                sbHTMLBuilder.Append("</td></tr>")
                iRowNum += 1
            Next


            sbHTMLBuilder.Append("</table>")

            If iRowNum > 1 Then
                DivExclusion.InnerHtml = sbHTMLBuilder.ToString
            Else
                DivExclusion.InnerHtml = "<p class='secMsg' style='color: Red; font: bold 14px;'>Refer to Contract.</p>"
            End If

        Catch ex As Exception

        Finally

        End Try
    End Sub
    Function ShowData(ByVal sCol1 As String, ByVal sCol2 As String, ByVal sCol3 As String, ByVal sData1 As String, ByVal sData2 As String)
        Dim sHtml As String = String.Empty
        Dim sVal1 As String = String.Empty
        Dim sVal2 As String = String.Empty
        Dim sSty1 As String = String.Empty
        Dim sSty2 As String = String.Empty

        If Len(Trim(sCol2)) > 0 Then
            sVal1 = sCol2
        Else
            sVal1 = sData1
            Select Case sData1
                Case "NONE"
                    sSty1 = "style='color:red;'"
                Case "STANDARD"
                    sSty1 = "style='color:green;'"
                Case Else
                    sSty1 = ""
            End Select
        End If

        If Len(Trim(sCol3)) > 0 Then
            sVal2 = sCol3
        Else
            sVal2 = sData2
            Select Case sData2
                Case "NONE"
                    sSty2 = "style='color:red;'"
                Case "STANDARD"
                    sSty2 = "style='color:green;'"
                Case Else
                    sSty2 = ""
            End Select
        End If
        '"<tr class='nml'><td class='cn'>".$col1."</td><td class='cv' align='center' ".$sty1.">".$val1."</td><td align='center' class='cv' ".$sty2.">".$val2."</td></tr>\n"
        sHtml = "<tr><td class='col1'>" & sCol1 & "</td><td class='ecscol2'" & sSty1 & ">" & sVal1 & "</td><td class='col3'" & sSty2 & ">" & sVal2 & "</td></tr>"
        Return sHtml
    End Function

    Public Sub LoadStandard(ByVal sType As String)
        Dim sbHTMLBuilder As New StringBuilder
        Select Case sType
            Case "HCS"

                With sbHTMLBuilder
                    .Append("        Members may avail of services in any of the 240 accredited hospitals and more than 3,000 medical professionals and specialists accredited by MEDICard. The member must be admitted under the services of the primary physician in the accredited hospital to avail of the following benefits:")
                    .Append("<br />")
                    .Append("<ul><li>No deposit upon admission.</li>")
                    .Append("<li>Room and board according to type of enrollment.</li>")
                    .Append("<li>X-ray and laboratory examinations.</li>")
                    .Append("<li>Services of MEDICard specialist like anaesthesiologists, internists, surgeons, etc.</li>")
                    .Append("<li>Surgery and anaesthesia, dressings, sutures and plaster casts, etc.</li>")
                    .Append("<li>Tranfusion of fresh whole blood and intravenous fluids.</li>")
                    .Append("<li>ICU confinements, chemotherapy, radiotherapy and dialysis are covered subject to the maximum limits and pre-existing conditions coverage.</li>")
                    .Append("<li>Modern therapeutic modalities and interventional surgical procedures such as, but not limited to laparoscopic surgery and lithotripsy, are covered up to P20,000.00 each per member per year subject to the pre-existing conditions coverage*.</li>")
                    .Append("<li>Complex diagnostic procedures such as, but not limited to MRI, CT scan and ultrasound, are covered up to P5,000.00 each per member per year subject to the pre-existing conditions coverage*.</li>")
                    .Append("<li>All other items related to the management of the case.</li>")
                    .Append("<li>Assistance in administrative requirements through the liaison officers.</li>")
                    .Append("</ul><br />*Inclusive of room and board, operating room charges, professional fees and other incidental expenses relative to the procedure.")
                    .Append("<ul><li>Referral to specialists.</li>")
                    .Append("<li>Regular consultations & treatment (except prescribed medicines).</li>")
                    .Append("<li>Treatment of minor injuries and surgery not requiring confinement.</li>")
                    .Append("<li>X-ray and laboratory examinations.</li>")
                    .Append("<li>Eye, Ear, Nose & Throat treatment.</li>")
                    .Append("</ul><br />The member can go directly to the primary physician of any accredited hospital or at the Head Office clinic for out-patient conditions. The primary physician will request for laboratory or diagnostic examinations or refer the member to a specialist. The member may avail of services from any accredited hospital of his choice.")
                    standardHCS.InnerHtml = sbHTMLBuilder.ToString
                End With

            Case "PHCS"
                With sbHTMLBuilder
                    .Append("<ul class='standardList'><li>Annual Physical Examination (APE), to include</li>")
                    .Append("<li>Complete Blood Count</li>")
                    .Append("<li>Urinalysis</li>")
                    .Append("<li>Fecalysis (stool exam)</li>")
                    .Append("<li>Chest x-ray</li>")
                    .Append("<li>Electrocardiogram (adults age 40 and above, or if prescribed)</li>")
                    .Append("<li>Pap smear (Women age 40 and above, or if prescribed)</li>")
                    .Append("<li>Management of Health Problems</li>")
                    .Append("<li>Routine Immunization (except administered medicine)</li>")
                    .Append("<li>Counseling on Health habits, diets and Family Planning</li>")
                    .Append("<li>Record keeping of medical history.</li>")
                    .Append("</ul>APE may be conducted at the MEDICard head office clinic located at the 51 Paseo de Roxas, Makati City or at the company premises through a MEDICard mobile medical team on a scheduled basis for a minimum of 50 principal members and after having paid at least the semi-annual premium.")
                    standardPHCS.InnerHtml = sbHTMLBuilder.ToString
                End With

            Case "ECS"

                With sbHTMLBuilder
                    .Append("When a member in an emergency case ends up at the emergency room of an accredited hospital or clinic, the following are provided free of charge:<br />")
                    .Append("<ul><li>Doctor's services</li>")
                    .Append("<li>Medicines used during treatment of for immediate relief</li>")
                    .Append("<li>Oxygen and intravenous fluids</li>")
                    .Append("<li>Dressings, casts and sutures</li>")
                    .Append("<li>Laboratory, X-ray and other diagnostic examinations directly related to the emergency management of the patient</li></ul>")
                    .Append("EMERGENCY CARE IN NON-ACCREDITED HOSPITALS <br />")
                    .Append("MEDICard agrees to reimburse 80% of the total hospital bills and doctor's professional fees based on MEDICard relative values up to P5,000.00.         <br />")
                    .Append("MEMBERS() ' FINANCIAL ASSISTANCE (For principal members) <br /> <br />")
                    .Append("Aside from the standard benefits to which MEDICard member is entitled to, MEDICard PHILS., INC., also hereby agrees to give/provide the heirs and/or assigns of any member who is enrolled in this health care program in the even of death or injuries through natural causes or accidental means, the following amounts by way of financial assistance:  <br /> <br />")
                    .Append("SCHEDULE OF FINANCIAL ASSISTANCE: <br /> <br />")
                    .Append("Type Rate of Coverage <br />")
                    .Append("Natural Death P10,000.00 <br />")
                    .Append("Accidental Death P20,000.00 <br />")
                    .Append("Loss of sight, or two limbs P10,000.00 <br />")
                    .Append("Loss of sight of one eye, one hand or foot P 5,000.00 <br />")
                    .Append("Provide, that the death or injury results from causes that are covered and are not under the exclusions or uncovered pre-existing conditions as stated in the MEDICard Membership Contract. <br />")
                    .Append("Also, total annual premium for the contract year should have been paid at the time of availment. Otherwise, all remaining unpaid premium will be deducted from the amount of assistance. <br />")
                    standardECS.InnerHtml = sbHTMLBuilder.ToString
                End With

            Case "MFA"
                With sbHTMLBuilder
                    .Append("Aside from the standard benefits to which MEDICard member is entitled to, MEDICard PHILS., INC., also hereby agrees to give/provide the heirs and/or assigns of any member who is enrolled in this health care program in the even of death or injuries through natural causes or accidental means, the following amounts by way of financial assistance:")
                    .Append("<br /> <br />SCHEDULE OF FINANCIAL ASSISTANCE: <br /> <br />")
                    .Append("Type Rate of Coverage <br />")
                    .Append("Natural Death P10,000.00 <br />")
                    .Append("Accidental Death P20,000.00 <br />")
                    .Append("Loss of sight, or two limbs P10,000.00 <br />")
                    .Append("Loss of sight of one eye, one hand or foot P 5,000.00 <br />")
                    .Append("Provide, that the death or injury results from causes that are covered and are not under the exclusions or uncovered pre-existing conditions as stated in the MEDICard Membership Contract. <br />")
                    .Append("Also, total annual premium for the contract year should have been paid at the time of availment. Otherwise, all remaining unpaid premium will be deducted from the amount of assistance. <br />")
                    standardMFA.InnerHtml = sbHTMLBuilder.ToString
                End With

            Case "DCS"
                With sbHTMLBuilder
                    .Append("Members may avail of the following dental care services from any of the accredited dental clinics'<br />")
                    .Append("<ul><li>Annual Prophylaxis (after having paid at least the semi-annual premium)</li>")
                    .Append("<li>Consultations and Oral Examinations</li>")
                    .Append("<li>Simple tooth extractions</li>")
                    .Append("<li>Temporary fillings</li>")
                    .Append("<li>Gum treatment and adjustment of dentures</li>")
                    .Append("<li>Recementation of loose jackets, crowns, in-lays and on-lays</li>")
                    .Append("<li>Treatment of mouth lesions, wounds and burns</li></ul>")
                    standardDCS.InnerHtml = sbHTMLBuilder.ToString
                End With
            Case "PEC"
                With sbHTMLBuilder
                    .Append("Pre-existing condition is an illness, injury or any adverse medical case present prior to and within the first twelve months from effectivity of coverage.  <br />")
                    .Append("Coverage of pre-existing conditions is subject to the following:  <br />  <br />")
                    .Append("Number of Employees Pre-existing disease coverage(per illness per year)  <br />")
                    .Append("Under 100 No coverage  <br />")
                    .Append("100 to 199 Up to P5,000.00  <br />")
                    .Append("200 to 299 50% of the dreaded disease limit  <br />")
                    .Append("300 to up Up to the limit of dreaded disease  <br />")
                    .Append("<br />The following are automatically considered as pre-existing conditions if consultation or treatment is sought within the first twelve (12) months of coverage:  <br />")
                    .Append("<br />1. Dreaded Diseases listed above except for numbers 11 & 12  <br />")
                    .Append("2. Hypertension  <br />")
                    .Append("3. Goiter (Hypo/Hyperthyroidism)  <br />")
                    .Append("4. Cataracts/Glaucoma  <br />")
                    .Append("5. ENT conditions requiring surgery  <br />")
                    .Append("6. Bronchial Asthma  <br />")
                    .Append("7. Turberculosis  <br />")
                    .Append("8. Chronic Cholecystitis/Cholelithiasis (gall bladder stones)  <br />")
                    .Append("9. Acquired Hernias  <br />")
                    .Append("10. Prostate disorders  <br />")
                    .Append("11. Hemorrhoids and Anal Fistulae  <br />")
                    .Append("12. Benign Tumors  <br />")
                    .Append("13. Uterine Myoma, Ovarian cysts, Endometriosis  <br />")
                    .Append("14. Buergher's Disease  <br />")
                    .Append("15. Varicose Veins  <br />")
                    .Append("16. Arthritis  <br />")
                    .Append("17. Migraine headache  <br />")
                    .Append("18. Gastritis / duodenal or gastric ulcers  <br />")
                End With
                standardPEC.InnerHtml = sbHTMLBuilder.ToString

            Case "DRD"

                With sbHTMLBuilder
                    .Append("<p>Dreaded diseases are potentially or actually life threatening conditions or illness which may require prolonged or repeated hospitalization or intensive care management. MEDICard shall pay for hospitalization services up to the maximum limit subject to the pre-existing conditions coverage. <br />")
                    .Append("The following are considered dreaded disease: </p>")
                    .Append("<ul><li>Cerebrovascular accident (stroke)</li>")
                    .Append("<li>Central nervous system lesions (Poliomyelitis/Meningitis/Encephalitis/ Neurosurgical conditions)</li>")
                    .Append("<li>Cardiovascular Disease (Coronary/Valvular/Hypertensive Heart Disease/ Cardiomyopathy)</li>")
                    .Append("<li>Chronic Obstructive Pulmonary Disease (Chronic Bronchitis / Emphysema), Restrictive lund disease</li>")
                    .Append("<li>Liver Parenchymal Disease [Cirrhosis, Hepatitis (except type A), New growth]</li>")
                    .Append("<li>Chronic Kidney/Urological disease (Urolithiasis, Obstructive Uropathies, etc)</li>")
                    .Append("<li>Chronic Gastrointestinal Tract Disease requiring bowel resection and/or anastomosis</li>")
                    .Append("<li>Collagen diseases (Rheumatoid Arthritis, Systemic Lupus Erythematosus)</li>")
                    .Append("<li>Diabetes Mellitus and its complications</li>")
                    .Append("<li>Malignancies and Blood Dyscrasias (Cancer, Leukemia, Idiopathic Thrombocytopenic Purpura)</li>")
                    .Append("<li>Injuries from accidents or assaults, frustrated homicide or frustrated murder</li>")
                    .Append("<li>Complications of an apparent ordinary illness including MODS and SIRS (e.g. sepsis due to pneumonia, typhoid ileitis, cerebral malaria, etc.)</li>")
                    .Append("<li>Single or multiple organ dysfunction and failure (MODS and MOF)</li>")
                    .Append("<li>Conditions that may require dialysis</li>")
                    .Append("<li>Chronic pain syndrome (greater than six weeks)</li>")
                    .Append("<li>Any illness other than the above which would require Intensive Care Unit confinement</li>")
                    .Append("<li>Et cetera</li></ul>")
                End With
                drdContent.InnerHtml = sbHTMLBuilder.ToString
            Case "ME"

                With sbHTMLBuilder
                    .Append("<p>  PRINCIPAL  <br />Salaries personnel at least 18 years old up to age 60 and employed by the company on permanent basis.  <br /> <br />")
                    .Append("DEPENDENT  <br />For Married Employees </p>")
                    .Append("<ul><li>The legal spouse at least 18 years old up to age 60.</li>")
                    .Append("<li>Legitimate and/or legally adopted children above 90 days up to 21 years of age living under the same roof as the principal member.</li>")
                    .Append("</ul> <p>For Single Employees</p>")
                    .Append("<ul> <li>Parents up to age 60, unemployed and dependent upon the principal member.</li>")
                    .Append("<li>Brothers and sisters above 90 days up to 21 years of age who are not gainfully employed and are living under the same roof as the principal member.</li>")
                    .Append("</ul> <p>Enrollees age 41 and above are required to undergo a medical evaluation at the MPI head office clinic, 9th floor Sagittarius Bldg., H.V. dela Costa St., Salcedo Village, Makati City with a minimal fee of P330.00 per head.</p>")
                    .Append("<p>The choice of enrolling dependents must follow a hierarchy. This means that the spouse first must be enrolled followed by the eldest child, second child and so on for married personnel. For single personnel, the parents must be enrolled first followed by eldest brother/sister and so on.</p>")
                    .Append("<p>At least 75% of the total number of principal members shall enroll all their immediate dependents to be able to avail of dependent's coverage. If the above condition is not met, dependents would be subject to a separate program and/or premium rate as may be determined from their exact demographics.</p>")
                End With
                meContent.InnerHtml = sbHTMLBuilder.ToString
        End Select

    End Sub
End Class