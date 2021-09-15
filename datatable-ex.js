/**
 * Datatable helper class
 */
class datatableHelper {

    getMainButtonO() {
        return '<div class="btn-group"><button id="btnAction" class="btn btn-xs green dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">Actions<i class="fa fa-angle-down"></i></button><ul class="dropdown-menu pull-left dropdown-action" style="background:#ffffff;" role="menu">';
    }

    getMainButtonC() {
        return '</ul></div>';
    }

    getDetailButton(dLink, data) {
        return '<li><a class="dowpdown-item" href="' + GetBaseUrl() + dLink + data + '"> View Detail </a></li>';
    }

    getEditButton(dLink, data) {
        return '<li><a class="dowpdown-item" href="' + GetBaseUrl() + dLink + data + '"> Edit </a></li>';
    }

    getDeleteButton(dLink, data) {
        return '<li><a class="dowpdown-item" href="' + GetBaseUrl() + dLink + data + '" onclick="return confirm(\'are you sure ?\')"> Delete </a></li>';
    }

    
    getSignatureButton(data, profileType) {
        return '<li><a class="dowpdown-item" href="#" onclick="return ShowSignaturePopup(' + data + ',\'' + profileType +'\')"> Acknowledge PDPA </a></li>';
    }

    getNewVisitationButton(dLink, data) {
        return '<li><a class="dowpdown-item" href="' + GetBaseUrl() + dLink + data + '"> New Visitation </a></li>';
    }

    getNewDeclarationButton(dLink, data) {
        return '<li><a class="dowpdown-item" href="' + GetBaseUrl() + dLink + data + '"> New Declaration </a></li>';
    }

    getIdCheckbox(id) {
        return '<input type="checkbox" name="cbId" value="' + id + '">';
    }

    getVisIdCheckbox(id, visType) {
        return '<input type="checkbox" visType="' + visType + '" name="cbId" value="' + id + '">';
    }

    getPreviewImage(imgSrc) {
        $('.preview').anarchytip();
        return '<a href="' + imgSrc + '" class="preview">'
            + '<img src="' + imgSrc + '" alt="gallery thumbnail" style="width:50px;height:50px;"/></a>';
    }

    createDataTable(params) {
        var dom = params["DOM"];
        if (dom === undefined) { dom = "lfrtip"; }

        var lenMenu = [[10, 25, 50, 100, 300, 500, 1000, 2000, 5000, 999999], ['10', '25', '50', '100', '300', '500', '1000', '2000', '5000', 'Show All']];
        if (params["LENMENU"] === undefined) {
            lenMenu = [[10, 25, 50, 100, 300, 500, 1000, 2000, 5000, 999999], ['10', '25', '50', '100', '300', '500', '1000', '2000', '5000','Show All']];
        } else {
            lenMenu = params["LENMENU"];
        }

        var defaultPageSize = 10;
        if (params["PAGELENGTH"] != undefined && params["PAGELENGTH"] > 0) {
            defaultPageSize = params["PAGELENGTH"];
        }

        var allowPaging = params["PAGING"];
        if (allowPaging === undefined) { allowPaging = true; }

        var table = params["TABLE"].dataTable({
            "processing": true,
            "serverSide": true,
            "searching": true,
            "paging": allowPaging,
            "pageLength": defaultPageSize,
            "pagingType": "simple_numbers",
            "oLanguage": {
                "sInfoFiltered": "",
                "sProcessing": "<h2>Please wait <img src='" + GetBaseUrl() + "/assets3/global/img/loading.gif' /></h2>"
            },
            //"oLanguage":  { "sInfoFiltered": "" },
            "stateSave": true,
            "ajax": {
                "url": GetBaseUrl() + params["URL"],
                "type": "POST",
                "data": params["AJAXDATA"],
                error: function (x, e, xhr) {
                    if (e == "parsererror") {
                        $("#btnSessiontimeout").click();
                    }
                }
              },
            "dom": dom,
            "columns": dtHelper.getColumns(params["COLUMNS"]),
            "order": params["ORDER"],
            "search": {
                "search": params["DSEARCH"]
            },
            "columnDefs": params["COLUMNDEFS"],
            buttons: params["BUTTONS"],
            lengthMenu: lenMenu,
        });

        $('div.dataTables_filter input').focus();
        return table;

    }

