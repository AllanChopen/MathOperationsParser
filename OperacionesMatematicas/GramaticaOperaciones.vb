﻿'Generated by the GOLD Parser Builder

Option Explicit On
Option Strict Off

Imports System.IO
Imports System.Windows.Forms;


Module MyParser
    Private Parser As New GOLD.Parser

    Private Enum SymbolIndex
        [Eof] = 0                                 ' (EOF)
        [Error] = 1                               ' (Error)
        [Whitespace] = 2                          ' Whitespace
        [Minus] = 3                               ' '-'
        [Lparen] = 4                              ' '('
        [Rparen] = 5                              ' ')'
        [Times] = 6                               ' '*'
        [Div] = 7                                 ' '/'
        [Plus] = 8                                ' '+'
        [Digit] = 9                               ' digit
        [Expresion] = 10                          ' <Expresion>
        [Factor] = 11                             ' <Factor>
        [Numero] = 12                             ' <Numero>
        [Termino] = 13                            ' <Termino>
    End Enum

    Private Enum ProductionIndex
        [Numero_Digit] = 0                        ' <Numero> ::= <Numero> digit
        [Numero_Digit2] = 1                       ' <Numero> ::= digit
        [Expresion_Plus] = 2                      ' <Expresion> ::= <Expresion> '+' <Termino>
        [Expresion_Minus] = 3                     ' <Expresion> ::= <Expresion> '-' <Termino>
        [Expresion] = 4                           ' <Expresion> ::= <Termino>
        [Termino_Times] = 5                       ' <Termino> ::= <Termino> '*' <Factor>
        [Termino_Div] = 6                         ' <Termino> ::= <Termino> '/' <Factor>
        [Termino] = 7                             ' <Termino> ::= <Factor>
        [Factor_Lparen_Rparen] = 8                ' <Factor> ::= '(' <Expresion> ')'
        [Factor] = 9                              ' <Factor> ::= <Numero>
    End Enum

    Public Program As Object     'You might derive a specific object

    Public Sub Setup()
        'This procedure can be called to load the parse tables. The class can
        'read tables using a BinaryReader.
        
        Parser.LoadTables(Path.Combine(Application.StartupPath, "grammar.egt"))
    End Sub
    
    Public Function Parse(ByVal Reader As TextReader) As Boolean
        'This procedure starts the GOLD Parser Engine and handles each of the
        'messages it returns. Each time a reduction is made, you can create new
        'custom object and reassign the .CurrentReduction property. Otherwise, 
        'the system will use the Reduction object that was returned.
        '
        'The resulting tree will be a pure representation of the language 
        'and will be ready to implement.

        Dim Response As GOLD.ParseMessage
        Dim Done as Boolean                  'Controls when we leave the loop
        Dim Accepted As Boolean = False      'Was the parse successful?

        Accepted = False    'Unless the program is accepted by the parser

        Parser.Open(Reader)
        Parser.TrimReductions = False  'Please read about this feature before enabling  

        Done = False
        Do Until Done
            Response = Parser.Parse()

            Select Case Response              
                Case GOLD.ParseMessage.LexicalError
                    'Cannot recognize token
                    Done = True

                Case GOLD.ParseMessage.SyntaxError
                    'Expecting a different token
                    Done = True

                Case GOLD.ParseMessage.Reduction
                    'Create a customized object to store the reduction
                    .CurrentReduction = CreateNewObject(Parser.CurrentReduction)

                Case GOLD.ParseMessage.Accept
                    'Accepted!
                    'Program = Parser.CurrentReduction  'The root node!                 
                    Done = True
                    Accepted = True

                Case GOLD.ParseMessage.TokenRead
                    'You don't have to do anything here.

                Case GOLD.ParseMessage.InternalError
                    'INTERNAL ERROR! Something is horribly wrong.
                    Done = True

                Case GOLD.ParseMessage.NotLoadedError
                    'This error occurs if the CGT was not loaded.                   
                    Done = True

                Case GOLD.ParseMessage.GroupError 
                    'COMMENT ERROR! Unexpected end of file
                    Done = True
            End Select
        Loop

        Return Accepted
    End Function

    Private Function CreateNewObject(Reduction as GOLD.Reduction) As Object
        Dim Result As Object = Nothing

        With Reduction
            Select Case .Parent.TableIndex                        
                Case ProductionIndex.Numero_Digit                 
                    ' <Numero> ::= <Numero> digit 

                Case ProductionIndex.Numero_Digit2                 
                    ' <Numero> ::= digit 

                Case ProductionIndex.Expresion_Plus                 
                    ' <Expresion> ::= <Expresion> '+' <Termino> 

                Case ProductionIndex.Expresion_Minus                 
                    ' <Expresion> ::= <Expresion> '-' <Termino> 

                Case ProductionIndex.Expresion                 
                    ' <Expresion> ::= <Termino> 

                Case ProductionIndex.Termino_Times                 
                    ' <Termino> ::= <Termino> '*' <Factor> 

                Case ProductionIndex.Termino_Div                 
                    ' <Termino> ::= <Termino> '/' <Factor> 

                Case ProductionIndex.Termino                 
                    ' <Termino> ::= <Factor> 

                Case ProductionIndex.Factor_Lparen_Rparen                 
                    ' <Factor> ::= '(' <Expresion> ')' 

                Case ProductionIndex.Factor                 
                    ' <Factor> ::= <Numero> 

            End Select
        End With     

        Return Result
    End Function
End Module
