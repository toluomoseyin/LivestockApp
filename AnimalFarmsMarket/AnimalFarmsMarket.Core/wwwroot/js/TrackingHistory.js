
function TrackingBtn1() {
    var p = $("#trackingIndex").val();
    trackingHistory1(p);
}

function trackingHistory1(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/Tracking-Orders/?Id=" + x,
        data: $("#trackingHistory1").serialize(),
        success: function (data) {
            console.log("click");
            $('#track').modal('hide');
            $('#trackingHistory').modal('show')
            $("#trackingHistoryPage").html(data);
        }
    });
};


function TrackingBtn2() {
    var p = $("#trackingIndex1").val();
    trackingHistory2(p);
}

function trackingHistory2(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/Tracking-Orders/?Id=" + x,
        data: $("#trackingHistory2").serialize(),
        success: function (data) {
            console.log("click");
            $('#track').modal('dispose');
            $('#trackingHistory').modal('show')
            $("#trackingHistoryPage").html(data);
        }
    });
};


function TrackingBtn3() {
    var p = $("#trackingIndex2").val();
    trackingHistory2(p);
}

function trackingHistory2(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/Tracking-Orders/?Id=" + x,
        data: $("#trackingHistory3").serialize(),
        success: function (data) {
            console.log(data);
            $('#track').modal('dispose');
            $('#trackingHistory').modal('show')
            $("#trackingHistoryPage").html(data);
        }
    });
};

