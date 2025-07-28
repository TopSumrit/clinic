Imports System.Data.OleDb
Imports System.Reflection.Emit
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form7
    Dim conn As OleDbConnection
    Dim objAdapter As OleDbDataAdapter
    Dim objDataset As DataSet
    Dim objRow As DataRow
    Dim objCmd As OleDbCommand
    Dim sql As String
    Dim no_record As Integer
    Dim chkInsert As Boolean = False
    Dim i As Integer
    Dim empID As String
    Public Shared Property employeeID As String

    Sub openForm(ByVal formToOpen As Form)
        Dim OpenForm As Form
        For Each OpenForm In Me.MdiChildren
            OpenForm.MdiParent = Me
            OpenForm.Close()
        Next
        For Each OpenForm In Me.MdiChildren
            If OpenForm.GetType() Is formToOpen.GetType() Then
                OpenForm.MdiParent = Me
                OpenForm.Close()
                formToOpen.MdiParent = Me
                formToOpen.Show()


                Exit Sub
            End If
        Next
        formToOpen.MdiParent = Me


        formToOpen.Show()
        formToOpen.Location = New Point(0, 0)

    End Sub







    Private Sub ขอมลผปวยToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ขอมลผปวยToolStripMenuItem.Click
        Dim newForm As New Form3()
        openForm(newForm)
    End Sub

    Private Sub ขอมลพนกงานToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ขอมลพนกงานToolStripMenuItem.Click
        Dim newForm As New Form2()
        openForm(newForm)
    End Sub



    Private Sub ขอมลผปวยToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ขอมลผปวยToolStripMenuItem1.Click
        Dim newForm As New Form5()
        openForm(newForm)
    End Sub

    Private Sub ขอมลพนกงานToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ขอมลพนกงานToolStripMenuItem1.Click
        Dim newForm As New Form4()
        openForm(newForm)
    End Sub

    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = connectDB()

        Dim username As String = Form8.LoggedInUsername
        sql = "select * from employeedetail where username='" & username & "'"
        objAdapter = New OleDbDataAdapter(sql, conn)
        objDataset = New DataSet()
        objAdapter.Fill(objDataset, "dtEmp")
        For Each objRow In objDataset.Tables("dtEmp").Rows
            employeeID = objRow("emp_id")
            empID = objRow("emp_id")
            Label1.Text = "ยินดีต้อนรับคุณ " & objRow("firstname") & " " & objRow("lastname")
        Next


        Dim newForm As New Form1()
        openForm(newForm)
    End Sub

    Private Sub ขอมลการจองควToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ขอมลการจองควToolStripMenuItem1.Click
        Dim newForm As New Form9()
        openForm(newForm)
    End Sub



    Private Sub เรยกควToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles เรยกควToolStripMenuItem1.Click
        Dim newForm As New Form1()
        openForm(newForm)
    End Sub



    Private Sub รายงานขอมลการจองควToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles รายงานขอมลการจองควToolStripMenuItem.Click
        Dim newForm As New appointmentReport()
        openForm(newForm)
    End Sub

    Private Sub รายงานขอมลผปวยToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles รายงานขอมลผปวยToolStripMenuItem.Click
        Dim newForm As New patientReport()
        openForm(newForm)
    End Sub

    Private Sub รายงานขอมลพนกงานToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles รายงานขอมลพนกงานToolStripMenuItem.Click
        Dim newForm As New employeeReport()
        openForm(newForm)
    End Sub

    Private Sub จดการขอมลจองควToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles จดการขอมลจองควToolStripMenuItem.Click
        Dim newForm As New Form6()
        openForm(newForm)
    End Sub
End Class