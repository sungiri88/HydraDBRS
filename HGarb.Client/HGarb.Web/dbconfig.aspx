<%@ Page Title="" Language="C#" MasterPageFile="~/hgarb.Master" AutoEventWireup="true" CodeBehind="dbconfig.aspx.cs" EnableEventValidation="false" Inherits="HGarb.Web.dbconfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <section class="panel general">
            <header class="panel-heading tab-bg-dark-navy-blue">
                <ul class="nav nav-tabs">
                    <li class="active">
                        <a data-toggle="tab" href="#home" target="_self">Company Config</a>
                    </li>
                    <li class="">
                        <a data-toggle="tab" href="#profile" target="_self">Users</a>
                    </li>
                </ul>
            </header>
            <div class="panel-body">
                <div class="tab-content">
                   
                        <div id="home" class="tab-pane active">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:UpdatePanel ID="AddCompanyPanel" runat="server">
                                        <ContentTemplate>
                                        <div class="row form-group">
                                            <div class="col-lg-12">
                                                <p class="h4">Add Company</p>
                                            </div>
                                        </div>
                                        <div class="row form-group">
                                            <label class="control-label col-lg-2" for="txt_add_company">Add Company</label>
                                            <div class="col-lg-10">
                                                <asp:TextBox runat="server" ID="txt_add_company" />
                                            </div>
                                        </div>
                                        <div class="form-group">  
                                            <div class="col-lg-2"></div>
                                            <div class="col-lg-10" style="text-align: left; padding-top: 15px;">
                                                <asp:Button runat="server" class="btn btn-info" ID="btn_add_company" Text="Save" OnClick="btnAddCompany_Click" />
                                            </div>
                                        </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="col-lg-12">
                                    <asp:UpdatePanel ID="AddCompanyHeader" runat="server">
                                        <ContentTemplate>
                                        <div class="row form-group">
                                            <div class="col-lg-12">
                                                <p class="h4">Add Company Header</p>
                                            </div>
                                        </div>
                                        <div class="row form-group">
                                            <label class="control-label col-lg-2" for="ddlCompany">Select Company</label>
                                            <div class="col-lg-10">
                                                <asp:DropDownList ID="ddlCompany" runat="server" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                           
                                        </div>
                                        <div class="row form-group">
                                            <label class="control-label col-lg-2" for="ddlCompanyHeaders">Existing Company Headers</label>
                                            <div class="col-lg-10">
                                                <asp:DropDownList ID="ddlCompanyHeaders" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row form-group">
                                            <label class="control-label col-lg-2" for="companyHeader_1">Add Company Header</label>
                                            <div class="col-lg-10">
                                                <asp:TextBox runat="server" AutoPostBack="true" ID="txt_companyHeader_1" />
                                            </div>
                                        </div>
                                        <div class="form-group">  
                                            <div class="col-lg-2"></div>
                                            <div class="col-lg-10" style="text-align: left; padding-top: 15px;">
                                                <asp:Button runat="server" class="btn btn-info" ID="btn_companyHeader_save" Text="Save" OnClick="btnAddCompanyHeader_Click" />
                                            </div>
                                        </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div ui-grid="{ data: myData }" class="myGrid"></div>
                                </div>
                            </div>
                        </div>

                    <div id="profile" class="tab-pane">Profile</div>
                    <div id="contact" class="tab-pane">Contact</div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>
