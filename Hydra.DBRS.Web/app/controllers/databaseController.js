'use strict';
mainApp.directive('ngFiles', ['$parse', function ($parse) {
    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, {
                $files: event.target.files
            });
        });
    };
    return {
        link: fn_link
    }
}])

mainApp.controller('DatabaseController', ['$scope', '$http', 'RulesService', function ($scope, $http, rulesService) {
    $scope.companyName = '';
    $scope.companyHeader = '';
    $scope.template = '';
    $scope.myData = [{ 'CompanyName': '', 'User': '' }];
    $scope.companies = [];
    $scope.templates = [];
    $scope.selectedCompany = {};
    $scope.selectedTemplate = {};
    $scope.customFields = [];
    $scope.selectedCustField = {};
    $scope.customFieldName = '';
    $scope.lookupData = '';
    $scope.dynamicFields = []; //[{ "TemplateId": 12, "FieldName": "hgjhg", "LookupData": ["sdf", "56eg"] }, { "TemplateId": 12, "FieldName": "hgjhg", "LookupData": ["sdf", "56eg"] }];
    var vm = {};
    vm.loadTemplates = function (companyId) {
        rulesService.loadTemplates(companyId).then(function (d) {
            $scope.templates = d.data ? d.data : {};
        },
        function (e, f) {
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    }

    vm.loadCustomFields = function (templateId) {
        rulesService.loadCustomFields(templateId).then(function (d) {
            $scope.customFields = d.data ? d.data[0] ? d.data[0].Fields : [] : [];
        },
        function (err) {
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    }

    vm.loadCompanies = function (companyId) {
        rulesService.loadCompanies().then(function (d) {
            $scope.companies = d.data ? d.data : [];
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
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    }

    vm.updateCustomFields = function (requestData) {
        rulesService.updateCustomFields(requestData).then(function (d) {
            toastr["success"]('Custom Fields Saved Successfully!');
        },
        function (e, f) {
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    }

    vm.insertDynamicFields = function (requestData) {
        rulesService.insertDynamicFields(requestData).then(function (d) {
            toastr["success"]('Custom Fields Saved Successfully!');
        },
        function (e, f) {
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    }

    $scope.companyChange1 = function () {
        vm.loadTemplates($scope.selectedCompany.Id);
    }

    $scope.templateChange = function () {
        //vm.loadCustomFields($scope.selectedTemplate.Id);
        vm.loadDynamicFields($scope.selectedTemplate.Id);
    }

    $scope.navCustomFields = function () {
        vm.loadCompanies();
    };

    $scope.addCustomField = function () {
        if ($scope.selectedTemplate.Id) {
            if (!$scope.dynamicFields || $scope.dynamicFields.length == 0) {
                $scope.dynamicFields = [];
            }
            var lookUpData = $scope.lookupData.split(",");
            $scope.dynamicFields.push({ TemplateId: $scope.selectedTemplate.Id, FieldName: $scope.customFieldName, LookupData: lookUpData });
        }
        else {
            toastr["error"]('Oops! Please select the template before adding custom fields!');
        }
    }

    $scope.updateCustomFields = function () {
        vm.insertDynamicFields($scope.dynamicFields);
    }

    var formdata = new FormData();
    $scope.getTheFiles = function ($files) {
        angular.forEach($files, function (value, key) {
            formdata.append(key, value);
        });
    };

    // NOW UPLOAD THE FILES.
    $scope.uploadFiles = function () {
        formdata.append('companyName', $scope.companyName);
        formdata.append('companyHeader', $scope.companyHeader);
        rulesService.uploadFiles(formdata).then(function (d) {
            $scope.myData.push({ CompanyName: $scope.companyName });
            formdata = new FormData();
            $scope.companyName = '';
            $scope.companyHeader = '';
            $scope.template = '';
            document.getElementById("fpTemp").value = "";
            toastr["success"]("Template added successfully!")
        },
        function (e, f) {
            formdata = new FormData();
            $scope.companyName = '';
            $scope.companyHeader = '';
            $scope.template = '';
            document.getElementById("fpTemp").value = "";
            toastr["error"](e ? e.Message : 'Oops! Something not right!');
        });
    };

    rulesService.loadCompanies().then(function (d) {
        $scope.myData = d.data ? d.data : [];
    },
    function (e, f) {
        toastr["error"](e ? e.Message : 'Oops! Something not right!');
    });
}]);
