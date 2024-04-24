$(document).ready(function () {

    $('#form1').submit(function () {

        $('#MainContent_cboPrincipalStatus').prop('selectedIndex', 0);
        $('#MainContent_txtPrincipalCode').val('');
        $('MainContent_rdDepBirthdate').val('');
        $('#MainContent_cboPrincipalCode').hide();
        $('#MainContent_txtPrincipalCode').show();

    });

})