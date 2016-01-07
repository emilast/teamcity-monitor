(function() {

    var alertTypeFromStatusService = function() {
        return function(status) {
            if (status === "Success")
                return "alert-success";

            if (status === "Running")
                return "alert-warning";

            if (status === "Failure")
            	return "alert-danger";

            if (status === "Unknown")
            	return "alert-disabled";

            return "alert-info";
        }
    };

    var module = angular.module("teamCityMonitor");
    module.factory("alertTypeFromStatus", alertTypeFromStatusService);

}());