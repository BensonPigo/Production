/**************************************************************************
;***                                                                    ***
;***    Product name    : BHT Protocol                                  ***
;***    File name       : BhtpApi.h                                     ***
;***    Function        : Error code and API prototype definition       ***
;***                                                                    ***
;***    Edition History                                                 ***
;***    Version  Date       Comments                            Name    ***
;***    -------- ---------- ----------------------------------- ------  ***
;***    1.00.00  03/02/2004 Original                            NODA    ***
;***                                                                    ***
;***                   (C) Copyright 2002-2004 DENSO WAVE INCORPORATED  ***
;*************************************************************************/

#ifndef _BHTPRTAPI_H_
#define _BHTPRTAPI_H_

#ifdef __cplusplus
extern "C" {
#endif  /* __cplusplus */

#ifndef BOOL
typedef int BOOL;
#define TRUE 1
#define FALSE 0
#endif

#ifndef PASCAL
#define PASCAL _pascal
#endif

#ifndef FAR
#define FAR     _far
#endif



/*==============================================================*/
/*      Error code                                              */
/*==============================================================*/
#ifndef ENUM_ERRCODE
#define ENUM_ERRCODE

typedef enum errorcode {
        Er_NOERROR         =  0, /* Communication ended normally.       */
        Er_NOFILE          =  1, /* Designated file not found.          */
        Er_FILENAME        =  2, /* File name entered in wrong format.  */
        Er_RECORDS         =  3, /* Number of records exceed 32767.     */
        Er_DIGITS          =  4, /* Field length is out of range.       */
        Er_ITEMS           =  5, /* Number of field length is out of range.*/
        Er_SUMOFDIGITS     =  6, /* Record length is out of range.      */
        Er_PROGWITHPARAM   =  7, /* Parameter mismatch.                 */
        Er_DATWITHOUTPARAM =  8, /* Field length not found.             */
        Er_UNDEFOPT        =  9, /* Option mismatch.                    */

        Er_INVALIDPROTOCOL = 30,    /* Invalid protocol */

        Er_TXTIMEOUT       = 51, /* Communication error.(Tx time out)   */
        Er_RXTIMEOUT       = 52, /* Communication error.(Rx time out)   */
        Er_TXNAKEXPIRED    = 53, /* Communication error.(Rx NAK counter expired) */
        Er_RXNAKEXPIRED    = 54, /* Communication error.(Rx NAK counter expired) */
        Er_RECEIVEEOT      = 55, /* Communication error.(Receive EOT)   */

        Er_TRANSMITTING    = 60, /* Now transmitting.                   */
        Er_RECEIVING       = 61, /* Now receiving.                      */

        Er_HEADINGTEXT     = 70, /* Illegal heading text format.        */
        Er_PATHNOTFOUND    = 71, /* Path not found.                     */
        Er_DISKFULL        = 72, /* Disk memory full.                   */
        Er_NOTIMER         = 74, /* No available timer is left.         */
        Er_NOCOMPORT       = 75, /* Designated Com Port cannot open.    */
        Er_RECORDFORMAT    = 76, /* Illegal record format.              */
        Er_FNAMCHECKER     = 77, /* Wrong file received.                */

        Er_ABORT           = 90, /* Aborted by break key.               */
        Er_GENERALFAIL     = 99, /* General failure.                    */
} Er;

#endif

/*==============================================*/
/*      Constant                                */
/*==============================================*/
/* Protocols */
#define PRTCL_NOTSPECIFIED		(0)		// Not specified
#define PRTCL_YMODEMBATCH		(2)		// YMODEM-Batch
#define PRTCL_BHTIR				(3)		// BHT-Ir
/* Abort */
#define ABORT_ALL				(0)		// Abort all of communication ports.

/*==============================================*/
/*      Prototype                               */
/*==============================================*/
Er    WINAPI ExecProtocol(HWND hMWnd, char *parameters, char *TransFileName, int Protocol);
char* WINAPI GetProtocolDllVersion(char *Version, int Protocol);
int   WINAPI AbortProtocol(int Port);
int   WINAPI GetSupportProtocolNum(void);
int   WINAPI GetSupportProtocolList(int Index, char* Outline, int* Protocol, char* DllName, char* Version);
/* For compatibility to It3cw32.dll */
Er    WINAPI ExecIt3c(HWND hMWnd, char *parameters, char *TransFileName);
char* WINAPI GetIt3cDllVersion(char *Version);
int   WINAPI AbortIt3c(void);

#ifdef __cplusplus
}
#endif

#endif /* #ifndef _BHTPRTAPI_H_ */

