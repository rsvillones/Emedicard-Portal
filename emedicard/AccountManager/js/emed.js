$(document).ready(function () {
    $("#tabs").tabs();
    $("#MemEndorsement").tabs({ selected: 1 });
    $("#RequestList").tabs();
    $("#subtabs").tabs();
    $("#tabPHS").tabs();
    $("#tabECS").tabs();
    $("#tabMFA").tabs();
    $("#tabDCS").tabs();
    $("#tabPEC").tabs();


    $('.EditPlan').live('click', function (e) {
        var wWidth = $(window).width();
        var dWidth = wWidth * 0.8;
        InitializeDialog($("#divPlan"), $(this).attr("href"), "Plans to utilize", 800, 600);
        e.preventDefault();
        $("#divPlan").dialog("open");
    });

    //Method to Initialize the DialogBox
    function InitializeDialog($element, page, title_label, w, h) {

        $element.dialog({
            autoOpen: true,
            width: w,
            height: h,
            resizable: false,
            draggable: true,
            title: title_label,
            modal: true,
            show: 'fold',
            closeText: 'x',
            //dialogClass: 'alert',
            closeOnEscape: true,
            position: "center",
            buttons: {
                "Print": {
                    text: 'Save',
                    click: function () {
                        // do stuff
                        alert('Hello');
                    }
                },
                "Close": {
                    text: 'Close',
                    click: function () {
                        // do stuff
                        $('#divPlan').dialog('close');

                    }
                }
            },
            open: function (event, ui) {
                $element.load(page);
            },

            close: function () {
                $('#divPlan').dialog('close');

            }
        });
    }

    $("#chkALL").live('click', function (e) {
        if ($(this).is(':checked')) {
            $('.case').attr('checked', true);
        } else {
            $('.case').attr('checked', false);
        }

    });

    $(".case").live('click', function (e) {

        if ($(".case").length == $(".case:checked").length) {
            $("#chkALL").attr("checked", "checked");
        } else {
            $("#chkALL").removeAttr("checked");
        }

    });

    $('.case').removeAttr('class');

    $('#MainContent_ddlSubject').change(function () {
        if ($(this).val() == 'Other') {

            $('#MainContent_txtSubject').removeAttr("disabled");

        } else {

            $('#MainContent_txtSubject').val("");
            $('#MainContent_txtSubject').attr("disabled", "disabled");

        }
    });

    $('#MainContent_ddlService').on('change', function () {

        if (this.value == 'util_all_service') {
            $('#memcode').show();
        } else {
            $('#memcode').hide();
        }


    });

    $('#lnkReply').parent().parent().next('.box-content').slideToggle();

    $('.txtPrinCode').blur(function () {
        var memcode = $('#MainContent_txtPrincipalCode').val();
        if (memcode != '') {
            CheckPrincipalValidity();
        }
    });

    function CheckPrincipalValidity() {
        $.ajax({
            type: "POST",
            url: "Endorsement.aspx/CheckPrinStatus",
            data: '{sMemCode: "' + $('#MainContent_txtPrincipalCode').val() + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: IsActiveMember,
            error: function (response) {
                alert(response.d);
            }
        });
    }

    function IsActiveMember(response) {
        var obj = $.parseJSON(response.d);
        if (obj.MemberCode == '') {
            $('#prinvalidator').css({ "color": "red", "display": "inline-block" });
            $('#prinvalidator').empty().append('Invalid member code.')

        } else {
            $('#prinvalidator').empty().append(obj.first_name + ' ' + obj.last_name)
            $('#prinvalidator').css({ "color": "gray", "display": "inline-block" });
        }
    }

    $('#MainContent_cboPrincipalStatus').live("change", function () {
        if ($(this).val() == 1) {
            $('#prinvalidator').empty().css({ "display": "none" });
            $('#MainContent_cboPrincipalCode').show();
            $('#MainContent_txtPrincipalCode').hide();
            $('#MainContent_cboPrincipalCode').empty();

            $.ajax({
                type: "POST",
                url: "Endorsement.aspx/GetListOfPrincipals",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var obj = $.parseJSON(response.d);
                    $.each(obj, function (i, item) {
                        $("#MainContent_cboPrincipalCode").append('<option value=' + item.value + '>' + item.description + '</option>');
                    }); // end each
                },
                error: function (response) {
                    alert(response.d);
                }
            });

        } else {
            $('#MainContent_cboPrincipalCode').hide();
            $('#MainContent_txtPrincipalCode').show();
        }
    });

    $('#MainContent_cboPrincipalCode').hide();

    $('#MainContent_btnSaveDependent').live('click', function () {
        if (Page_IsValid) {
            $('#MainContent_cboPrincipalStatus').prop('selectedIndex', 0);
            $('#MainContent_txtPrincipalCode').val('');
            $('#MainContent_rdDepBirthdate').val('');
            $('#MainContent_cboPrincipalCode').hide();
            $('#MainContent_txtPrincipalCode').show();
            $('#prinvalidator').empty();
        }
    });


    $('a.vdetails').fancybox({
        'transitionIn': 'elastic',
        'transitionOut': 'elastic',
        'speedIn': 600,
        'speedOut': 200,
        'width': 450,
        'autoDimensions': false,
        'overlayShow': false
    });

});