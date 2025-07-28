
Imports System.Data
Imports System.Data.OleDb
Imports Microsoft.Reporting.WinForms

Public Class employeeReport
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim strCmd As String
    Private Sub employeeReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()
        strCmd = "SELECT employeedetail.firstname, employeedetail.lastname, employeedetail.dateofbirth, employeedetail.telephone, employeedetail.address, employeedetail.salary FROM employeedetail;"
        ' Dim cmd As New OleDbCommand(strCmd, conn)
        objAdapter = New OleDbDataAdapter(strCmd, conn)
        Dim dt As New DataTable
        objAdapter.Fill(dt)
        Dim datasource As New ReportDataSource("employeeDataSet", dt)
        Me.ReportViewer1.LocalReport.DataSources.Clear()
        Me.ReportViewer1.LocalReport.DataSources.Add(datasource)
        Me.ReportViewer1.RefreshReport()
    End Sub
End Class


