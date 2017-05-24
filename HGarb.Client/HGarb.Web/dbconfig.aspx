<%@ Page Title="" Language="C#" MasterPageFile="~/hgarb.Master" AutoEventWireup="true" CodeBehind="dbconfig.aspx.cs" Inherits="HGarb.Web.dbconfig" %>

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
                                <form class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label class="control-label col-lg-3">Company Name</label>
                                        <div class="col-lg-9">
                                            <input type="text" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-lg-3">Company Header</label>
                                        <div class="col-lg-9">
                                            <input type="text" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-lg-8">

                                        </div>
                                        <div class="col-lg-4" style="text-align: right; padding-top: 15px;">
                                            <button class="btn btn-info" type="button">Save</button>
                                        </div>
                                    </div>
                                </form>
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
