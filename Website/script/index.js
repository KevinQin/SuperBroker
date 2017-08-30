(function ($, AdminLTE) {
    $.widget.bridge('uibutton', $.ui.button);   
    setup();  
   
    function setup() {  
        if ($get("workno") == "") {
            $go("login.html");
        }
        
        var photoUrl="/images/defphoto.jpg";
        $("img[data-img]").attr("src", photoUrl);
        $("p[data-name]").text($get("Name"));
        $("span[data-name]").text($get("Account"));
        $("#uDetail").html($get("Name") + "<small>" + $get("DepartmentName") + " - " + $get("RoleName") + "</small>")
        $("#lnkLogout").on("click", loginOut);

        $("#btnShowChgPwdDlg").on("click", function () {
            $("#changePwdDlg").modal("show");
            $("#btnSaveOk").unbind().on("click", function () {
                var oldPwd = $("#txtOldPwd").val();
                var newPwd = $("#txtNewPwd").val();
                var rePwd = $("#txtConformPwd").val();
                if (oldPwd == "") {
                    showToast("请输入原始密码");
                    return;
                }

                if (newPwd != rePwd) {
                    showToast("两次密码输入不同，请确认");
                    $("#txtConformPwd").focus();
                    return;
                }

                $ajax({ fn: 3, oldpwd: oldPwd, newPwd: newPwd, workno: $get("WorkNo") }, function (o) {
                    if (o.Return == 0) {
                        $("#changePwdDlg").modal("hide");
                        showToast("密码修改成功");
                        $("#txtOldPwd").val("");
                        $("#txtNewPwd").val("");
                        $("#txtConformPwd").val("");
                    } else {
                        showToast("原始密码错误，请确认");
                    }
                }, true);
            });
        });

    }
    function loginOut() {
        _setLogout();
        $go("login.html");
    }    
})(jQuery, $.AdminLTE);