(function ($, AdminLTE) {
    $('#logTable').DataTable({
        "ajax": {
            'url':'/Service/Handler.ashx?fn=2',
            'dataSrc':'data',
        },        
        "columns": [
            { "data": "AddOn" },
            { "data": "Msg" },
            { "data": "Params" }
        ]
    });
})(jQuery, $.AdminLTE);