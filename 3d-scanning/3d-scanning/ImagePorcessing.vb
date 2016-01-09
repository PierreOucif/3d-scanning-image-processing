Public Class ImagePorcessing

    Public Function LaserRecognition(ByVal image(,) As Color)
        Dim threshold As Integer = 230
        Dim imagetoget = New Bitmap(image.GetLength(0), image.GetLength(1))
        For i As Integer = 0 To image.GetLength(0) - 1
            For j As Integer = 0 To image.GetLength(1) - 1
                If (image(i, j).R / 3 + image(i, j).G / 3 + image(i, j).B / 3) >= threshold Then
                    imagetoget.SetPixel(i, j, Color.White)
                Else : imagetoget.SetPixel(i, j, Color.Black)
                End If
            Next
        Next

        Return imagetoget
    End Function







End Class
