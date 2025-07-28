Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Public Class Form4
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim objDataset As DataSet
    Dim objRow As DataRow
    Dim sql As String
    Dim no_record As Integer

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()
        sql = "select emp_id, firstname, lastname, dateofbirth, telephone, address, salary from employeedetail ORDER BY emp_id ASC"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtEmployee")
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "dtEmployee"
        showData()
    End Sub


    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        If My.Application.OpenForms.OfType(Of Form2).Any() Then

            Dim form2Instance As Form2 = My.Application.OpenForms.OfType(Of Form2).FirstOrDefault()

            form2Instance.TextBox1.Text = DataGridView1.SelectedCells(0).Value.ToString()
            Me.Close()

        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        sql = "select emp_id, firstname, lastname, dateofbirth, telephone, address, salary from employeedetail 
                where firstname like '%" & TextBox1.Text & "%' or lastname like '%" & TextBox1.Text & "%' ORDER BY emp_id ASC"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtEmployee1")
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "dtEmployee1"
        no_record = objDataset.Tables("dtEmployee1").Rows.Count
        If no_record = 0 Then
            MessageBox.Show("ค้นหาข้อมูลไม่พบ")
            TextBox1.Text = ""
        End If
        showData()
    End Sub

    Sub showData()
        Dim colName As Array = {"รหัสพนักงาน", "ชื่อ", "นามสกุล", "วันเกิด", "เบอร์โทรศัพท์", "ที่อยู่", "เงินเดือน"}
        For i = 0 To colName.Length - 1
            DataGridView1.Columns(i).HeaderText = colName(i)
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class