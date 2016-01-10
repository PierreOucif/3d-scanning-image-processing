Public Class Form1
    Dim ImagePorcessing As New ImagePorcessing
    Dim dll As New DimensionnementImage.GestionImage
    Dim Drawing As Graphics
    Dim BlackPen As New Pen(Color.Black)
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        
        ' Test de l'existance du fichier proposé
        Dim Path As String = TextBox1.Text
        If IO.File.Exists(Path) Then
            ' Instantiation du chrono pour comparaisons

            Dim ImageAAnalyser(,) As Color = Nothing

            ImageAAnalyser = ImagePorcessing.GenererMatriceFromJPGFastOne(Path)

            If Not (ImageAAnalyser Is Nothing) Then
                ' Format and display the TimeSpan value.
               

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
    

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        ' Test de l'existance du fichier proposé
        Dim Path As String = TextBox2.Text
        If IO.File.Exists(Path) Then
            ' Instantiation du chrono pour comparaisons

            Dim ImageAAnalyser(,) As Color = Nothing
            Dim ImageAAfficher(,) As Color = Nothing
            ImageAAnalyser = ImagePorcessing.GenererMatriceFromJPGFastOne(Path)
            ImageAAfficher = dll.agrandissementaupproche(500, 500, ImageAAfficher)
            If Not (ImageAAnalyser Is Nothing) Then
                ' Format and display the TimeSpan value.


                Dim image = New Bitmap(ImageAAfficher.GetLength(0), ImageAAfficher.GetLength(1))
                ' Utilisation de la solution GetPixel qui est très très lente...
                For colonne As Integer = 0 To PictureBox2.Width - 1
                    For ligne As Integer = 0 To PictureBox2.Height - 1
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
End Class

