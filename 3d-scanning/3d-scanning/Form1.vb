Public Class Form1
    
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim ImagePorcessing As New ImagePorcessing
        Dim dll As New DimensionnementImage.GestionImage
        Dim Drawing As Graphics
        Dim BlackPen As New Pen(Color.Black)
        ' Test de l'existance du fichier proposé
        Dim Path As String = TextBox1.Text
        If IO.File.Exists(Path) Then
            ' Instantiation du chrono pour comparaisons
            Dim stopWatch As New Stopwatch()
            Dim ImageAAnalyser(,) As Color = Nothing


            stopWatch.Start()
            ImageAAnalyser = GenererMatriceFromJPGFastOne(Path)

                
            stopWatch.Stop()

            If Not (ImageAAnalyser Is Nothing) Then
                ' Format and display the TimeSpan value.
                Dim ts As TimeSpan = stopWatch.Elapsed
                Dim elapsedTime As String = String.Format("{0:00}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds)


                Dim image = New Bitmap(ImageAAnalyser.GetLength(0), ImageAAnalyser.GetLength(1))
                ' Utilisation de la solution GetPixel qui est très très lente...
                For colonne As Integer = 0 To PictureBox1.Width - 1
                    For ligne As Integer = 0 To PictureBox1.Height - 1
                        image.SetPixel(colonne, ligne, ImageAAnalyser(colonne, ligne))
                    Next
                Next

                Me.PictureBox1.Image = image
                Me.PictureBox2.Image = ImagePorcessing.LaserRecognitionBitMap(dll.agrandissementaupproche(500, 500, ImageAAnalyser))
                Drawing = PictureBox3.CreateGraphics
                Drawing.DrawLines(BlackPen, ImagePorcessing.TableOfPoints(ImagePorcessing.LaserRecognitionColor(dll.agrandissementaupproche(500, 500, ImageAAnalyser))))
            End If
        Else
            MsgBox("Problème : le fichier à lire n'existe pas ! A vérifier")
        End If

        

    End Sub

    Function GenererMatriceFromJPGFastOne(ByVal Path As String) As Color(,) ' Génération d'une matrice d'objets color (3 propriétés importantes : R, G et B)
        ' Solution à privilégier !!!!

        Dim imageaanalyser = New Bitmap(Path)
        Dim Matrice(imageaanalyser.Width - 1, imageaanalyser.Height - 1) As Color

        ' Vérouillage mémoire des octets de l'image
        Dim rect As New Rectangle(0, 0, imageaanalyser.Width, imageaanalyser.Height)

        Dim bmpData As System.Drawing.Imaging.BitmapData = imageaanalyser.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadOnly, imageaanalyser.PixelFormat)

        ' Détermination de l'addresse mémorie de la première ligne de l'image
        Dim ptr As IntPtr = bmpData.Scan0

        ' Création d'un vecteur d'octets, codant les couleurs des pixels de l'image
        Dim bytes As Integer = Math.Abs(bmpData.Stride) * imageaanalyser.Height
        Dim rgbValues(bytes - 1) As Byte

        ' Copie des octets dans le tableau
        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)

        ' Remplissage de la matrice à partir de la liste d'octets
        Dim ligne As Integer = 0
        Dim colonne As Integer = 0

        For compteur As Integer = 0 To rgbValues.Length - 1 Step 3
            ' On prend des paquets de 3 octets pour la codification RGB
            Matrice(colonne, ligne) = Color.FromArgb(rgbValues(compteur + 2), rgbValues(compteur + 1), rgbValues(compteur))

            ' Gestion de la position dans l'image
            colonne += 1
            If colonne > imageaanalyser.Width - 1 Then
                colonne = 0
                ligne += 1
            End If
        Next

        ' On relache l'espace mémoire alloué à l'image
        imageaanalyser.UnlockBits(bmpData)

        Return Matrice
    End Function
    Function GenererMatriceFromJPGSlowOne(ByVal Path As String) As Color(,)
        Dim imageaanalyser = New Bitmap(Path)
        Dim Matrice(imageaanalyser.Width - 1, imageaanalyser.Height - 1) As Color

        ' Utilisation de la solution GetPixel qui est très très lente...
        For colonne As Integer = 0 To UBound(Matrice, 2)
            For ligne As Integer = 0 To UBound(Matrice, 1)
                Matrice(ligne, colonne) = imageaanalyser.GetPixel(ligne, colonne)
            Next
        Next

        Return Matrice
    End Function

    
End Class

