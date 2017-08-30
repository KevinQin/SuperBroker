$(document).ready(
    $(function () {
        $('input').iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-blue',
            increaseArea: '20%' // optional
        });

        $("#txtAccount").on("keyup", AccountCheck);
        $("#txtPassword").on("keyup", PasswordCheck);
        $("#txtvcode").on("keyup", VcodeCheck);
        $("#imgCode").on("click", function () {
            $("#imgCode").attr("src", "img.aspx?v=" + new Date().getMilliseconds());
        });

        $("#btnLogin").on("click", DoLogin);

        $("#txtAccount").on("keyup", onEnterKeyPress);
        $("#txtPassword").on("keyup", onEnterKeyPress);
        $("#txtvcode").on("keyup", onEnterKeyPress);
        $("#btnLogin").on("keyup", onEnterKeyPress);
        $(document).on("keyup", onEnterKeyPress);
    })
);

function onEnterKeyPress(event) {
    if (event.keyCode == 13) {
        DoLogin();
    }    
}

function DoLogin() {
    var account = $("#txtAccount").val();
    var password = $("#txtPassword").val();
    var vcode = $("#txtvcode").val();
    if (account == "" || password == "" || vcode.length != 5) {
        if (account == "") { AccountCheck(); }
        if (password == "") { PasswordCheck(); }
        if (vcode.length != 5) { VcodeCheck(); }
    }
    else {
        $ajax({ fn: 1, account: account, password: password, vcode: vcode }, function (o) {
            if (o.Return == 0) {
                _setLogin(o.data,o.Ext);
                $go("default.html");
            }
            else {
                if (o.SubCode == 2) {
                    $("#txtvcode").val("");
                    VcodeCheck();
                }
                else {
                    $("#txtPassword").val("");
                    PasswordCheck();
                }                
            }
        }, true);
    }
}

function AccountCheck() {
    var account = $("#txtAccount").val();
    if (account == "") {
        $(".form-group:eq(0)").addClass("has-error");
        $(".help-block:eq(0)").show();
    }
    else {
        $(".form-group:eq(0)").removeClass("has-error").addClass("has-success");
        $(".help-block:eq(0)").hide();
    }
}

function PasswordCheck() {
    var password = $("#txtPassword").val();
    if (password == "") {
        $(".form-group:eq(1)").addClass("has-error");
        $(".help-block:eq(1)").show();
    }
    else {
        $(".form-group:eq(1)").removeClass("has-error").addClass("has-success");
        $(".help-block:eq(1)").hide();
    }
}

function VcodeCheck() {
    var vcode = $("#txtvcode").val();
    if (vcode.length != 5) {
        $(".form-group:eq(2)").addClass("has-error");
        $(".help-block:eq(2)").show();
    }
    else {
        $(".form-group:eq(2)").removeClass("has-error").addClass("has-success");
        $(".help-block:eq(2)").hide();
    }
}
