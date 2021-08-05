

//invoice pagination
function GoToPage(btn) {
    var pageNumber = +btn.id;
    GetInvoice(pageNumber);
}


function GetInvoice(pageNumber) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/Invoice/?paginated=true&" +"page=" + pageNumber,
        data: $("#invoiceForm").serialize(),
        success: function (data) {
            
            $("#accordionFlushExample").html(data);
            history.pushState('', "/Dashboard/Invoice/?" + "page=" + pageNumber,);
        }
    });
};


//market place pagination
//market view pagination
function curBtnPayment(btn) {
    var p = +btn.id;
    doPayment(p);
}


function doPayment(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/PaymentHistory?check=true&page=" + x,
        data: $("#tbody").serialize(),
        success: function (data) {
            console.log(data);
            $("#paymentInfo").html(data);
            console.log(x);
            history.pushState({},"", "/Dashboard/PaymentHistory/?page=" + x);
        }
    });
};


//market view pagination
function curBtn(btn) {
    var p = +btn.id;
    var location = document.getElementById("location");
    doLivestock(p, location);
}


function doLivestock(x, location) {
    if (x == 1) {
        $.ajax({
            type: "GET",
            url: "/Market/Index/?check=false&page=" + x + "&location=" + location,
            data: $("#myForm").serialize(),
            success: function (data) {
                console.log(data);
                $("#customerInformation").html(data);
            }
        });
    } else {
        $.ajax({
            type: "GET",
            url: "/Market/Index/?check=true&page=" + x + "&location=" + location,
            data: $("#myForm").serialize(),
            success: function (data) {
                console.log(data);
                $("#customerInformation").html(data);
            }
        });

    }
};

//Toggle accordion mobile view
var btn = document.getElementById("acc-btn");
btn.addEventListener("click", function (e) {
    e.preventDefault();
    var acc = document.getElementById("accordions");
    if (acc.style.display === "none") {
        acc.style.display = "block";
    }
    else {
        acc.style.display = "none";
    }
});


function displayData(btn) {
    let id = btn.id;
    console.log(id);
    let marketLink;

    var result;
    $.ajax({
        method: "GET",
        url: "/Dashboard/GetUserById/?id=" + id,
        success: function (res) {
            if (res == null) {
                console.log("not found");
            } else {
                result = res["roles"];
                document.getElementById("fullName").innerHTML = "Name: " + " " + res["firstName"] + " " + res["lastName"];
                document.getElementById("role").innerHTML = res["roles"];
                document.getElementById("businessLocation").innerHTML = "Location:" + " " + res["street"] + ", " + res["city"] + ", " + res["state"] + ".";
                document.getElementById("email").innerHTML = "Email:" + " " + res["email"];
                //document.getElementById("user-image").src = res["photo"];
                callBtn(res["roles"]);      
               
            }
        }


    });

    function callBtn(roles) {
        
        
        if (roles[0] == ("Agent")) {
            marketLink = "/Dashboard/DisplayAgentsmarket/?id=" + id ;
            document.getElementById("getMarket").style.visibility = 'visible';
            document.getElementById("getMarket").href = marketLink;

        }
    }

}




