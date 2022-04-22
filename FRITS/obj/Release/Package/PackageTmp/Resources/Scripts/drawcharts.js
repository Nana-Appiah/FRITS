

function drawLineChart(chartContainerName, lineData, colsNames, chartTitle, chartHeight, chartWidth) {

    google.charts.load('current', { 'packages': ['line'] });
    google.charts.setOnLoadCallback(drawLnChart);

    function drawLnChart() {

        var data = new google.visualization.DataTable();
        data.addColumn('string', "Categories");
        for (i = 0; i < colsNames.length; i++) {
            data.addColumn('number', "'" + colsNames[i][1] + "'")
        }

        /*data.addColumn('number', 'Day');
        data.addColumn('number', 'Guardians of the Galaxy');
        data.addColumn('number', 'The Avengers');
        data.addColumn('number', 'Transformers: Age of Extinction');*/

        var arr = [];
        for (i = 0; i < lineData.length; i++) {
            var x = [];
            var inCnt = 0
            for (k = 0; k < lineData[i].length; k++) {
                if (k == 0) {
                    x.push(lineData[i][k]);
                } else {
                    x.push(lineData[i][k]);
                }
            }
            arr.push(x);
        }

        data.addRows(arr);


        /*data.addRows([
         [1, 37.8, 80.8, 41.8],
         [2, 30.9, 69.5, 32.4],
        [3, 25.4, 57, 25.7],
         [4, 11.7, 18.8, 10.5],
         [5, 11.9, 17.6, 10.4],
        [6, 8.8, 13.6, 7.7],
          [7, 7.6, 12.3, 9.6],
         [8, 12.3, 29.2, 10.6],
         [9, 16.9, 42.9, 14.8],
          [10, 12.8, 30.9, 11.6],
          [11, 5.3, 7.9, 4.7],
          [12, 6.6, 8.4, 5.2],
         [13, 4.8, 6.3, 3.6],
          [14, 4.2, 6.2, 3.4]
        ]); */

        var options = {
            chart: {
                title: chartTitle,
                subtitle: ''
            },
            width: chartWidth,
            height: chartHeight,
            axes: {
                x: {
                    0: { side: 'top' }
                }
            }
        };

        var lnChart = new google.charts.Line(document.getElementById(chartContainerName));

        lnChart.draw(data, google.charts.Line.convertOptions(options));

    }

}




function drawPieChart(chartContainerName, pieData, chartTitle, dipNmeDesc, dispValDesc, do3D, doSlice, showLegend) {

    google.charts.load("current", { packages: ["corechart"] });
    google.charts.setOnLoadCallback(drawPiChart);

    function drawPiChart() {
        //var data = google.visualization.arrayToDataTable(createPieChartDataTable(pieData));
        var data = createPieChartDataTable(pieData);
        var pieSlice = null;

        if (doSlice == true) {
            pieSlice = ({
                1: { offset: 0.2 },
                3: { offset: 0.3 },
                5: { offset: 0.4 },
                6: { offset: 0.5 },
            });
        }
        var options = {
            title: chartTitle,
            legend: (showLegend == false ? 'none' : showLegend),
            is3D: do3D,
            slices: pieSlice,
        };

        /* 
        [
          ['Task', 'Hours per Day'],
          ['Work', 11],
          ['Eat', 2],
          ['Commute', 2],
          ['Watch TV', 2],
          ['Sleep', 7]
        ]
        */
        var piChart = new google.visualization.PieChart(document.getElementById(chartContainerName));
        piChart.draw(data, options);
    }
}


var barDirections = ({
    barVertical: 'vertical',
    barHorizontal: 'horizontal',
});

function drawBarChart(chartContainerName, barData, chartTitle, barDirection, showLegend, subTitles) {

    google.charts.load('current', { 'packages': ['bar'] });
    google.charts.setOnLoadCallback(drawChart);

   /* var arrObj = new google.visualization.DataTable();
    for (var i = 0; i < barData.length; i++) {
        var dr = [];
        dr.push(barData[i].seriesName)
        for (var k = 0; k < barData[i].displayValues[0].length; k++) {
            dr.push(barData[i].displayValues[0][k])
        }
        //data.addRow([dr]);
    } */


    function drawChart() {
       // var data = google.visualization.arrayToDataTable(createBarChartDataTable(barData));
        var data = createBarChartDataTable(barData)
        /* 
        [
          ['Year', 'Sales', 'Expenses', 'Profit'],
          ['2014', 1000, 400, 200],
          ['2015', 1170, 460, 250],
          ['2016', 660, 1120, 300],
          ['2017', 1030, 540, 350]
        ] */
        var options = {
            chart: {
                title: chartTitle,
                subtitle: (subTitles != null ? subTitles : null),
            },
            bars: barDirection // Required for Material Bar Charts.
        };
        var chart = new google.charts.Bar(document.getElementById(chartContainerName));
        chart.draw(data, google.charts.Bar.convertOptions(options));
    }

}

function createPieChartDataTable(dataValues, dispNmeDesc, dispValDesc) {

    var data = new google.visualization.DataTable();

    data.addColumn('string', 'displayName');
    data.addColumn('number', 'displayValue');

    data.addRow([dispNmeDesc, dispValDesc]);
    for (var i = 0; i < dataValues.length; i++) {
        var val = parseInt(dataValues[i].displayValue)

        data.addRow([dataValues[i].displayName, val !== null ? val : 0.00]);
    }

    return data

}

function createBarChartDataTable(barData) {

    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Name');

    for (var i = 0; i < barData.length -1; i++){   
        data.addColumn('number', barData[i].seriesName);
    }
    for (var i = 0; i < barData.length; i++) {
        var dr = [];
        dr.push(barData[i].seriesName)  
        for (var k = 0; k < barData[i].displayValues[0].length; k++) {
            dr.push(barData[i].displayValues[0][k])
        }
        data.addRow(dr);
    }
    return data
}

