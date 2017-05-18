'use strict';
var mainApp = angular.module("mainApp", ['ngRoute', 'ngAnimate', 'ui.grid']);
mainApp.config(function ($routeProvider) {
    $routeProvider
        .when('/dashboard', {
            templateUrl: 'app/views/dashboard.html',
            controller: 'DashboardController'
        })
		.when('/rulesconfig', {
		    templateUrl: 'app/views/rulesconfig.html',
		    controller: 'RulesConfigController'
		})
		.when('/database', {
		    templateUrl: 'app/views/database.html',
		    controller: 'DatabaseController'
		})
        .when('/reports', {
            templateUrl: 'app/views/reports.html',
            controller: 'ReportsController'
        })
		.otherwise({
		    redirectTo: '/dashboard'
		});
});

mainApp.constant("HYDRA_CONSTANTS", {
    "SERVICEBASE": "/HydraAPI/api/"
});

mainApp.run(['$rootScope', '$location', function ($rootScope, $location) {
    $rootScope.user = sessionStorage.userName;
    $rootScope.$on('$routeChangeStart', function (next, current) {
        $('.sidebar-toggle').click();
    });
}]);
