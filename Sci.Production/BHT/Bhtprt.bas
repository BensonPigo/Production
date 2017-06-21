Attribute VB_Name = "ModuleBhtprt"
'************************************************************************/
'*                                                                      */
'*      Ir Transfer Utility C for BHT-6000 and later                    */
'*                                                                      */
'*      File name       : It3cw32.bas                                   */
'*      Function        : Error Code and API proto-type definition      */
'*                                                                      */
'*      Edition History                                                 */
'*      Version   Date      Comments                            Name    */
'*      --------  --------  ----------------------------------- ------  */
'*      1.02.00   97. 7. 7  Original                            YAMA    */
'*      1.03.00   97. 9.18  Add AbortIt3c proto-type            YAMA    */
'*                          Add new error (Er_FNAMCHECKER)              */
'*      1.04.00   97.11.10  Correct comments (not program)      YAMA    */
'*      2.00.00   04.01.27  Add YModem Protocol                 YSN     */
'*                          Add Select Protocol Button                  */
'*      2.01.00   04.03.01  Add two declare statements          NODA    */
'*                                                                      */
'*                              (C) Copyright 1997 DENSO CORPORATION    */
'************************************************************************/
Option Explicit

'-------------------------------------------------------
'   Bhtprt API Declare
'-------------------------------------------------------
' For all protocols
Declare Function ExecProtocol Lib "Bhtprtd.dll" (ByVal hWnd As Long, ByVal Param As String, ByVal TransferFileName As String, ByVal ProtocolType As Long) As Long
Declare Function GetProtocolDllVersion Lib "Bhtprtd.dll" (ByVal Param As String, ByVal ProtocolType As Long) As String
Declare Function AbortProtocol Lib "Bhtprtd.dll" (ByVal Port As Long) As Long
Declare Function GetSupportProtocolNum Lib "Bhtprtd.dll" () As Long
Declare Function GetSupportProtocolList Lib "Bhtprtd.dll" (ByVal Index As Long, ByVal Outline As String, ByRef Protocol As Long, ByVal DllName As String, ByVal Version As String) As Long
' For BHT-Ir protocol
Declare Function ExecIt3c Lib "Bhtprtd.dll" (ByVal hWnd As Long, ByVal Param As String, ByVal TransferFileName As String) As Long
Declare Function GetIt3cDllVersion Lib "Bhtprtd.dll" (ByVal Param As String) As String
Declare Function AbortIt3c Lib "Bhtprtd.dll" () As Long

'-------------------------------------------------------
'   Windows API Declare
'-------------------------------------------------------
Declare Function EnableWindow Lib "user32" (ByVal hWnd As Long, ByVal fEnable As Long) As Long
Declare Function GetDesktopWindow Lib "user32" () As Long

'-------------------------------------------------------
'   It3cw32 API return codes
'-------------------------------------------------------
Public Const Er_NOERROR         As Integer = 0  ' Communication ended normally.
Public Const Er_NOFILE          As Integer = 1  ' Designated file not found.
Public Const Er_FILENAME        As Integer = 2  ' File name entered in wrong format.
Public Const Er_RECORDS         As Integer = 3  ' Number of records exceed 32767.
Public Const Er_DIGITS          As Integer = 4  ' Field length is out of range.
Public Const Er_ITEMS           As Integer = 5  ' Number of field length is out of range.
Public Const Er_SUMOFDIGITS     As Integer = 6  ' Record length is out of range.
Public Const Er_PROGWITHPARAM   As Integer = 7  ' Parameter mismatch.
Public Const Er_DATWITHOUTPARAM As Integer = 8  ' Field length not found.
Public Const Er_UNDEFOPT        As Integer = 9  ' Option mismatch.

Public Const Er_INVALIDPROTOCOL As Integer = 30 ' Invalid protocol

Public Const Er_TXTIMEOUT       As Integer = 51 ' Communication error.(Tx time out)
Public Const Er_RXTIMEOUT       As Integer = 52 ' Communication error.(Rx time out)
Public Const Er_TXNAKEXPIRED    As Integer = 53 ' Communication error.(Rx NAK counter expired)
Public Const Er_RXNAKEXPIRED    As Integer = 54 ' Communication error.(Rx NAK counter expired)
Public Const Er_RECEIVEEOT      As Integer = 55 ' Communication error.(Receive EOT)

Public Const Er_TRANSMITTING    As Integer = 60 ' Now transmitting.
Public Const Er_RECEIVING       As Integer = 61 ' Now receiving.

Public Const Er_HEADINGTEXT     As Integer = 70 ' Illegal heading text format.
Public Const Er_PATHNOTFOUND    As Integer = 71 ' Path not found.
Public Const Er_DISKFULL        As Integer = 72 ' Disk memory full.
Public Const Er_NOTIMER         As Integer = 74 ' No available timer is left.
Public Const Er_NOCOMPORT       As Integer = 75 ' Designated Com Port cannot open.
Public Const Er_RECORDFORMAT    As Integer = 76 ' Illegal record format.
Public Const Er_FNAMCHECKER     As Integer = 77 ' Wrong file received.

