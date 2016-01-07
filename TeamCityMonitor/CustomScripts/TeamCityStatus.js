(function () {

    var app = angular.module("teamCityMonitor", []);

    //var _alertTypeFromStatus = function (status) {
    //    if (status === "Success")
    //        return "alert-success";

    //    if (status === "Running")
    //        return "alert-warning";

    //    if (status === "Failure")
    //        return "alert-danger";

    //    return "alert-info";
    //};

    ////var module = angular.module("teamCityMonitor");
    //app.factory("alertTypeFromStatus", function() { return _alertTypeFromStatus; });

    var teamCityStatusController = function ($scope, $http, $interval, alertTypeFromStatus) {
        $scope.projects = [{ 'Name': "Loading..." }];
        $scope.refreshTime = "Never";
        $scope.alertTypeFromStatus = alertTypeFromStatus;
        $scope.moment = moment;

        var onStatusComplete = function (response) {
            $scope.projects = response.data;
            //$scope.refreshTime = moment().format("YYYY-MM-DD HH:mm:ss");
            $scope.refreshTime = moment().format("HH:mm:ss");
            $scope.error = null;
        };

        var onError = function (reason) {
            //$scope.error = "Could not fetch the user";
            $scope.error = reason;
        };


        var refreshStatus = function() {
            //$http.get("/api/TeamCityStatus/builds")
            $http.get("/api/TeamCityStatus/projects")
                .then(onStatusComplete, onError);
        }

        refreshStatus();
        $interval(refreshStatus, 5*1000);
    };

    app.controller("TeamCityStatusController", ["$scope", "$http", "$interval", "alertTypeFromStatus", teamCityStatusController]);
}());