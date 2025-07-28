Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel

Public Class Form6
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim objDataset As DataSet
    Dim objRow As DataRow
    Dim objCmd As OleDbCommand
    Dim sql As String
    Dim no_record As Integer
    Dim chkInsert As Boolean = False
    Dim empID As String
    Dim appdate As Date
    Dim idtime As String
    Public Shared Property employeeID As String

    Sub autoid()
        '1.หารหัสสูงสุด
        Dim objDataset2 As DataSet = New DataSet
        conn = connectDB()
        sql = "select max(app_id) as maxid from appointment"
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
            TextBox1.Text = "A000" & newid
        ElseIf Len(CStr(newid)) = 2 Then
            TextBox1.Text = "A00" & newid
        ElseIf Len(CStr(newid)) = 3 Then
            TextBox1.Text = "A0" & newid
        Else
            TextBox1.Text = "A" & newid
        End If
        TextBox1.Enabled = False

    End Sub
    Sub disableButton()
        Button1.Enabled = False
        Button2.Enabled = True
        Button4.Enabled = False
        Button5.Enabled = False
    End Sub

    Sub enableButton()
        Button1.Enabled = True
        Button4.Enabled = True
        Button5.Enabled = True
    End Sub

    Sub clearControl()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox4.Text = ""
        DateTimePicker1.ResetText()
        DateTimePicker2.ResetText()


    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form5.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        disableButton()
        chkInsert = True
        TextBox2.Focus()
        autoid()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        conn = connectDB()

        appdate = DateTimePicker1.Value.Date
        appdate = appdate.AddYears(-543)


        Dim desiredTime As DateTime = DateTimePicker2.Value
        Dim startTime As DateTime = desiredTime.AddMinutes(-20)
        Dim endTime As DateTime = desiredTime.AddMinutes(20)
        Dim record_time As Integer

        sql = "SELECT * FROM appointment WHERE app_time BETWEEN #" & startTime.ToString("HH:mm") & "# AND #" & endTime.ToString("HH:mm") & "# and app_date = #" & appdate.ToString("MM/dd/yyyy") & "#"

        'MsgBox(sql)
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dttime")
        record_time = objDataset.Tables("dttime").Rows.Count
        'MsgBox(record_time)


        For Each objRow In objDataset.Tables("dttime").Rows
            idtime = objRow("app_id")
        Next


        'MsgBox(startTime & "    " & endTime)

        If record_time = 0 Then

            If chkInsert = True Then
                sql = "insert into appointment(app_id,patient_id,emp_id,app_date,app_time,symptom,status) values ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & empID & "',#" & appdate.ToString("MM/dd/yyyy") & "#,#" & DateTimePicker2.Value.ToString("HH:mm") & "#,'" & TextBox4.Text & "','" & ComboBox1.Text & "')"
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
                sql = "update appointment set patient_id = '" & TextBox2.Text & "', emp_id = '" & empID & "', app_date = #" & appdate.ToString("MM/dd/yyyy") & "#, app_time = #" & DateTimePicker2.Value.ToString("HH:mm") & "#, symptom = '" & TextBox4.Text & "', status = '" & ComboBox1.Text & "' where app_id = '" & TextBox1.Text & "'"
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

        ElseIf record_time = 1 And idtime = TextBox1.Text And chkInsert = False Then
            sql = "update appointment set patient_id = '" & TextBox2.Text & "', emp_id = '" & empID & "', app_date = #" & appdate.ToString("MM/dd/yyyy") & "#, app_time = #" & DateTimePicker2.Value.ToString("HH:mm") & "#, symptom = '" & TextBox4.Text & "', status = '" & ComboBox1.Text & "' where app_id = '" & TextBox1.Text & "'"
            objCmd = New OleDbCommand(sql, conn)
            no_record = objCmd.ExecuteNonQuery()
            If no_record >= 1 Then
                MsgBox("แก้ไขข้อมูลเรียบร้อยแล้ว")
            Else
                MsgBox("ไม่สามารถแก้ไขข้อมูลได้")
            End If
            enableButton()
            clearControl()
        Else
            MessageBox.Show("ไม่สามารถเพิ่มข้อมูลได้ เนื่องจากมีข้อมูลในช่วงเวลานี้แล้ว")
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        conn = connectDB()
        sql = "delete from appointment where app_id='" & TextBox1.Text & "'"
        If MessageBox.Show("ต้องการลบข้อมูลใช่หรือไม่ ?", "ยืนยันการลบข้อมูล", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
            objCmd = New OleDbCommand(sql, conn)
            objCmd.ExecuteNonQuery()
            MsgBox("ลบข้อมูลแล้วจำนวน 1 Record")
        End If
        clearControl()
        enableButton()

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form9.Show()
    End Sub

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()

        Dim username As String = Form8.LoggedInUsername
        sql = "select * from employeedetail where username='" & username & "'"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtEmp")
        For Each objRow In objDataset.Tables("dtEmp").Rows
            employeeID = objRow("emp_id")
            empID = objRow("emp_id")

        Next
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            sql = "select * from appointment where app_id ='" & TextBox1.Text & "'"
            'MsgBox(sql)
            objAdapter = New OleDbDataAdapter(sql, conn)
            objDataset = New DataSet()
            objAdapter.Fill(objDataset, "dtApp")
            For Each objRow In objDataset.Tables("dtApp").Rows
                TextBox2.Text = objRow("patient_id")

                DateTimePicker1.Value = objRow("app_date")
                DateTimePicker2.Value = objRow("app_time")
                TextBox4.Text = objRow("symptom")
                ComboBox1.Text = objRow("status")
            Next
            Button1.Enabled = False

        End If

    End Sub
End Class