function curBtn(btn) {
    var p = +btn.id;
    console.log("clicked")
    doLivestock(p);
}

function doLivestock(x) {
    $.ajax({
        type: "GET", 
        url: "/Dashboard/AssignmentPartialView/?page=" + x,
        data: $("#myForm").serialize(),
        success: function (data) {
            console.log(data);
            $("#myInfo").html(data);
        }
    });
};


function curBtnA(btn) {
    var p = +btn.id;
    console.log("clicked")
    assignment(p);
}

function assignment(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/DeliveryAssignmenAcceptedPartialView/?page=" + x,
        data: $("#myForm1").serialize(),
        success: function (data) {
            console.log(data);
            $("#myInfo1").html(data);
        }
    });
};



function declineBtn(btn) {
    var p = btn.id;
    console.log("clicked")
    assignment1(p);
}

function assignment1(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/Decline/?id=" + x,
        data: $("#myForm").serialize(),
        success: function (data) {
            console.log(data);
            $("#myInfo").html(data);
        }
    });
};


function acceptBtn(event) {
    var getId = event.target;

    var p = getId.dataset.delivery;
   
    console.log("clicked")
    assignment5(p);
}

function assignment5(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/Accept/?assignmentId=" + x,
        data: $("#myForm").serialize(),
        success: function (data) {
            console.log(data);
            $("#myInfo").html(data);
        }
    });
};







function okayBtn(btn) {
    var p = +btn.id;
    console.log("clicked")
    okay(p);
}

function okay(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/DeliveryAssignment?check=true&page=" + x,
        data: $("#myForm").serialize(),
        success: function (data) {
            console.log(data);
            $("#myInfo").html(data);
        }
    });
};


function yesBtn(btn) {
    var p = btn.id;
    console.log("clicked")
    assignment123(p);
}

function assignment123(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/Accept/?assignmentId=" + x,
        data: $("#myForm").serialize(),
        success: function (data) {
            console.log(data);
            $("#myInfo").html(data);
        }
    });
};


function acceptedBtn(btn) {
    var p = +btn.id;
    accepted(p);
}

function accepted(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/AcceptedPartialView/?page=" + x,
        data: $("#myForm").serialize(),
        success: function (data) {
            console.log(data);
            $("#myInfo").html(data);
        }
    });
};