Public Const Er_ABORT           As Integer = 90 ' Aborted by break key.
Public Const Er_GENERALFAIL     As Integer = 99 ' General failure.

'-------------------------------------------------------
'   constant
'-------------------------------------------------------
Public Const C_SEND     As Integer = 1
Public Const C_RCV      As Integer = 2

Public Const PRTCL_UNKNOWN As Integer = 0
Public Const PRTCL_YMODEMBATCH As Integer = 2
Public Const PRTCL_BHTIR As Integer = 3

Sub vbBhtTransExec(Options As String)

    Dim CmdLine     As String

    ' Set options
    CmdLine = "BhtTrans.exe " & Options

    ' Execute BhtTrans.exe
    On Error Resume Next

    Call Shell(CmdLine, vbNormalFocus)
    Dim Error As Integer
    Error = Err
    If (Err <> 0) Then
        Beep
        Call MsgBox("BhtTrans.exe is not found.", vbExclamation, "Shell execution error")
    End If

    On Error GoTo 0

    Exit Sub

End Sub


'-------------------------------------------------------
'  For any protocol...
'-------------------------------------------------------
Function vbGetProtocolDllVersion(ByVal ProtocolType As Long) As String
    
    Dim VersionData     As String
    Dim CntNull         As Integer

    On Error Resume Next

    ' Get version information
    VersionData = String(100, " ")
    Call GetProtocolDllVersion(VersionData, ProtocolType)
    If (Err <> 0) Then
        vbGetProtocolDllVersion = ""
        Exit Function
    End If

    CntNull = InStr(VersionData, Chr(&H0))
    vbGetProtocolDllVersion = Left(VersionData, CntNull - 1)

    On Error GoTo 0

    Exit Function

End Function

Function vbExecProtocol(hWnd As Long, Options As String, TransferFile As String, RcvMode As Integer, ProtocolType As Integer) As Long

    Dim Param           As String
    Dim FileNameBuf     As String
    Dim CntNull         As Integer
    Dim Ret             As Long

    ' Set option string
    If (RcvMode = C_RCV) Then
        Param = Options & " +R"
    Else
        Param = Options & " -R"
    End If
    Param = Param & Chr(&H0)
    FileNameBuf = String(100, " ")

    On Error Resume Next

    ' Call API
    Ret = ExecProtocol(hWnd, Param, FileNameBuf, ProtocolType)
    vbExecProtocol = Ret
    If (Ret = 0) Then
        CntNull = InStr(FileNameBuf, Chr(&H0))
        TransferFile = Left(FileNameBuf, CntNull - 1)
    End If

    On Error GoTo 0

    Exit Function

End Function

Public Function vbAbortProtocol(ByVal Port As Long) As Long

    Dim Ret As Long

    On Error Resume Next

    ' Call API
    vbAbortProtocol = AbortProtocol(Port)

    On Error GoTo 0

    Exit Function

End Function

'-------------------------------------------------------
'  For BHT-Ir protocol...
'-------------------------------------------------------
Function vbGetIt3cDllVersion() As String

    Dim VersionData     As String
    Dim CntNull         As Integer

    On Error Resume Next

    ' Get version information
    VersionData = String(100, " ")
    Call GetIt3cDllVersion(VersionData)
    If (Err <> 0) Then
        vbGetIt3cDllVersion = ""
        Exit Function
    End If

    CntNull = InStr(VersionData, Chr(&H0))
    vbGetIt3cDllVersion = Left(VersionData, CntNull - 1)

    On Error GoTo 0

End Function

Function vbExecIt3c(hWnd As Long, Options As String, TransferFile As String, RcvMode As Integer) As Integer

    Dim Param           As String
    Dim FileNameBuf     As String
    Dim CntNull         As Integer
    Dim Ret             As Long

    ' Set option string
    If (RcvMode = C_RCV) Then
        Param = Options & " +R"
    Else
        Param = Options & " -R"
    End If
    Param = Param & Chr(&H0)
    FileNameBuf = String(100, " ")

    On Error Resume Next

    ' Call API
    Ret = ExecIt3c(hWnd, Param, FileNameBuf)
    If (Err <> 0) Then
        vbExecIt3c = -1
    Else
        vbExecIt3c = Ret
        CntNull = InStr(FileNameBuf, Chr(&H0))
        TransferFile = Left(FileNameBuf, CntNull - 1)
    End If

    On Error GoTo 0

End Function

Public Function vbAbortIt3c() As Integer
    Dim Ret             As Long

    On Error Resume Next

    ' Call API
    Ret = AbortIt3c()
    If (Err <> 0) Then
        vbAbortIt3c = -1
    Else
        vbAbortIt3c = Ret
    End If

    On Error GoTo 0
End Function

