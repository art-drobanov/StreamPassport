Imports System.IO

Public Interface IStreamPassport
    ReadOnly Property Ext As String
    ReadOnly Property OKMarker As String
    ReadOnly Property CorruptedMarker As String
    ReadOnly Property TextMarker As String
    ReadOnly Property ProcessingMarker As String
    Property ID As String
    Property HASH As String
    Property StreamSize As String
    Property Total As String
    Function CreatePassport(id As String, stream As Stream) As IStreamPassport
    Function LoadPassport(stream As Stream) As IStreamPassport
    Function Serialize() As String
    Sub Deserialize(objJson As String)
    Function IsValid() As Boolean
    Function Compare(sprt As IStreamPassport, Optional noTotalHashCheck As Boolean = False) As Boolean
    Function ToString() As String
    Function ToText() As String
End Interface
