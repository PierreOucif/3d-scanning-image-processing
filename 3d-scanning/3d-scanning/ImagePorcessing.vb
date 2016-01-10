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

    Public Function SubPixel(ByVal image(,) As Color, ByRef Area(,) As Integer)
        Dim sub_pixel(GetNbPixel(image), 1) As Double
        Dim Segment As Integer = 0

        For j As Integer = Area(1, 0) To Area(1, 1)

            Dim SizeSegment As Integer = 0
            Dim y_average As Integer = 0

            For i As Integer = Area(0, 0) To Area(0, 1)
                If image(i, j) = Color.White Then
                    y_average = y_average + i
                    SizeSegment = SizeSegment + 1
                ElseIf image(i - 1, j) = Color.White Then
                    If SizeSegment > 0 Then
                        sub_pixel(Segment, 0) = j
                        sub_pixel(Segment, 1) = y_average / SizeSegment
                        Segment = Segment + 1
                    End If

                End If
            Next
        Next
        Dim Pixel(Segment - 1, 1) As Double
        For i As Integer = 0 To Segment - 1
            If sub_pixel(i, 0) > 0 And sub_pixel(i, 1) > 0 Then
                Pixel(i, 0) = sub_pixel(i, 0)
                Pixel(i, 1) = sub_pixel(i, 1)
            End If
        Next
        Return Pixel
    End Function

    Public Function OrthogonalProjection(ByVal laserpattern(,) As Integer)
        Return Nothing
    End Function
    Public Function TableOfPoints(ByVal image(,) As Color)
        Dim Area(1, 1) As Integer
        Area(0, 0) = 150
        Area(0, 1) = 350
        Area(1, 0) = 50
        Area(1, 1) = 350
        Dim Pixel(,) As Double = SubPixel(image, Area)
        Dim Table(Pixel.GetLength(0) - 1) As Point

        For i As Integer = 0 To Pixel.GetLength(0) - 1
            Table(i) = New Point(Pixel(i, 1), Pixel(i, 0))
        Next
        Return Table
    End Function
End Class
