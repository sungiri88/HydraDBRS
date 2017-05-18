<%@ Page Title="" Language="C#" MasterPageFile="~/hgarb.Master" AutoEventWireup="true" CodeBehind="dataexport.aspx.cs" Inherits="HGarb.Web.dataexport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <section class="panel general">
            <div class="panel-body table-responsive" style="width: 100%;">
                <div class="row form-group">
                    <div class="col-lg-10">
                        <label class="control-label" for="tbRuleName">Company Name</label>
                        <asp:DropDownList ID="ddlCOmpany" runat="server" Width="200" AutoPostBack="true" OnSelectedIndexChanged="ddlCOmpany_SelectedIndexChanged">
                        </asp:DropDownList>
                        <label class="control-label" for="tbRuleName" style="padding-left: 20px;">Company Header</label>
                        <asp:DropDownList ID="ddlCompanyHeaders" runat="server" Width="250" AutoPostBack="true">
                        </asp:DropDownList>
                        <label class="control-label" for="tbRuleName" style="padding-left: 20px;">Date(MMYYYY)</label>
                        <asp:TextBox ID="txtYear" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-lg-2" style="text-align: right;">
                        <asp:Button ID="btnExeRules" runat="server" Text="Execute Rules" class="btn btn-info" OnClick="btnExeRules_Click" />
                    </div>
                </div>
                <asp:Panel ID="pnlResult" runat="server" Visible="false">
                    <table class="table table-hover">
                        <tr style="background-color: silver;">
                            <th>Rule Name</th>
                            <th>Status</th>
                            <th>View Data</th>
                            <th>Edit Data</th>
                            <th>Assign</th>
                        </tr>
                        <tr style="background-color: forestgreen; color: whitesmoke; font-weight: bolder;">
                            <td>Rule 1</td>
                            <td>Success</td>
                            <td>
                                <a style="color: whitesmoke;">View Data</a>
                            </td>
                            <td>
                                <a href="javascript:return false;" target="_self" style="color: whitesmoke;">
                                    <i class="ion-edit" style="font-size: 20px;"></i>
                                </a>
                            </td>
                            <td></td>
                        </tr>
                        <tr style="background-color: darkred; color: whitesmoke; font-weight: bolder;">
                            <td>Rule 2</td>
                            <td>Fail</td>
                            <td>
                                <a style="color: whitesmoke;">View Data</a>
                            </td>
                            <td>
                                <a href="javascript:return false;" target="_self" style="color: whitesmoke;">
                                    <i class="ion-edit" style="font-size: 20px;"></i>
                                </a>
                            </td>
                            <td>
                                <a target="_self" style="color: whitesmoke;" data-toggle="modal" href="#myModal">
                                    <i class="ion-android-person" style="font-size: 20px;"></i>
                                </a>
                            </td>
                        </tr>
                        <tr style="background-color: forestgreen; color: whitesmoke; font-weight: bolder;">
                            <td>Rule 3</td>
                            <td>Success</td>
                            <td>
                                <a style="color: whitesmoke;">View Data</a>
                            </td>
                            <td>
                                <a href="javascript:return false;" target="_self" style="color: whitesmoke;">
                                    <i class="ion-edit" style="font-size: 20px;"></i>
                                </a>
                            </td>
                            <td></td>
                        </tr>
                        <tr style="background-color: forestgreen; color: whitesmoke; font-weight: bolder;">
                            <td>Rule 4</td>
                            <td>Success</td>
                            <td>
                                <a style="color: whitesmoke;">View Data</a>
                            </td>
                            <td>
                                <a href="javascript:return false;" target="_self" style="color: whitesmoke;">
                                    <i class="ion-edit" style="font-size: 20px;"></i>
                                </a>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                    <br />
                    <div class="row form-group">
                        <div class="col-lg-8">
                        </div>
                        <div class="col-lg-4" style="text-align: right;">
                            <button class="btn btn-info" type="button">Export Data</button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </section>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Users</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:ListBox CssClass="col-lg-10" ID="lbUsers" runat="server">
                            <asp:ListItem Text="admin" />
                            <asp:ListItem Text="aathma" />
                            <asp:ListItem Text="karthik" />
                            <asp:ListItem Text="shanki" />
                        </asp:ListBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button data-dismiss="modal" class="btn btn-default" type="button">Close</button>
                    <button class="btn btn-success" type="button" data-dismiss="modal">Assign</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
