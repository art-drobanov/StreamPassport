Imports System.IO
Imports StreamPassport

Public Interface IStreamPassport
    ReadOnly Property CorruptedMarker As String
    ReadOnly Property Ext As String
    Property ID As String
    ReadOnly Property OKMarker As String
    Property HASH As String
    Property StreamSize As String
    ReadOnly Property TextMarker As String
    Property Total As String
    Sub Deserialize(objJson As String)
    Function Compare(sprt As IStreamPassport, noTotalHash As Boolean) As Boolean
    Function CreatePassport(id As String, stream As Stream) As IStreamPassport
    Function IsValid() As Boolean
    Function LoadPassport(stream As Stream) As IStreamPassport
    Function Serialize() As String
    Function ToString() As String
    Function ToText() As String
End Interface
