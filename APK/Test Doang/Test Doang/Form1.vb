Imports System
Imports System.IO.Ports
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form1
    Dim comPort As String
    Dim receiveData As String = ""
    Dim chartvisual As New Series

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LblKeterangan.Text = ""
        Timer1.Enabled = False
        comPort = ""
        For Each sp As String In My.Computer.Ports.SerialPortNames
            ComboBox1.Items.Add(sp)
        Next
        chartvis()
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        If (Button1.Text = "Connect") Then
            If (comPort <> "") Then
                SerialPort1.Close()
                SerialPort1.PortName = comPort
                SerialPort1.BaudRate = 9600
                SerialPort1.DataBits = 8
                SerialPort1.Parity = Parity.None
                SerialPort1.StopBits = StopBits.One
                SerialPort1.Handshake = Handshake.None
                SerialPort1.Encoding = System.Text.Encoding.Default
                SerialPort1.ReadTimeout = 1000
                SerialPort1.Open()
                Button1.Text = "Dis-Connect"
                LblKeterangan.Text = "AKTIF"
                Timer1.Start()
            Else
                MsgBox("Pilih Port Yang Akan Anda Gunakan")
            End If
        Else
            SerialPort1.Close()
            Button1.Text = "Connect"
            LblKeterangan.Text = "TIDAK AKTIF"
            Timer1.Enabled = False
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If (ComboBox1.SelectedItem <> "") Then
            comPort = ComboBox1.SelectedItem
        End If
    End Sub
    Function receiverserialdata() As String
        Dim incoming As String
        Try
            incoming = SerialPort1.ReadExisting()
            If incoming Is Nothing Then
                Return "Nothing" & vbCrLf
            Else
                Return incoming
            End If
        Catch ex As Exception
            Return "Error : Serial Port Read Time Out"
        End Try
    End Function

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer1.Tick
        receiveData = receiverserialdata()
        RichTextBox1.Text &= receiveData
        chartvisual.Points.AddY(receiveData)
    End Sub

    Private Sub RichTextBox1_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RichTextBox1.TextChanged
        If Val(receiveData) >= 1000 Then
            TextBox1.Text = 1
            TextBox1.BackColor = Color.Red
        ElseIf Val(receiveData) < 1000 Then
            TextBox1.Text = 0
            TextBox1.BackColor = Color.Green

        End If
    End Sub
    Sub chartvis()
        chartvisual.Name = "Vibration Detection"
        chartvisual.ChartType = SeriesChartType.Line
        chartvisual.BackSecondaryColor = Color.Red
        chartvisual.Points.AddY(receiveData)
        Chart1.Series.Add(chartvisual)
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        Timer1.Stop()
    End Sub

    Private Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        Timer1.Start()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Chart1.Printing.PrintPreview()
    End Sub
End Class
