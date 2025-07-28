Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form8
    Public Shared Property LoggedInUsername As String
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim objDataset As DataSet = New DataSet
    Dim objRow As DataRow
    Dim sql As String
    Dim no_record As Integer
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        conn = connectDB()
        Dim username As String = TextBox1.Text.Trim()
        Dim password As String = TextBox2.Text.Trim()

        ' Replace this query with your actual query to check username and password
        sql = "SELECT * FROM employeedetail WHERE username = '" & TextBox1.Text & "' AND pwd ='" & TextBox2.Text & "'"
        ' MsgBox(sql)
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtEmployee")
        no_record = objDataset.Tables("dtEmployee").Rows.Count
        'MsgBox(no_record)

        If no_record > 0 Then
            MessageBox.Show("เข้าสู่ระบบสำเร็จ")
            ' Store the logged-in username
            LoggedInUsername = username

            ' If login successful, you can open another form or perform other actions
            Dim mainForm As New MainMenu()
            Form7.Show()
            Me.Hide()
        Else
            MessageBox.Show("Username หรือ Password ไม่ถูกต้อง โปรดลองอีกครั้ง")
        End If
    End Sub


End Class