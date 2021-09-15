var app = new Vue({
    el: '#positiondiv',
    data: {
        selectNode: '',
        checkNodeIdStr: '',
        selectSaveNodesIdObj: [],
        selectedBuildingList: [],
        selectBuilding: '',
        regionOnly: '',
        firstShowModel: true,
        rootNode: null
    },
    methods: {
        Select: function () {
            if (this.firstShowModel) {
                this.checkNodeIdStr = '';
                var childSelect = [];
                var checkNodes = $('#PositionTree').treeview('getChecked');
                for (var index in checkNodes) {
                    this.checkNodeIdStr += checkNodes[index].nodeId + ",";

                    var childNodesArry = this.GetChildNode(checkNodes[index]);
                    childSelect = childSelect.concat(childNodesArry);
                }
                this.checkNodeIdStr = this.checkNodeIdStr.substring(0, this.checkNodeIdStr.length - 1);// 去掉最后一个，
            }

            this.firstShowModel = false;

            $('#selectPositionModel').modal('show');
            $('#PositionTree').treeview('uncheckAll', { silent: true });
            console.log(this.checkNodeIdStr);
            var nodeIds = this.checkNodeIdStr;
            if (nodeIds != null && nodeIds != "") {
                var arryNodeIds = nodeIds.split(',');
                for (var index in arryNodeIds) {
                    $("#PositionTree").treeview("checkNode", [parseInt(arryNodeIds[index]), { silent: true }]);

                    if (this.regionOnly == '') {
                        var node = $("#PositionTree").treeview("getNode", [parseInt(arryNodeIds[index]), { silent: true }]);
                        var regionNode = this.GetParentNode(node);
                        this.regionOnly = regionNode.Id;
                    }
                }

            }


        },
        Cancel: function () {
            this.regionOnly = '';
            $('#selectPositionModel').modal('hide');
        },
        Save: function () {
            this.GetCheckNodePosition();
            this.regionOnly = '';
            $('#selectPositionModel').modal('hide');
        },
        SelectDefaultBuilding: function () {
            alert($("#defaultBuildingDropDown").val());
            $("#DefaultBuilding").val($("#defaultBuildingDropDown").val());
        },
        // load data
        GetPositionJson: function () {
            //debugger;
            var _self = this;
            var selectedID = $("#selectPositionID").val() == null || $("#selectPositionID").val() == "" ? $("#selectPositionID").html() : $("#selectPositionID").val();
            // debugger;
            $.ajax({
                type: "POST",
                data: { "checkNodesString": selectedID },
                url: GetBaseUrl() + '/Position/GetPositionAuthorityData',
                success: function (data) {
                    var jsObject = JSON.parse(data);
                    _self.rootNode = jsObject;
                    //console.log(jsObject);
                    //Set position .if there is only one child node. default the value and readonly (not allow to change)
                    var bool = IsOnlyNode(jsObject[0]);
                    if (bool) {
                        jsObject[0].nodes[0].nodes[0].nodes[0].checked = true;
                        jsObject[0].nodes[0].nodes[0].nodes[0].state.checked = true;
                        jsObject[0].nodes[0].nodes[0].nodes[0].nodes[0].checked = true;
                        jsObject[0].nodes[0].nodes[0].nodes[0].nodes[0].state.checked = true;
                        $('#addbutton').attr('disabled', true);
                    }
                    //------------
                    $('#PositionTree').treeview({
                        data: jsObject,
                        showBorder: true,
                        showCheckbox: true,
                        showIcon: true,
                        highlightSelected: true,
                        multiSelect: false,
                        showTags: false,
                        onNodeChecked: function (event, node) {
                            if (!node.selectable) {
                                $('#PositionTree').treeview('uncheckNode', [node.nodeId, { silent: true }]);
                                return;
                            }
                            if (node.tags != 0) {
                                var parentRegion = _self.GetParentNode(node);
                                if (_self.regionOnly != '' && parentRegion.Id != _self.regionOnly) {
                                    $('#PositionTree').treeview('uncheckNode', [node.nodeId, { silent: true }]);
                                    alert("Only nodes under the same Region can be checked.");
                                    return;
                                }
                            }

                            var selectNodes = _self.GetChildNodeIdArr(node);
                            if (selectNodes) {

                                $('#PositionTree').treeview('checkNode', [selectNodes, { silent: true }]);
                            }

                            var parentNode = $('#PositionTree').treeview("getNode", node.parentId);
                            _self.SetParentNodeCheck(node, "PositionTree");

                            $('#PositionTree').treeview('uncheckNode', [0, { silent: true }]);
                        },
                        onNodeUnchecked: function (event, node) {
                            var selectNodes = _self.GetChildNodeIdArr(node);
                            if (selectNodes) {
                                $('#PositionTree').treeview("uncheckNode", [selectNodes, { silent: true }]);

                            }
                            _self.SetParentNodeUnCheck(node, "PositionTree");

                            var getcheckedNode = $("#PositionTree").treeview("getChecked");
                            if (getcheckedNode.length == 0) {
                                _self.regionOnly = '';
                            }
                        }
                    });

                    //$("#PositionTree").treeview('collapseAll');

                    ////CT requirement . expend to low/high/mid rise
                    $('#PositionTree').treeview('expandAll', { levels: 2, silent: true });

                    _self.GetCheckNodePosition();


                }
            });
        },
        GetChildNodeIdArr: function (node) {
            var ts = [];
            if (node.nodes) {
                for (x in node.nodes) {
                    ts.push(node.nodes[x].nodeId);
                    if (node.nodes[x].nodes) {
                        var getNodeDieDai = this.GetChildNodeIdArr(node.nodes[x]);
                        for (j in getNodeDieDai) {
                            ts.push(getNodeDieDai[j]);
                        }
                    }
                }
            } else {
                ts.push(node.nodeId);
            }
            return ts;
        },
        SetParentNodeCheck: function (node, treeId) {
            var parentNode = $("#" + treeId + "").treeview("getNode", node.parentId);
            if (parentNode.nodes) {
                var checkedCount = 0;
                for (x in parentNode.nodes) {
                    if (parentNode.nodes[x].state.checked) {
                        checkedCount++;
                    } else {
                        break;
                    }
                }
                if (checkedCount === parentNode.nodes.length) {
                    $("#" + treeId + "").treeview("checkNode", parentNode.nodeId);
                    this.SetParentNodeCheck(parentNode);
                }
            }
        },
        SetParentNodeUnCheck: function (node, treeId) {
            var nodeparent = node.parentId
            if (nodeparent) {
                var parentNode = $("#" + treeId + "").treeview("getNode", node.parentId);
                $("#" + treeId + "").treeview("uncheckNode", [parentNode, { silent: true }]);
                this.SetParentNodeUnCheck(parentNode, treeId)
            }

        },
        IsNodeInNodeList: function (node, nodeList) {
            for (var i = 0; i < nodeList.length; i++) {
                if (nodeList[i].nodeId == node.nodeId) {
                    return true;
                }
            }
            return false;
        },
        GetNodesBuildingList: function (nodes) {
            var list = [];
            if (nodes != null && nodes != undefined) {
                for (var i = 0; i < nodes.length; i++) {
                    var buildingNodeList = [];
                    var node = nodes[i];
                    if (node.tags == 1) {
                        if (node.nodes) {
                            for (x in node.nodes) {
                                buildingNodeList.push(node.nodes[x]);
                            }
                        }

                    } else if (node.tags == 2) {
                        buildingNodeList.push(node);
                    } else if (node.tags == 3) {
                        var parentBuildingNode = $('#PositionTree').treeview('getParent', node);
                        if (parentBuildingNode != undefined) {
                            buildingNodeList.push(parentBuildingNode);
                        }
                    } else if (node.tags == 4) {
                        var parentFloorNode = $('#PositionTree').treeview('getParent', node);
                        if (parentFloorNode != undefined) {
                            var parentBuildingNode = $('#PositionTree').treeview('getParent', parentFloorNode);
                            if (parentBuildingNode != undefined) {
                                buildingNodeList.push(parentBuildingNode);
                            }
                        }
                    }


                    if (buildingNodeList != null && buildingNodeList.length > 0) {
                        for (var j = 0; j < buildingNodeList.length; j++) {
                            var n = buildingNodeList[j];
                            var isNodeIdInList = false;
                            for (var k = 0; k < list.length; k++) {
                                if (list[k].Id == n.Id) {
                                    isNodeIdInList = true;
                                    break;
                                }
                            }
                            if (isNodeIdInList == false) {
                                list.push({ Text: n.text, Id: n.Id });
                            }
                        }
                    }
                }
            }

            this.selectedBuildingList = list;
            if (list.length > 0) {
                $("#DefaultBuilding").val(list[0].Id);
            }
        },
        GetCheckNodePosition: function () {
            this.checkNodeIdStr = '';
            //debugger;
            filteredTree();
            var childSelect = [];
            var checkNodes = $('#PositionTree').treeview('getChecked');
            var buildingNodesList = this.GetNodesBuildingList(checkNodes);
            for (var index in checkNodes) {
                this.checkNodeIdStr += checkNodes[index].nodeId + ",";

                var childNodesArry = this.GetChildNode(checkNodes[index]);
                childSelect = childSelect.concat(childNodesArry);
            }
            this.checkNodeIdStr = this.checkNodeIdStr.substring(0, this.checkNodeIdStr.length - 1);


            var result = checkNodes.filter(function (item1) {
                return childSelect.every(function (item2) {
                    return item2.nodeId !== item1.nodeId;
                });
            });


            var selectNodesText = "";
            var arr = new Array();

            $(result).each(function (index, item) {
                selectNodesText += item.text + " ,";
                var obj = new Object();
                obj.Id = item.Id;
                obj.tags = item.tags;
                arr.push(obj);
            });
            selectNodesText = selectNodesText.substring(0, selectNodesText.length - 1);
            this.selectNode = selectNodesText;
            this.selectSaveNodesIdObj = arr;

            //debugger;
           
            var finalArr = filteredTree();
            $("#selectPosition").val(selectNodesText);
            $('#divSelectPosition').treeview({
                data: finalArr,
                showBorder: true,
                //nodeIcon:'glyphicon glyphicon-stop',
                //showCheckbox: true,
                showIcon: false,
                highlightSelected: false,
                multiSelect: false,
                showTags: false,
                //collapseIcon: 'none'
            }
            );
            $("span[class='icon expand-icon glyphicon glyphicon-minus']").removeClass();
            $("#TreeList").val(JSON.stringify(arr));
            if (selectNodesText.length > 0) {
                $('#selectPositionText').hide();
            } else {
                $('#selectPositionText').show();
            }


        },
        GetChildNode: function (node) {
            var ts = [];
            if (node.nodes) {
                for (x in node.nodes) {
                    ts.push(node.nodes[x]);
                    this.GetChildNode(node.nodes[x]);
                }
            }
            return ts;
        },
        GetParentNode: function (node) {
            if (node.tags == 1) {
                if (this.regionOnly == '') {
                    this.regionOnly = node.Id
                }
                return node;
            }
            else {
                var parentNode = $('#PositionTree').treeview('getParent', node);
                return this.GetParentNode(parentNode);
            }
        }


    },
    created: function () {
        this.GetPositionJson();
    }
});


