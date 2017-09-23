var RoutePlanner = RoutePlanner || {};

RoutePlanner.Application = function ($containerElement, $directionsElement) {
    var self = this;

    var latlng;
    var map;
    var marker;
    self.success = function (position) {
        var s = document.querySelector('#status');

        if (s.className == 'success') {
            // not sure why we're hitting this twice in FF, I think it's to do with a cached result coming back    
            return;
        }

        s.innerHTML = "found you!";
        s.className = 'success';

        latlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
        self.options = {
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            mapTypeControl: false,
            zoom: 15,
            center: latlng
            //center: new google.maps.LatLng(47.0500, 8.3000)   // Luzern, Switzerland
        };

        map = new google.maps.Map($containerElement[0], self.options);
        
        marker = new google.maps.Marker({
            position: latlng,
            map: map,
            title: "You are here! (at least within a " + position.coords.accuracy + " meter radius)"
        });

        self.renderMap(map);
    };

    self.error = function (msg) {
        var s = document.querySelector('#status');
        s.innerHTML = typeof msg == 'string' ? msg : "failed";
        s.className = 'fail';

        // console.log(arguments);
    };

    var directionsService;
    var directionsRenderer;

    self.renderMap = function (map) {
        directionsService = new google.maps.DirectionsService();
        directionsRenderer = new google.maps.DirectionsRenderer({ draggable: true });
        //directionsRenderer.setMap(new google.maps.Map($containerElement[0], options));
        directionsRenderer.setMap(map);
        directionsRenderer.setPanel($directionsElement[0]);
        self.directionsRenderer = directionsRenderer;
    };

    self.clear = function () {
        $directionsElement.empty();
    };

    self.shouldNotify = true;

    self.calculateRoute = function (travelMode, start, end) {
        var request = {
            origin: start,
            destination: end,
            travelMode: google.maps.DirectionsTravelMode[travelMode]
        };

        directionsService.route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                self.clear();
                self.directionsRenderer.setDirections(response);
            } else {
                alert("Error! See console log for status.");
                console.log(status);
            }
        });
    };

    self.getRoute = function () {
        var routeLeg = self.directionsRenderer.directions.routes[0].legs[0];
        var route = {};
        route.travelMode = self.directionsRenderer.directions.Mb.travelMode;
        route.startAddress = routeLeg.start_address;
        route.endAddress = routeLeg.end_address;
        route.start = {
            latitude: routeLeg.start_location.lat(),
            longitude: routeLeg.start_location.lng()
        };
        route.end = {
            latitude: routeLeg.end_location.lat(),
            longitude: routeLeg.end_location.lng()
        };
        route.waypoints = [];

        for (var i = 0; i < routeLeg.via_waypoints.length; i++) {
            route.waypoints[i] = [routeLeg.via_waypoints[i].lat(), routeLeg.via_waypoints[i].lng()];
        }

        return route;
    };

    self.reloadRoute = function (route, notify) {
        self.shouldNotify = notify ? true : false;
        var googleMapWaypoints = [];
        for (var i = 0; i < route.waypoints.length; i++) {
            googleMapWaypoints[i] = {
                location: new google.maps.LatLng(route.waypoints[i][0], route.waypoints[i][1]),
                stopover: false
            };
        }

        var request = {
            origin: new google.maps.LatLng(route.start.latitude, route.start.longitude),
            destination: new google.maps.LatLng(route.end.latitude, route.end.longitude),
            travelMode: google.maps.DirectionsTravelMode[route.travelMode],
            waypoints: googleMapWaypoints
        };

        directionsService.route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                self.clear();
                self.directionsRenderer.setDirections(response);
            } else {
                alert("Error! See console log for status.");
                console.log(status);
            }
            self.shouldNotify = true;
        });
    };

};

// Array.prototype.findIndex - MIT License (c) 2013 Paul Miller <http://paulmillr.com>
// For all details and docs: <https://github.com/paulmillr/Array.prototype.findIndex>
(function (globals) {

    if (Array.prototype.findIndex) return;

    var findIndex = function (predicate) {
        var list = Object(this);
        var length = list.length >>> 0; // ES.ToUint32;
        if (length === 0) return -1;
        
        if (typeof predicate !== 'function') {
            throw new TypeError('Array#findIndex: predicate must be a function');
        }

        var thisArg = arguments.length > 1 ? arguments[1] : undefined;
        for (var i = 0; i < length && i in list; i++) {
            if (predicate.call(thisArg, list[i], i, list)) return i;           
        }
        return -1;
    };

    if (Object.defineProperty) {
        try {
            Object.defineProperty(Array.prototype, 'findIndex', {
                value: findIndex, configurable: true, writable: true
            });
        } catch (e) { }
    }

    if (!Array.prototype.findIndex) {
        Array.prototype.findIndex = findIndex;
    }
}(this));


function getIndex(element, index, array) {
    if (element.id() == this)
        return true
}

function PropertyItem(item) {
    var self = this;
    self.id = ko.observable(item.PropertyTypeId)
    self.name = ko.observable(item.Name);
};


function PropertyREItem(item) {
    var self = this;
    self.id = ko.observable(item.PropertyREId)
    self.desc = ko.observable(item.Description);
    self.subtype = ko.observable(item.PropertyTypeSub);
    self.loc = ko.observable(item.Location);
    self.price = ko.observable(item.Price);
    self.proptype = ko.observable(item.PropertyType);
    self.community = ko.observable(item.Community);
    self.owner = ko.observable(item.Owner);

    // with SignalR
    self.update = function (property) {
        updating = true;
        self.desc(property.Description);
        self.subtype(property.PropertyTypeSub);
        self.loc(property.Location);
        self.price(property.Price);
        self.proptype(property.PropertyType);
        self.community(property.Community);
        self.owner(property.Owner);
        updating = false;
    };
};

