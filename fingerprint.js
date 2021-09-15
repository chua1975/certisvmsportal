var scanfingerAudio = new Audio(GetBaseUrl() + '/assets3/sound/please-place-your-finger.wav');

var urlRegFinger = ''; 

var regFingerFunc = function (ctl) {
    scanfingerAudio.play();
    $.get(urlRegFinger, function (data, status) {
        var obj = JSON.parse(data);
        console.log(obj);
        if (obj.IsSuccess == true) {
            $(ctl).val(obj.FingerBase64);
            refreshFingerLabel();
        } else {
            alert(obj.Error);
        }

    });
};


$("#btnEnrollLeft").click(function () {
    $("#hdnLeftFinger").val("");
    $("#lblLeftFinger").removeClass("label label-success").html("Not Enrolled");
    regFingerFunc("#hdnLeftFinger");
});
$("#imgLeftFinger").click(function () {
    $("#hdnLeftFinger").val("");
    $("#lblLeftFinger").removeClass("label label-success").html("Not Enrolled");
    regFingerFunc("#hdnLeftFinger");
});

$("#btnEnrollRight").click(function () {
    $("#hdnRightFinger").val("");
    $("#lblRightFinger").removeClass("label label-success").html("Not Enrolled");
    regFingerFunc("#hdnRightFinger");
});
$("#imgRightFinger").click(function () {
    $("#hdnRightFinger").val("");
    $("#lblRightFinger").removeClass("label label-success").html("Not Enrolled");
    regFingerFunc("#hdnRightFinger");
});


// finger print events handling
var refreshFingerLabel = function () {
    var leftData = $("#hdnLeftFinger").val();
    var rightData = $("#hdnRightFinger").val();

    if (leftData != "") {
        $("#lblLeftFinger").html("Enrolled");
        //$("#lblLeftFinger").addClass("label label-success");
        $("#imgLeftTick").attr("src", GetBaseUrl() + "/assets3/layouts/layout/img/green_tick1.png");
    } else {
        $("#lblLeftFinger").html("Not Enrolled");
    }
    if (rightData != "") {
        $("#lblRightFinger").html("Enrolled");
        //$("#lblRightFinger").addClass("label label-success");
        $("#imgRightTick").attr("src", GetBaseUrl() + "/assets3/layouts/layout/img/green_tick1.png");
    } else {
        $("#lblRightFinger").html("Not Enrolled");
    }
};

refreshFingerLabel();

fetch(GetBaseUrl() + "/Config/GetFingerPrintApi_Reg/",
    {
        method: "GET",
        cache: "no-cache"
    }).then(function (response) {
        return response.text();
    }).then(function (response) {
        urlRegFinger = response;
    });

function IsBioCheckEnableForVisitor() {
    fetch(GetBaseUrl() + "/VisitorType/IsBioCheckEnable/", {
        method: "GET",
        cache: "no-cache"
    }).then(function (response) {
        return response.text();
    }).then(function (response) {
        if (response == "True") {
            $("#divLeftFinger").show();
            $("#divRightFinger").show();
        } else {
            $("#divLeftFinger").hide();
            $("#divRightFinger").hide();
        }
    });
}