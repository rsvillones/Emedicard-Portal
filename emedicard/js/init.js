$.fn.modal = function(){
	var th = $(this);
		$('button').on('click', function(e){
			e.preventDefault();

			if($(this).attr("href") == ('#'+th.attr('id')) ){
				th.toggle();
			}
		});
		
};


$(document).ready(function () {


    //    $("#modal_window").modal();

    //    $('.view-dtl').click(function () {
    //        //$('#modal_content').load($(this).attr('href'));
    //        $('#modal_content').load('page2.html');
    //    });
    if ($.browser.msie) {
        if ($.browser.version == 10.0) {
            $("#dialog").removeAttr('style');
            $("#dialog").dialog();
        }
    }

});
