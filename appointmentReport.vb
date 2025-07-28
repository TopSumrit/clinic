Imports System.Data
Imports System.Data.OleDb
Imports Microsoft.Reporting.WinForms
Public Class appointmentReport
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim strCmd As String
    Private Sub appointmentReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()
        strCmd = "SELECT appointment.app_id, appointment.app_date, appointment.app_time, appointment.status, patientinfo.firstname, patientinfo.lastname, appointment.symptom
FROM patientinfo INNER JOIN (employeedetail INNER JOIN appointment ON employeedetail.emp_id = appointment.emp_id) ON patientinfo.patient_id = appointment.patient_id order by app_date asc,app_time asc"
        ' Dim cmd As New OleDbCommand(strCmd, conn)
        objAdapter = New OleDbDataAdapter(strCmd, conn)
        Dim dt As New DataTable
        objAdapter.Fill(dt)
        Dim datasource As New ReportDataSource("appointmentDataset", dt)
        Me.ReportViewer1.LocalReport.DataSources.Clear()
        Me.ReportViewer1.LocalReport.DataSources.Add(datasource)
        Me.ReportViewer1.RefreshReport()
    End Sub
End Class