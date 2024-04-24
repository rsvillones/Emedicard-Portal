Public Class ReimbDetails
    Inherits System.Web.UI.Page
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Load_Disapproved()
        'Load_AcctionMemo()
        'Load_WHB_Memo()
        Dim controlCode As String = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("ctr"), key)
        Select Case GetControlStatus()
            Case "Disapproved"
                Load_Disapproved()
            Case "With lacking documents"
                Load_AcctionMemo()
            Case "With Action Memo"
                'Load_AcctionMemo()
                Dim emember = New ememberBLL()
                Dim status As String = emember.GetActionMemoSubstatus(controlCode)
                Select Case status
                    Case "Lacking Of Documents"
                        Load_AcctionMemo()
                    Case "Hospital Bills"
                        Load_WHB_Memo()
                    Case "Disapproved"
                        Load_Disapproved()
                End Select

            Case "Waiting for hospital bills"
                Load_WHB_Memo()
            Case "With URG Concern"
                Load_Reimbursement_Details(2)
            Case "On Hold With Pending Requirements"
                Load_Reimbursement_Details(2)
            Case "On Hold By Billing"
                Load_Reimbursement_Details(1)
            Case "With billing concern"
                Load_Reimbursement_Details(1)
            Case "For check preparation"
                Load_Reimbursement_Details(0)
            Case "Processed With Check"
                Load_Reimbursement_Details(0)
            Case "For release"
                Load_Reimbursement_Details(3)
            Case "Released"
                'Load_Reimbursement_Details(4)
                ' Edited by allan 05/30/2016
                Dim emember = New ememberBLL()
                Dim status As String = emember.GetActionMemoSubstatus(controlCode)
                Select Case status
                    Case "Lacking Of Documents"
                        Load_AcctionMemo()
                    Case "Hospital Bills"
                        Load_WHB_Memo()
                    Case "Disapproved"
                        Load_Disapproved()
                    Case Else
                        'Load_Reimbursement_Details(4)

                        LoadReleasedReimbursement()
                End Select


        End Select
    End Sub
    Function WithActionMemoStatus()
        Dim controlCoode As String = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("ctr"), key)

    End Function
    Function GetControlStatus()
        Dim sStatus As String = String.Empty

        sStatus = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("rsd"), key)

        Return sStatus
    End Function

    Public Sub Load_Disapproved()

        Using bll = New ememberBLL
            bll.Control_Code = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("ctr"), key)
            'bll.Control_Code = "RE-OP058825"
            bll.GetDisapprovalReason()


            Dim sHTML As New StringBuilder

            sHTML.Append("<strong>Reason for disapproval</strong>")
            sHTML.Append("<div class='cont-div'>" & bll.reason_of_disapproval & "</div><br />")
            sHTML.Append("Final memo date:&nbsp;<strong>" & bll.denied_date & "</strong><br />")
            sHTML.Append("Memo released date:&nbsp;<strong>" & bll.memo_released_date & "</strong>")
            reimbcontent.InnerHtml = sHTML.ToString

        End Using

    End Sub

    Public Sub Load_AcctionMemo()

        Using bll = New ememberBLL
            bll.Control_Code = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("ctr"), key)
            'bll.Control_Code = "RE-OP058888"
            bll.GetActionMemo()


            Dim sHTML As New StringBuilder
            Dim sUL As String
            Dim sArr() As String = Split(bll.remarks, "*")

            sUL = "<ul class='req-doc'>"
            For i = 0 To sArr.Length - 1
                sUL = sUL & "<li>" & sArr(i) & "</li>"
            Next
            sUL = sUL & "</ul>"

            sHTML.Append("<strong>Required Document(s)</strong>")
            sHTML.Append("<div class='cont-div'>" & sUL & "</div><br />")
            sHTML.Append("Final memo date:&nbsp;<strong>" & bll.lacking_date & "</strong><br />")
            sHTML.Append("Memo released date:&nbsp;<strong>" & bll.memo_released_date & "</strong>")
            reimbcontent.InnerHtml = sHTML.ToString

        End Using

    End Sub

    Public Sub Load_WHB_Memo()

        Using bll = New ememberBLL
            bll.Control_Code = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("ctr"), key)
            'bll.Control_Code = "RE-OP034837"
            bll.GetWHBMemo()


            Dim sHTML As New StringBuilder
            Dim sUL As String


            sUL = "<ul class='req-doc'>"
            If bll.hospital_bills_from Then
                sUL = sUL & "<li>Hospital bills from " & bll.hospital_bills_from_value & "</li>"
            End If
            If bll.moreover_please_submit Then
                sUL = sUL & "<li>Moreover please submit " & bll.moreover_value & "</li>"
            End If
            If bll.copy_of_transmittal_loa Then
                sUL = sUL & "<li>Copy of transmittal slip LOA No. " & bll.loa_no & "</li>"
            End If
            sUL = sUL & "</ul>"

            sHTML.Append("<strong>Waiting for hospital bills</strong>")
            sHTML.Append("<div class='cont-div'>" & sUL & "</div><br />")
            sHTML.Append("Final memo date:&nbsp;<strong>" & bll.lacking_date & "</strong><br />")
            sHTML.Append("Memo released date:&nbsp;<strong>" & bll.memo_released_date & "</strong>")

            reimbcontent.InnerHtml = sHTML.ToString

        End Using

    End Sub

    Public Sub LoadReleasedReimbursement()
        Dim sStatus = String.Empty
        Dim sMembersCode = String.Empty
        Dim sControlCode = String.Empty
        Try
            Using bll = New ememberBLL
                sControlCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("ctr"), key)
                sStatus = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("rsd"), key)
                sMembersCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("mc"), key)
                reimbcontent.InnerHtml = bll.GetReimDetails(sControlCode, sMembersCode)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())

        End Try


    End Sub
    Public Sub Load_Reimbursement_Details(ByVal itype As Short)
        Dim sStatus = String.Empty

        Using bll = New ememberBLL
            bll.Control_Code = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("ctr"), key)
            sStatus = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("rsd"), key)
            'bll.Control_Code = "RE-OP058825"
            'bll.MemberCode = "31704643"
            'bll.Control_Code = "RE-OP053973"
            bll.MemberCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("mc"), key)
            bll.GetReimbDetails()


            Dim sHTML As New StringBuilder

            sHTML.Append("<strong>Reimbursement Details</strong>")
            sHTML.Append("<div class='cont-div'><table class='reimbinfo'><tr><td>Approved Amount: </td><td><strong>" & bll.approved_amount & "</strong></td></tr>")
            sHTML.Append("<tr><td>Check No.: </td><td><strong>" & bll.check_no & "</strong></td></tr>")
            sHTML.Append("<tr><td>Check date.: </td><td><strong>" & bll.check_date & "</strong></td></tr>")

            Select Case itype
                Case 1
                    ' sHTML.Append("<tr><td>Remarks: </td><td><strong>" & bll.remarks & "</strong></td></tr>")
                    sHTML.Append("<tr><td>Remarks: </td><td><strong>With billing concern</strong></td></tr>")
                Case 2
                    sHTML.Append("<tr><td>Remarks: </td><td><strong>" & bll.OtherMemoRemarks & "</strong></td></tr>")

                Case 3
                    'ready for release
                    sHTML.Append("<tr><td>Ready for release: </td><td><strong>" & bll.ready_for_release_date & "</strong></td></tr>")
                Case 4
                    'released
                    sHTML.Append("<tr><td>Ready for release: </td><td><strong>" & bll.ready_for_release_date & "</strong></td></tr>")
                    sHTML.Append("<tr><td>Released date: </td><td><strong>" & bll.check_release_date & "</strong></td></tr>")
            End Select

            sHTML.Append("</table></div>")

            reimbcontent.InnerHtml = sHTML.ToString

        End Using

    End Sub
End Class