//var lifxMvcApp = angular.module("lifxMvcApp", []);
var lifxMvcApp = angular.module("lifxMvcApp", ['angularSpectrumColorpicker']);



var indexController = lifxMvcApp.controller("bulbIndexController", ['$scope', '$window', '$log', '$http', function ($scope, $window, $log, $http) {


	$scope.models = [
		  { color: "rgb(5, 5, 5)" },
		  { color: "#cf1115" },
		  { color: "#999999" },
	];

	var mycolor = { color: "rgb(0, 255, 0)" };
	$scope.mycolor = mycolor;

	$scope.getModel = function () {
		$http.get('/Bulb/IndexJson')
			.success(function (result) {
				$scope.groups = result.Groups;
			})
			.error(function (data, status) {
				$log.error(data);
			})
	};

	$scope.areAnyOn = function () {
		var result = false;
		if ($scope.groups) {
			var glen = $scope.groups.length;
			for (i = 0; i < glen; i++) {
				if ($scope.isGroupOn($scope.groups[i])) {
					result = true;
					break;
				}
			}
		}
		return result;
	};

	$scope.isGroupOn = function (group) {
		var result = false;
		var len = group.Bulbs.length;
		for (i = 0; i < len; i++) {
			if (group.Bulbs[i].IsOn) {
				result = true;
				break;
			}
		}
		return result;
	};

	$scope.togglePowerAll = function () {
		$http.post('/Bulb/TogglePowerAllJson')
			.success(function (result) {
				$scope.groups = result.Groups;
			})
			.error(function (data, status) {
				$log.error(data);
			});
	};

	$scope.togglePowerGroup = function (group) {
		var wha = { name: group.Name };
		$http.post('/Bulb/TogglePowerGroupJson', { name: group.Name })
			.success(function (result) {
				$scope.groups = result.Groups;
			})
			.error(function (data, status) {
				$log.error(data);
			});
	};

	$scope.togglePowerBulb = function (bulbId) {
		$http.post('/Bulb/TogglePowerBulbJson', { bulbId: bulbId })
			.success(function (result) {
				$scope.groups = result.Groups;
			})
			.error(function (data, status) {
				$log.error(data);
			});

	};



	$scope.getModel();


	

}]);


$(".basic").spectrum({
	color: "#f00",
	change: function (color) {
		$("#basic-log").text("change called: " + color.toHexString());
	}
});
