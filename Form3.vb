Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Globalization
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Public Class Form3
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
        sql = "select max(patient_id) as maxid from patientinfo"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objAdapter.Fill(objDataset2, "dtMaxPatID")
        '2.นำรหัสสูงสุด +1
        Dim maxid As String
        maxid = objDataset2.Tables("dtMaxPatID").Rows(0).Item("maxid")
        ' MsgBox(maxid)
        Dim newid As Integer
        newid = Mid(maxid, 2, 4)
        newid = newid + 1
        '3.นำรหัสที่ได้ใส่ลงใน (textbox)
        If Len(CStr(newid)) = 1 Then
            TextBox1.Text = "P000" & newid
        ElseIf Len(CStr(newid)) = 2 Then
            TextBox1.Text = "P00" & newid
        ElseIf Len(CStr(newid)) = 3 Then
            TextBox1.Text = "P0" & newid
        Else
            TextBox1.Text = "P" & newid
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
        DateTimePicker1.ResetText()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form5.Show()
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
            sql = "insert into patientinfo(patient_id,firstname,lastname,dateofbirth,address,telephone,idcard) values ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "',#" & appdate.ToString("MM/dd/yyyy") & "#,'" & TextBox4.Text & "','" & TextBox5.Text & "','" & TextBox6.Text & "')"
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
            sql = "UPDATE patientinfo SET firstname = '" & TextBox2.Text & "', lastname = '" & TextBox3.Text & "', dateofbirth = #" & appdate.ToString("MM/dd/yyyy") & "#, address = '" & TextBox4.Text & "', telephone = '" & TextBox5.Text & "', idcard = '" & TextBox6.Text & "' WHERE patient_id = '" & TextBox1.Text & "'"
            'MsgBox(sql)

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

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'ปุ่มลบข้อมูล
        conn = connectDB()
        sql = "delete from patientinfo where patient_id='" & TextBox1.Text & "'"
        If MessageBox.Show("ต้องการลบข้อมูลใช่หรือไม่ ?", "ยืนยันการลบข้อมูล", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
            objCmd = New OleDbCommand(sql, conn)
            objCmd.ExecuteNonQuery()
            MsgBox("ลบข้อมูลแล้วจำนวน 1 Record")
        End If
        clearControl()
        enableButton()

    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()
        sql = "select * from patientinfo"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtPat1")
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            sql = "select * from patientinfo where patient_id ='" & TextBox1.Text & "'"
            'MsgBox(sql)
            objAdapter = New OleDbDataAdapter(sql, conn)
            objDataset = New DataSet()
            objAdapter.Fill(objDataset, "dtPat")
            For Each objRow In objDataset.Tables("dtPat").Rows
                TextBox2.Text = objRow("firstname")
                TextBox3.Text = objRow("lastname")
                DateTimePicker1.Value = objRow("dateofbirth")
                TextBox4.Text = objRow("address")
                TextBox5.Text = objRow("telephone")
                TextBox6.Text = objRow("idcard")
            Next
            Button2.Enabled = False
        End If

    End Sub
End Class