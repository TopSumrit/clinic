Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Reflection.Emit
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Public Class Form1
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim objDataset As DataSet
    Dim objRow As DataRow
    Dim objCmd As OleDbCommand
    Dim sql As String
    Dim no_record As Integer
    Dim chkInsert As Boolean = False
    Dim isSearch As Boolean = False
    Dim currentRowIndex As Integer = -1


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()
        sql = "SELECT app_id, app_date, app_time,status, patientinfo.firstname, patientinfo.lastname, symptom, patientinfo.telephone, patientinfo.idcard FROM appointment LEFT JOIN patientinfo ON appointment.patient_id = patientinfo.patient_id ORDER BY app_date ASC, app_time ASC"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtApp")
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "dtApp"
        DataGridView1.Columns("app_time").DefaultCellStyle.Format = "HH:mm"

        sql = "select DISTINCT app_date from appointment ORDER BY app_date ASC"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtDate")
        ComboBox2.DataSource = objDataset.Tables("dtDate")
        ComboBox2.DisplayMember = "app_date"
        ComboBox2.ValueMember = "app_date"
        ComboBox2.Enabled = False
        showData()

    End Sub




    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        HighlightNextRow()
        ShowHighlightedData()
    End Sub


    Private Sub HighlightNextRow()
        If currentRowIndex < DataGridView1.Rows.Count - 1 Then
            If currentRowIndex >= 0 Then
                DataGridView1.Rows(currentRowIndex).DefaultCellStyle.BackColor = Color.White
            End If
            currentRowIndex += 1
            DataGridView1.Rows(currentRowIndex).DefaultCellStyle.BackColor = Color.LightBlue
        End If
    End Sub



    Private Sub ShowHighlightedData()

        If currentRowIndex >= 0 AndAlso currentRowIndex < DataGridView1.Rows.Count - 1 Then
            Dim highlightedData As String = DataGridView1.Rows(currentRowIndex).Cells(4).Value.ToString() & "  " & DataGridView1.Rows(currentRowIndex).Cells(5).Value.ToString()
            Label2.Text = "คุณ  " & highlightedData
            TextBox2.Text = DataGridView1.Rows(currentRowIndex).Cells(0).Value.ToString()
        Else
            Label2.Text = "ไม่มีข้อมูลแล้ว"
            TextBox2.Text = ""
            ComboBox1.Text = ""
        End If
    End Sub


    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        sql = "select status from appointment where app_id ='" & TextBox2.Text & "'"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtEmp")
        For Each objRow In objDataset.Tables("dtEmp").Rows
            ComboBox1.Text = objRow("status")
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        conn = connectDB()
        sql = "UPDATE appointment SET status = '" & ComboBox1.Text & "' WHERE app_id = '" & TextBox2.Text & "'"

        objCmd = New OleDbCommand(sql, conn)
        no_record = objCmd.ExecuteNonQuery()
        If no_record >= 1 Then
            MsgBox("แก้ไขสถานะเรียบร้อยแล้ว")
            retriveData()
            DataGridView1.Rows(currentRowIndex).DefaultCellStyle.BackColor = Color.LightBlue
        Else
            MsgBox("ไม่สามารถแก้ไขสถานะได้")
        End If
    End Sub
    Sub resetData()
        If currentRowIndex <> -1 Then
            If currentRowIndex < DataGridView1.RowCount Then
                'MsgBox(DataGridView1.RowCount)
                DataGridView1.Rows(currentRowIndex).DefaultCellStyle.BackColor = Color.White
            End If
            currentRowIndex = -1
            TextBox2.Text = ""
            ComboBox1.Text = ""
            Label2.Text = "---"
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        resetData()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        resetData()
        If CheckBox1.Checked = True Then
            sql = "SELECT app_id, app_date, app_time,status, patientinfo.firstname, patientinfo.lastname, symptom, patientinfo.telephone, patientinfo.idcard FROM appointment LEFT JOIN patientinfo ON appointment.patient_id = patientinfo.patient_id where (patientinfo.firstname like '%" & TextBox1.Text & "%' or patientinfo.lastname like '%" & TextBox1.Text & "%') and app_date = CDate('" & ComboBox2.SelectedValue & "') ORDER BY app_date ASC, app_time ASC"
        Else
            sql = "SELECT app_id, app_date, app_time,status, patientinfo.firstname, patientinfo.lastname, symptom, patientinfo.telephone, patientinfo.idcard FROM appointment LEFT JOIN patientinfo ON appointment.patient_id = patientinfo.patient_id where patientinfo.firstname like '%" & TextBox1.Text & "%' or patientinfo.lastname like '%" & TextBox1.Text & "%' ORDER BY app_date ASC, app_time ASC"
        End If
        'MsgBox(sql)
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtSearch")
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "dtSearch"
        If TextBox1.Text = "" And CheckBox1.Checked = False Then
            isSearch = False
        Else
            isSearch = True
        End If
        no_record = objDataset.Tables("dtSearch").Rows.Count
        If no_record = 0 Then
            MessageBox.Show("ค้นหาข้อมูลไม่พบ")
            TextBox1.Text = ""
            isSearch = False
            retriveData()
        Else
            MessageBox.Show("ค้นหาข้อมูลสำเร็จ")
        End If


        DataGridView1.Columns("app_time").DefaultCellStyle.Format = "HH:mm"

        showData()
    End Sub

    Sub retriveData()
        If isSearch = True And CheckBox1.Checked = True Then

            sql = "SELECT app_id, app_date, app_time,status, patientinfo.firstname, patientinfo.lastname, symptom, patientinfo.telephone, patientinfo.idcard FROM appointment LEFT JOIN patientinfo ON appointment.patient_id = patientinfo.patient_id where (patientinfo.firstname like '%" & TextBox1.Text & "%' or patientinfo.lastname like '%" & TextBox1.Text & "%') and app_date = CDate('" & ComboBox2.SelectedValue & "') ORDER BY app_date ASC, app_time ASC"
        ElseIf isSearch = True And CheckBox1.Checked = False Then
            sql = "SELECT app_id, app_date, app_time,status, patientinfo.firstname, patientinfo.lastname, symptom, patientinfo.telephone, patientinfo.idcard FROM appointment LEFT JOIN patientinfo ON appointment.patient_id = patientinfo.patient_id where patientinfo.firstname like '%" & TextBox1.Text & "%' or patientinfo.lastname like '%" & TextBox1.Text & "%' ORDER BY app_date ASC, app_time ASC"

        Else
            sql = "SELECT app_id, app_date, app_time,status, patientinfo.firstname, patientinfo.lastname, symptom, patientinfo.telephone, patientinfo.idcard FROM appointment LEFT JOIN patientinfo ON appointment.patient_id = patientinfo.patient_id ORDER BY app_date ASC, app_time ASC"

        End If
        objAdapter = New OleDbDataAdapter(sql, conn)
        objAdapter.Fill(objDataset, "app1")
        DataGridView1.DataSource = objDataset.Tables("app1")
        objDataset.Tables("app1").Rows.Clear()
        objDataset.Tables("app1").Columns.Clear()
        DataGridView1.DataSource = objDataset.Tables("app1")
        objAdapter.Fill(objDataset, "app1")
        DataGridView1.DataSource = objDataset
        DataGridView1.DataMember = "app1"


        DataGridView1.Columns("app_time").DefaultCellStyle.Format = "HH:mm"
        showData()
        conn.Close()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            ComboBox2.Enabled = True

        Else
            ComboBox2.Enabled = False
        End If
    End Sub

    Sub showData()
        Dim colName As Array = {"รหัสคิว", "วันที่", "เวลา", "สถานะ", "ชื่อ", "นามสกุล", "อาการ", "เบอร์โทรศัพท์", "รหัสบัตรประชาชน"}
        For i = 0 To colName.Length - 1
            DataGridView1.Columns(i).HeaderText = colName(i)
        Next
    End Sub
End Class
