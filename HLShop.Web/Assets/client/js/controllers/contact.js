var contact = {
    init: function () {
        contact.registerEvent();
    },
    registerEvent: function () {
        contact.initMap();
    },
    initMap: function () {
        // toa do
        var myShop = { lat: parseFloat($('#hidLat').val()), lng: parseFloat($('#hidLng').val()) };

        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 17,
            center: myShop 
        });

        var contentString = $('#hidInfor').val();

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        var marker = new google.maps.Marker({
            position: myShop ,
            map: map,
            title: $('#hidName').val()
        });
        infowindow.open(map, marker);
    }
}
contact.init();