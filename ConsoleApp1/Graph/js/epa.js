jQuery(document).ready(function() {
  var epaDataRecords = JSON.parse(window.epadata);

    console.log(epaDataRecords[0].host);

    makeReqPerMinuteChart(epaDataRecords);
});

function makeReqPerMinuteChart(epaDataRecords) {
    var minDay = 29,
        minHour = 23,
        minMinute = 53;

    var minutesInFirstDay = 60 - minMinute;

    var labels = [];
    var data = [];

    for (var i = 0; i < epaDataRecords.length; i++) {
        var d = epaDataRecords[i].datetime;
        var minuteOrder = (d.day === minDay) ? (d.minute - minMinute) : (minutesInFirstDay + d.hour * 60 + d.minute);

        if (typeof data[minuteOrder] === 'undefined')
            data[minuteOrder] = 0;
        else
            data[minuteOrder]++;

        labels[minuteOrder] = d.day + ':' + d.hour + ':' + d.minute;
    }

    var ctx = document.getElementById('requestsPerMinuteChart').getContext('2d');
    var chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                borderColor: 'rgb(255, 99, 132)',
                data: data,
            }]
        },

        options: {}
    });
}