function filteredTree() {
    var checkNodes = $('#PositionTree').treeview('getChecked');
    var checkedIdArr = checkNodes.map(n => n.nodeId);
    var checkedIdArr2 = [];
    var root = $('#PositionTree').treeview("getNode", 0);
    var appendParents = function (node) {
        if (node == null || !node.nodeId) {
            return;
        }
        ////if come from $('#PositionTree').treeview("getNode", node.parentId) will be nodeId
        if (node.nodeId > 0 && checkedIdArr2.indexOf(node.nodeId) < 0) {
            checkedIdArr2.push(node.nodeId);
        }

        if (node.parentId > 0) {
            var p = $('#PositionTree').treeview("getNode", node.parentId);
            appendParents(p);
        }
    };

    for (var i = 0; i < checkedIdArr.length; i++) {
        if (checkedIdArr2.indexOf(checkedIdArr[i]) < 0) {
            checkedIdArr2.push(checkedIdArr[i]);
        }

        var n = $('#PositionTree').treeview("getNode", checkedIdArr[i]);
        appendParents(n);
    }

    var newRoot = $.extend(true, [], root);


    var filterChecked = function (n) {
        if (n.nodes != null) {
            for (var i = 0; i < n.nodes.length; i++) {
                if (checkedIdArr2.indexOf(n.nodes[i].nodeId) < 0) {
                    n.nodes.splice(i, 1);
                    i--;
                } else {
                    filterChecked(n.nodes[i]);
                }
            }
        }
    };
    filterChecked(newRoot);
    return [newRoot];
}
function GetFinalParentNode(node) {
    //debugger;
    newNode = $.extend(true, [], node);

    if (newNode.parentId !== 0) {
        if (GetNodeChecked(newNode, 'PositionTree')) {
            newNode.nodes = [];
        }
        var parentNode = $('#PositionTree').treeview("getNode", newNode.parentId);
        newData = $.extend(true, [], parentNode);
        var arr = [];
        arr.push(newNode);
        newData.nodes = arr;
        return GetFinalParentNode(newData);
    } else {
        if (GetNodeChecked(newNode, 'PositionTree')) {
            newNode.nodes = [];
        }
        return newNode;
    }
}
function GetNodeChecked(node, treeId) {
    
    return node.state.checked;
}

function IsOnlyNode(root) {
    if (root.nodes == null && root.tags === "4") {
        return true;
    }
    else if (root.nodes.length === 1 && root.tags !== "4") {
        return IsOnlyNode(root.nodes[0]);
    }
    else {
        return false;
    }
}