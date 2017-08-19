Imports System
Imports System.Xml
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Namespace EValuate
    Public Class EvalRunTime
        Private _connectionString As String
        Public Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
            Set(ByVal value As String)
                _connectionString = value
            End Set
        End Property
        Private _companyId As String
        Public Property CompanyId() As String
            Get
                Return _companyId
            End Get
            Set(ByVal value As String)
                _companyId = value
            End Set
        End Property
        Private _companyHeader As String
        Public Property CompanyHeader() As String
            Get
                Return _companyHeader
            End Get
            Set(ByVal value As String)
                _companyHeader = value
            End Set
        End Property
        Private _year As String
        Public Property Year() As String
            Get
                Return _year
            End Get
            Set(ByVal value As String)
                _year = value
            End Set
        End Property
        Private _dictGlobalValues As System.Collections.Generic.Dictionary(Of String, String)
        Public Property DictGlobalValues() As System.Collections.Generic.Dictionary(Of String, String)
            Get
                Return _dictGlobalValues
            End Get
            Set(ByVal value As System.Collections.Generic.Dictionary(Of String, String))
                _dictGlobalValues = value
            End Set
        End Property
        Private _errorLog As New System.Text.StringBuilder
        Public Property ErrorLog() As System.Text.StringBuilder
            Get
                Return _errorLog
            End Get
            Set(ByVal value As System.Text.StringBuilder)
                _errorLog = value
            End Set
        End Property
        Private _TraceLog As New System.Text.StringBuilder
        Public Property TraceLog() As System.Text.StringBuilder
            Get
                Return _TraceLog
            End Get
            Set(ByVal value As System.Text.StringBuilder)
                _TraceLog = value
            End Set
        End Property
        Private _dictRulesResult As System.Collections.Generic.Dictionary(Of String, String)
        Public Property DictRulesResult() As System.Collections.Generic.Dictionary(Of String, String)
            Get
                Return _dictRulesResult
            End Get
            Set(ByVal value As System.Collections.Generic.Dictionary(Of String, String))
                _dictRulesResult = value
            End Set
        End Property
        Public Sub New()
        End Sub
        Public Sub New(ByVal compId As String, ByVal compHeader As String, ByVal connectionStr As String, ByVal yr As String)
            ConnectionString = connectionStr
            CompanyId = compId
            CompanyHeader = compHeader
            Year = yr
            SetGlobalValues()
        End Sub
        Private Function GetDBParameter(ByVal ParamName As String, ByVal dataType As String, ByVal ParameterValue As String, ByVal Direction As String) As SqlParameter
            Dim dbParam As New SqlParameter
            dbParam.ParameterName = "@" + ParamName
            Dim isNumeric As Boolean = False
            Dim isChar As Boolean = False
            GetParameterType(dataType, isNumeric, isChar)
            If Direction.ToUpper.Equals("IN") Then
                dbParam.Direction = ParameterDirection.Input
            ElseIf Direction.ToUpper.Equals("OUT") Then
                dbParam.Direction = ParameterDirection.Output
            ElseIf Direction.ToUpper.Equals("INOUT") Then
                dbParam.Direction = ParameterDirection.InputOutput
            End If
            If isNumeric = True Then
                dbParam.DbType = DbType.Int32
            ElseIf isChar = True Then
                dbParam.DbType = DbType.String
            ElseIf dataType.ToUpper.Equals("DATE") Then
                dbParam.DbType = DbType.Date
            ElseIf dataType.ToUpper.Equals("DATETIME") Then
                dbParam.DbType = DbType.DateTime
            ElseIf dataType.ToUpper.Equals("BOOLEAN") Then
                dbParam.DbType = DbType.Boolean
            ElseIf dataType.ToUpper.Equals("OBJECT") Then
                dbParam.DbType = DbType.Object
            End If
            dbParam.Value = ParameterValue
            Return dbParam
        End Function
        Private Sub GetParameterType(ByVal dataType As String, ByRef isNumeric As Boolean, ByRef isString As Boolean)
            If dataType.ToUpper().Equals("DOUBLE") Or dataType.ToUpper().Equals("INTEGER") Then
                isNumeric = True
            ElseIf dataType.ToUpper.Equals("STRING") Then
                isString = True
            End If
        End Sub
        Private Function GetItemValue(ByVal elementType As String, ByVal elementName As String, ByVal key As String)
            If Not DictGlobalValues Is Nothing AndAlso DictGlobalValues.ContainsKey(elementType + "~" + elementName + "~" + key) Then
                Return DictGlobalValues(elementType + "~" + elementName + "~" + key)
            Else
                Return ""
            End If
        End Function
        Private Function GetDataSetFromStoreProc(ByVal spName As String, ByVal parametercols As ArrayList) As DataTable
            Dim result As DataTable = New DataTable()
            Dim cmd_GetStatus As New SqlCommand
            Dim conObj As New SqlConnection(ConnectionString)
            Dim outParam As SqlParameter = Nothing
            Try

                If conObj.State = ConnectionState.Open Then conObj.Close()

                conObj.Open()

                cmd_GetStatus.Connection = conObj
                cmd_GetStatus.CommandText = spName
                cmd_GetStatus.CommandType = CommandType.StoredProcedure
                For Each parameter As SqlParameter In parametercols
                    Dim tempParam As New SqlParameter
                    tempParam.DbType = parameter.DbType
                    tempParam.Direction = parameter.Direction
                    tempParam.ForceColumnEncryption = parameter.ForceColumnEncryption
                    tempParam.IsNullable = parameter.IsNullable
                    tempParam.LocaleId = parameter.LocaleId
                    tempParam.Offset = parameter.Offset
                    tempParam.ParameterName = parameter.ParameterName
                    tempParam.Size = parameter.Size
                    tempParam.SqlDbType = parameter.SqlDbType
                    tempParam.SqlValue = parameter.SqlValue
                    tempParam.Value = parameter.Value
                    cmd_GetStatus.Parameters.Add(tempParam)
                Next

                Dim sqlresult As SqlDataReader = cmd_GetStatus.ExecuteReader()
                result.Load(sqlresult)
                Return result
            Catch ex As Exception

            Finally
                cmd_GetStatus.Dispose()
                conObj.Close()
            End Try
            Return Nothing

        End Function
        Private Sub SetGlobalValues()
            DictRulesResult = New System.Collections.Generic.Dictionary(Of String, String)()
            Dim arrParams As New ArrayList
            arrParams.Add(GetDBParameter("CompanyHeader", "String", CompanyHeader, "IN"))
            arrParams.Add(GetDBParameter("Year", "String", Year, "IN"))
            Dim dsGlobalValues As DataTable = GetDataSetFromStoreProc("pGetStandardFieldDetails", arrParams)
            If dsGlobalValues IsNot Nothing AndAlso dsGlobalValues.Rows.Count > 0 Then
                DictGlobalValues = New System.Collections.Generic.Dictionary(Of String, String)()
                For Each item As DataRow In dsGlobalValues.Rows
                    Dim stdFieldName As String
                    Dim stdFieldValue As String
                    stdFieldName = Convert.ToString(item("StandardFieldName"))
                    stdFieldValue = Convert.ToString(item("StandardValue"))
                    If Not String.IsNullOrWhiteSpace(stdFieldName) AndAlso Not DictGlobalValues.ContainsKey(stdFieldName) Then
                        DictGlobalValues.Add(stdFieldName, stdFieldValue)
                    End If
                Next
            End If
        End Sub

        Private Function GetPreviousYear() As String
            DictRulesResult = New System.Collections.Generic.Dictionary(Of String, String)()
            Dim arrParams As New ArrayList
            arrParams.Add(GetDBParameter("CompanyHeader", "String", CompanyHeader, "IN"))
            arrParams.Add(GetDBParameter("Year", "String", Year, "IN"))
            Dim dsGlobalValues As DataTable = GetDataSetFromStoreProc("pGetStandardFieldDetails", arrParams)
            If dsGlobalValues IsNot Nothing AndAlso dsGlobalValues.Rows.Count > 0 Then
                DictGlobalValues = New System.Collections.Generic.Dictionary(Of String, String)()
                For Each item As DataRow In dsGlobalValues.Rows
                    Dim stdFieldName As String
                    Dim stdFieldValue As String
                    stdFieldName = Convert.ToString(item("StandardFieldName"))
                    stdFieldValue = Convert.ToString(item("StandardValue"))
                    If Not String.IsNullOrWhiteSpace(stdFieldName) AndAlso Not DictGlobalValues.ContainsKey(stdFieldName) Then
                        DictGlobalValues.Add(stdFieldName, stdFieldValue)
                    End If
                Next
            End If
        End Function

        Private Function GetDouble(ByVal value As String) As Double
            Try
                Return Convert.ToDouble(value)
            Catch ex As Exception
                Return 0.00
            End Try
        End Function
        Private Sub BegBalanceCheck()
            Dim elementType As String = "Tranche"
            Dim elementName As String = "Class A Notes"
            If GetItemValue(elementType, elementName, "BegBalance") > GetDouble("30000") Then
                DictRulesResult.Add("BegBalanceCheck", "SUCCESS~" + GetItemValue(elementType, elementName, "BegBalance"))
            Else
                DictRulesResult.Add("BegBalanceCheck", "FAIL~" + GetItemValue(elementType, elementName, "BegBalance"))
            End If
        End Sub
        Private Sub LoanCountCheck()
            Dim elementType As String = "Tranche"
            Dim elementName As String = "Class A Notes"

            If elementName = "N" OrElse elementName = "Y" Then
                'SUCCESS
            End If

            If GetItemValue(elementType, elementName, "LoanCount") > "14000" Then
                DictRulesResult.Add("LoanCountCheck", "SUCCESS~" + GetItemValue(elementType, elementName, "LoanCount"))
            Else
                DictRulesResult.Add("LoanCountCheck", "FAIL~" + GetItemValue(elementType, elementName, "LoanCount"))
            End If
        End Sub

        Public Sub runRules()
            BegBalanceCheck()
            LoanCountCheck()
        End Sub
    End Class
End Namespace
