Public Class ImagePorcessing

    Public Function LaserRecognitionBitMap(ByVal image(,) As Color)
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
    Public Function LaserRecognitionColor(ByVal image(,) As Color)
        Dim threshold As Integer = 230
        For i As Integer = 0 To image.GetLength(0) - 1
            For j As Integer = 0 To image.GetLength(1) - 1
                If (image(i, j).R / 3 + image(i, j).G / 3 + image(i, j).B / 3) >= threshold Then
                    image(i, j) = Color.White
                Else : image(i, j) = Color.Black
                End If
            Next
        Next
        Return image
    End Function

    Private Function GetNbPixel(ByVal image(,) As Color)
        Dim NbPixel As Integer = 0
        For i As Integer = 0 To image.GetLength(0) - 1
            For j As Integer = 0 To image.GetLength(1) - 1
                If image(i, j) = Color.White Then
                    NbPixel = NbPixel + 1
                End If
            Next
        Next
        Return NbPixel
    End Function

    Public Function SubPixel(ByVal image(,) As Color)
        Dim sub_pixel(2, GetNbPixel(image)) As Integer
        Dim pixel As Integer = 0

        For x_pixel As Integer = 0 To image.GetLength(0) - 1
            Dim nb_pixel As Integer = 0
            Dim y_average As Integer = 0
            Dim line As Integer = 1
            For y_pixel As Integer = 0 To image.GetLength(1) - 1
                If image(x_pixel, y_pixel) = Color.White Then
                    y_average = y_average + y_pixel And nb_pixel = nb_pixel + 1 And line = 1
                Else : line = 0
                End If
            Next
            If line = 0 Then
                pixel = pixel + 1
                sub_pixel(pixel, 0) = x_pixel
                sub_pixel(pixel, 1) = y_average / nb_pixel
            End If
        Next
        Return sub_pixel
    End Function

    Public Function OrthogonalProjection(ByVal laserpattern(,) As Integer)
        Return Nothing
    End Function
    Public Function TableOfPoints(ByVal image(,) As Color)
        Dim Table(GetNbPixel(image)) As Point
        Dim NbPoints As Integer = 0
        Dim Pixel(,) As Integer = SubPixel(image)
        For i As Integer = 0 To image.GetLength(1) - 1
            For j As Integer = 0 To image.GetLength(0) - 1
                If image(i, j) = Color.White Then
                    Table(NbPoints) = New Point(i, j)
                    NbPoints = NbPoints + 1
                End If
            Next
        Next
        Return Table
    End Function
End Class
