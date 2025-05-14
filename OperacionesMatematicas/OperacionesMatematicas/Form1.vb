Imports System.IO

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Configura el parser antes de usarlo
        MyParser.Setup()

        ' Intenta parsear la entrada de texto
        If (MyParser.Parse(New StringReader(RichTextBox1.Text))) Then
            ' Ejecuta las acciones y obtiene el resultado
            Dim resultado As Integer
            resultado = accion(MyParser.Root, Nothing)
            ' Muestra el resultado en el RichTextBox
            RichTextBox1.Text = resultado.ToString()
        Else
            MessageBox.Show("Expresión inválida o error de sintaxis.")
        End If
    End Sub

    Protected Function accion(ByVal Root As GOLD.Reduction, ByVal obj As Object) As Object
        Select Case Root.Parent.TableIndex

            Case ProductionIndex.Expresion_Plus
                Dim izquierda As Integer = Integer.Parse(accion(Root(0).Data, Nothing).ToString())
                Dim derecha As Integer = Integer.Parse(accion(Root(2).Data, Nothing).ToString())
                Return izquierda + derecha

            Case ProductionIndex.Expresion_Minus
                Dim izquierda As Integer = Integer.Parse(accion(Root(0).Data, Nothing).ToString())
                Dim derecha As Integer = Integer.Parse(accion(Root(2).Data, Nothing).ToString())
                Return izquierda - derecha

            Case ProductionIndex.Expresion
                Return accion(Root(0).Data, Nothing)

            Case ProductionIndex.Termino_Times
                Dim izquierda As Integer = Integer.Parse(accion(Root(0).Data, Nothing).ToString())
                Dim derecha As Integer = Integer.Parse(accion(Root(2).Data, Nothing).ToString())
                Return izquierda * derecha

            Case ProductionIndex.Termino_Div
                Dim izquierda As Integer = Integer.Parse(accion(Root(0).Data, Nothing).ToString())
                Dim derecha As Integer = Integer.Parse(accion(Root(2).Data, Nothing).ToString())
                Return izquierda \ derecha ' División entera

            Case ProductionIndex.Termino
                Return accion(Root(0).Data, Nothing)

            Case ProductionIndex.Factor_Lparen_Rparen
                Return accion(Root(1).Data, Nothing)

            Case ProductionIndex.Factor
                Return accion(Root(0).Data, Nothing)

            Case ProductionIndex.Numero_Digit
                Return Integer.Parse(accion(Root(0).Data, Nothing).ToString() & Root(1).Data.ToString())

            Case ProductionIndex.Numero_Digit2
                Return Integer.Parse(Root(0).Data.ToString())

            Case Else
                MessageBox.Show("Producción no manejada: " & Root.Parent.TableIndex.ToString())
                Return 0

        End Select
    End Function




    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged

    End Sub
End Class