function PropertyViewModel() {
    /*
    * In your viewmodel, it’s often useful to declare self (or some other variable) as an alias for this. 
    * Doing so avoids any problems with this being redefined to mean something else in event handlers or Ajax request callbacks
    * http://knockoutjs.com/documentation/click-binding.html
    */
    var self = this;
    self.UserId = "";

    // property Types
    self.items = ko.observableArray([]);

    // real estate properties
    self.idx = ko.observable(-1);
    self.properties = ko.observableArray([]);
    self.propertydetails = ko.observable("");

    self.addPropertyRE = function (item) {
        self.properties.push(new PropertyREItem(item));
    };

    self.browse = function (item) {
        var idd = $('#proptype > ul > li .class' + item.name()).attr("id");

        $('#proptype > ul > li')
            .filter(function (index) {
                return index != idd;
            })
            .css("opacity", "0.25")

        $('#proptype > ul > li')
            .filter(function (index) {
                return index == idd;
            })
            .css("opacity", "1")

        // get all properties with property type = item.name
        $.getJSON("api/properties/" + item.name(), function (allData) {
            //alert(item.name());
            var mappedProperties = $.map(allData, function (item) { return new PropertyREItem(item) });
            self.properties(mappedProperties);

            // if only one property, show detail at once
            self.propertydetails(undefined);
            if (self.properties().length == 1)
                self.getproperty(self.properties()[0]);
        });

    }

    self.getproperty = function (item) {
        var idd = $('#prop > ul > li .class' + item.id()).attr("id");

        $('#prop > ul > li')
            .filter(function (index) {
                return index != idd;
            })
            .css("opacity", "0.25")

        $('#prop > ul > li')
            .filter(function (index) {
                return index == idd;
            })
            .css("opacity", "1")

        /*
        $.getJSON("api/properties/" + item.id(), function (property) {
            self.propertydetails({ subtype: property.PropertyTypeSub, location: property.Location, price: property.Price, proptype: property.PropertyType, community: property.Community, owner: property.Owner});
        }); */
        //self.propertydetails(ko.utils.arrayFirst(self.properties(), function (i) {return i.id === item.id;}));

        // find index of property item clicked
        var idx = self.properties().findIndex(getIndex, item.id()); // -1, not found    
        self.idx(idx);
    }

    self.remove = function (id) { 
        self.properties.remove(function (property) { return property.PropertyREId === id; });
    };

    self.updateProperty = function (property) {
        var oldProperty = ko.utils.arrayFirst(self.properties(), function (i) {
            return i.id() === property.PropertyREId;
        });
        if (oldProperty) {
            oldProperty.update(property);
            self.propertydetails(property);
        }
    };


    /* Map only */
    self.routes = ko.observableArray([]);
    self.travelMode = ko.observable("DRIVING");
    self.start = ko.observable("pasig, ph");
    self.end = ko.observable("taguig, ph");

    self.showMap = function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(self.app.success, self.app.error);
        } else {
            error('not supported');
        }
    };

    self.calculate = function () {
        self.app.calculateRoute(self.travelMode(), self.start(), self.end());
    };

    self.reload = function (route) {
        self.app.reloadRoute(route, true);
    };

    self.addRoute = function (route) {
        var routeMatch = ko.utils.arrayFirst(self.routes(), function (item) {
            return route.start.latitude == item.start.latitude && route.start.longitude == item.start.longitude &&
                  route.end.latitude == item.end.latitude && route.end.longitude == item.end.longitude && route.travelMode == item.travelMode;
        });

        if (!routeMatch) {
            self.routes.push(route);
        }
    };

    // Method 2: Load initial state from server, convert it to PropertyItem instances, then populate self.items
    $.getJSON("/api/types", function (allData) {
        var mappedItems = $.map(allData, function (item) { return new PropertyItem(item) });
        self.items(mappedItems);
    });

};

$(function () {
    var viewModel = new PropertyViewModel();
    viewModel.app = new RoutePlanner.Application($("#map"), $("#directionsPanel"));

    ko.applyBindings(viewModel);
    /*
    google.maps.event.addListener(viewModel.app.directionsRenderer, "directions_changed", function () {
        var route = viewModel.app.getRoute();
        viewModel.start(route.startAddress);
        viewModel.end(route.endAddress);
        viewModel.travelMode(route.travelMode);
        if (viewModel.app.shouldNotify) {
            planner.server.refreshRoute(route);
        }
        viewModel.addRoute(route);
    }); */

    /*
    planner = $.connection.routeplanner;
    planner.client.refreshRoutes = function (route) {
        viewModel.app.reloadRoute(route);
    };

    $.connection.planner.start(); */

    // signalR
    var hub = $.connection.reprop;

    hub.client.addItem = function (item) {
        viewModel.addPropertyRE(item);
    };
    hub.client.deleteItem = function (id) {
        viewModel.remove(id);
    };
    hub.client.updateItem = function (item) {
        viewModel.updateProperty(item);
    };

    $.connection.hub.start().done(function () {
        viewModel.UserId = $.connection.hub.id;
    });

    /* Method 1: Load initial state from server, convert it to PropertyItem instances, then populate self.items
    $.get("/api/types", function (items) {
        $.each(items, function (idx, item) {
            viewModel.add(item.PropertyTypeId, item.Name);          // field names must be same case as the model
        });
    }, "json"); */

});