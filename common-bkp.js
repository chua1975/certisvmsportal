$(document).ready(function(){
	// code total
	Highcharts.chart('code-total', {
	    chart: {
	    	backgroundColor: 'transparent',
	        type: 'column',
	        animation: Highcharts.svg,
	        events: {
		      load: function () {
		         // set up the updating of the chart each second
		         var series = this.series[0];
		         
		         setInterval(function () {
		            var x = (new Date()).getTime(), // current time
		            y = Math.random();
		            series.addPoint([x, y], true, true);
		         }, 1000);
		      }
		   }
	    },
	    credits: {
	        enabled: false
	    },
	    legend: {
	        itemStyle: {
	        	color: '#dd5d9b'
	        }
	    },
	    xAxis: {
	        categories: [
	            '0800',
	            '1000',
	            '1200',
	            '1400',
	            '1600',
	            '1800',
	            '2000',
	        ],
	        labels: {
	            style: {
	                color: '#ffbf00'
	            }
	        },
	        lineColor: '#0f5b6f',
        	lineWidth: 1,
	        crosshair: false,
	        title: {
	            text: null
	        }
	    },
	    yAxis: {
	        min: 0,
	        title: {
	            text: null
	        },
	        labels: {
	            style: {
	                color: '#ffbf00'
	            }
	        },
	        gridLineWidth: 1,
	        gridLineColor: '#0f5b6f',
	    },
	    plotOptions: {
	        column: {
	            pointPadding: 0.2,
	            borderWidth: 0
	        }
	    },
	    plotOptions: {
	        series: {
	            borderRadius: 5,
	            borderWidth: 0
	        }
	    },
	    colors: ['#dd5d9b', '#787df1'],
	    series: [{
	        name: 'Staff',
	        data: [16000, 16000, 17000, 17000, 17000, 12000, 6000],
	        color: {
		        linearGradient: {
		          x1: 0,
		          x2: 0,
		          y1: 0,
		          y2: 1
		        },
		        stops: [
		          [0, '#ec9e4b'],
		          [1, '#ff6e75']
		        ]
		      }

	    }, {
	        name: 'Visitor',
	        data: [10000, 14000, 15000, 16000, 18000, 9000, 8000],
	        color: {
		        linearGradient: {
		          x1: 0,
		          x2: 0,
		          y1: 0,
		          y2: 1
		        },
		        stops: [
		          [0, '#899ff9'],
		          [1, '#787df1']
		        ]
		      }
	    }]
	});


	// staff-attendance
	Highcharts.chart('staff-attendance', {
	    chart: {
	    	backgroundColor: 'transparent',
	        type: 'line',
	        height: 200,
	        animation: {
	            duration: 2000
	        }
	    },
	    credits: {
	        enabled: false
	    },
	    legend: {
		    enabled: false
		},
	    xAxis: {
	        categories: [
	            '0800',
	            '1000',
	            '1200',
	            '1400',
	            '1600',
	            '1800',
	            '2000',
	        ],
	        labels: {
	            style: {
	                color: '#ffbf00'
	            }
	        },
	        lineColor: '#0f5b6f',
        	lineWidth: 1,
	        crosshair: false,
	        title: {
	            text: null
	        },
	        tickPixelInterval: 50,
	        gridLineWidth: 1,
	        gridLineColor: '#0f5b6f',
	    },
	    yAxis: {
	        min: 0,
	        title: {
	            text: null
	        },
	        labels: {
	            style: {
	                color: '#ffbf00'
	            }
	        },
	        gridLineWidth: 1,
	        gridLineColor: '#0f5b6f',
	    },
	    colors: ['#dd5d9b', '#787df1'],
	    series: [{
	        name: 'Time',
	        data: [190, 90, 70, 100, 80]

	    }, {
	        name: 'Late',
	        data: [0, 110, 140, 110, 130]
	    }]
	});

	// total-access
	Highcharts.chart('total-access', {
	    chart: {
	    	backgroundColor: 'transparent',
	        polar: true,
	        height: 200,
	        animation: {
	            duration: 2000
	        }
	    },
	    credits: {
	        enabled: false
	    },
	    legend: {
		    enabled: false
		},
	    pane: {
	        startAngle: 0,
	        endAngle: 360
	    },
	    xAxis: {
	        tickInterval: 360,
	        min: 0,
	        max: 360,
	        labels: {
	            enabled: false
	        },
	        lineWidth: 0
	    },

	    yAxis: {
	        min: 0,
	        gridLineWidth: 0,
	        lineWidth: 0,
	        labels: {
	            enabled: false
	        },
	    },
	    plotOptions: {
	        series: {
	            pointStart: 0,
	            pointInterval: 45,
	            borderRadius: 5,
	            borderWidth: 0
	        }
	    },
	    series: [ {
	        type: 'area',
	        name: 'Record',
	        data: [1, 8, 2, 7, 3, 6, 4, 5],
	        color: {
		        linearGradient: {
		          x1: 0,
		          x2: 0,
		          y1: 0,
		          y2: 1
		        },
		        stops: [
		          [0, '#ec9e4b'],
		          [1, '#ff6e75']
		        ]
		    }
	    }]
	});



	// tenant-pie
	Highcharts.chart('tenant-pie', {
	    chart: {
	    	backgroundColor: 'transparent',
	        type: 'variablepie',
	        height: 280,
	        animation: {
	            duration: 2000
	        }
	    },
	    credits: {
	        enabled: false
	    },
	    legend: {
		    enabled: false
		},
	    plotOptions: {
	        series: {
	            borderWidth: 0
	        }
	    },
	    series: [{
	        minPointSize: 0,
	        innerSize: '30%',
	        zMin: 60,
	        name: 'tenant',
	        data: [{
	            name: 'Certis',
	            y: 505370,
	            z: 92.9,
	            color: {
			        linearGradient: {
			          x1: 0,
			          x2: 0,
			          y1: 0,
			          y2: 1
			        },
			        stops: [
			          [0, '#6895ec'],
			          [1, '#7170eb']
			        ]
			    }
	        }, {
	            name: 'Wework',
	            y: 551500,
	            z: 118.7,
	            color: {
			        linearGradient: {
			          x1: 0,
			          x2: 0,
			          y1: 0,
			          y2: 1
			        },
			        stops: [
			          [0, '#c666e3'],
			          [1, '#8258e0']
			        ]
			    }
	        }, {
	            name: 'PMO',
	            y: 312685,
	            z: 124.6,
	            color: {
			        linearGradient: {
			          x1: 0,
			          x2: 0,
			          y1: 0,
			          y2: 1
			        },
			        stops: [
			          [0, '#a25ee1'],
			          [1, '#e0538c']
			        ]
			    }
	        }, {
	            name: 'GIC',
	            y: 78867,
	            z: 137.5,
	            color: {
			        linearGradient: {
			          x1: 0,
			          x2: 0,
			          y1: 0,
			          y2: 1
			        },
			        stops: [
			          [0, '#ee7276'],
			          [1, '#ff9959']
			        ]
			    }
	        }]
	    }]
	});




	// on prem/hour
	Highcharts.chart('prem-hour', {
	    chart: {
	    	backgroundColor: 'transparent',
	        type: 'area',
	        height: 200,
	        animation: {
	            duration: 2000
	        }
	    },
	    credits: {
	        enabled: false
	    },
	    legend: {
		    enabled: false
		},
	    xAxis: {
	        categories: [
	            '0800',
	            '1000',
	            '1200',
	            '1400',
	            '1600',
	            '1800',
	            '2000',
	        ],
	        labels: {
	            style: {
	                color: '#ffbf00'
	            }
	        },
	        lineColor: '#0f5b6f',
        	lineWidth: 1,
	        crosshair: false,
	        title: {
	            text: null
	        },
	        tickPixelInterval: 50,
	        gridLineWidth: 0,
	        gridLineColor: '#0f5b6f',
	    },
	    yAxis: {
	        min: 0,
	        title: {
	            text: null
	        },
	        labels: {
	            style: {
	                color: '#ffbf00'
	            }
	        },
	        gridLineWidth: 1,
	        gridLineColor: '#0f5b6f',
	    },
	    plotOptions: {
	        area: {
	            fillOpacity: 0.5
	        }
	    },
	    series: [{
	        name: 'On Prem/Hour',
	        data: [0, 1.5, 1.65, 1.3, 1.4, 1.45, 1.45],
	        color: {
		        linearGradient: {
		          x1: 0,
		          x2: 0,
		          y1: 0,
		          y2: 1
		        },
		        stops: [
		          [0, 'rgba(116, 55, 76, 0.73)'],
		          [1, 'rgba(0,0,0,0.8)']
		        ]
		    }
	    }]
	});


	// total-registeration
	Highcharts.chart('total-register', {
	    chart: {
	    	backgroundColor: 'transparent',
	        type: 'pie',
	        height: 220,
	        animation: {
	            duration: 2000
	        }
	    },
	    credits: {
	        enabled: false
	    },
	    legend: {
		    enabled: true
		},
	    plotOptions: {
	        series: {
	            borderWidth: 0
	        }
	    },
	    series: [{
	        zMin: 60,
	        name: 'Total Registration',
	        data: [
	            {
	            	name: '1210', 
	            	y: 1210,
	            	color: {
				        linearGradient: {
				          x1: 0,
				          x2: 0,
				          y1: 0,
				          y2: 1
				        },
				        stops: [
				          [0, '#dd5c9d'],
				          [1, '#e9a544']
				        ]
				    }
	            },
	            {
	            	name: '5135', 
	            	y: 5135,
	            	color: {
				         radialGradient: {
				          cx: 0.5,
				          cy: 0.3,
				          r: 0.7
				        },
				        stops: [
				          [0, '#7980f1'],
				          [1, '#889df8']
				        ]
				    }
	            }
	        ]
	    }]
	});


	// category-bars
	Highcharts.chart('category-bars', {
	    chart: {
	    	backgroundColor: 'transparent',
	        type: 'bar', 
	        height: 180,
	        animation: {
	            duration: 2000
	        }
	    },
	    credits: {
	        enabled: false
	    },
	    legend: {
	        enabled: false
	    },
	    xAxis: {
	        categories: ['Visitor', 'Contractor', 'Delivery', 'Staff', 'Meeting'],
	        title: {
	            text: null
	        },
	        labels: {
	            style: {
	                color: '#00fcff'
	            }
	        },
	        gridLineWidth: 0
	    },
	     yAxis: {
	        min: 0,
	        title: {
	            text: null,
	            align: 'high'
	        },
	        labels: {
	        	enabled: false
	        },
	        gridLineWidth: 0
	    },
	    plotOptions: {
	        bar: {
	            dataLabels: {
	                enabled: true,
	                borderWidth: 0,
	                borderColor: '#000',
	                color: '#ffbf00'
	            }
	        }
	    },
	    series: [{
	        name: 'Category',
	        data: [87, 1192, 98, 1384, 48],
	        color: {
		        linearGradient: {
		          x1: 0,
		          x2: 0,
		          y1: 0,
		          y2: 1
		        },
		        stops: [
		          [0, '#dd5d9c'],
		          [1, '#e9a643']
		        ]
		      }

	    }]
	});


	// record-bars
	Highcharts.chart('record-bars', {
	    chart: {
	    	backgroundColor: 'transparent',
	        type: 'bar', 
	        height: 180,
	        animation: {
	            duration: 2000
	        }
	    },
	    credits: {
	        enabled: false
	    },
	    legend: {
	        enabled: false
	    },
	    xAxis: {
	        categories: ['Pin', 'Fingerprint', 'Access Card', 'QrCode', 'FR'],
	        title: {
	            text: null
	        },
	        labels: {
	            style: {
	                color: '#00fcff'
	            }
	        },
	        gridLineWidth: 0
	    },
	     yAxis: {
	        min: 0,
	        title: {
	            text: null,
	            align: 'high'
	        },
	        labels: {
	        	enabled: false
	        },
	        gridLineWidth: 0
	    },
	    plotOptions: {
	        bar: {
	            dataLabels: {
	                enabled: true,
	                borderWidth: 0,
	                borderColor: '#000',
	                color: '#ffbf00'
	            }
	        }
	    },
	    series: [{
	        name: 'Category',
	        data: [87, 1192, 98, 1384, 48],
	        color: {
		        linearGradient: {
		          x1: 0,
		          x2: 0,
		          y1: 0,
		          y2: 1
		        },
		        stops: [
		          [0, '#dd5d9c'],
		          [1, '#e9a643']
		        ]
		      }

	    }]
	});
})