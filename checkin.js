
$(document).ready(function () {
    var dtHelper = new datatableHelper();

    var columns = [
        "VisitationType",
        "IdNo",
        "PassNumber",
        "Name",
        //"ModelNo",
        "HostName",
        {
            NAME: "DtVisitFrom",
            DATA: "DtVisitFrom",
            RENDER: ChangeDateTimeFormat
        },
        {
            NAME: "DtVisitTo",
            DATA: "DtVisitTo",
            RENDER: ChangeDateTimeFormat
        },
        "Type",
        {
            NAME: "Direction",
            DATA: "Direction",
            RENDER: function (data, type, full, meta) {
                if (full.Direction == true) {
                    return 'Entry';
                } else if (full.Direction == false) {
                    return "Exit";
                } else {
                    return 'NA';
                }
            }
        },
        {
            NAME: "IsOverStay",
            DATA: "IsOverStay",
            RENDER: function (data, type, full, meta) {
                if (full.IsOverStay == false) {
                    return "No";
                } else {
                    return "Yes";
                }
            }
        },
        {
            NAME: "Time",
            DATA: "Time",
            RENDER: ChangeDateTimeFormat
        },
        "DeviceName",
        {
            NAME: "VisitationId",
            DATA: "VisitationId",
            RENDER: function (data, type, full, meta) {
                if (type === 'display') {
                    var buttons = dtHelper.getMainButtonO();
                    buttons += dtHelper.getEditButton("/" + full.VisitationType + "/VisitationEdit/", data);
                    buttons += dtHelper.getMainButtonC();
                    return buttons;
                } else return data;
            }
        }
    ];

    var paras = {
        TABLE: $('#tblData'),
        URL: "/Access/GetJsonData",
        COLUMNS: columns,
        ORDER: [[10, "desc"]],
        COLUMNDEFS: [
            {
                'targets': [12],
                'orderable': false
            },
            {
                'targets': [1, 2, 11],
                'visible': false
            }
        ],
        //    COLUMNDEFS: [{ orderable: false, targets: [3] }]
    };

    var table = dtHelper.createDataTable(paras);

    dtHelper.showExtraSearch();

    $('#txtNRIC').bind("enterKey", function (e) {
        var nric = $("#txtNRIC").val();
        if (nric == null || nric == "") {
            alert("Please enter your Pass number first.");
            return;
        }

        if (nric.indexOf('|') > -1) {
            checkQrCodeContent(nric);
            return;
        }

        apiAc.checkinValidate(nric).then(data => {
            checkVisitResult(data);
        });
    });


    function checkVisitResult(data) {
        console.log("===========/api/certis/checkVisitResult done =============");
        console.log(data);
        if (data.IsSuccess != true) {
            showAlert("#alertbox-Error", data.ServerError);
            return;
        }

        var result = data.Data;
        if (result.HasProfile == false) {
            showAlert("#alertbox-Error", "No Profile found.");
            return;
        }

        if (result.IsValidVisit == false) {
            var reason = result.Reason;
            if (reason != "") {
                showAlert("#alertbox-Error", reason);
            } else {
                showAlert("#alertbox-Error");
            }

            return;
        }
        dataContext.regId = result.RegId;
        dataContext.profileId = result.VisitorID;
        dataContext.isStaff = result.IsStaff;

        showAlert("#alertbox-GrantAccess", null, result.IsStaff);

        $("#divShowVisitorPic").show();
    }

    $('#txtNRIC').keyup(function (e) {
        var nric = $("#txtNRIC").val();
        if (!nric) {
            return;
        }


        if (e.keyCode == 13) {
            $(this).trigger("enterKey");
            return false;
        }

    });





    function checkQrCodeContent(qrContent) {
        apiAc.checkinValidateQr(qrContent).then(data => {
            console.log("[qrcode pass] #1");
            console.log(data);
            if (data.IsSuccess) {
                var qrArr = qrContent.split('|');
                dataContext.profileId = qrArr[9];
                checkVisitResult(data);
                //return saveRegVisitorCheckedIn();
            } else {
                showAlert("#alertbox-Error", data.Reason);
            }
        });
    }



    /*
     * onload ,focus NRIC
     */
    $("#txtNRIC").focus();

    $("#btnSkip").click(function () {
        $("#divShowVisitorPic").hide();
        saveRegVisitorCheckedIn(dataContext.profileId);
    });

    $("#btnUpdatePic").click(function () {

        if ($("#ipt_Photo").val() == "") {
            alert("Please take photo to check in.");
            return;
        }

        var base64 = $("#ipt_Photo").val();
        apiAc.updatePhoto(dataContext.profileId, base64).then(data => {
            console.log("[update visitor pic]");
            console.log(data);
            var msg = "";
            if (data.IsSuccess) {
                msg = "Update Success!";
                saveRegVisitorCheckedIn(dataContext.profileId);
            } else {
                msg = data.ServerError;
            }
            $("#divPhotoRes").show();
            $("#divPhotoRes").html(msg);
        });
    });

    $("#btnRegPass").click(function () {
        console.log("[reg pass] #1");
        var nric = $("#txtNRIC").val();
        var pubPass = $("#txtPassNo").val();
        console.log("[reg pass] #2");
        if (nric && pubPass && nric != "" && pubPass != "") {
            console.log("[reg pass] #3");
            apiAc.regPass(pubPass, dataContext.regId).then(data => {
                console.log("[reg pass] #5");
                console.log("================RegisterVisitorPass done==============");
                console.log(data);
                toastr.success('Thank you.');
                setTimeout(function () {
                    window.location = GetBaseUrl() + "/Access";
                }, 500);

            });
        } else {
            alert("NRIC or Pass # cannot be empty.");
        }
    });

    function saveRegVisitorCheckedIn() {
        apiAc.checkinSaveState(dataContext.regId).then(data => {
            console.log("[SaveRegVisitorCheckedIn]");
            console.log(data);
            var msg = "";
            if (data.IsSuccess) {
                if (config.NeedChangePass.toLowerCase() == "true" &&
                    dataContext.isStaff == false) {
                    showAlert("#alertbox-GrantAccess", null, dataContext.isStaff);
                    $("#alertbox-GrantAccess-Message").show();
                } else {
                    toastr.success('Thank you.');
                    setTimeout(function () {
                        window.location = GetBaseUrl() + "/Access";
                    }, 500);
                }
                return;
            } else {
                msg = data.ServerError;
            }
            alert(msg);
            $("#divPhotoRes").show();
            $("#divPhotoRes").html(msg);
        });
    }
});

function ifIdleReload() {
    var showingMsg = $(".alert :visible").length;
    var now = moment();
    var copyTime;
    copyTime = moment(dataContext.lastMsgTime);

    if (showingMsg > 0 && dataContext.lastMsgTime && copyTime.add(30, 'seconds') < now) {
        window.location = GetBaseUrl() + "/Access";
    } else {
        console.log(dataContext.lastMsgTime);
        setTimeout(ifIdleReload, 5 * 1000);
    }
}

ifIdleReload();
nricField("txtNRIC");

