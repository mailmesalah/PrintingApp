Imports System.Drawing.Printing

Public Class FMain
    Public doc As PrintDocument = New PrintDocument()

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Static currentChar As Integer
        Static currentLine As Integer
        Dim textfont As Font = TextBox1.Font
        Dim h, w As Integer
        Dim left, top As Integer



        Dim printCtl As PreviewPrintController = New PreviewPrintController()
        'printCtl.UseAntiAlias = UseAntiAlias

        PrintDocument1.PrintController = New PrintControllerWithStatusDialog(printCtl, vbNull)


        With PrintDocument1.DefaultPageSettings
            h = .PaperSize.Height - .Margins.Top - .Margins.Bottom
            w = .PaperSize.Width - .Margins.Left - .Margins.Right
            left = PrintDocument1.DefaultPageSettings.Margins.Left
            top = PrintDocument1.DefaultPageSettings.Margins.Top
        End With


        e.Graphics.DrawRectangle(Pens.Blue, New Rectangle(left, top, w, h))
        If PrintDocument1.DefaultPageSettings.Landscape Then
            Dim a As Integer
            a = h
            h = w
            w = a
        End If
        Dim lines As Integer = CInt(Math.Round(h / textfont.Height))

        Dim format As StringFormat
        If Not TextBox1.WordWrap Then
            format = New StringFormat(StringFormatFlags.NoWrap)
            format.Trimming = StringTrimming.EllipsisWord
            Dim i As Integer
            For i = currentLine To Math.Min(currentLine + lines, TextBox1.Lines.Length - 1)
                e.Graphics.DrawString(TextBox1.Lines(i), textfont, Brushes.Black, New RectangleF(left, top + textfont.Height * (i - currentLine), w, textfont.Height), format)
            Next
            currentLine += lines
            If currentLine >= TextBox1.Lines.Length Then
                e.HasMorePages = False
                currentLine = 0
            Else
                e.HasMorePages = True
            End If
            Exit Sub
        End If
        format = New StringFormat(StringFormatFlags.LineLimit)
        Dim line, chars As Integer
        e.Graphics.MeasureString(Mid(TextBox1.Text, currentChar + 1), textfont, New SizeF(w, h), format, chars, line)
        If currentChar + chars < TextBox1.Text.Length Then
            If TextBox1.Text.Substring(currentChar + chars, 1) <> " " And TextBox1.Text.Substring(currentChar + chars, 1) <> vbLf Then
                While chars > 0
                    TextBox1.Text.Substring(currentChar + chars, 1)
                    TextBox1.Text.Substring(currentChar + chars, 1)
                    chars -= 1
                End While
                chars += 1
            End If
        End If
        'e.Graphics.DrawString(TextBox1.Text.Substring(currentChar, chars), textfont, Brushes.Black, b, format)
        currentChar = currentChar + chars
        If currentChar < TextBox1.Text.Length Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
            currentChar = 0
        End If
    End Sub

    Private Sub ButtonPrint_Click(sender As Object, e As EventArgs) Handles ButtonPrint.Click

        doc.PrintController = New StandardPrintController()

        AddHandler doc.PrintPage, AddressOf PrintDocumentPrintPage

        doc.Print()


    End Sub

    Private Sub PrintDocumentPrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim drawString As String = "إختبار الرسم"
        Dim drawBrush As SolidBrush = New SolidBrush(Color.Black)
        Dim drawFont As Font = New System.Drawing.Font("Arail", 16)
        Dim format As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
        'format.FormatFlags = StringFormatFlags.DirectionRightToLeft
        Dim recAtZero As RectangleF = New RectangleF(-100, 10, e.PageBounds.Width, e.PageBounds.Height)
        e.Graphics.DrawString(drawString, drawFont, drawBrush, New PointF(200, 10), format)

    End Sub
End Class
