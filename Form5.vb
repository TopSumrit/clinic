Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Public Class Form5
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim objDataset As DataSet
    Dim objRow As DataRow
    Dim sql As String
    Dim no_record As Integer
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()
        sql = "select * from patientinfo ORDER BY patient_id ASC"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtPatient")
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "dtPatient"
        showData()
    End Sub


    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        If My.Application.OpenForms.OfType(Of Form3).Any() Then

            Dim form3Instance As Form3 = My.Application.OpenForms.OfType(Of Form3).FirstOrDefault()

            form3Instance.TextBox1.Text = DataGridView1.SelectedCells(0).Value.ToString()
            Me.Close()


        ElseIf My.Application.OpenForms.OfType(Of Form6).Any() Then

            Dim form6Instance As Form6 = My.Application.OpenForms.OfType(Of Form6).FirstOrDefault()

            form6Instance.TextBox2.Text = DataGridView1.SelectedCells(0).Value.ToString()

            Me.Close()
        End If



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        sql = "select * from patientinfo
                where firstname like '%" & TextBox1.Text & "%' or lastname like '%" & TextBox1.Text & "%' ORDER BY patient_id ASC"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtPatient1")
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "dtPatient1"
        no_record = objDataset.Tables("dtPatient1").Rows.Count
        If no_record = 0 Then
            MessageBox.Show("ค้นหาข้อมูลไม่พบ")
            TextBox1.Text = ""
        End If
        showData()
    End Sub
    Sub showData()
        Dim colName As Array = {"รหัสผู้ป่วย", "ชื่อ", "นามสกุล", "วันเกิด", "ที่อยู่", "เบอร์โทรศัพท์", "รหัสบัตรประชาชน"}
        For i = 0 To colName.Length - 1
            DataGridView1.Columns(i).HeaderText = colName(i)
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class