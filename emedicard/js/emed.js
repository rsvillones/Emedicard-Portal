$(document).ready(function () {
    $("#tabs").tabs();
    $("#tabPHS").tabs();
    $("#tabECS").tabs();
    $("#tabMFA").tabs();
    $("#tabDCS").tabs();
    $("#tabPEC").tabs();
    $("#tabAgntAccnt").tabs();

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

    $('#lnkReply').parent().parent().next('.box-content').slideToggle();

    $('a.vdetails').fancybox({
        'transitionIn': 'elastic',
        'transitionOut': 'elastic',
        'speedIn': 600,
        'speedOut': 200,
        'overlayShow': false
    });

    function checkfilesize(source, arguments) {

        $('#MainContent_btnUpload').removeAttr("disabled");
        arguments.IsValid = false;

        var axo = new ActiveXObject("Scripting.FileSystemObject");
        thefile = axo.getFile(arguments.Value);
        var size = thefile.size;
        if (size > 5000) {
            arguments.IsValid = false;
        }
        else {
            arguments.IsValid = true;
        }


    }

});