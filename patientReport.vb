Imports System.Data
Imports System.Data.OleDb
Imports Microsoft.Reporting.WinForms
Public Class patientReport
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim strCmd As String
    Private Sub patientReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()
        strCmd = "SELECT patientinfo.firstname, patientinfo.lastname, patientinfo.dateofbirth, patientinfo.address, patientinfo.telephone FROM patientinfo;"
        ' Dim cmd As New OleDbCommand(strCmd, conn)
        objAdapter = New OleDbDataAdapter(strCmd, conn)
        Dim dt As New DataTable
        objAdapter.Fill(dt)
        Dim datasource As New ReportDataSource("patientDataSet", dt)
        Me.ReportViewer1.LocalReport.DataSources.Clear()
        Me.ReportViewer1.LocalReport.DataSources.Add(datasource)
        Me.ReportViewer1.RefreshReport()
    End Sub
End Class


