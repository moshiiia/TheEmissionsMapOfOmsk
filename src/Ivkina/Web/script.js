let center = [54.99132369838494,73.3667615263671];

function init() {
	let map = new ymaps.Map('map-test', {
		center: center,
		zoom: 10
	});

  map.controls.remove('geolocationControl'); // удаляем геолокацию
  map.controls.remove('searchControl'); // удаляем поиск
  map.controls.remove('trafficControl'); // удаляем контроль трафика
  map.controls.remove('typeSelector'); // удаляем тип
  map.controls.remove('fullscreenControl'); // удаляем кнопку перехода в полноэкранный режим
  map.controls.remove('rulerControl'); // удаляем контрол правил
  //map.controls.remove('zoomControl'); // удаляем контрол зуммирования
}

ymaps.ready(init);