    createDataTableWithDateRange(params) {
        var dom = params["DOM"];
        if (dom === undefined) { dom = "lfrtip"; }

        var lenMenu = [[10, 25, 50, 100, 300, 500, 1000, 2000, 5000, 999999], ['10', '25', '50', '100', '300', '500', '1000', '2000', '5000', 'Show All']];
        if (params["LENMENU"] === undefined) {
            lenMenu = [[10, 25, 50, 100, 300, 500, 1000, 2000, 5000, 999999], ['10', '25', '50', '100', '300', '500', '1000', '2000', '5000', 'Show All']];
        } else {
            lenMenu = params["LENMENU"];
        }

        var allowPaging = params["PAGING"];
        if (allowPaging === undefined) { allowPaging = true; }

        var table = params["TABLE"].dataTable({
            "processing": true,
            "serverSide": true,
            "searching": true,
            "paging": allowPaging,
            "pagingType": "simple_numbers",
            //"oLanguage": { "sInfoFiltered": "" },
            "oLanguage": {
                "sInfoFiltered": "",
                "sProcessing": "<h2>Please wait <img src='"+ GetBaseUrl() + "/assets3/global/img/loading.gif' /></h2>"
            },
            "stateSave": true,
            'stateSaveParams': function (settings, data) {
                //data.dateStart = $("#dateStart").val();
                //data.dateEnd = $("#dateEnd").val();
                //console.log("stateSaveParams");
            },
            'stateLoadParams': function (settings, data) {
                //dateStart = data.dateStart;
                //dateEnd = data.dateEnd;
               //console.log("stateLoadParams");
            },
            'initComplete': function () {
                //var dtFrom = new moment().add(-30, 'days').format("DD/MM/YY");
                //var dtTo = new moment().format("DD/MM/YY");

                //if (dateStart == null || dateStart == "" || dateStart == undefined) {
                //    $("#dateStart").val(dtFrom);//No saved value. Set default value
                //} else {
                //    $("#dateStart").val(dateStart);                    
                //}

                //if (dateEnd == null || dateEnd == "" || dateEnd == undefined) {
                //    $("#dateEnd").val(dtTo);//No saved value. Set default value
                //} else {
                //    $("#dateEnd").val(dateEnd);
                //}

                //this.api().state.save();
                //console.log("initComplete");
            },
            "ajax": {
               // console.log("Send to server."),
                "url": GetBaseUrl() + params["URL"],
                "type": "POST",
                "data": params["AJAXDATA"],
                error: function (x, e, xhr) {
                    //Write you code here
                    if (e == "parsererror") {
                        $("#btnSessiontimeout").click();
                    }
                    //alert(x.readyState + " " + x.status + " " + e.msg);
                }
                //    function (d) {
                //    d.dateFrom = dateStart;
                //    d.dateTo = dateEnd;
                //}
            },
            "dom": dom,
            "columns": dtHelper.getColumns(params["COLUMNS"]),
            "order": params["ORDER"],
            "search": {
                "search": params["DSEARCH"]
            },
            "columnDefs": params["COLUMNDEFS"],
            buttons: params["BUTTONS"],
            lengthMenu: lenMenu
           
        });

        $('div.dataTables_filter input').focus();

        return table;
    }

    //No PageLength, No Sorting and With Refresh Option in this fun.
    createDataTableWithRrefreshOption(params) {
        var dom = params["DOM"];
        if (dom === undefined) { dom = "lfrtip"; }

        var lenMenu = [[10, 25, 50, 100, 300, 500, 1000, 2000, 5000, 999999], ['10', '25', '50', '100', '300', '500', '1000', '2000', '5000', 'Show All']];
        if (params["LENMENU"] === undefined) {
            lenMenu = [[10, 25, 50, 100, 300, 500, 1000, 2000, 5000, 999999], ['10', '25', '50', '100', '300', '500', '1000', '2000', '5000', 'Show All']];
        }

        var allowPaging = params["PAGING"];
        if (allowPaging === undefined) { allowPaging = true; }

        var table = params["TABLE"].dataTable({
            "processing": false,
            "serverSide": true,
            "searching": true,
            "pagingType": "simple_numbers",
            "paging": allowPaging,
            "oLanguage": { "sInfoFiltered": "" },
            "ajax": {
                "url": GetBaseUrl() + params["URL"],
                "type": "POST",
                "data": params["AJAXDATA"],
                error: function (x, e, xhr) {
                    //Write you code here
                    //console.log(x);
                    //console.log(e);
                    //console.log(xhr);
                    if (e == "parsererror") {
                        $("#btnSessiontimeout").click();
                    }
                    //alert(x.readyState + " " + x.status + " " + e.msg);
                }
            },
            "dom": dom,
            "columns": dtHelper.getColumns(params["COLUMNS"]),
            "ordering": false,
            "search": {
                "search": params["DSEARCH"]
            },
            "columnDefs": params["COLUMNDEFS"],
            buttons: params["BUTTONS"],
            "lengthChange": false,
            "pageLength": 8,
            "drawCallback": function (settings) {
                if (params["onRefresh"]) {
                    params["onRefresh"]();
                }
            }
        });

        $('div.dataTables_filter input').focus();
        return table;
    }

