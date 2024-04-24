Imports emedicard_DAL
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.EntityClient
Imports System.Data.Objects
Imports System.Linq
Imports System.Data.SqlClient

Public Class eConsultDAL
    Inherits ememberDAL
    Public Sub New()

    End Sub

    Public Function GetCosultationList(ByVal iUserID As Long, ByVal isDoctor As Boolean) As DataTable
        Using db = New emedicardEntities
            Dim objConsult As ObjectResult(Of emed_consultation_list_Result) = db.emed_consultation_list(iUserID, isDoctor)

            Return ToDataTable(objConsult.ToList)
        End Using
    End Function

    Public Function GetConsultationMsg(ByVal iconsilttid As Integer)
        Dim conn As SqlConnection
        Dim cmd As SqlCommand
        Dim da As New SqlDataAdapter

        Dim ds As New DataSet

        'conn = New SqlConnection(ConfigurationManager.ConnectionStrings("tritonDevEmed").ToString)
        conn = New SqlConnection(ConfigurationManager.ConnectionStrings("tritonDevEmed").ToString)
        cmd = New SqlCommand

        Try
            conn.Open()
            With cmd
                .Connection = conn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "emed_consultation_conversation"

                .Parameters.Add("@consultation_id", SqlDbType.Int).Value = iconsilttid
            End With

            da.SelectCommand = cmd
            da.Fill(ds)

        Catch ex As Exception
            Throw
        Finally
            conn.Close()
        End Try

        Return ds
    End Function

    Public Sub MarkConversation(ByVal iConsID As Long, ByVal isDoc As Boolean)
        Using db = New emedicardEntities
            db.emed_mark_coversation(iConsID, isDoc)
        End Using
    End Sub

    Public Function GetDoctorInfo(ByVal iDocID As Integer) As emed_doctors_user
        Dim objDoc As New emed_doctors_user

        Using db = New emedicardEntities
            objDoc = (From p In db.emed_doctors_user
                     Where p.DoctorID = iDocID
                     Select p).FirstOrDefault
        End Using

        Return objDoc
    End Function

    Public Function GetDoctorEmail(ByVal iConsID As Integer) As emed_doctors_user
        Dim objConsult As New emed_consultation
        Dim objUserPnt As New emed_doctors_user

        Using db = New emedicardEntities
            objConsult = (From p In db.emed_consultation
                          Where p.ConsultationID = iConsID).FirstOrDefault

            objUserPnt = GetDoctorInfo(objConsult.DoctorID)

            Return objUserPnt
        End Using

    End Function
End Class
