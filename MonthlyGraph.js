var cellSize = [80, 80];
var pieRadius = 30;

var types = [];
var dataProvider = [];
var index = -1;

function getVirtulData() {
    var date = +echarts.number.parseDate('2019-10-01');
    var end = +echarts.number.parseDate('2019-11-01');
    var dayTime = 3600 * 24 * 1000;
    var data = [];
    for (var time = date; time < end; time += dayTime) {
        data.push([
            echarts.format.formatTime('yyyy-MM-dd', time),
            //Math.floor(Math.random() * 10000) //Where this one is use ??
        ]);
    }
    return data;
}

function getPieSeries(scatterData, chart) {
    debugger;
    return echarts.util.map(scatterData, function (item, index) {
        var center = chart.convertToPixel('calendar', item);
        index++;
        return {
            id: index + 'pie',
            type: 'pie',
            center: center,
            label: {
                normal: {
                    formatter: '{c}',
                    position: 'inside'
                }
            },
            radius: pieRadius,
            data: dataProvider[index]
        };
    });
}

function getPieSeriesUpdate(scatterData, chart) {
    return echarts.util.map(scatterData, function (item, index) {
        var center = chart.convertToPixel('calendar', item);
        return {
            id: index + 'pie',
            center: center
        };
    });
}


function loadMonthlyTenantsChart(year, month, chartDiv) {
    index = -1;
    var selectedDate = "01/" + month + "/" + year;

    fetch(GetBaseUrl() + '/ChartApi/GetTenantsByMonth?date=' + selectedDate, {
        method: "GET",
        cache: "no-cache",
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (response) {
        return response.json();
        }).then(function (response) {
        if (response.types.length == 0) {
            $("#" + chartDiv).hide();
            return;
        } else {
            $("#" + chartDiv).show();
        }

        types = response.types;
        dataProvider = response.dataProvider;

        var scatterData = getVirtulData();

        option = {
            tooltip: {},
            legend: {
                data: types,
                bottom: 20
            },
            calendar: {
                top: 'middle',
                left: 'center',
                orient: 'vertical',
                cellSize: cellSize,
                yearLabel: {
                    show: false,
                    textStyle: {
                        fontSize: 30
                    }
                },
                dayLabel: {
                    margin: 20,
                    firstDay: 1,
                    nameMap: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']
                },
                monthLabel: {
                    show: false
                },
                range: ['2019-10']
            },
            series: [{
                id: 'label',
                type: 'scatter',
                coordinateSystem: 'calendar',
                symbolSize: 1,
                label: {
                    normal: {
                        show: true,
                        formatter: function (params) {
                            return echarts.format.formatTime('dd', params.value[0]);
                        },
                        offset: [-cellSize[0] / 2 + 10, -cellSize[1] / 2 + 10],
                        textStyle: {
                            color: '#000',
                            fontSize: 14
                        }
                    }
                },
                data: scatterData
            }]
        };

        var app = window;
        if (!app.inNode) {
            var pieInitialized;
            setTimeout(function () {
                pieInitialized = true;
                myChart.setOption({
                    series: getPieSeries(scatterData, myChart)
                });
            }, 10);

            app.onresize = function () {
                if (pieInitialized) {
                    myChart.setOption({
                        series: getPieSeriesUpdate(scatterData, myChart)
                    });
                }
            };
        }

        var chart = document.getElementById(chartDiv);
        var myChart = echarts.init(chart);
        myChart.setOption(option);

    });
}