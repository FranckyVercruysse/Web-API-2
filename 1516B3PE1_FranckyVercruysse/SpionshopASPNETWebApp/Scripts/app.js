var Uri = 'http://spionshopapi2.azurewebsites.net/';
var categoriesUri = 'http://spionshopapi2.azurewebsites.net/api/Categorie/';
var artikelsUri = 'http://spionshopapi2.azurewebsites.net/api/Artikels/';
var bestellingUri = 'http://spionshopapi2.azurewebsites.net/api/Bestelling/';

var accountUri = '/Account/GetSessionVariable';
var loginUri = '/Account/Login';
var klantDTO = undefined;       // variable om een klantDTO op te slaan, wordt ook gebruikt om authenticatie te testen
// moet niet observable zijn omdat die variable niet gebruikt wordt in de user interface

var ViewModel = function () {
    var self = this;
    self.error = ko.observable();
    self.categories = ko.observableArray();     // deze observable array zal objecten van CategorieDTO ( Cat_id,Categorie1) bevatten 
    self.artikelsPerCategorie = ko.observableArray();  //  deze observable array zal objecten van ArtikelDTO (Artikel_id,Artikel1,Omschrijving,Verkoopprijs,...)
    self.artikeldetail = ko.observable();   // bevat een ArtikelDTO 
    self.Aantal = ko.observable(1);         // aantal dat ingegeven kan worden bij detailweergave

    self.getArtikelsPerCategorie = function (item) {        // item bevat een CategorieDTO (item.Cat_id, item.Categorie1)
        ajaxHelper(categoriesUri + item.Cat_id, 'GET').done(function (data) {
            self.artikelsPerCategorie(data);
        }
    );
    }

    function getAllCategories() {
        ajaxHelper(categoriesUri, 'GET').done(function (data) {
            self.categories(data);
        });
    }

    self.getDetail = function (item) {
        isAuthenticated();  // indien niet geauthenticeerd, dan vragen aan de gebruiker om zich in te loggen of te registreren
        if (!(klantDTO == undefined))
        {
            self.Aantal(1);                         
            ajaxHelper(artikelsUri + item.Artikel_id, 'GET')
                .done(function (data) {
                    self.artikeldetail(data);
                });
        }
        else
        {
            login();
        }
    }

    // WinkelMandje begin               // http://knockoutjs.com/examples/cartEditor.html
    self.lijnen = ko.observableArray();
    self.grandTotal = ko.computed(function () {
        var total = 0;
        $.each(self.lijnen(), function () { total += this.subtotaal() })
        return total;
    });

    // Operations winkelmandje
    self.addLine = function (data) {
        var lijn = new Lijn();
        lijn.artikelMandje(self.artikeldetail().Artikel1);
        lijn.prijsMandje(self.artikeldetail().Verkoopprijs);

        lijn.Artikel_id(self.artikeldetail().Artikel_id);
        lijn.aantalMandje(this.Aantal());
        self.lijnen.push(lijn);
        self.artikeldetail(null);
    };

    self.removeLine = function (lijn) { self.lijnen.remove(lijn) };

    self.bestel = function () {
        var bestellingen = $.map(self.lijnen(), function (lijn) {
            return (lijn.artikelMandje() ? {
                Artikel_id: lijn.Artikel_id(),
                artikel: lijn.artikelMandje(),
                Aantal: lijn.aantalMandje(),
            } : undefined)
        });
        var bestellingDto = {
            Klant_id: klantDTO.Klant_id,
            Bestellingen: bestellingen,
        };

        //alert(bestellingUri + JSON.stringify(bestellingDto));

        ajaxHelper(bestellingUri, 'POST', bestellingDto);
        self.lijnen.removeAll();        // maakt het winkelmandje leeg
    };

    var Lijn = function () {        // een lijn in winkelmandje
        var self = this;

        self.Artikel_id = ko.observable();
        self.artikelMandje = ko.observable();
        self.aantalMandje = ko.observable(1);
        self.prijsMandje = ko.observable();

        self.subtotaal = ko.pureComputed(function () {
            return self.artikelMandje() ? parseInt("0" + self.aantalMandje(), 10) * self.prijsMandje() : 0;
        });
    };
    // WinkelMandje einde

    self.GetThumbs = function (Artikel_id) {
        var d = new Date();                                     // disable cache for images     ???????????? Wanneer een figuur aangepast wordt in de wpf toepassing duurt het te lang vooraleer de website aangepast wordt
        return Uri + '/Images/thumbs/' + Artikel_id + '.gif?d.getTime()';     //  '/Images/thumbs/'+ Artikel_id +'.gif' ?dummy=371662 no cache
    }

    self.GetGroteAfbeelding = function (Artikel_id) {
        var d = new Date();                                                 // disable cache for images     ????????????
        return Uri + '/Images/' + Artikel_id + '.gif?d.getTime()';     //  '/Images/thumbs/'+ Artikel_id +'.gif'
    }

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                self.error(errorThrown);
            });
    }

    // Fetch the initial data.
    getAllCategories();
};

ko.applyBindings(new ViewModel(), document.getElementById('spionshop'));



//////////////////////////////////////////////////
//             functies                         //
//////////////////////////////////////////////////

function formatCurrency(value) {
    if (value == null) value = 0;
    if (value > 10000) { return "€" + value.toFixed(0) };
    return "€" + value.toFixed(2);
}


// Authenticatie begin
function isAuthenticated() {
    $.ajax({
        type: "POST",
        url: accountUri,
        data: {},
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        success: successFunc
    });
};

function successFunc(data, status) {
    klantDTO = data;
    //if (klantDTO == undefined) { alert("klantDTO = undefined"); } else { alert("klantDTO not undefined");}
}

function errorFunc() {
    //if (klantDTO == undefined) { alert("klantDTO = undefined"); } else { alert("klantDTO not undefined"); }
}

function login() {
    window.location = location.protocol + '//' + location.host + '/Account/Login';
    var form = $('#__AjaxAntiForgeryForm');                                         //????? werkt niet altijd ???
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    
    //alert('token =' + token);
    $.ajax({
        url: loginUri,
        dataType: "json",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        cache: false,
        data: { __RequestVerificationToken: token },                        //  //  http://stackoverflow.com/questions/14473597/include-antiforgerytoken-in-ajax-post-asp-net-mvc
        //headers: {
        //    //'RequestVerificationToken': '@TokenHeaderValue()'       //  //(http://www.asp.net/web-api/overview/security/preventing-cross-site-request-forgery-csrf-attacks)
        //}
    });
}
// Authenticatie einde
