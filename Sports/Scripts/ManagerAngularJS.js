var app = angular.module("myApp", []);  

app.controller("myCtrl", function ($scope, $http) {


    $scope.Get_AllEmployee = function () {
        $http({
            method: "get",
            url: "/Manager/Get_AllEmployee"
        }).then(function (response) {
            $scope.manager = response.data;
        }, function (data) {
            alert(data.message);
        })
    };  

    $scope.DeleteEmp = function (manager) {
        $http({
            method: "post",
            url: "/Manager/Delete",

            data: {
                id: manager
                }
        }).then(function (response) {
            if (response.success == false) {
                alert(response.data.message);
            }
            else {
                alert(response.data.message);
                $scope.Get_AllEmployee();
            }
         
        })
    };  

})