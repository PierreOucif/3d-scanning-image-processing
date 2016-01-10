Public Class ImagePorcessing

    Dim dll As New DimensionnementImage.GestionImage
    Dim Drawing As Graphics
    Dim BlackPen As New Pen(Color.Black)

    Public Function LaserRecognitionBitMap(ByVal image(,) As Color)
        Dim threshold As Integer = 240
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
        Dim Angle As Double = 20


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
    Function GenererMatriceFromJPGFastOne(ByVal Path As String) As Color(,)
        Dim imageaanalyser = New Bitmap(Path)
        Dim Matrice(imageaanalyser.Width - 1, imageaanalyser.Height - 1) As Color
        Dim rect As New Rectangle(0, 0, imageaanalyser.Width, imageaanalyser.Height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = imageaanalyser.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadOnly, imageaanalyser.PixelFormat)
        Dim ptr As IntPtr = bmpData.Scan0


        Dim bytes As Integer = Math.Abs(bmpData.Stride) * imageaanalyser.Height
        Dim rgbValues(bytes - 1) As Byte


        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)


        Dim ligne As Integer = 0
        Dim colonne As Integer = 0

        For compteur As Integer = 0 To rgbValues.Length - 1 Step 3

            Matrice(colonne, ligne) = Color.FromArgb(rgbValues(compteur + 2), rgbValues(compteur + 1), rgbValues(compteur))
            colonne += 1
            If colonne > imageaanalyser.Width - 1 Then
                colonne = 0
                ligne += 1
            End If
        Next

        imageaanalyser.UnlockBits(bmpData)

        Return Matrice
    End Function
    Public Sub AfficherImage(ByRef TextBox As TextBox, ByRef PictureBoxImage As PictureBox, ByRef PictureBoxDrawing As PictureBox)
        ' Test de l'existance du fichier proposé
        Dim Path As String = TextBox.Text
        If IO.File.Exists(Path) Then
            ' Instantiation du chrono pour comparaisons

            Dim ImageAAnalyser(,) As Color = Nothing
            Dim ImageAAfficher(,) As Color = Nothing
            ImageAAnalyser = GenererMatriceFromJPGFastOne(Path)
            ImageAAfficher = dll.agrandissementaupproche(500, 500, ImageAAfficher)
            If Not (ImageAAnalyser Is Nothing) Then
                ' Format and display the TimeSpan value.


                Dim image = New Bitmap(ImageAAfficher.GetLength(0), ImageAAfficher.GetLength(1))
                ' Utilisation de la solution GetPixel qui est très très lente...
                For colonne As Integer = 0 To PictureBoxImage.Width - 1
                    For ligne As Integer = 0 To PictureBoxImage.Height - 1
                        image.SetPixel(colonne, ligne, ImageAAfficher(colonne, ligne))
                    Next
                Next

                PictureBoxImage.Image = image
                PictureBoxDrawing.Image = LaserRecognitionBitMap(ImageAAnalyser)
                Drawing = PictureBoxDrawing.CreateGraphics
                Drawing.DrawLines(BlackPen, TableOfPoints(LaserRecognitionColor(ImageAAfficher)))
            End If
        Else
            MsgBox("Problème : le fichier à lire n'existe pas ! A vérifier")
        End If
    End Sub
End Class
