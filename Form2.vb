Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Public Class Form2
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim objDataset As DataSet
    Dim objRow As DataRow
    Dim objCmd As OleDbCommand
    Dim sql As String
    Dim no_record As Integer
    Dim chkInsert As Boolean = False
    Dim appdate As Date


    Sub autoid()
        '1.หารหัสสูงสุด
        Dim objDataset2 As DataSet = New DataSet
        conn = connectDB()
        sql = "select max(emp_id) as maxid from employeedetail"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objAdapter.Fill(objDataset2, "dtMaxEmpID")
        '2.นำรหัสสูงสุด +1
        Dim maxid As String
        maxid = objDataset2.Tables("dtMaxEmpID").Rows(0).Item("maxid")
        'MsgBox(maxid)
        Dim newid As Integer
        newid = Mid(maxid, 2, 4)
        newid = newid + 1
        '3.นำรหัสที่ได้ใส่ลงใน (textbox)
        If Len(CStr(newid)) = 1 Then
            TextBox1.Text = "E000" & newid
        ElseIf Len(CStr(newid)) = 2 Then
            TextBox1.Text = "E00" & newid
        ElseIf Len(CStr(newid)) = 3 Then
            TextBox1.Text = "E0" & newid
        Else
            TextBox1.Text = "E" & newid
        End If
        TextBox1.Enabled = False

    End Sub
    Sub disableButton()
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = True
        Button4.Enabled = False
    End Sub

    Sub enableButton()
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True
    End Sub

    Sub clearControl()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""
        DateTimePicker1.ResetText()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        disableButton()
        chkInsert = True
        TextBox2.Focus()
        autoid()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        conn = connectDB()

        appdate = DateTimePicker1.Value.Date
        appdate = appdate.AddYears(-543)

        If chkInsert = True Then
            sql = "insert into employeedetail(emp_id,firstname,lastname,dateofbirth,telephone,address,salary,username,pwd) values('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "',#" & appdate.ToString("MM/dd/yyyy") & "#,'" & TextBox4.Text & "','" & TextBox5.Text & "'," & TextBox6.Text & ",'" & TextBox7.Text & "','" & TextBox8.Text & "')"
            'MsgBox(sql)
            objCmd = New OleDbCommand(sql, conn)
            no_record = objCmd.ExecuteNonQuery()
            If no_record >= 1 Then
                MsgBox("เพิ่มข้อมูลเรียบร้อยแล้ว")
            Else
                MsgBox("ไม่สามารถเพิ่มข้อมูลได้")
            End If
            chkInsert = False
        Else
            sql = "UPDATE employeedetail SET firstname = '" & TextBox2.Text & "', lastname = '" & TextBox3.Text & "', dateofbirth = #" & appdate.ToString("MM/dd/yyyy") & "#, telephone = '" & TextBox4.Text & "', address = '" & TextBox5.Text & "', salary = " & TextBox6.Text & ", username = '" & TextBox7.Text & "', pwd = '" & TextBox8.Text & "' WHERE emp_id = '" & TextBox1.Text & "'"
            objCmd = New OleDbCommand(sql, conn)
            no_record = objCmd.ExecuteNonQuery()
            If no_record >= 1 Then
                MsgBox("แก้ไขข้อมูลเรียบร้อยแล้ว")
            Else
                MsgBox("ไม่สามารถแก้ไขข้อมูลได้")
            End If
        End If
        enableButton()
        clearControl()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form4.Show()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            sql = "select * from employeedetail where emp_id ='" & TextBox1.Text & "'"
            objAdapter = New OleDbDataAdapter(sql, conn)
            objDataset = New DataSet()
            objAdapter.Fill(objDataset, "dtEmp")
            For Each objRow In objDataset.Tables("dtEmp").Rows
                TextBox2.Text = objRow("firstname")
                TextBox3.Text = objRow("lastname")
                DateTimePicker1.Value = objRow("dateofbirth")
                TextBox4.Text = objRow("telephone")
                TextBox5.Text = objRow("address")
                TextBox6.Text = objRow("salary")
                TextBox7.Text = objRow("username")
                TextBox8.Text = objRow("pwd")
            Next
            Button2.Enabled = False

        End If

    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()
        sql = "select * from employeedetail"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtEmp1")


    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'ปุ่มลบข้อมูล
        conn = connectDB()
        sql = "delete from employeedetail where emp_id='" & TextBox1.Text & "'"
        If MessageBox.Show("ต้องการลบข้อมูลใช่หรือไม่ ?", "ยืนยันการลบข้อมูล", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
            objCmd = New OleDbCommand(sql, conn)
            objCmd.ExecuteNonQuery()
            MsgBox("ลบข้อมูลแล้วจำนวน 1 Record")
        End If
        clearControl()
        enableButton()

    End Sub
End Class