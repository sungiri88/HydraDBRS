using HGarb.Infrastructure;
using HGarb.Models;
using Microsoft.VisualBasic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGarb.Business.Rules
{
    public class BuildRule
    {
        private Business.RulesConfig rulesConfigBusiness = null;

        public BuildRule()
        {
            this.rulesConfigBusiness = new Business.RulesConfig();
        }
        public StringBuilder RuleCode { get; set; }
        public List<string> Operators { get; set; }
        public List<string> StandardFieldNames { get; set; }
        public List<String> MethodNames { get; set; }
        public bool ConstructRuleCode(string companyHeader)
        {
            try
            {
                this.MethodNames = new List<string>();
                this.StandardFieldNames = this.rulesConfigBusiness.LoadStandardFieldNames(companyHeader);
                this.Operators = new List<string>() { "PLUS", "MINUS", "MULTIPLY", "DIVIDE", "GS", "GE", "EQUALTO", "GREATERTHAN", "LESSERTHAN", "GREATERTHANEQUALTO", "LESSERTHANEQUALTO", "ORELSE", "ANDALSO", "GetDouble" };
                this.RuleCode = new StringBuilder();
                this.RuleCode.AppendLine("Imports System");
                this.RuleCode.AppendLine("Imports System.Xml");
                this.RuleCode.AppendLine("Imports System.Data");
                this.RuleCode.AppendLine("Imports System.Collections");
                this.RuleCode.AppendLine("Imports System.Data.SqlClient");
                this.RuleCode.AppendLine("Namespace EValuate");
                this.RuleCode.AppendLine("Public Class EvalRunTime ");
                this.RuleCode.AppendLine(@"Private _connectionString As String
                             Public Property ConnectionString() As String
                                Get
                                    Return _connectionString
                                End Get
                                Set(ByVal value As String)
                                     _connectionString = value
                                End Set
                              End Property");
                this.RuleCode.AppendLine(@"Private _companyId As String
                             Public Property CompanyId() As String
                                Get
                                    Return _companyId
                                End Get
                                Set(ByVal value As String)
                                     _companyId = value
                                End Set
                              End Property");
                this.RuleCode.AppendLine(@"Private _companyHeader As String
                             Public Property CompanyHeader() As String
                                Get
                                    Return _companyHeader
                                End Get
                                Set(ByVal value As String)
                                     _companyHeader = value
                                End Set
                              End Property");
                this.RuleCode.AppendLine(@"Private _year As String
                             Public Property Year() As String
                                Get
                                    Return _year
                                End Get
                                Set(ByVal value As String)
                                     _year = value
                                End Set
                              End Property");
                this.RuleCode.AppendLine(@"Private _dictGlobalValues As System.Collections.Generic.Dictionary(Of String, String)
                                            Public Property DictGlobalValues() As System.Collections.Generic.Dictionary(Of String, String)
                                            Get
                                                Return _dictGlobalValues
                                            End Get
                                            Set(ByVal value As System.Collections.Generic.Dictionary(Of String, String))
                                                _dictGlobalValues = value
                                            End Set
                                            End Property");
                this.RuleCode.AppendLine(@"Private _errorLog   As New System.Text.StringBuilder
                             Public Property ErrorLog  () As System.Text.StringBuilder
                                Get
                                    Return _errorLog  
                                End Get
                                Set(ByVal value As System.Text.StringBuilder)
                                     _errorLog   = value
                                End Set
                              End Property");
                this.RuleCode.AppendLine(@"Private _TraceLog As New System.Text.StringBuilder
                             Public Property TraceLog   () As System.Text.StringBuilder
                                Get
                                    Return _TraceLog  
                                End Get
                                Set(ByVal value As System.Text.StringBuilder)
                                     _TraceLog   = value
                                End Set
                              End Property");
                this.RuleCode.AppendLine(@"Private _dictRulesResult As System.Collections.Generic.Dictionary(Of String, String)
                                            Public Property DictRulesResult() As System.Collections.Generic.Dictionary(Of String, String)
                                            Get
                                                Return _dictRulesResult
                                            End Get
                                            Set(ByVal value As System.Collections.Generic.Dictionary(Of String, String))
                                                _dictRulesResult = value
                                            End Set
                                            End Property");


                this.RuleCode.AppendLine("Public Sub New()");
                this.RuleCode.AppendLine("End Sub");
                this.RuleCode.AppendLine("Public Sub New(ByVal compId As String, ByVal compHeader as String, ByVal connectionStr As String, ByVal yr As String)");
                this.RuleCode.AppendLine("ConnectionString = connectionStr");
                this.RuleCode.AppendLine("CompanyId = compId");
                this.RuleCode.AppendLine("CompanyHeader = compHeader");
                this.RuleCode.AppendLine("Year = yr");
                this.RuleCode.AppendLine("SetGlobalValues()");
                this.RuleCode.AppendLine("End Sub");

                this.RuleCode.AppendLine(@"Private Function GetDBParameter(ByVal ParamName As String, ByVal dataType As String, ByVal ParameterValue As String, ByVal Direction As String) as SqlParameter
                                                Dim dbParam As New SqlParameter
                                                dbParam.ParameterName =""@"" + ParamName
                                                Dim isNumeric As Boolean = False
                                                Dim isChar As Boolean = False
                                                GetParameterType(dataType, isNumeric, isChar)
                                                If Direction.ToUpper.Equals(""IN"") Then
                                                    dbParam.Direction = ParameterDirection.Input
                                                ElseIf Direction.ToUpper.Equals(""OUT"") Then
                                                    dbParam.Direction = ParameterDirection.Output
                                                ElseIf Direction.ToUpper.Equals(""INOUT"") Then
                                                    dbParam.Direction = ParameterDirection.InputOutput
                                                End If
                                                If isNumeric = True Then
                                                    dbParam.DbType = DbType.Int32
                                                ElseIf isChar = True Then
                                                    dbParam.DbType = DbType.String
                                                ElseIf dataType.ToUpper.Equals(""DATE"") Then
                                                    dbParam.DbType = DbType.Date
                                                ElseIf dataType.ToUpper.Equals(""DATETIME"") Then
                                                    dbParam.DbType = DbType.DateTime
                                                ElseIf dataType.ToUpper.Equals(""BOOLEAN"") Then
                                                    dbParam.DbType = DbType.Boolean
                                                ElseIf dataType.ToUpper.Equals(""OBJECT"") Then
                                                    dbParam.DbType = DbType.Object
                                                End If
                                                dbParam.Value = ParameterValue
                                                Return dbParam
                                            End Function");
                this.RuleCode.AppendLine(@" Private Sub GetParameterType(ByVal dataType As String, ByRef isNumeric As Boolean, ByRef isString As Boolean)
                                                If dataType.ToUpper().Equals(""DOUBLE"") or dataType.ToUpper().Equals(""INTEGER"") Then
                                                    isNumeric = True
                                                ElseIf dataType.ToUpper.Equals(""STRING"") Then
                                                    isString = True
                                                End If
                                            End Sub");

                this.RuleCode.AppendLine(@" Private Function GetItemValue(ByVal elementType As String, ByVal elementName As String, ByVal key As String)
                                                If Not DictGlobalValues Is Nothing AndAlso DictGlobalValues.ContainsKey(elementType + ""~"" + elementName + ""~"" + key) Then
                                                    Return DictGlobalValues(elementType + ""~"" + elementName + ""~"" + key)
                                                Else
                                                    Return """"
                                                End If
                                            End Function");

                this.RuleCode.AppendLine(@" Private Function GetDataSetFromStoreProc(ByVal spName As String, ByVal parametercols As ArrayList) As DataTable
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

                                            End Function");

                this.RuleCode.AppendLine(@"Private Sub SetGlobalValues()                                                
                                                DictRulesResult = New System.Collections.Generic.Dictionary(Of String, String)()
                                                Dim arrParams As New ArrayList
                                                arrParams.Add(GetDBParameter(""CompanyHeader"", ""String"", CompanyHeader, ""IN""))
                                                arrParams.Add(GetDBParameter(""Year"", ""String"", Year, ""IN""))
                                                Dim dsGlobalValues as DataTable = GetDataSetFromStoreProc(""pGetStandardFieldDetails"", arrParams)
                                                If dsGlobalValues IsNot Nothing AndAlso dsGlobalValues.Rows.Count > 0 Then
                                                    DictGlobalValues = New System.Collections.Generic.Dictionary(Of String, String)()
                                                    For Each item As DataRow In dsGlobalValues.Rows
                                                        Dim stdFieldName As String
                                                        Dim stdFieldValue As String
                                                        stdFieldName = Convert.ToString(item(""StandardFieldName""))
                                                        stdFieldValue = Convert.ToString(item(""StandardValue""))
                                                        If Not String.IsNullOrWhiteSpace(stdFieldName) AndAlso Not DictGlobalValues.ContainsKey(stdFieldName) Then
                                                            DictGlobalValues.Add(stdFieldName, stdFieldValue)
                                                        End If
                                                    Next
                                                End If
                                           End Sub");

                this.RuleCode.AppendLine(@"Private Function GetDouble(ByVal value As String) As Double
                                                Try
                                                    Return Convert.ToDouble(value)
                                                Catch ex As Exception
                                                    Return 0.00
                                                End Try
                                          End Function");

                RulesConfig rulesConfig = new RulesConfig();
                List<Models.RulesInfo> lstRules = rulesConfig.LoadRules(companyHeader);

                foreach (var item in lstRules)
                {
                    RootObject rootObject = (RootObject)HGarb.DataAccess.RulesConfig.ObjectFromXML(item.RuleCondition, typeof(RootObject));
                    this.RuleCode.AppendLine(BuildBusinessFunction(rootObject));
                }

                this.RuleCode.AppendLine("Public Sub runRules()");
                foreach (var methodName in this.MethodNames)
                {
                    this.RuleCode.AppendLine(methodName + "()");
                }
                this.RuleCode.AppendLine("End Sub");

                this.RuleCode.AppendLine("End Class ");
                this.RuleCode.AppendLine("End Namespace");
                string log = string.Empty;
                this.Eval(this.RuleCode.ToString(), ref log, companyHeader);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string BuildBusinessFunction(RootObject rootObject)
        {
            StringBuilder funcCode = new StringBuilder();
            rootObject.Rules.ForEach(rulesConfig =>
            {
                string ruleCond = rulesConfig.RuleData;
                string ruleName = rulesConfig.RuleName;
                string stdFieldName = "";
                this.MethodNames.Add(ruleName);
                funcCode.AppendLine("Private Sub " + ruleName + "()");
                funcCode.AppendLine("Dim elementType As String = \"" + rootObject.ElementType + "\"");
                funcCode.AppendLine("Dim elementName As String = \"" + rootObject.ElementName + "\"");
                string[] rulesBreak = ruleCond.Split('~');
                string ruleCode = "If";
                foreach (string field in rulesBreak)
                {
                    string item = field.Trim();
                    if (string.IsNullOrWhiteSpace(item)) { continue; }
                    if (Operators.Contains(item))
                    {
                        switch (item.ToUpper())
                        {
                            case "PLUS":
                                ruleCode += " +";
                                break;
                            case "MINUS":
                                ruleCode += " -";
                                break;
                            case "MULTIPLY":
                                ruleCode += " *";
                                break;
                            case "DIVIDE":
                                ruleCode += " /";
                                break;
                            case "GS":
                                ruleCode += " (";
                                break;
                            case "GE":
                                ruleCode += " )";
                                break;
                            case "EQUALTO":
                                ruleCode += " =";
                                break;
                            case "GREATERTHAN":
                                ruleCode += " >";
                                break;
                            case "LESSERTHAN":
                                ruleCode += " <";
                                break;
                            case "GREATERTHANEQUALTO":
                                ruleCode += " >=";
                                break;
                            case "LESSERTHANEQUALTO":
                                ruleCode += " <=";
                                break;
                            case "ORELSE":
                                ruleCode += " OrElse";
                                break;
                            case "ANDALSO":
                                ruleCode += " AndAlso";
                                break;
                            case "GetDouble":
                                ruleCode += item;
                                break;
                            default:
                                break;
                        }
                    }
                    else if (StandardFieldNames.Contains(item))
                    {
                        stdFieldName = item;
                        ruleCode += " GetItemValue(elementType, elementName, \"" + item + "\")";
                    }
                    else if (item.Contains("GetDouble"))
                    {
                        ruleCode += " " + item;
                    }
                    else
                    {
                        ruleCode += " " + "\"" + item + "\"";
                    }
                }

                ruleCode += " Then";
                funcCode.AppendLine(ruleCode);
                funcCode.AppendLine("DictRulesResult.Add(\"" + ruleName + "\", \"SUCCESS~\" + GetItemValue(elementType, elementName, \"" + stdFieldName + "\"))");
                funcCode.AppendLine("Else");
                funcCode.AppendLine("DictRulesResult.Add(\"" + ruleName + "\", \"FAIL~\" + GetItemValue(elementType, elementName, \"" + stdFieldName + "\"))");
                funcCode.AppendLine("End If");
                funcCode.AppendLine("End Sub");
            });

            return funcCode.ToString();
        }

        public object Eval(string vbCode, ref string log, string companyHeader)
        {
            companyHeader = System.Text.RegularExpressions.Regex.Replace(companyHeader, "[^0-9a-zA-Z]+", "");
            VBCodeProvider oCodeProvider = new VBCodeProvider();
            CompilerParameters oCParams = new CompilerParameters();
            CompilerResults oCResults = null;
            object oExecInstance = null;
            bool oRetObj = false;
            object[] parameter = new object[1];
            parameter[0] = string.Empty;

            try
            {
                oCParams.ReferencedAssemblies.Add("system.dll");
                oCParams.ReferencedAssemblies.Add("system.xml.dll");
                oCParams.ReferencedAssemblies.Add("system.Data.dll");
                oCParams.ReferencedAssemblies.Add("mscorlib.dll");
                oCParams.CompilerOptions = "/t:library";
                oCParams.GenerateInMemory = true;
                string filePath = System.IO.Path.Combine(Helper.GetAppSetting("RuleDllSavePath"), companyHeader + ".dll");
                oCParams.OutputAssembly = filePath;
                //StringBuilder sb = new StringBuilder("");
                //sb.Append("Imports System" + Environment.NewLine);
                //sb.Append("Imports System.Xml" + Environment.NewLine);
                //sb.Append("Imports System.Data" + Environment.NewLine);
                //sb.Append("Imports System.Collections" + Environment.NewLine);
                //sb.Append("Imports System.Data.SqlClient" + Environment.NewLine);
                //sb.Append("Namespace EValuate" + Environment.NewLine);
                //sb.Append("Public Class EvalRunTime " + Environment.NewLine);
                //sb.Append(vbCode + Environment.NewLine);
                //sb.Append("End Class " + Environment.NewLine);
                //sb.Append("End Namespace" + Environment.NewLine);
                try
                {
                    oCResults = oCodeProvider.CompileAssemblyFromSource(oCParams, vbCode);
                    if (oCResults.Errors.Count != 0)
                    {
                        //this.CompilerErrors = oCResults.Errors;
                        foreach (CompilerError item in oCResults.Errors)
                        {
                            log += item.ErrorText + Environment.NewLine;
                        }
                        oRetObj = false;
                    }
                    else
                    {
                        oRetObj = true;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
            }
            return oRetObj;
        }
    }
}
