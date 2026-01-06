$(document).ready(function () {

    //START - Function to disable mailing address fields if mailing address is same as headquarter address
    //$("#address-check").click(function () {
    $("#Megegyezik").click(function () {
        if ($(this).prop("checked")) {
            $("#mailing-address-wrapper").addClass("disabled");
            $("#mailing-address-wrapper input").prop('readonly', true);
            AddressMapping();
        }
        else {
            $("#mailing-address-wrapper").removeClass("disabled");
            $("#mailing-address-wrapper input").prop('readonly', false);
            
        }
    });
    //END - Function to disable mailing address fields if mailing address is same as headquarter address

    //START - Bindig event to all input and mapping data using AddressMapping function if mailing address must be same to headquarter address
    $('html').bind('input', function () {
        AddressMapping();
    });
    //END - Bindig event to input and mapping data using AddressMapping function if mailing address must be same to headquarter address

    //START - Function to map data from headquerter address to mailing address
    function AddressMapping() {
        //if ($("#address-check").prop("checked")) {
        if ($("#Megegyezik").prop("checked")) {

            var KapcsolatID = $("KapcsolatTartoID");


            //elsodleges adatok
            let nev = "#CegNev";
            let adoszam = "#CegAdoszam";
            let szamlaszam = "#CegSzamlaszam";
            let meghatalmazott = "#CegMeghatalmazott";

            //cim adatok
            let orszag = "#CegOrszag";
            let varos = "#CegVaros";
            let utca = "#CegUtca";
            let szam = "#CegSzam";
            let egyeb = "#CegEgyeb";

            //elerhetosegek
            let telszam1 = "#CegTelszam1";
            let telszam2 = "#CegTelszam2";
            let email = "#CegEmail";
            let web = "#CegWeb";
            let megjegyzes = "#CegMegjegyzes";

            //TEST
            let partnerID = "#TempInt";

            $(partnerID).val($("#KapcsolatTartoID").val());

            $(nev).val($("#Nev").val());
            $(adoszam).val($("#AdoSzam").val());
            $(szamlaszam).val($("#SzamlaSzam").val());
            $(meghatalmazott).val($("#Nev").val());

            $(orszag).val($("#Orszag").val());
            $(varos).val($("#Varos").val());
            $(utca).val($("#Utca").val());
            $(szam).val($("#Szam").val());
            $(egyeb).val($("#Egyeb").val());

            $(telszam1).val($("#Telszam1").val());
            $(telszam2).val($("#Telszam2").val());
            $(email).val($("#Email").val());
            $(web).val($("#Web").val());
            $(megjegyzes).val($("#Megjegyzes_Kontakt").val());


            $(nev + " + span").empty();
            $(adoszam + " + span").empty();
            $(szamlaszam + " + span").empty();
            $(meghatalmazott + " + span").empty();


            $(orszag + " + span").empty();
            $(varos + " + span").empty();
            $(utca + " + span").empty();
            $(szam + " + span").empty();
            $(egyeb + " + span").empty();

            $(telszam1 + " + span").empty();
            $(telszam2 + " + span").empty();
            $(email + " + span").empty();
            $(web + " + span").empty();
            $(megjegyzes + " + span").empty();
        }
    }
    //END - Function to map data from headquerter address to mailing address


});