    createDataTableClientSide(params) {
        var dom = params["DOM"];
        if (dom === undefined) { dom = "lfrtip"; }

        var lenMenu = [[10, 25, 50, 100, 300, 500, 1000, 2000, 5000, 999999], ['10', '25', '50', '100', '300', '500', '1000', '2000', '5000', 'Show All']];

        var table = params["TABLE"].dataTable(
            {
            "searching": true,
            "paging": true,
            "pagingType": "simple_numbers",
            "oLanguage": { "sInfoFiltered": "" },
            "dom": params["DOM"],

            "order": params["ORDER"],
            "search": {
                "search": params["DSEARCH"]
            },
            "columnDefs": params["COLUMNDEFS"],
            buttons: params["BUTTONS"],
            lengthMenu: lenMenu,
            "aaSorting": [],
            });

        params["TABLE"].css("visibility", "visible"); 
        return table;

    }

    getColumns(colPara){
        var array = [];
        for (var x = 0; x < colPara.length; x++) {
            var value = colPara[x];

            if (value.constructor === Object) {
                array.push({
                    "name": value["NAME"],
                    "data": value["DATA"],
                    "render": value["RENDER"]
                });
            } else {
                array.push({
                    "name": value,
                    "data": value
                });
            }
        }
        return array;
    }

    showExtraSearch() {
        $('#extraSearch').appendTo('#tblData_filter');
        $('#extraSearch').show();
    }

    addSelect2DropDown(table, visibleColumns) {
        /*  Start of Select Columns - TO DO - Move This functions into Site.JS  */

        $(".selected-columns").change(function () {
            var columns = $(".selected-columns").val();

            $('#tblData > tbody  > tr').each(function (i, row) {
                for (var r = 0; r < row.cells.length; r++) {
                    var status = 'none';
                    if ($.inArray(r.toString(), columns) != -1) {
                        status = '';
                    }
                    var cell = table[0].rows[i].cells[r];
                    cell.style.display = status;

                    //For Last Row
                    if (i === table[0].rows.length - 2) {
                        cell = table[0].rows[i + 1].cells[r];
                        cell.style.display = status;
                    }
                }
            });
        });

        $('#tblData').on('draw.dt', function () {
            var columns = $(".selected-columns").val();
            if (columns != null && columns.length > 0) {
                $('#tblData > tbody  > tr').each(function (i, row) {
                    for (var c = 0; c < row.cells.length; c++) {
                        var status = 'none';
                        if ($.inArray(c.toString(), columns) != -1) {
                            status = '';
                        }
                        var cell = row.cells[c];
                        cell.style.display = status;

                        if (i == 0) {
                            var cell = table[0].rows[0].cells[c];
                            cell.style.display = status;
                        }
                    }
                });
            }
        });

        /*  End of Select Columns */
        var selectIndex = 0;
        $("thead > tr > th").each(function () {
            var optionHtml = "<option value='" + selectIndex + "'>" + $(this).html() + "</option>";
            $(".selected-columns").append(optionHtml);
            selectIndex++;
        });

        /*  Start of Select Columns */
        $(".selected-columns").select2({ theme: "classic", width: '100%', multiple: true });
        $('.selected-columns').val(visibleColumns).trigger('change');
            /*  End of Select Columns */
    }

}

var dtHelper = new datatableHelper();
