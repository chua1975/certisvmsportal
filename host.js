function SelectHost(id, name, hostContact) {
    GetHostPosition(id);
    $("#basic").modal("hide");
    if (updateTo == "Host") {
        $("#HostName").val(unescape(name));
        $("#HostId").val(id)
        $("#HostContact").val(hostContact)
        if ($("#notification").attr("Notification").slice(0, 4) == "HOST") {
            $("#notification").text(unescape(name) + $("#notification").attr("Notification").slice(4))
        }
    } else {
        $("#AuthorizedBy").val(unescape(name));
    }

}

var intiTable = false;
var updateTo = "Host";
function ShowHosts(param) {
    updateTo = param;
    if (intiTable) return;

    intiTable = true;
    var columns = [
        {
            NAME: "StaffId",
            DATA: "StaffId"
        },
        "StaffName",
        {
            NAME: "StaffMobile",
            DATA: "StaffMobile",
            RENDER: function (data, type, full, meta) {
                return showHalfStar(data);
            }
        },
        {
            NAME: "Email",
            DATA: "Email",
            RENDER: function (data, type, full, meta) {
                return showHalfStar(data);
            }
        },
        {
            NAME: "StaffId",
            DATA: "StaffId",
            RENDER: function (data, type, full, meta) {
                var hostContact = "";
                if (full.IdType == "Email") {
                    hostContact = full.Email;
                } else { hostContact = full.StaffMobile; }
                
                return "<input type='button' class='btn btn_blue'   onclick=SelectHost('" + data + "','" + escape(full.StaffName) + "','" + showHalfStar(hostContact) + "') value='Select'/> "
            }
        }
    ];

    var paras = {
        TABLE: $('#tblData'),
        URL: "/Staff/GetHosts",
        COLUMNS: columns,
        ORDER: [[1, "asc"]],
        COLUMNDEFS: [
            {
                'targets': [4],
                'orderable': false
            }
        ],
       // "ordering": false,
        //    COLUMNDEFS: [{ orderable: false, targets: [3] }]
    };

    var table = dtHelper.createDataTable(paras);

}
function GetHostPosition(hostid) {
    $.ajax({
        type: "POST",
        data: { "hostid": hostid },
        url: GetBaseUrl() + '/staff/GetHostPosition',
        success: function (data) {

            $("#selectPositionID").val(data);
            app.GetPositionJson();

        }
    });
}