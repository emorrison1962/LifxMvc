var lifxMvcApp = angular.module("lifxMvcApp", ['angularSpectrumColorpicker']);

var indexController = lifxMvcApp.controller("bulbIndexController", ['$scope', '$window', '$log', '$http', function ($scope, $window, $log, $http) {

	var palette = [
				"rgb(255,159,70)",
				"rgb(255,167,87)",
				"rgb(255,177,110)",
				"rgb(255,184,123)",
				"rgb(255,193,141)",
				"rgb(255,206,166)",
				"rgb(255,218,187)",
				"rgb(255,228,206)",
				"rgb(255,237,222)",
				"rgb(255,246,237)",
				"rgb(255,254,250)",
				"rgb(243,242,255)",
				"rgb(230,235,255)",
				"rgb(221,230,255)",
				"rgb(215,226,255)",
				"rgb(210,223,255)"
	];
	$scope.palette = palette;


	$scope.getModel = function () {
		$http.get('/Bulb/IndexJson')
			.success(function (result) {
				$scope.groups = result.Groups;
				for (g = 0; g < $scope.groups.length; ++g) {
					var group = $scope.groups[g];
				}
			})
			.error(function (data, status) {
				$log.error(data);
			})
	};

	$scope.$on('colorpicker-closed', function (event, color) {
		var bulbId = event.targetScope.bulb.BulbId;
		$scope.setColorBulb(bulbId, color.value);
	});

	$scope.$on('colorpicker-selected', function (event, color) {
		var bulbId = event.targetScope.bulb.BulbId;
		$scope.setColorBulb(bulbId, color.value);
	});

	$scope.bulbColorChanged = function (bulb) {
		$scope.setColorBulb(bulb.BulbId, bulb.ColorString);
	};

	$scope.areAnyOn = function () {
		var result = false;
		var arr = jQuery.makeArray($scope.groups)
		result = arr.some($scope.isGroupOn);
		return result;
	};

	function isBulbOn(bulb) {
		return bulb.IsOn;
	}

	$scope.isGroupOn = function (group) {
		var result = false;
		var arr = jQuery.makeArray(group.Bulbs)
		result = arr.some(isBulbOn);
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

	$scope.setColorBulb = function (bulbId, color) {
		$http.post('/Bulb/SetColorBulbJson', { bulbId: bulbId, color: color })
			.success(function (result) {
				//$scope.groups = result.Groups;
			})
			.error(function (data, status) {
				$log.error(data);
			});
	};

	$scope.getButtonBackground = function (color) {
		var result = { 'background-color' : color };
		return result;
	};


	$scope.getModel();


	

}]);


