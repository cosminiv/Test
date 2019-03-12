jQuery(document).ready(function() {
  var epaDataRecords = JSON.parse(window.epadata);

    makeReqPerMinuteChart(epaDataRecords);
});

function makeReqPerMinuteChart(epaDataRecords) {
    // Build data
    var minDay = 29,
        minHour = 23,
        minMinute = 53;

    var minutesInFirstDay = 60 - minMinute;

    var labels = [];
    var requestCount = [];

    for (var i = 0; i < epaDataRecords.length; i++) {
        var d = epaDataRecords[i].datetime;
        var minuteOrder = (d.day === minDay) ? (d.minute - minMinute) : (minutesInFirstDay + d.hour * 60 + d.minute);

        if (typeof requestCount[minuteOrder] === 'undefined')
            requestCount[minuteOrder] = 0;
        else
            requestCount[minuteOrder]++;

        labels[minuteOrder] = d.day + ' Aug ' + d.hour + ':' + d.minute;
    }

    // This uses chart.js (https://www.chartjs.org)
    var ctx = document.getElementById('requestsPerMinuteChart').getContext('2d');
    var chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                borderColor: 'rgb(255, 99, 132)',
                data: requestCount,
            }]
        },

        options: {}
    });
}