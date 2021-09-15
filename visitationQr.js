function getVisitationQrCode(fun, id, isStaff, printData, printUrl, generatingErrMess) {
    $.ajax({
        type: "POST",
        data: {
            "idNo": id,
            "isStaff": isStaff,
        },
        url: GetBaseUrl() + "/QrcodeApi/GetQr",
        success: function (data) {
            if ((typeof data.IsSuccess == "undefined") || (!data.IsSuccess)) {
                showMessage('error', 'Failure', generatingErrMess);
                return false;
            }

            console.log(data.RawContent);

            if (fun == "View") {
                $("#imgQrCode").attr("src", "data:image/jpeg;base64," + data.Base64);
                $('#qrViewModel').modal('show');
            } else {
                $("#qrcode").html("<img src='data:image/jpeg;base64," + data.Base64 + "' />");
                printData.QR = data.Base64;
                $.ajax({
                    type: "POST",
                    data: printData,
                    url: printUrl,
                });
            }
        }
    });
}