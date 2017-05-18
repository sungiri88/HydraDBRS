'use strict';
mainApp.controller('RulesConfigController', ['$scope', '$http', 'RulesService', function ($scope, $http, rulesService) {
    $scope.templateHeaders = [];
    $scope.company = {};
    $scope.companies = [];
    $scope.templates = [];
    $scope.customFields = [];
    $scope.selectedCompany = {};
    $scope.selectedTemplate = {};
    $scope.selectedCustomField = '';
    $scope.searchText = '';
    $scope.rules = [];
    $scope.rule = {
        CustomFieldName: '',
        Headers: [],
        RowName: '',
        ColumnName: '',
        ColOccurrence: '1',
        Format: '',
        DynamicFields: []
    };
    $scope.header = '';
    $scope.fieldOccurrence = '1';
    $scope.dynamicFields = [];
    $scope.SelectedDFVal = '';
    $scope.ruleConfigId = 0;
    var vm = {};
    vm.loadCustomFields = function (templateId) {
        rulesService.loadCustomFields(templateId).then(function (d) {
            $scope.customFields = d.data ? d.data[0] ? d.data[0].Fields : [] : [];
        },
        function (err) {
        });
    }

    vm.loadFields = function (templateId) {
        rulesService.loadFields(templateId).then(function (d) {
            $scope.templateHeaders = d.data ? JSON.parse(d.data[0] ? d.data[0].Fields : []) : [];
        },
        function (e, f) {
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    }

    vm.loadDynamicFields = function (templateId) {
        rulesService.loadDynamicFields(templateId).then(function (d) {
            $scope.dynamicFields = d.data ? d.data : [];
        },
        function (e, f) {
            toastr["error"](e ? e.message : 'Oops! Something not right!');
        });
    }

    vm.loadTemplates = function (companyId) {
        rulesService.loadTemplates(companyId).then(function (d) {
            $scope.templates = d.data ? d.data : {};
        },
        function (e, f) {
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    }

    vm.loadCompanies = function () {
        rulesService.loadCompanies().then(function (d) {
            $scope.companies = d.data ? d.data : [];
        },
        function (e, f) {
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    }

    vm.saveRulesConfig = function (requestData) {
        rulesService.saveRulesConfig(requestData).then(function (d) {
            toastr["success"]('Saved Successfully!');
        },
        function (e, f) {
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    }

    vm.loadRules = function (templateId) {
        rulesService.loadRules(templateId).then(function (d) {
            $scope.rules = d.data ? d.data.RulesConfig : [];
            $scope.ruleConfigId = d.data ? d.data.Id : [];
        },
        function (e, f) {
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    }

    $scope.message = "Click on the hyper link to view the students list.";

    $scope.$on('$viewContentLoaded', function () {
        vm.loadCompanies();
    });

    $scope.companyChange = function () {
        vm.loadTemplates($scope.selectedCompany.Id);
    }

    $scope.templateChange = function () {
        vm.loadFields($scope.selectedTemplate.Id);
        vm.loadCustomFields($scope.selectedTemplate.Id);
        vm.loadDynamicFields($scope.selectedTemplate.Id);
        vm.loadRules($scope.selectedTemplate.Id);
        document.getElementById("pdfFrame").src = $scope.selectedTemplate.MainTemplate;
    }

    $scope.addRule = function () {
        if ($scope.header[0]) {
            var dataTag = '<span class="label label-success">' + $scope.header[0] + '(' + $scope.fieldOccurrence + ')</span>';
            document.getElementById('tblTagItems').innerHTML += dataTag;
            $scope.rule.Headers.push({ 'Path': $scope.header[0] + '~' + $scope.fieldOccurrence });
            $scope.header = undefined;
            $scope.fieldOccurrence = '1';
            $scope.searchText = '';
        }
    }

    $scope.clear = function () {
        document.getElementById('tblTagItems').innerHTML = "";
        $scope.rule = {
            CustomFieldName: '',
            Headers: [],
            RowName: '',
            ColumnName: '',
            ColOccurrence: '1',
            Format: '',
            DynamicFields: []
        };
        $scope.header = undefined;
        $scope.fieldOccurrence = '1';
        $scope.searchText = '';
    }

    $scope.addRuleConfig = function () {
        angular.forEach($scope.dynamicFields, function (value, key) {
            var fieldvalue = $('#' + 'select' + '_' + key).val();
            $scope.rule.DynamicFields.push({ FieldName: value.FieldName, FieldValue: fieldvalue });
        });

        $scope.rules.push($scope.rule);
        $scope.clear();
    }

    $scope.saveConfig = function () {
        var rcRequest = {
            Id: $scope.ruleConfigId,
            TemplateId: $scope.selectedTemplate.Id,
            RulesConfig: $scope.rules
        };

        vm.saveRulesConfig(rcRequest);
        $scope.clear();
        $scope.rules = [];
    }

    angular.element(document).ready(function () {
        //$('#noti-box1').slimScroll({
        //    height: (screen.height - 230) + 'px',
        //    size: '5px',
        //    BorderRadius: '5px'
        //});
        //$('#noti-box2').slimScroll({
        //    height: (screen.height - 230) + 'px',
        //    size: '5px',
        //    BorderRadius: '5px'
        //});

        $('.split-me').touchSplit({ leftMin: 50, rightMin: 10, thickness: "20px", dock: "left" });
    });
}]);