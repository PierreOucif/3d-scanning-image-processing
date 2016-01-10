Public Class Form1
    Dim ImagePorcessing As New ImagePorcessing
    Dim dll As New DimensionnementImage.GestionImage
    Dim Drawing As Graphics
    Dim BlackPen As New Pen(Color.Black)
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ImagePorcessing.AfficherImage(TextBox1, PictureBox1, PictureBox2, PictureBox3)
    End Sub
    

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ImagePorcessing.AfficherImage(TextBox2, PictureBox4, PictureBox5, PictureBox6)
    End Sub

 
End Class

