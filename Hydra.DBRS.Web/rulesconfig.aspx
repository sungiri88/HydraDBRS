<%@ Page Title="" Language="C#" MasterPageFile="~/dbrs.master" AutoEventWireup="true" CodeFile="rulesconfig.aspx.cs" Inherits="rulesconfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <section class="panel general">
            <header class="panel-heading tab-bg-dark-navy-blue">
                <ul class="nav nav-tabs">
                    <li class="active">
                        <a data-toggle="tab" href="#about" target="_self">Company Fields</a>
                    </li>
                </ul>
            </header>
            <div class="panel-body">
                <div class="tab-content">
                    <div id="about" class="tab-pane active">
                        <div class="row form-group">
                            <label class="control-label col-lg-2" for="listCompany">Companies</label>
                            <div class="col-lg-10">
                                <asp:DropDownList runat="server" ID="listCompany"></asp:DropDownList>
                                <select id="listCompany" class="form-control m-b-10" ng-options="company.CompanyName for company in companies"
                                    ng-model="selectedCompany" ng-change="companyChange1()">
                                </select>
                            </div>
                        </div>
                        <div class="row form-group">
                            <label class="control-label col-lg-2" for="listTemplates">Templates</label>
                            <div class="col-lg-10">
                                <select id="listTemplates" class="form-control m-b-10" ng-options="template.CompanyHeader for template in templates"
                                    ng-model="selectedTemplate" ng-change="templateChange()">
                                </select>
                            </div>
                        </div>
                        <div class="row form-group">
                            <label class="control-label col-lg-2">Custom Field Name</label>
                            <div class="col-lg-4">
                                <input type="text" class="form-control" ng-model="customFieldName" />
                            </div>
                            <div class="col-lg-5">
                                <!--<select id="listFields" multiple="" class="form-control" ng-model="selectedCustField">
                                <option ng-repeat="field in customFields[0].Fields">{{field}}</option>
                            </select>-->
                                <textarea style="width: 100%; height: 100%;" placeholder="Please provide values in comma(,) seperated" ng-model="lookupData"></textarea>
                            </div>
                            <div class="col-lg-1">
                                <button class="btn btn-info" type="button" ng-click="addCustomField()">Add</button>
                            </div>
                        </div>
                        <div class="row form-group">
                            <table class="table table-bordered">
                                <tbody>
                                    <tr>
                                        <th>Field Name</th>
                                        <th>LookUpData</th>
                                        <th>Action</th>
                                    </tr>
                                    <tr ng-repeat="cf in dynamicFields">
                                        <td>{{cf.FieldName}}</td>
                                        <td>
                                            <div>
                                                {{cf.LookupData}}
                                            </div>
                                        </td>
                                        <td>
                                            <a href=""><i class="fa fa-2x fa-edit"></i></a>
                                            <a href=""><i class="fa fa-2x fa-remove"></i></a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="row form-group">
                            <div class="col-lg-8">
                            </div>
                            <div class="col-lg-4" style="text-align: right;">
                                <button class="btn btn-info" type="button" ng-click="updateCustomFields()">Save</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>

