
function ChangeDateFormat(value) {
    if (value === null || value == "") return "";
    return moment(value).format("DD/MM/YYYY");
}

function ChangeDateTimeFormat(value) {
    if (value === null || value == "") return "";
    return moment(value).format("DD/MM/YYYY HH:mm:ss");
}

function ChangeTimeFormat(value) {
    if (value === null) return "";
    return moment(value).format("HH:mm:ss");
}


function appendReportSearchDateRange(f, t) {
    var dtFrom = new moment().add(-30, 'days').format("DD/MM/YY");
    var dtTo = new moment().add(7, 'days').format("DD/MM/YY");
    $("#dateStart").val(dtFrom);
    $("#dateEnd").val(dtTo);

    $(".datepicker1").datetimepicker({
        viewMode: "days",
        format: "DD/MM/YY",
    });


    var onChangeDate = function () {
        var start = $("#dateStart").val().split('/').reverse().join("");
        if (start.length > 2 && start.length == 6) {
            start = "20" + start;
        }
        var end = $("#dateEnd").val().split('/').reverse().join("");
        if (end.length > 2 && end.length == 6) {
            end = "20" + end;
        }

        if (start > end) {
            showMessage('error', 'Failure', 'start Date must before end date');
            var from = dtFrom;
            var to = dtTo;

            $("#dateStart").val(from);
            $("#dateEnd").val(to);
            return;
        }

        //debugger;
        $("#tblData").dataTable().fnUpdate();
        //table.state.save();
    }

    $("#dateStart").on('dp.change', onChangeDate);
    $("#dateEnd").on('dp.change', onChangeDate);
}

function getYesNo(c) {
    return c == "Y" || c == "1" ? "Yes" : "No";
}
function getActiveStatus(s) {
    return s == "A" ? "Active" : "Inactive";
}
var constants = {

}

function GetChangePasswordButton(dLink, data) {
    return '<li><a href="' + GetBaseUrl() + dLink + data + '" "><i class="icon-detail fa fa-edit"></i>Change Password</a></li>';
}

function nricField(ctlId) {
    $("#" + ctlId).css("text-security", "disc");
    $("#" + ctlId).css("-webkit-text-security", "disc");
    $("#" + ctlId).attr("autocomplete", "off");
}

function setPreferenceTheme(theme) {
    var postData = {
        themeName: theme
    };

    fetch(GetBaseUrl() + '/UserAccount/SetPreferenceTheme', {
        method: "POST",
        cache: "no-cache",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(postData)
    }).then(function (response) {
        debugger;
        return response.json();
    }).then(function (response) {
        if (response.IsSuccess != undefined && response.IsSuccess == true) {
            window.location.href = window.location.href.replace("#", "");
            //toastr.success(response.message);
        } else {
            toastr.error(response.message);
            return;
        }
    });
}


$("#btnClearState").on('click', function () {
    var table = $('#tblData').dataTable().api();
    table.state.clear();
    window.location.reload();
});


function showHalfStar(val) {
    if (!val) {
        return "";
    }
    val = val.replace(/(\r\n|\n|\r)/gm, "").trim();
    if (!val || val=="") {
        return "";
    }

    var ret = "";
    var star = "****";

    if (val.length >= 4) {
        ret = val.substr(val.length - 4);
    } else {
        ret = val;
    }

    //var mid = val.length % 2 == 0 ? val.length / 2 : (val.length - 1) / 2 - 1;
    //for (var i = mid; i < val.length; i++) {
    //    star += "*";
    //    console.log(mid + i);
    //    ret += val[i];
    //}

    return star + ret;
}


$("#checkAll").click(function (e) {
    $('input[name="cbId"]').not(this).prop('checked', this.checked);
});

function reloadPage() {
    window.location.reload();
}

function confirmBulkDelete(url, selectToDelete, confirmDeleteAll) {

    var idList = [];
    $("input:checkbox[name=cbId]:checked").each(function () {
        idList.push($(this).val());
    });

    if (idList.length == 0) { alert(selectToDelete); return false; }

    if (confirm(confirmDeleteAll)) {
        var paraData = { "idList": idList };
        doBulkDelete(url, paraData);
    }
}

function doBulkDelete(url, paraData) {
    $.blockUI({ message: '<h1>Please wait <img src="'+ GetBaseUrl() +'/assets3/global/img/loading.gif" /></h1>' });
    var message = "";

    fetch(GetBaseUrl() + url, {
        method: 'POST',
        cache: "no-cache",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(paraData)
    }).then(function (response) {
        return response.text();
        }).then(function (response) {
            message = response;
    }).finally(function (response) {
        $.unblockUI();
        if (message != 'undefined' && message.indexOf("failed") == -1) {
                toastr.success(message);
                setInterval(reloadPage, 2000);
            } else {
                toastr.error(response);
            }
        });;
}

function confirmBulkDeleteVisitation(url, selectToDelete, confirmDeleteAll) {
    var idListVis = [];
    var idListSta = [];
    $("input:checkbox[visType=Visitor]:checked").each(function () {
        idListVis.push($(this).val());
    });

    $("input:checkbox[visType=Tenant]:checked").each(function () {
        idListSta.push($(this).val());
    });

    $("input:checkbox[visType=Staff]:checked").each(function () {
        idListSta.push($(this).val());
    });

    if (idListVis.length == 0 && idListSta.length == 0) { alert(selectToDelete); return false; }

    if (confirm(confirmDeleteAll)) {

        var paraData = { "idListVis": idListVis, "idListSta": idListSta };
        doBulkDelete(url, paraData);
        
    }
}

/*  Start of Select Columns */
function UseSelect2ForCompany() {
    $("#Company").select2({
        placeholder: 'Company Name',
        minimumInputLength: 0,
        allowClear: true,
        multiple: false,
        tags: true,
        ajax: {
            url: GetBaseUrl() + "/Company/SearchCompany",
            type: "post",
            dataType: 'json',
            delay: 250,
            async: true,
            data: function (params) {
                return {
                    searchTerm: params.term
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (obj) {
                        return { id: obj.id, text: obj.name };
                    }),
                    pagination: {
                        more: (params.page * 100) <= data.Total
                    }
                };
            },
            cache: true
        }
    });
}

/*  Start of Select Columns: Sub Questionnaire Answer CodeType */
function UseSelect2ForCodeType() {
    $("#SubCodeType").select2({
        placeholder: 'Code Type',
        minimumInputLength: 0,
        allowClear: true,
        multiple: false,
        tags: true,
        ajax: {
            url: GetBaseUrl() + "/SubQuestionnaireAnswerCode/SearchCodeType",
            type: "post",
            dataType: 'json',
            delay: 250,
            async: true,
            data: function (params) {
                return {
                    searchTerm: params.term
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (obj) {
                        return { id: obj.id, text: obj.name };
                    }),
                    pagination: {
                        more: (params.page * 100) <= data.Total
                    }
                };
            },
            cache: true
        }
    });
}
