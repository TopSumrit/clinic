Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Public Class Form9
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim objDataset As DataSet
    Dim objRow As DataRow
    Dim sql As String
    Dim no_record As Integer
    Private Sub Form9_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()
        sql = "SELECT app_id, app_date, app_time,status, patientinfo.firstname, patientinfo.lastname, symptom, patientinfo.telephone, patientinfo.idcard FROM appointment LEFT JOIN patientinfo ON appointment.patient_id = patientinfo.patient_id ORDER BY app_id ASC"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtApp")
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "dtApp"
        DataGridView1.Columns("app_time").DefaultCellStyle.Format = "HH:mm"
        showData()
    End Sub


    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        If My.Application.OpenForms.OfType(Of Form6).Any() Then

            Dim form6Instance As Form6 = My.Application.OpenForms.OfType(Of Form6).FirstOrDefault()
            form6Instance.TextBox1.Text = DataGridView1.SelectedCells(0).Value.ToString()
            Me.Close()
        End If


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        sql = "SELECT app_id, app_date, app_time,status, patientinfo.firstname, patientinfo.lastname, symptom, patientinfo.telephone, patientinfo.idcard FROM appointment LEFT JOIN patientinfo ON appointment.patient_id = patientinfo.patient_id where patientinfo.firstname like '%" & TextBox1.Text & "%' or patientinfo.lastname like '%" & TextBox1.Text & "%' ORDER BY app_id ASC"

        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtApp1")
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "dtApp1"
        DataGridView1.Columns("app_time").DefaultCellStyle.Format = "HH:mm"
        no_record = objDataset.Tables("dtApp1").Rows.Count
        If no_record = 0 Then
            MessageBox.Show("ค้นหาข้อมูลไม่พบ")
            TextBox1.Text = ""
        End If
        showData()
    End Sub
    Sub showData()
        Dim colName As Array = {"รหัสคิว", "วันที่", "เวลา", "สถานะ", "ชื่อ", "นามสกุล", "อาการ", "เบอร์โทรศัพท์", "รหัสบัตรประชาชน"}
        For i = 0 To colName.Length - 1
            DataGridView1.Columns(i).HeaderText = colName(i)
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class