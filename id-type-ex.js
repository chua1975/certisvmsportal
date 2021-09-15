function IdTypeChange(obj) {
    $("#IdNo").text("");
    if ($("#IDType").val() == "FIN" || $("#IDType").val() == "Passport" || $("#IDType").val() == "NRIC") {
        if ($("#IDType").val() == "NRIC") {
            $("#lblFinNRIC").html("NRIC Number<span style='color: red'>*</span>");
            $("#FINNum").attr("placeholder", "NRIC Number");
        } else {
            $("#lblFinNRIC").html("FIN Number<span style='color: red'>*</span>");
            $("#FINNum").attr("placeholder", "FIN Number");
        }

        $("#divExpiryDate").show();
        $("#ExpiryDate").attr("required", "true");
        $("#divEmail").removeAttr("required");
        $("#label_email").css("display", "none");
        $("#label_mobile").css("display", "none");
        $("#MobilePhone").removeAttr("required");
        $("#Email").removeAttr("required");

        if ($("#IDType").val() == "NRIC") {
            $("#label_expirydate").css("display", "none");
            $("#ExpiryDate").removeAttr("required");
        }
        if ($("#IDType").val() == "Passport") {
            $("#PassportNo").attr("required", "true");
        }

        reflashIDNum()
    } else if ($("#IDType").val() == "Email") {
        //$("#divEmail").show();
        $("#divExpiryDate").hide();
        $("#ExpiryDate").removeAttr("required");
        $("#ExpiryDate").val("");
        $("#label_email").css("display", "inline");
        $("#label_mobile").css("display", "none");
        $("#MobilePhone").removeAttr("required");
        $("#Email").attr("required", "true");
        $("#FINNum").val("");
        $("#PassportNo").val("");
        reflashIDNum()

    } else if ($("#IDType").val() == "Phone") {
        //$("#divMobilePhone").show();
        //$("#divMobilePhone").attr("required", "true");
        $("#divExpiryDate").hide();
        $("#ExpiryDate").removeAttr("required");
        $("#ExpiryDate").val("");
        $("#label_email").css("display", "none");
        $("#label_mobile").css("display", "inline");
        $("#MobilePhone").attr("required", "true");
        $("#Email").removeAttr("required");
        $("#FINNum").val("");
        $("#PassportNo").val("");
        reflashIDNum()
    } else {
        $("#divExpiryDate").hide();
        $("#ExpiryDate").removeAttr("required");
        $("#ExpiryDate").val("");
        $("#label_email").css("display", "none");
        $("#label_mobile").css("display", "none");
        $("#MobilePhone").removeAttr("required");
        $("#Email").removeAttr("required");
    }
    if ($("#IDType").val() == "StaffID") {
        $("#label_passId").css("display", "inline");
        $("#StaffPassNo").attr("required", "true");
        reflashIDNum()
    } else {
        $("#label_passId").css("display", "none");
        $("#StaffPassNo").removeAttr("required");
    }


    $("#divFINNum").hide();
    if ($("#IDType").val() == "FIN" || $("#IDType").val() == "NRIC") {
        $("#FINNum").show();
        $("#label_mobile").css("display", "inline");
        $("#MobilePhone").attr("required", "true");
        $("#FINNum").attr("required", "true");
        $("#divFINNum").show();
        reflashIDNum()
    }

    $("#divPassport").hide();
    if ($("#IDType").val() == "Passport") {
        $("#divPassport").show();
    }

}

function reflashIDNum() {
    if ($("#IDType").val() == "Phone") {
        $("#IdNo").text($("#MobilePhone").val());
    } else if ($("#IDType").val() == "Passport") {
        $("#IdNo").text($("#PassportNo").val());
    }
    else if ($("#IDType").val() == "FIN" || $("#IDType").val() == "NRIC") {
        $("#IdNo").text($("#FINNum").val());//$("#MobilePhone").val() +
    } else if ($("#IDType").val() == "Email") {
        $("#IdNo").text($("#Email").val());
    } else if ($("#IDType").val() == "StaffID") {
        $("#IdNo").text($("#StaffPassNo").val());//$("#Company").val() + 
    }

    if ($("#IdNo").text() == "null")
        $("#IdNo").text("");

    $("#hdnIdNo").val($("#IdNo").text());
}

