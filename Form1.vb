Imports System.IO

Public Class Form1
    Dim version As Integer
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

            Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open)
            Dim br As New BinaryReader(fs)
            'Debug.Print("el tamaño es: " & fs.Length)
            fs.Position = 60
            version = br.ReadInt16
            ' Support for PES4 1.0
            If (fs.Length = 8503296) Then
                version = fs.Length
                'Debug.Print(version)
            End If
            'MsgBox(version)
            Select Case version
                Case 8503296
                    'pes4 1.0
                    fs.Position = 385965
                    'Dim zoom As String = br.ReadInt32()
                    Dim decValue As Integer = 0
                    Dim hexString As String = Nothing

                    decValue = Int(br.ReadInt32())
                    'convert into hexadecimal 
                    hexString = Hex(decValue)

                    'MsgBox("Hexadecimal value is: " & hexString)

                    Dim convert As Single = HextoFloatIEEE(hexString) ' Result: 3.367481

                    'MsgBox("Hexadecimal value is: " & convert)
                    NumericUpDown1.Value = convert

                Case 2320
                    'pes4 1.10
                    fs.Position = 389749
                    'Dim zoom As String = br.ReadInt32()
                    Dim decValue As Integer = 0
                    Dim hexString As String = Nothing

                    decValue = Int(br.ReadInt32())
                    'convert into hexadecimal 
                    hexString = Hex(decValue)

                    'MsgBox("Hexadecimal value is: " & hexString)

                    Dim convert As Single = HextoFloatIEEE(hexString) ' Result: 3.367481

                    'MsgBox("Hexadecimal value is: " & convert)
                    NumericUpDown1.Value = convert

                Case 140
                    'WE8I
                    fs.Position = 386397
                    'Dim zoom As String = br.ReadInt32()
                    Dim decValue As Integer = 0
                    Dim hexString As String = Nothing

                    decValue = Int(br.ReadInt32())
                    'convert into hexadecimal 
                    hexString = Hex(decValue)

                    'MsgBox("Hexadecimal value is: " & hexString)

                    Dim convert As Single = HextoFloatIEEE(hexString) ' Result: 3.367481

                    'MsgBox("Hexadecimal value is: " & convert)
                    NumericUpDown1.Value = convert
                Case Else
                    MsgBox("No PES4 & WE8 exe", MsgBoxStyle.Exclamation)
            End Select
            fs.Close()
            br.Close()

        End If
    End Sub

   
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open)
        Dim bw As New BinaryWriter(fs)
        Select Case version
            Case 8503296
                'pes4 1.0
                Dim hexString As String = ToHexString(NumericUpDown1.Value) ' Result: 405784D0
                'hexString
                fs.Position = 385965
                bw.Write(Int32.Parse(CInt("&H" & hexString)))
            Case 2320
                'pes4 1.10
                Dim hexString As String = ToHexString(NumericUpDown1.Value) ' Result: 405784D0
                'hexString
                fs.Position = 389749
                bw.Write(Int32.Parse(CInt("&H" & hexString)))
            Case 140
                'we8
                Dim hexString As String = ToHexString(NumericUpDown1.Value) ' Result: 405784D0
                'hexString
                fs.Position = 386397
                bw.Write(Int32.Parse(CInt("&H" & hexString)))
        End Select

        
        fs.Close()
        bw.Close()
        MsgBox("OK", MsgBoxStyle.Information)
    End Sub


    'INPUT: It is a hexadecimal number of eight characters
    'RETURN: Single Floating Point
    Function HextoFloatIEEE(ByVal txt As String) As Single
        Dim out As Single
        Try
            Dim by(3) As Byte
            Dim i As Integer
            For i = 0 To 3
                by(i) = Convert.ToInt32(txt.Substring(i * 2, 2), 16)
            Next

            Array.Reverse(by)
            out = BitConverter.ToSingle(by, 0)
        Catch ex As Exception
            out = 0
        End Try
        Return out
    End Function
    Private Function ToHexString(ByVal f As Single) As String
        Dim bytes = BitConverter.GetBytes(f)
        Dim i = BitConverter.ToInt32(bytes, 0)
        Return i.ToString("X8")
    End Function

End Class
