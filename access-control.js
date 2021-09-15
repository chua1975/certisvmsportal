// fingerprint logic
var grantedAudio = new Audio(GetBaseUrl() + '/assets3/sound/granted_TY.wav');
var rejectedAudio = new Audio(GetBaseUrl() + '/assets3/sound/rejected_pls_review_reg.wav');
//var scanoutAudio = new Audio(GetBaseUrl() + '/assets3/sound/scannedout_TY.wav');
var scanoutAudio = new Audio(GetBaseUrl() + '/assets3/sound/good-bye.wav');
var scanfingerAudio = new Audio(GetBaseUrl() + '/assets3/sound/please-place-your-finger.wav');
var scanSecondfingerAudio = new Audio(GetBaseUrl() + '/assets3/sound/enrollment_successful_please_scan.wav');

//TODO save all these in settings
var threshold = 10;
//TODO save all these in settings


var showAlert = function (boxId, message, isStaff) {

    ////TODO Fix this .
    console.log(dataContext.lastMsgTime);

    $(".alertbox").hide();

    if (message) {
        $(boxId + "-Message").html(message);
    }

    console.info(boxId);
    $(boxId).show();

    if (boxId.includes("-Error")) {
        $("#txtNRIC").val("");
        $("#txtNRIC").focus();
        rejectedAudio.play();

        dataContext.lastMsgTime = moment();
    }
    debugger;

    if (boxId.includes("GrantAccess")) {
        if (isStaff) {
            // $("#txtPassNo").prop("readonly", "readonly");
            //do nothing
        } else {
            $("#alertbox-GrantAccess-Message").show();
            $("#txtPassNo").val("");
            $("#txtPassNo").focus();
        }

        console.log("PUB pass textbox should be focused.");
        $('#txtPassNo').keyup(function (e) {
            if (e.keyCode == 13) {
                var selfVal = $(this).val();
                console.log(selfVal);
                if (selfVal != "") {
                    $("#btnRegPass").click();
                }
            }
        });
        grantedAudio.play();

        dataContext.lastMsgTime = moment();
    }
};
showAlert("#alertbox-Info");// hide all after loaded page
$("#pnlScanFinger").hide(); // hide scan finger div

var comparefingerFlow = function(leftBase64,
    rightBase64,
    isOut,
    isStaff,
    passno,
    resultObj) {
    var leftFinger = leftBase64;
    var rightFinger = rightBase64;

    $("#pnlScanFinger").show();
    $("#lblScanFinger").html("Scan Your Finger");
    scanfingerAudio.play();

    //  loadingWithTimeout();
    apiAc.getFingerData().then(data => {
        debugger;
        var obj = JSON.parse(data);

        $("#pnlScanFinger").hide();
        console.log("=========== capture finger done ==========");
        console.log(obj);
        //  debugger;
        if (obj.IsSuccess == true) {
            var capturedFinger = obj.FingerBase64;
            if (capturedFinger == "" || capturedFinger.length == 0) {
                showAlert("#alertbox-Error", "No finger data detected. Please try scan again.");
                return;
            }

            apiAc.compareFingerData(leftFinger, rightFinger, capturedFinger).then(data => {
                console.log("=========== comparing finger done ==========");
                console.log(data);
                var result = JSON.parse(data);
                console.log(isOut + "," + (isOut === true));

                if (result.Score > threshold) {
                    apiAc.movement(dataContext.profileId, dataContext.isStaff).then(data => {
                        if (isOut && isOut == true) {
                            //var pass = $("#txtNRIC").val();
                            //apiAc.regPass(pass, dataContext.regId).then(data => {
                            //    console.log("[exit reg finger] #3");
                            //    console.log(
                            //        "================RegisterVisitorPass done==============");
                            //    console.log(data);

                            //    scanoutAudio.play();
                            //    toastr.success('Good Bye.');
                            //    setTimeout(function () {
                            //        window.location = GetBaseUrl() + "/Access";
                            //    }, 1500);
                            //});

                            scanoutAudio.play();
                            toastr.success('Good Bye.');
                            setTimeout(function () {
                                window.location = GetBaseUrl() + "/Access";
                            }, 1500);

                        } else {
                            if (config.NeedChangePass.toLowerCase() == "true" &&
                                dataContext.isStaff == false) {
                                showAlert("#alertbox-GrantAccess", null, dataContext.isStaff);
                                $("#alertbox-GrantAccess-Message").show();
                            } else {
                                grantedAudio.play();
                                toastr.success('Thank you.');
                                setTimeout(function () {
                                    window.location = GetBaseUrl() + "/Access";
                                }, 2000);
                            }
                        }
                    });

                } else {
                    showAlert("#alertbox-Error", "Fingerprint mismatch.");
                }
            });

        } else {
            alert(obj.Error);
        }

    });
};


var regFingerFlow = function () {
    $(".alertbox").hide();
    $("#pnlScanFinger").show();
    $("#lblScanFinger").html("<span class='label label-success'>Scan Your 1st Finger</span>");
    scanfingerAudio.play();

    //1st finger data
    apiAc.getFingerData().then(data => {
        var obj = JSON.parse(data);
        if (obj.IsSuccess == true) {
            if (obj.FingerBase64 == "" || obj.FingerBase64.length == 0) {
                showAlert("#alertbox-Error", "No finger data detected. Please try scan again.");
                return;
            }

            var base64_1 = obj.FingerBase64;
            $("#lblScanFinger").html("<span class='label label-primary'>Scan Your 2nd Finger</span>");
            toastr.success('Enrollment Successful! Please scan your 2nd Finger.');
            scanSecondfingerAudio.play();

            //2nd finger data
            apiAc.getFingerData().then(data2 => {
                var obj2 = JSON.parse(data2);
                if (obj2.IsSuccess == true) {
                    var base64_2 = obj2.FingerBase64;
                    if (obj2.FingerBase64 == "" || obj2.FingerBase64.length == 0) {
                        showAlert("#alertbox-Error",
                            "No finger data detected. Please try scan again.");
                        return;
                    }

                    //Show Pass Number 

                    // save both finger print in server
                    var idNo = $("#txtNRIC").val();
                    apiAc.regFinger(idNo, base64_1, base64_2).then(data => {
                        //apiAc.movement(dataContext.profileId, dataContext.isStaff).then(data2 => {
                        //    if (data.IsSuccess == true) {
                        //        showAlert("#alertbox-GrantAccess");
                        //        alert("Thank you.");
                        //        window.location = GetBaseUrl() + "/Access";
                        //    } else {
                        //        showAlert("#alertbox-Error");
                        //    }
                        //});
                        toastr.success('Enrollment Successful!');
                        $("#alertbox-GrantAccess-Message").show();
                        showAlert("#alertbox-GrantAccess", null, dataContext.isStaff);
                    });
                } else {
                    showAlert("#alertbox-Error", obj2.ServerError);
                }
            });

        } else {
            showAlert("#alertbox-Error", obj.ServerError);
        }
    });
}