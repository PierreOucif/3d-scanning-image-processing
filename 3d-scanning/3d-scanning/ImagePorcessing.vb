Public Class ImagePorcessing

    Dim dll As New DimensionnementImage.GestionImage
    Dim BlackPen As New Pen(Color.Black, 2)


    Public Function LaserRecognitionBitMap(ByVal image(,) As Color)
        Dim imagetoget = New Bitmap(image.GetLength(0), image.GetLength(1))
        For i As Integer = 0 To image.GetLength(0) - 1
            For j As Integer = 0 To image.GetLength(1) - 1
                If (image(i, j).R / 3 + image(i, j).G / 3 + image(i, j).B / 3) >= 230 Then
                    imagetoget.SetPixel(i, j, Color.White)
                Else : imagetoget.SetPixel(i, j, Color.Black)
                End If
            Next
        Next
        Return imagetoget
    End Function
    Public Function LaserRecognitionColor(ByVal image(,) As Color)
        For i As Integer = 0 To image.GetLength(0) - 1
            For j As Integer = 0 To image.GetLength(1) - 1
                If (image(i, j).R / 3 + image(i, j).G / 3 + image(i, j).B / 3) >= 230 Then
                    image(i, j) = Color.White
                Else : image(i, j) = Color.Black
                End If
            Next
        Next
        Return image
    End Function
    Public Function LaserRecognitionColorThreshold(ByVal image(,) As Color, ByRef Threshold() As Integer)
        For i As Integer = 0 To image.GetLength(0) - 1
            For j As Integer = 0 To image.GetLength(1) - 1
                If image(i, j).R >= Threshold(0) And image(i, j).G <= Threshold(1) + Threshold(3) And image(i, j).G >= Threshold(1) - Threshold(3) And image(i, j).B <= Threshold(2) + Threshold(3) And image(i, j).B >= Threshold(2) - Threshold(3) Then
                    image(i, j) = Color.White
                Else : image(i, j) = Color.Black
                End If
            Next
        Next
        Return image
    End Function
    Public Function LaserRecognitionBitMapThreshold(ByVal image(,) As Color, ByRef Threshold() As Integer)
        Dim imagetoget = New Bitmap(image.GetLength(0), image.GetLength(1))

        For i As Integer = 0 To image.GetLength(0) - 1
            For j As Integer = 0 To image.GetLength(1) - 1
                If image(i, j).R >= Threshold(0) And image(i, j).G <= Threshold(1) + Threshold(3) And image(i, j).G >= Threshold(1) - Threshold(3) And image(i, j).B <= Threshold(2) + Threshold(3) And image(i, j).B >= Threshold(2) - Threshold(3) Then
                    imagetoget.SetPixel(i, j, Color.White)
                Else : imagetoget.SetPixel(i, j, Color.Black)
                End If
            Next
        Next
        Return imagetoget
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

            For i As Integer = Area(0, 0) + 1 To Area(0, 1)
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

    Public Function TableOfPoints(ByVal image(,) As Color)
        Dim Area(1, 1) As Integer
        Area(0, 0) = 0
        Area(0, 1) = image.GetLength(0) - 1
        Area(1, 0) = 0
        Area(1, 1) = image.GetLength(1) - 1
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
        Dim bmpData As System.Drawing.Imaging.BitmapData = imageaanalyser.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, imageaanalyser.PixelFormat)
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
    Public Function AfficherImage(ByRef TextBox As TextBox, ByRef PictureBoxImage As PictureBox, ByRef PictureBoxBW As PictureBox, ByRef PictureBoxDrawing As PictureBox)
        ' Test de l'existance du fichier proposé
        Dim Drawing As Graphics
        Dim Path As String = TextBox.Text
        Dim SquareSize As Integer = 250
        Dim ImageAAnalyser(,) As Color = Nothing
        Dim ImageAAfficher(,) As Color = Nothing
        If IO.File.Exists(Path) Then

            
            ImageAAnalyser = GenererMatriceFromJPGFastOne(Path)
            ImageAAfficher = dll.agrandissementaupproche(SquareSize, SquareSize, ImageAAnalyser)
            If Not (ImageAAnalyser Is Nothing) Then

                Dim image = New Bitmap(ImageAAfficher.GetLength(0), ImageAAfficher.GetLength(1))

                For colonne As Integer = 0 To PictureBoxImage.Width - 1
                    For ligne As Integer = 0 To PictureBoxImage.Height - 1
                        image.SetPixel(colonne, ligne, ImageAAfficher(colonne, ligne))
                    Next
                Next

                PictureBoxImage.Image = image
                PictureBoxBW.Image = LaserRecognitionBitMap(dll.agrandissementaupproche(SquareSize, SquareSize, ImageAAnalyser))
                Drawing = PictureBoxDrawing.CreateGraphics
                Drawing.DrawLines(BlackPen, TableOfPoints(LaserRecognitionColor(dll.agrandissementaupproche(SquareSize, SquareSize, ImageAAnalyser))))
            End If
        Else
            MsgBox("Problème : le fichier à lire n'existe pas ! A vérifier")
        End If
        Return ImageAAnalyser
    End Function
    Public Function AfficherImageWithThreshold(ByRef TextBox As TextBox, ByRef PictureBoxImage As PictureBox, ByRef PictureBoxBW As PictureBox, ByRef PictureBoxDrawing As PictureBox, ByRef Threshold() As Integer)
        ' Test de l'existance du fichier proposé
        Dim Drawing As Graphics
        Dim Path As String = TextBox.Text
        Dim SquareSize As Integer = 250
        Dim ImageAAnalyser(,) As Color = Nothing
        Dim ImageAAfficher(,) As Color = Nothing
        If IO.File.Exists(Path) Then


            ImageAAnalyser = GenererMatriceFromJPGFastOne(Path)
            ImageAAfficher = dll.agrandissementaupproche(SquareSize, SquareSize, ImageAAnalyser)
            If Not (ImageAAnalyser Is Nothing) Then

                Dim image = New Bitmap(ImageAAfficher.GetLength(0), ImageAAfficher.GetLength(1))

                For colonne As Integer = 0 To PictureBoxImage.Width - 1
                    For ligne As Integer = 0 To PictureBoxImage.Height - 1
                        image.SetPixel(colonne, ligne, ImageAAfficher(colonne, ligne))
                    Next
                Next

                PictureBoxImage.Image = image
                PictureBoxBW.Image = LaserRecognitionBitMapThreshold(dll.agrandissementaupproche(SquareSize, SquareSize, ImageAAnalyser), Threshold)
                Drawing = PictureBoxDrawing.CreateGraphics
                Drawing.DrawLines(BlackPen, TableOfPoints(LaserRecognitionColorThreshold(ImageAAfficher, Threshold)))
            End If
        Else
            MsgBox("Problème : le fichier à lire n'existe pas ! A vérifier")
        End If
        Return ImageAAnalyser
    End Function
    Public Function FindCentroid(ByVal image(,) As Color)
        Dim centroid(1) As Double
        Dim Area(1, 1) As Integer
        Dim nb As Double = 0
        Dim imageBW(,) As Color = LaserRecognitionColor(image)
        Area(0, 0) = 0
        Area(0, 1) = image.GetLength(0) - 1
        Area(1, 0) = 0
        Area(1, 1) = image.GetLength(1) - 1
        Dim table(,) As Double = SubPixel(imageBW, Area)
        For i As Integer = 0 To table.GetLength(0) - 1
            centroid(0) += table(i, 0)
            centroid(1) += table(i, 1)
            nb += 1
        Next
        centroid(0) /= nb
        centroid(1) /= nb
        Return centroid
    End Function
    Public Function CameraToObjet(ByVal image(,) As Color, ByVal Distance As Double, ByRef Angle As Double)
        Dim Area(1, 1) As Integer
        Area(0, 0) = 0
        Area(0, 1) = image.GetLength(0) - 1
        Area(1, 0) = 0
        Area(1, 1) = image.GetLength(1) - 1
        Dim PointCamera(,) As Double = SubPixel(LaserRecognitionColor(image), Area)
        Dim PointImage(PointCamera.GetLength(0) - 1, 1) As Double
        Dim TranslationVector(1) As Double
        TranslationVector(0) = Distance
        TranslationVector(1) = Distance / Math.Tan(Angle)
        For i As Integer = 0 To PointCamera.GetLength(0) - 1
            PointImage(i, 0) = PointCamera(i, 0) * Math.Cos(Angle) - PointCamera(i, 1) * Math.Sin(Angle) + TranslationVector(0)
            PointImage(i, 1) = PointCamera(i, 0) * Math.Sin(Angle) + PointCamera(i, 1) * Math.Cos(Angle) + TranslationVector(1)
        Next
        Return PointImage
    End Function
    Public Function GetImageFromFolder(ByVal PathName As String)
        Dim result As IEnumerable
        result = IO.Directory.GetFiles(PathName)
        Dim a As String = result(2)
        Return result
    End Function
End Class
