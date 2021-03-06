﻿<%@ Page Title="" Language="C#" MasterPageFile="~/hgarb.Master" AutoEventWireup="true" CodeBehind="dataexport.aspx.cs" Inherits="HGarb.Web.dataexport" %>

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
                <asp:Panel ID="pnlResult" runat="server" Visible="false" Width="100%">
                    <asp:GridView runat="server" ID="gvRulesResult" AutoGenerateColumns="true" Width="100%"></asp:GridView>
                    <br />
                    <div class="row form-group">
                        <div class="col-lg-8">
                        </div>
                        <div class="col-lg-4" style="text-align: right;">
                            <asp:Button class="btn btn-info" runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export Data"></asp:Button>
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
