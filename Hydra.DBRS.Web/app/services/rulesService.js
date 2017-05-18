mainApp.service('RulesService', ["HYDRA_CONSTANTS", '$http', function (HC, $http) {
    this.loadCustomFields = function (templateId) {
        var request = {
            method: 'GET',
            url: HC.SERVICEBASE + 'customfields/' + templateId,
            headers: {
                'Content-Type': 'application/json'
            }
        };

        return $http(request);
    }

    this.loadFields = function (templateId) {
        var request = {
            method: 'GET',
            url: HC.SERVICEBASE + 'fields/' + templateId,
            headers: {
                'Content-Type': 'application/json'
            }
        };

        return $http(request);
    }

    this.loadTemplates = function (companyId) {
        var request = {
            method: 'GET',
            url: HC.SERVICEBASE + 'templates/' + companyId,
            headers: {
                'Content-Type': 'application/json'
            }
        };

        return $http(request);
    }

    this.loadCompanies = function () {
        var request = {
            method: 'GET',
            url: HC.SERVICEBASE + 'companies',
            headers: {
                'Content-Type': 'application/json'
            }
        };

        return $http(request);
    }

    this.loadDynamicFields = function (id) {
        var request = {
            method: 'GET',
            url: HC.SERVICEBASE + 'dynamicFields/' + id,
            headers: {
                'Content-Type': 'application/json'
            }
        };

        return $http(request);
    }

    this.loadRules = function (id) {
        var request = {
            method: 'GET',
            url: HC.SERVICEBASE + 'rulesonfig/' + id,
            headers: {
                'Content-Type': 'application/json'
            }
        };

        return $http(request);
    }

    this.saveRulesConfig = function (requestData) {
        var request = {
            method: 'POST',
            url: HC.SERVICEBASE + 'saverules',
            headers: {
                'Content-Type': 'application/json'
            },
            data: requestData
        };

        return $http(request);
    }

    this.updateCustomFields = function (requestData) {
        var request = {
            method: 'POST',
            url: HC.SERVICEBASE + 'addcustomfields',
            headers: {
                'Content-Type': 'application/json'
            },
            data: requestData
        };

        return $http(request);
    }

    this.insertDynamicFields = function (requestData) {
        var request = {
            method: 'POST',
            url: HC.SERVICEBASE + 'insertdynamicfields',
            headers: {
                'Content-Type': 'application/json'
            },
            data: requestData
        };

        return $http(request);
    }

    this.uploadFiles = function (formData) {
        var request = {
            method: 'POST',
            url: HC.SERVICEBASE + 'addcompany',
            data: formData,
            headers: {
                'Content-Type': undefined
            }
        };

        return $http(request);
    }
}]);